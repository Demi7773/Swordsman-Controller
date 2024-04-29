using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour, IHittable, IController
{

    [Header("Dependencies")]
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerHP _hp;
    [SerializeField] private PlayerStamina _stamina;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAttacks _attacks;
    [SerializeField] private PlayerAnimations _animations;
    [Space(10)]
    [Header("State")]
    [SerializeField] private State _state = State.Free;
    [SerializeField] private AttackType _currentAttackType;
    [Space(30)]
    [Header("Stamina Costs")]
    [SerializeField] private float _lightAtkStamina = 5f;
    [SerializeField] private float _heavyAtkStamina = 10f;
    [SerializeField] private float _sprintingAtkStamina = 10f;
    [SerializeField] private float _jumpingAtkStamina = 10f;
    [Space(10)]
    [SerializeField] private float _sprintStaminaCostPerSec = 5f;
    [SerializeField] private float _dashStaminaCost = 10f;
    [SerializeField] private float _jumpStaminaCost = 5f;
    [Space(10)]
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
    [SerializeField] private float _sprintingAtkDuration = 0.7f;
    [SerializeField] private float _jumpingAtkDuration = 0.5f;
    [Space(10)]
    [Header("Attack Timers")]
    [SerializeField] private bool _interruptedAtkCharge = false;
    [SerializeField] private float _chargingCountdown = 0f;
    [SerializeField] private float _lightAtkCountdown = 0f;
    [SerializeField] private float _heavyAtkCountdown = 0f;
    [SerializeField] private float _sprintingAtkCountdown = 0f;
    [SerializeField] private float _jumpingAtkCountdown = 0f;
    [Space(30)]
    [Header("Blocking")]
    [SerializeField] private float _staggerForceThreshold = 0.2f;
    [SerializeField] private float _deflectWindow = 0.2f;
    [SerializeField] private float _deflectDuration = 0.2f;
    [Space(10)]
    [SerializeField] private bool _isInDeflectWindow = false;
    [SerializeField] private float _deflectWindowCountdown = 0f;
    [SerializeField] private float _deflectCountdown = 0f;
    [Space(30)]
    [Header("Movement")]
    [SerializeField] private float _moveInputMinThreshold = 0.1f;
    [SerializeField] private Vector2 _inputMovement = Vector2.zero;
    [SerializeField] private Vector3 _movingDirection = Vector3.zero;
    [Header("Speed")]
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _jumpSpeed = 6f;
    [SerializeField] private float _fallingMoveSpeed = 6f;
    [SerializeField] private float _blockSpeed = 2f;
    [SerializeField] private float _chargingAtkSpeed = 2f;
    [SerializeField] private float _lightAtkSpeed = 2f;
    [Space(30)]
    [Header("Jump")]
    [SerializeField] private float _initialJumpVelocity = 10f;
    [SerializeField] private float _gravity = 10f;
    [SerializeField] private float _reducedGravity = 5f;
    [SerializeField] private float _holdToBoostJumpWindow = 0.5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [Space(10)]
    [SerializeField] private float _timeSinceJumpStarted = 0f;
    [SerializeField] private float _currentYVelocity = 0f;
    [Space(30)]
    [Header("Dash")]
    [SerializeField] private float _dashDuration = 0.5f;
    [SerializeField] private float _dashCountdown = 0f;
    [SerializeField] private float _dashDistance = 7f;
    [SerializeField] private Vector3 _dashStartPos = Vector3.zero;
    [SerializeField] private Vector3 _dashEndPos = Vector3.zero;
    [SerializeField] private AnimationCurve _dashCurve;
    [Space(30)]
    [Header("Stagger")]
    [SerializeField] private bool _staggerBackwards = true;
    [SerializeField] private float _staggerCountdown = 0f;




    public enum InputAction
    {
        None,
        Dash,
        Jump,
        Sprint,
        Attack,
        Block
    }
    public enum State
    {
        Free,
        ChargingAtk,
        Attacking,
        Blocking,
        Deflecting,
        Sprinting,
        Dashing,
        Jumping,
        Falling,
        Staggered
    }
    public enum AttackType
    {
        None,
        Light,
        Heavy,
        Sprinting,
        Jumping
    }




    private void Awake()
    {
        _attacks.Init(this, this);
    }
    private void Start()
    {
        _hp.Init(50f, 50f, this);
    }

    private void Update()
    {
        _stamina.RegenTick(_state);

        //HandleTimers();
        HandleInputs();
        HandleMovement();
        HandleState();
        bool isMoving = _inputMovement.magnitude > _moveInputMinThreshold;
        _animations.HandleAnimator(_state, isMoving, _staggerBackwards, _currentAttackType, _lightAtkIndex);
    }

    
    private void HandleTimers()
    {
        // moved to HandleState adequate cases

        //if (_state != State.Attacking)
        //{
        //    _comboTimer += Time.deltaTime;
        //    if (_comboTimer > _comboTimerDuration)
        //    {
        //        _currentCombo.Clear();
        //    }
        //}
    }




    // Inputs
    private void HandleInputs()
    {
        _inputMovement = _inputReader.MoveInput;

        switch (_state)
        {
            case State.Free:
                HandleFreeInputs();
                break;
            case State.ChargingAtk:
                HandleInputsWhileChargingAtk();
                break;
            case State.Attacking:
                //HandleInputsWhileAttacking(); // expand for combos ?
                break;
            case State.Blocking:
                HandleInputsWhileBlocking();
                break;
            case State.Deflecting:
                // short locked animation, add quick counterattack input ?
                break;
            case State.Sprinting:
                HandleInputsWhileSprinting();
                break;
            case State.Dashing:
                // locked animation. myb add interactions ?
                break;
            case State.Staggered:
                // locked animation
                break;
            case State.Jumping:
                HandleInputsWhileJumping();
                break;
            case State.Falling:
                HandleInputsWhileFalling();
                break;
        }
    }

    private void HandleFreeInputs()
    {
        InputAction action = _inputReader.DesiredAction;
        switch (action)
        {
            case InputAction.None:
                break;
            case InputAction.Dash:    
                TryDash();
                break;
            case InputAction.Jump:
                TryJump();
                break;
            case InputAction.Sprint:
                TrySprint();
                break;
            case InputAction.Attack:
                TryStandingAttack();
                break;
            case InputAction.Block:
                StartBlocking();
                break;
        }

        if (_state == State.Free)
        {
            if (_inputReader.IsPressingBlock)
            {
                StartBlocking();
                return;
            }
            if (_inputReader.IsPressingSprint)
            {
                TrySprint();
                return;
            }
            
        }
    }
    private void HandleInputsWhileChargingAtk()
    {
        InputAction action = _inputReader.DesiredAction;
        switch (action)
        {
            case InputAction.Dash:
                TryDash();
                break;
            case InputAction.Jump:
                TryJump();
                break;
            case InputAction.Sprint:
                TrySprint();
                break;
            case InputAction.Block:
                StartBlocking();
                break;

            default: break;
        }

        if (!_inputReader.IsPressingAttack)
            _interruptedAtkCharge = true;
    }
    private void HandleInputsWhileAttacking()
    {
        //switch (_currentAttackType)
        //{

        //}

        // add combos?
        // canceling?
    }
    private void HandleInputsWhileBlocking()
    {
        if (!_inputReader.IsPressingBlock)
            SwitchState(State.Free);

        InputAction action = _inputReader.DesiredAction;
        switch (action)
        {
            case InputAction.Dash:
                TryDash();
                break;
            case InputAction.Jump:
                TryJump();
                break;
            case InputAction.Sprint:
                TrySprint();
                break;
            case InputAction.Attack:
                TryStandingAttack();
                break;

            default : break;
                // refresh deflect window? tho should work as intended
        }
    }
    private void HandleInputsWhileSprinting()
    {
        if (!_inputReader.IsPressingSprint)
            SwitchState(State.Free);

        InputAction action = _inputReader.DesiredAction;
        switch (action)
        {
            case InputAction.Dash:
                TryDash();
                break;
            case InputAction.Jump:
                TryJump();
                break;
            case InputAction.Attack:
                TrySprintingAttack();
                break;
            case InputAction.Block:
                StartBlocking();
                break;

            default : break;
        }
    }
    private void HandleInputsWhileJumping()
    {
        float gravity = _gravity;
        if (_timeSinceJumpStarted < _holdToBoostJumpWindow)
        {
            _timeSinceJumpStarted += Time.deltaTime;
            if (_inputReader.IsPressingJump)
                gravity = _reducedGravity;
            _currentYVelocity -= gravity * Time.deltaTime;
            return;
        }
        _currentYVelocity -= gravity * Time.deltaTime;
        SwitchState(State.Falling);

        // add air dash, attack, deflect if perfect ? add above if in initial jump phase
        //InputAction action = _inputReader.DesiredAction;
        //switch (action)
        //{
        //    //case InputAction.Dash:
        //    //    TryDash();
        //    //    break;
        //    case InputAction.Attack:
        //        TryJumpingAttack();
        //        break;
        //    //case InputAction.Block:

        //    //    break;

        //    default: break;
        //}
    }
    private void HandleInputsWhileFalling()
    {
        _currentYVelocity -= _gravity * Time.deltaTime;
        if (_inputReader.DesiredAction == InputAction.Attack)
            TryJumpingAttack();


        // add air dash, attack, deflect if perfect ?
        //InputAction action = _inputReader.DesiredAction;
        //switch (action)
        //{
        //    //case InputAction.Dash:
        //    //    TryDash();
        //    //    break;
        //    case InputAction.Attack:
        //        TryJumpingAttack();
        //        break;
        //    //case InputAction.Block:

        //    //    break;

        //    default: break;
        //}
    }



    // Stamina checks -> SwitchState
    private void TryStandingAttack()
    {
        if (_stamina.CurrentStamina >= _lightAtkStamina)
        {
            if (_stamina.CurrentStamina < _heavyAtkStamina)
            {
                _interruptedAtkCharge = false;
                _chargingCountdown = _chargingAtkDuration * 0.5f;
                SwitchState(State.ChargingAtk);
                return;
            }
            _interruptedAtkCharge = false;
            _chargingCountdown = _chargingAtkDuration;
            SwitchState(State.ChargingAtk);
        }
    }
    private void TrySprintingAttack()
    {
        if (_stamina.CurrentStamina >= _sprintingAtkStamina)
        {
            _stamina.SpendStamina(_sprintingAtkStamina);
            StartAttack(AttackType.Sprinting);
        }
    }
    private void TryJumpingAttack()
    {
        if (_stamina.CurrentStamina >= _jumpingAtkStamina)
        {
            _stamina.SpendStamina(_jumpingAtkStamina);
            StartAttack(AttackType.Jumping);
        }
    }


    private void TryDash()
    {
        if (_stamina.CurrentStamina >= _dashStaminaCost)
        {
            _stamina.SpendStamina(_dashStaminaCost);
            _dashCountdown = _dashDuration;
            _dashStartPos = transform.position;
            Vector3 dir = _animator.transform.forward;
            if (_inputMovement.magnitude > _moveInputMinThreshold)
            {
                _inputMovement.Normalize();
                dir = new Vector3(_inputMovement.x, 0f, _inputMovement.y);
            }
            _dashEndPos = _dashStartPos + (dir * _dashDistance);
            _movingDirection = dir;
            SwitchState(State.Dashing);
        }
    }
    private void TryJump()
    {
        if (_stamina.CurrentStamina >= _jumpStaminaCost)
        {
            _stamina.SpendStamina(_jumpStaminaCost);
            _timeSinceJumpStarted = 0f;
            _currentYVelocity = _initialJumpVelocity;
            SwitchState(State.Jumping);
        }
    }
    private void TrySprint()
    {
        if (_stamina.CurrentStamina >= 3f)
        {
            SwitchState(State.Sprinting);
        }
    }
    private void StartBlocking()
    {
        _deflectCountdown = _deflectDuration;
        _isInDeflectWindow = true;
        SwitchState(State.Blocking);
    }



    private void SwitchState(State state)
    {
        _state = state;
    }





    // Movement
    private void HandleMovement()
    {
        switch (_state)
        {
            case State.Free:
                if (_inputMovement.magnitude > _moveInputMinThreshold)
                    Move(_runSpeed, _inputMovement);
                CheckGrounded();
                break;

            case State.ChargingAtk:
                Move(_chargingAtkSpeed, _inputMovement);
                break;

            case State.Attacking:
                HandleAttackingMovement(_currentAttackType);
                break;

            case State.Blocking:
                Move(_blockSpeed, _inputMovement);
                break;

            case State.Deflecting:
                break;

            case State.Sprinting:
                float staminaCostStep = _sprintStaminaCostPerSec * Time.deltaTime;
                if (_stamina.CurrentStamina < staminaCostStep)
                {
                    Move(_runSpeed, _inputMovement);
                    SwitchState(State.Free);
                    return;
                }
                _stamina.SpendStamina(staminaCostStep);
                Move(_sprintSpeed, _inputMovement);
                break;

            case State.Dashing:
                HandleDashing(_inputMovement);
                break;

            case State.Jumping:
                HandleJumpingMovement(_inputMovement);
                break;

            case State.Falling:
                HandleFallingMovement(_inputMovement);
                break;

            case State.Staggered:
                break;
        }
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
            SwitchState(State.Falling);
            _currentYVelocity = 0f;
            //transform.position += new Vector3(0f, _currentYVelocity * Time.deltaTime, 0f);
            Debug.Log("Groundcheck missed on MoveHorizontal, switching to Falling");
        }
    }

    private void HandleAttackingMovement(AttackType attack)
    {
        switch (attack)
        {
            case AttackType.None:
                Debug.Log("Attack type None in AttackingMovement");
                break;
            case AttackType.Light:
                _movingDirection = _animator.transform.forward;
                transform.position += _movingDirection * (_lightAtkSpeed * Time.deltaTime);
                break;
            case AttackType.Heavy:
                break;
            case AttackType.Sprinting:
                // change to dash system ?
                _movingDirection = _animator.transform.forward;
                transform.position += _movingDirection * (_sprintSpeed * Time.deltaTime);
                break;
            case AttackType.Jumping:
                transform.position += _movingDirection * (_jumpSpeed * Time.deltaTime);
                Ray ray = new Ray(_groundCheck.position, -transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _groundCheckDistance, _groundLayer))
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y + _groundCheckDistance + 0.5f, transform.position.z);
                    Debug.Log("Groundcheck hit at " + hit.point + " during JumpingAttack, adjusting Ypos but not switching state");
                    break;
                }
                _currentYVelocity -= _gravity * Time.deltaTime;
                transform.position += new Vector3(0f, _currentYVelocity * Time.deltaTime, 0f);
                break;
        }
    }
    private void HandleJumpingMovement(Vector2 inputDir)
    {
        Move(_jumpSpeed, inputDir);
        transform.position += new Vector3( 0f, _currentYVelocity * Time.deltaTime, 0f);
    }
    private void HandleFallingMovement(Vector2 inputDir)
    {
        Move(_fallingMoveSpeed, inputDir);
        transform.position += new Vector3(0f, _currentYVelocity * Time.deltaTime, 0f);
        Ray ray = new Ray(_groundCheck.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _groundCheckDistance, _groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + _groundCheckDistance + 0.5f, transform.position.z);
            SwitchState(State.Free);
            Debug.Log("Groundcheck hit at " + hit.point + ", landing and switching to Free");
        }
    }
    private void HandleDashing(Vector2 inputDir)
    {
        _dashCountdown -= Time.deltaTime;
        if (_dashCountdown < 0f)
        {
            Move(_runSpeed, inputDir);
            SwitchState(State.Free);
            return;
        }
        float progress = 1f - (_dashCountdown / _dashDuration);
        transform.position = Vector3.Lerp(_dashStartPos, _dashEndPos, progress);
    }
    private void Move(float speed, Vector2 inputDir)
    {
        _movingDirection = new Vector3(inputDir.x, 0f, inputDir.y);
        if (_movingDirection.magnitude < _moveInputMinThreshold)
            return;
        _movingDirection.Normalize();
        _animator.transform.rotation = Quaternion.RotateTowards(_animator.transform.rotation, Quaternion.LookRotation(_movingDirection), _rotationSpeed * Time.deltaTime);
        transform.position += _movingDirection * (speed * Time.deltaTime);
    }





    // Combat
    private void HandleChargingAtk()
    {
        _chargingCountdown -= Time.deltaTime;
        if (_chargingCountdown < 0f)
        {
            if (_interruptedAtkCharge || _stamina.CurrentStamina < _heavyAtkStamina)
            {
                _stamina.SpendStamina(_lightAtkStamina);
                StartAttack(AttackType.Light);
                return;
            }
            _stamina.SpendStamina(_heavyAtkStamina);
            StartAttack(AttackType.Heavy);
        }
    }

    private void StartAttack(AttackType attackType)
    {
        _currentAttackType = attackType;
        
        switch (attackType)
        { 
            case AttackType.None:
                Debug.Log("AttackType None in StartAttack");
                break;

            case AttackType.Light:

                _lightAtkCountdown = _lightAtkDuration;
                if (_currentCombo.Count > 1)
                {
                    if (_currentCombo[0] == AttackType.Light && _currentCombo[1] == AttackType.Light)
                    {
                        _lightAtkIndex = 2;
                        //_currentCombo.Clear();
                        _attacks.StartLightAttack(2);
                        Debug.Log("Combo step 2");
                        break;
                    }
                    _currentCombo.Clear();
                }
                if (_currentCombo.Count == 1)
                {
                    if (_currentCombo[0] == AttackType.Light)
                    {
                        _lightAtkIndex = 1;
                        _attacks.StartLightAttack(1);
                        Debug.Log("Combo step 1");
                        break;
                    }
                }
                _lightAtkIndex = 0;
                _attacks.StartLightAttack(0);
                Debug.Log("Combo step 0");
                break;

            case AttackType.Heavy:
                _heavyAtkCountdown = _heavyAtkDuration;
                _attacks.StartHeavyAttack();
                break;

            case AttackType.Sprinting:
                _sprintingAtkCountdown = _sprintingAtkDuration;
                _attacks.StartSprintingAttack();
                break;

            case AttackType.Jumping:
                _jumpingAtkCountdown = _jumpingAtkDuration;
                _attacks.StartJumpingAttack();
                break;
        }

        _currentCombo.Add(attackType);
        if (_currentCombo.Count > 2)
        {
            _currentCombo.Clear();
            Debug.Log("Combo Count exceeded, clearing");
        }

        SwitchState(State.Attacking);
    }

    private void HandleState()
    {
        switch (_state)
        {
            case State.ChargingAtk:
                HandleChargingAtk();
                break;
            case State.Attacking:
                HandleAttacking(_currentAttackType);
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
                    //MoveHorizontal(_runSpeed, _inputMovement);
                    SwitchState(State.Free);
                    break;
                }
                break;

            default:
                ComboTimerTick();
                break;
        }
    }
    private void ComboTimerTick()
    {
        _comboTimer += Time.deltaTime;
        if (_comboTimer > _comboTimerDuration)
        {
            _currentCombo.Clear();
        }
    }

    private void HandleAttacking(AttackType attackType)
    {
        float progress = 0f;
        switch (attackType)
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
            case AttackType.Sprinting:
                _sprintingAtkCountdown -= Time.deltaTime;
                if (_sprintingAtkCountdown < 0f)
                {
                    EndAttack();
                    return;
                }
                progress = 1f - (_sprintingAtkCountdown / _sprintingAtkDuration);
                _attacks.AttackingStep(progress);
                break;
            case AttackType.Jumping:
                _jumpingAtkCountdown -= Time.deltaTime;
                if (_jumpingAtkCountdown < 0f)
                {
                    EndAttack();
                    return;
                }
                progress = 1f - (_jumpingAtkCountdown / _jumpingAtkDuration);
                _attacks.AttackingStep(progress);
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



    // public methods for outside combat interactions
    // test if DOT check is correct
    public bool IsBlockingAttack(Vector3 hitPos, float force)
    {
        if (_state == State.Blocking)
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
    public void GetHit(Vector3 hitPos, float dmg, float force, IHittable hittable = null)
    {
        if (_state == State.Dashing)
        {
            Debug.Log("Dashing, attack dodged");
            return;
        }
        if (IsDeflectingAttack(hitPos, force))
        {
            _isInDeflectWindow = false;
            _deflectCountdown = _deflectDuration;
            SwitchState(State.Deflecting);
            Debug.Log("Attack deflected on GetHit");
            return;
        }
        if (IsBlockingAttack(hitPos, force))
        {
            dmg *= 0.5f;
            force *= 0.5f;
            Debug.Log("Attack blocked, dmg, force reduced to " + dmg + ", " + force);
        }
        _hp.LoseHP(dmg);
        GetStaggered(hitPos, force);
    }



    // IController
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

}