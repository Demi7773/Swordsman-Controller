using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour, IHittable, IController
{

    [Header("Dependencies")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Stamina _stamina;
    [SerializeField] private EnemyAttacks _attacks;
    [Header("UI")]
    [SerializeField] private EnemyHPUIAdvanced _hpUI;
    [Header("HP")]
    public float CurrentHP = 30f;
    public float MaxHP = 30f;
    [Header("Stamina Costs")]
    [SerializeField] private float _lightAtkStamina = 5f;
    [SerializeField] private float _heavyAtkStamina = 10f;
    [Space(30)]
    [Header("Decisions")]
    public Vector3 CurrentTargetPos;
    [SerializeField] private AttackType _queuedAtk = AttackType.None;
    [Header("State")]
    public AlertState MyAlertState = AlertState.Patrol;
    public Behavior MyBehavior = Behavior.LookAround;
    public State MyState = State.Idle;
    public AttackType CurrentAttackType = AttackType.None;
    [SerializeField] private float _lostTargetTimer = 0f;
    [SerializeField] private float _backToAlertedThreshold = 10f;

    [SerializeField] private float _lookAroundDurationMin = 1f;
    [SerializeField] private float _lookAroundDurationMax = 5f;
    [SerializeField] private float _lookAroundCountdown = 0f;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _targetPointOnMap;

    [Header("Attacks")]
    [Header("Combo")]
    [SerializeField] private float _endOfAttackForgivenessWindow = 0.2f;
    [SerializeField] private float _comboTimerDuration = 0.6f;
    [SerializeField] private float _comboTimer = 0f;
    [SerializeField] private int _lightAtkIndex = 0;
    [SerializeField] private AttackType _queuedComboStep = AttackType.None;
    [SerializeField] private List<AttackType> _currentCombo = new List<AttackType>();
    [Space(10)]
    [Header("Attack Durations")]
    [SerializeField] private float _chargingAtkDuration = 0.2f;
    [SerializeField] private float _lightAtkDuration = 0.5f;
    [SerializeField] private float _heavyAtkDuration = 1.0f;
    [Space(10)]
    [Header("Attack Timers")]
    [SerializeField] private float _chargingCountdown = 0f;
    [SerializeField] private float _lightAtkCountdown = 0f;
    [SerializeField] private float _heavyAtkCountdown = 0f;
    [SerializeField] private float _sprintingAtkCountdown = 0f;
    [SerializeField] private float _jumpingAtkCountdown = 0f;
    [Space(30)]
    [Header("Blocking")]
    [SerializeField] private float _staggerForceThreshold = 0.2f;
    [SerializeField] private float _deflectWindow = 0.2f;
    [SerializeField] private bool _isInDeflectWindow = false;
    [SerializeField] private float _deflectDuration = 0.2f;
    [SerializeField] private float _deflectWindowCountdown = 0f;
    [SerializeField] private float _deflectCountdown = 0f;
    [Space(30)]
    [Header("Movement")]
    [SerializeField] private Vector2 _movingDirection = Vector2.zero;
    [Header("Speed")]
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _idleSpeed = 3f;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _fallingMoveSpeed = 3f;
    [SerializeField] private float _blockSpeed = 0f;
    [SerializeField] private float _chargingAtkSpeed = 1f;
    [SerializeField] private float _lightAtkSpeed = 2f;
    [Space(30)]
    [Header("Falling")]
    [SerializeField] private float _gravity = 10f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private float _currentYVelocity = 0f;
    [Space(30)]
    [Header("Stagger")]
    [SerializeField] private bool _staggerBackwards = true;
    [SerializeField] private float _staggerCountdown = 0f;



    public enum AlertState
    {
        Patrol,
        Alerted,
        InCombat
    }

    public enum Behavior
    {
        MoveToDestination,
        LookAround,
        CombatActions
    }

    public enum State
    {
        Idle,
        Free,
        Falling,
        ChargingAttack,
        Attacking,
        Blocking,
        Deflecting,
        Staggered,
        Dead
    }
    public enum AttackType
    {
        None,
        Light,
        Heavy
    }





    private void Start()
    {
        Init();
    }

    // IHittable
    private void Init()
    {
        _hpUI.Init();
        GetHealed(MaxHP);
    }
    public void GetHealed(float amount)
    {
        float newHP = CurrentHP + amount;
        CurrentHP = Mathf.Clamp(newHP, 0f, MaxHP);
        _hpUI.UpdateHealthBar(CurrentHP, MaxHP);
        //_hpUI.DisplayFloatingNumber(FloatingNumber.Context.Heal, transform.position, amount);
    }
    public void GetHit(Vector3 hitPos, float dmg, float force, IHittable hittable = null)
    {
        if (IsDeflectingAttack(hitPos, force))
        {
            _isInDeflectWindow = false;
            _deflectCountdown = _deflectDuration;
            //SwitchState(State.Deflecting);
            Debug.Log("Attack deflected on GetHit");
            return;
        }
        if (IsBlockingAttack(hitPos, force))
        {
            dmg *= 0.5f;
            force *= 0.5f;
            Debug.Log("Attack blocked, dmg, force reduced to " + dmg + ", " + force);
        }
        float newHP = CurrentHP - dmg;
        GetStaggered(hitPos, force);
        if (newHP < 0f)
        {
            Die();
        }
        CurrentHP = Mathf.Clamp(newHP, 0f, MaxHP);
        _hpUI.UpdateHealthBar(CurrentHP, MaxHP);
        //_hpUI.DisplayFloatingNumber(FloatingNumber.Context.Damage, hitPos, dmg);
    }
    // IController
    public void GetStaggered(Vector3 startPos, float force)
    {
        if (force < _staggerForceThreshold)
            return;

        float dot = Vector3.Dot(transform.forward, (startPos - transform.position).normalized);
        if (dot < 0)
            _staggerBackwards = true;
        else
            _staggerBackwards = false;

        _staggerCountdown = force * 0.05f;
        EndAttack(true);
        SwitchState(State.Staggered);
    }
    public void GetBlocked(float force)
    {
        _stamina.SpendStamina(force * 0.5f);
    }
    public void GetDeflected(float force)
    {
        if (force < _staggerForceThreshold)
            return;
        _stamina.SpendStamina(force);
        _staggerCountdown = force * 0.05f;
        _staggerBackwards = true;
        EndAttack(true);
        SwitchState(State.Staggered);
    }

    



    private void SwitchState(State state)
    {
        MyState = state;
    }
    private void CheckGrounded()
    {
        Ray ray = new Ray(_groundCheck.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _groundCheckDistance, _groundLayer))
        {
            //Debug.Log("Groundcheck hit at " + hit.point);
            Vector3 closestPoint = hit.collider.ClosestPointOnBounds(transform.position);
            _currentYVelocity = 0f;
            transform.position = new Vector3(transform.position.x, /*hit.point.y*/closestPoint.y + _groundCheckDistance + 0.5f, transform.position.z);
            SwitchState(State.Free);
        }
        else
        {
            _currentYVelocity = 0f;
            SwitchState(State.Falling);
            //transform.position += new Vector3(0f, _currentYVelocity * Time.deltaTime, 0f);
            Debug.Log(this + " Groundcheck missed on MoveHorizontal, switching to Falling");
        }
    }
    private void Die()
    {
        SwitchState(State.Dead);
        Debug.Log("Enemy Ded");
    }

    private void Update()
    {
        _stamina.RegenTick(MyState);

        CheckGrounded();
        HandleDecisions();
        HandleMovement();
        HandleState();

        bool isMoving = true;
        if (_movingDirection.magnitude < 0.1f)
            isMoving = false;
        //_animations.HandleAnimator(MyState, isMoving, _staggerBackwards, _currentAttackType, _lightAtkIndex);
    }




    public bool HasLOSToTarget()
    {
        return true;
    }
    public void SetAlertState(AlertState alert)
    {
        MyAlertState = alert;
    }
    public void SetBehavior(Behavior behavior)
    {
        MyBehavior = behavior;
    }
    private void HandleDecisions()
    {
        bool hasAction = false;
        switch (MyState)
        {
            case State.Idle:
                hasAction = true;
                break;
            case State.Free:
                hasAction = true;
                break;
            case State.ChargingAttack:
                hasAction = true;
                break;
            case State.Blocking:
                hasAction = true;
                break;

            default: break;
        }
        if (!hasAction)
            return;



        switch (MyAlertState)
        {
            case AlertState.Patrol:
                // Check suspicions
                //if (MyBehavior != Behavior.LookAround)
                //{
                //    _lookAroundCountdown = Random.Range(_lookAroundDurationMin, _lookAroundDurationMax);
                //    SetBehavior(Behavior.LookAround);
                //}
                break;

            case AlertState.Alerted:
                if (HasLOSToTarget())
                {
                    SetAlertState(AlertState.InCombat);
                    break;
                }
                if (MyBehavior != Behavior.LookAround)
                {
                    _lookAroundCountdown = Random.Range(_lookAroundDurationMin, _lookAroundDurationMax);
                    SetBehavior(Behavior.LookAround);
                }
                break;

            case AlertState.InCombat:
                if (!HasLOSToTarget())
                {
                    _lostTargetTimer += Time.deltaTime;
                    if (MyBehavior != Behavior.LookAround)
                    {
                        _lookAroundCountdown = Random.Range(_lookAroundDurationMin, _lookAroundDurationMax);
                        SetBehavior(Behavior.LookAround);
                    }
                }
                if (_lostTargetTimer > _backToAlertedThreshold)
                {
                    SetAlertState(AlertState.Alerted);
                    break;
                }

                // set destination
                SetBehavior(Behavior.CombatActions);
                break;
        }



        switch (MyBehavior)
        {
            case Behavior.MoveToDestination:
                SwitchState(State.Free);
                // adjust direction to towards destination for patrol, alert, combat
                break;

            case Behavior.LookAround:
                SwitchState(State.Free);
                _lookAroundCountdown -= Time.deltaTime;
                if (_lookAroundCountdown < 0f)
                {
                    SetBehavior(Behavior.MoveToDestination); 
                    break;
                }
                // if found
                //      set destination
                //      SetAlertState(AlertState.InCombat);
                //      SetBehavior(Behavior.CombatActions);
                break;

            case Behavior.CombatActions:
                // check distances, reachable, stamina
                // decide next CombatAction:
                // (Move to better pos: towards or away destination -> adjust direction to towards destination
                // attack light, attack heavy, block -> set _queuedAction
                // SwitchState(State.Free);
                // SwitchState(State.ChargingAtk);  // add branching for attack types
                // SwitchState(State.Blocking);
                break;
        }

    }





    private void HandleMovement()
    {
        Vector3 towardTarget = CurrentTargetPos - transform.position;
        Vector2 horizontalTowardsTarget = new Vector2(towardTarget.x, towardTarget.z);
        switch (MyState)
        {
            case State.Idle:
                MoveHorizontal(_idleSpeed, horizontalTowardsTarget);
                break;

            case State.Free:
                MoveHorizontal(_runSpeed, horizontalTowardsTarget);
                break;

            case State.Falling:
                _currentYVelocity -= _gravity * Time.deltaTime;
                MoveHorizontal(_fallingMoveSpeed, _movingDirection);
                MoveVertical(_currentYVelocity * Time.deltaTime);
                Ray ray = new Ray(_groundCheck.position, -transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _groundCheckDistance, _groundLayer))
                {
                    _currentYVelocity = 0f;
                    transform.position = new Vector3(transform.position.x, hit.point.y + _groundCheckDistance + 0.5f, transform.position.z);
                    SwitchState(State.Free);
                    Debug.Log(this + " Groundcheck hit at " + hit.point + ", landing and switching to Free");
                }
                break;

            case State.ChargingAttack:
                MoveHorizontal(_chargingAtkSpeed, _movingDirection);
                break;

            case State.Attacking:
                HandleAttackingMovement();
                break;

            case State.Blocking:
                MoveHorizontal(_blockSpeed, horizontalTowardsTarget);
                break;

            case State.Deflecting:
                break;

            case State.Staggered:
                break;

            case State.Dead:
                break;
        }
    }
   
    private void MoveHorizontal(float speed, Vector2 targetDir)
    {
        _movingDirection = targetDir;
        Vector3 newPos = transform.position + (new Vector3(targetDir.x, 0f, targetDir.y).normalized * (speed * Time.deltaTime));
        transform.position = new Vector3(transform.position.x + targetDir.x, transform.position.y, speed);
    }
    private void MoveVertical(float yDiff)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + yDiff, transform.position.z);
    }
    private void HandleAttackingMovement()
    {
        switch (CurrentAttackType)
        {
            case AttackType.None:
                break;
            case AttackType.Light:
                break;
            case AttackType.Heavy:
                break;
        }
    }





    private void HandleState()
    {
        switch (MyState)
        {
            case State.ChargingAttack:
                HanleChargingAttack();
                break;

            case State.Attacking:
                HandleAttacking();
                break;

            case State.Blocking:
                if (_isInDeflectWindow)
                {
                    _deflectCountdown -= Time.deltaTime;
                    if (_deflectCountdown < 0f)
                        _isInDeflectWindow = false;
                }
                ComboTimerTick();
                break;

            case State.Deflecting:
                if (_deflectCountdown < 0f)
                {
                    SwitchState(State.Free);
                    return;
                }
                ComboTimerTick();
                break;

            case State.Staggered:
                _staggerCountdown -= Time.deltaTime;
                if (_staggerCountdown < 0f)
                {
                    SwitchState(State.Free);
                    break;
                }
                break;

            case State.Falling:
                CheckGrounded();
                ComboTimerTick();
                break;

            default:
                ComboTimerTick();
                break;
        }
    }





    // Attacking
    private void ComboTimerTick()
    {
        _comboTimer += Time.deltaTime;
        if (_comboTimer > _comboTimerDuration)
        {
            _currentCombo.Clear();
        }
    }
    private void StartAttack(AttackType attack)
    {
        switch (attack)
        {
            case AttackType.None:
                break;
            case AttackType.Light:
                break;
            case AttackType.Heavy:
                break;
        }
    }
    private void EndAttack(bool wasCanceled = false)
    {
        if (wasCanceled)
            _currentCombo.Clear();
        _comboTimer = 0f;
        _attacks.EndAttack();
        SwitchState(State.Free);
    }

    private void HanleChargingAttack()
    {
        _chargingCountdown -= Time.deltaTime;
        if (_chargingCountdown < 0f)
        {
            switch (_queuedAtk)
            {
                case AttackType.Light:
                    _stamina.SpendStamina(_lightAtkStamina);
                    StartAttack(AttackType.Light);
                    break;
                case AttackType.Heavy:
                    _stamina.SpendStamina(_heavyAtkStamina);
                    StartAttack(AttackType.Heavy);
                    break;

                default:
                    Debug.Log("No Queued attack on ChargingAtk, defaulting to Light");
                    _stamina.SpendStamina(_lightAtkStamina);
                    StartAttack(AttackType.Light);
                    break;
            }
        }
    }
    private void HandleAttacking()
    {
        float progress = 0f;
        switch (CurrentAttackType)
        {
            case AttackType.None:
                {
                    Debug.Log("AttackType None in HandleAttacking, Ending Attack");
                    EndAttack();
                    break;
                }
            case AttackType.Light:
                _lightAtkCountdown -= Time.deltaTime;
                if (_lightAtkCountdown < 0f)
                {
                    EndAttack();
                    return;
                }
                progress = 1f - (_lightAtkCountdown / _lightAtkDuration);
                _attacks.AttackingStep(progress, _lightAtkIndex);
                break;
            case AttackType.Heavy:
                _heavyAtkCountdown -= Time.deltaTime;
                if (_heavyAtkCountdown < 0f)
                {
                    EndAttack();
                    return;
                }
                progress = 1f - (_heavyAtkCountdown / _heavyAtkDuration);
                _attacks.AttackingStep(progress);
                break;
        }
    }



    // Blocking
    private void StartBlocking()
    {
        _deflectCountdown = _deflectDuration;
        _isInDeflectWindow = true;
        SwitchState(State.Blocking);
    }
    public bool IsBlockingAttack(Vector3 hitPos, float force)
    {
        if (MyState == State.Blocking)
        {
            float dot = Vector3.Dot(transform.forward, (hitPos - transform.position).normalized);
            Debug.Log("Check if DOT is correct: " + dot);
            if (dot < 0)
                return true;
        }

        return false;
    }
    public bool IsDeflectingAttack(Vector3 hitPos, float force)
    {
        if (IsBlockingAttack(hitPos, force))
            if (_isInDeflectWindow)
                return true;
        return false;
    }

}
