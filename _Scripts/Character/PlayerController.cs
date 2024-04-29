using UnityEngine;

public class PlayerController : MonoBehaviour/*, IHittable*/
{

    //[Header("Dependencies")]
    //[SerializeField] private Animator _animator;
    //[SerializeField] private PlayerHP _hp;
    //[SerializeField] private PlayerStamina _stamina;
    //[SerializeField] private InputReader _inputReader;
    //[Space(30)]
    //[Header("Attack")]
    //[SerializeField] private LightAttack _lightAtk0;
    //[SerializeField] private HeavyAttack _heavyAtk;
    //[SerializeField] private float _lightAtkDmg = 10f;
    //[SerializeField] private float _heavyAtkDmg = 30f;
    //[Space(10)]
    //[SerializeField] private float _lightAtkStamina = 5f;
    //[SerializeField] private float _heavyAtkStamina = 10f;
    //[Space(10)]
    //[SerializeField] private float _chargingAtkDuration = 0.2f;
    //[SerializeField] private float _lightAtkDuration = 0.5f;
    //[SerializeField] private float _heavyAtkDuration = 1.0f;
    //[Space(10)]
    //[SerializeField] private bool _interruptedAtkCharge = false;
    //[SerializeField] private float _chargingCountdown = 0f;
    //[SerializeField] private float _lightAtkCountdown = 0f;
    //[SerializeField] private float _heavyAtkCountdown = 0f;
    //[Space(30)]
    //[Header("Block")]
    //[SerializeField] private float _deflectWindow = 0.2f;
    //[SerializeField] private bool _isInDeflectWindow = false;
    //[SerializeField] private float _deflectDuration = 0.2f;
    //[SerializeField] private float _deflectWindowCountdown = 0f;
    //[SerializeField] private float _deflectCountdown = 0f;
    //[Space(30)]
    //[Header("Movement")]
    //[SerializeField] private float _runSpeed = 5f;
    //[SerializeField] private float _sprintSpeed = 8f;
    //[SerializeField] private float _jumpSpeed = 6f;
    //[SerializeField] private float _blockSpeed = 2f;
    //[Space(10)]
    //[SerializeField] private float _sprintStaminaCostPerSec = 5f;
    //[SerializeField] private float _dashStaminaCost = 10f;
    //[SerializeField] private float _jumpStaminaCost = 5f;
    //[Space(30)]
    //[Header("Dash")]
    //[SerializeField] private float _dashDuration = 0.5f;
    //[SerializeField] private float _dashCountdown = 0f;
    //[SerializeField] private float _dashDistance = 7f;
    //[SerializeField] private Vector3 _dashStartPos = Vector3.zero;
    //[SerializeField] private Vector3 _dashEndPos = Vector3.zero;
    //[SerializeField] private AnimationCurve _dashCurve;
    //[Space(10)]
    //[SerializeField] private float _staggerCountdown = 0f;
    //[Space(30)]
    //[Header("State")]
    //[SerializeField] private ControlState _controlState = ControlState.Free;
    //[SerializeField] private MovementType _movementType = MovementType.Idle;
    //[SerializeField] private CombatState _combatState = CombatState.Idle;

    //[Header("Animator Debug")]
    //[SerializeField] private bool _staggerBackwards = true;
    //[SerializeField] private int _lastAnimHash = 0;

    //readonly int IDLE1 = Animator.StringToHash("Idle");
    //readonly int CHARGEATK = Animator.StringToHash("ChargingAttack");
    //readonly int LIGHTATK0 = Animator.StringToHash("LightAttack");
    //readonly int HEAVYATK = Animator.StringToHash("HeavyAttack");
    //readonly int BLOCK = Animator.StringToHash("Block");
    //readonly int DEFLECT = Animator.StringToHash("Deflect");

    //readonly int RUN = Animator.StringToHash("Run");
    //readonly int SPRINT = Animator.StringToHash("Sprint");
    //readonly int DASH = Animator.StringToHash("Dash");

    //private int STAGGERBACK = Animator.StringToHash("StaggerBack");
    //private int STAGGERFWD = Animator.StringToHash("StaggerForward");


    //public enum ControlState
    //{
    //    Free,
    //    LockMovement,
    //    LockActions,
    //    LockBoth
    //}
    //public enum InputAction
    //{
    //    None,
    //    Dash,
    //    Jump,
    //    //Sprint,
    //    //Attack,
    //    //Block
    //}
    //public enum MovementType
    //{
    //    Idle,
    //    Run,
    //    Sprint,
    //    Dash,
    //    Jump,
    //    Attack,
    //    Block,
    //    Staggered
    //}
    //public enum CombatState
    //{
    //    Idle,
    //    ChargingAtk,
    //    LightAtk,
    //    HeavyAtk,
    //    Block,
    //    Deflect
    //}





    //private void Start()
    //{
    //    _hp.Init(50f, 50f, this);
    //}

    //private void Update()
    //{
    //    _stamina.RegenTick(_movementType);

    //    HandleInputs();
    //    HandleState();
    //    HandleAnimator();
    //}





    //// Inputs
    //private void HandleInputs()
    //{
        
    //    if (_combatState == CombatState.ChargingAtk)
    //        if (!_inputReader.IsPressingAttack)
    //            _interruptedAtkCharge = true;

    //    switch (_controlState)
    //    {
    //        case ControlState.Free:
    //            HandleInputActions();
    //            HandleMovement();
    //            break;
    //        case ControlState.LockMovement:
    //            HandleInputActions();
    //            break;
    //        case ControlState.LockActions:
    //            HandleMovement();
    //            break;
    //        case ControlState.LockBoth:
    //            if (_movementType == MovementType.Dash)
    //            {
    //                HandleDashing();
    //                break;
    //            }
    //            if (_movementType == MovementType.Staggered)
    //            {
    //                HandleStaggered();
    //            }
    //            break;

    //        default:
    //            break;
    //    }
    //}

    //private void HandleInputActions()
    //{
    //    InputAction action = _inputReader.DesiredAction;
    //    switch (action)
    //    {
    //        case InputAction.None:
    //            break;
    //        case InputAction.Dash:
    //            TryDash();
    //            break;
    //        case InputAction.Jump:
    //            TryJump();
    //            break;
    //    }

    //    if (_combatState != CombatState.ChargingAtk && _combatState != CombatState.LightAtk && _combatState != CombatState.HeavyAtk)
    //        if (_inputReader.IsPressingAttack)
    //        {
    //            TryStandingAttack();
    //            return;
    //        }       

    //    if (_combatState != CombatState.Block)
    //        if (_inputReader.IsPressingBlock)
    //        {
    //            StartBlocking();
    //            return;
    //        }

    //    if (_movementType != MovementType.Sprint)
    //        if (_inputReader.IsPressingSprint)
    //        {
    //            TrySprint();
    //            return;
    //        }
    //}

    //private void TryStandingAttack()
    //{
    //    if (_stamina.CurrentStamina >= _lightAtkStamina)
    //    {
    //        _interruptedAtkCharge = false;
    //        _chargingCountdown = _chargingAtkDuration;

    //        //_controlState = ControlState.LockBoth;
    //        SwitchCombatState(CombatState.ChargingAtk);
    //    }
    //}
    //private void TryDash()
    //{
    //    if (_stamina.CurrentStamina >= _dashStaminaCost)
    //    {
    //        StartDashing();
    //    }
    //}

    //private void StartDashing()
    //{
    //    _stamina.SpendStamina(_dashStaminaCost);
    //    _dashCountdown = _dashDuration;

    //    _dashStartPos = transform.position;
    //    Vector3 dir = Vector3.zero;
    //    Vector2 moveInput = _inputReader.MoveInput;
    //    if (moveInput.magnitude > 0f)
    //    {
    //        moveInput.Normalize();
    //        dir = new Vector3(moveInput.x, 0f, moveInput.y);
    //    }
    //    else
    //    {
    //        dir = _animator.transform.forward;
    //    }
    //    _dashEndPos = _dashStartPos + (dir * _dashDistance);

    //    SwitchMovementType(MovementType.Dash);
    //}

    //private void TryJump()
    //{
    //    if (_stamina.CurrentStamina >= _jumpStaminaCost)
    //    {
    //        _stamina.SpendStamina(_jumpStaminaCost);

    //        //_controlState = ControlState.Free;
    //        SwitchMovementType(MovementType.Jump);
    //    }
    //}
    //private void TrySprint()
    //{
    //    if (_stamina.CurrentStamina >= 3f)
    //    {
    //        //_controlState = ControlState.Free;
    //        SwitchMovementType(MovementType.Sprint);
    //    }
    //}
    //private void StartBlocking()
    //{
    //    _deflectCountdown = _deflectDuration;
    //    _isInDeflectWindow = true;

    //    //_controlState = ControlState.Free;
    //    SwitchCombatState(CombatState.Block);
    //    SwitchMovementType(MovementType.Block);
    //}





    //// Movement
    //private void SwitchMovementType(MovementType type)
    //{
    //    switch (type)
    //    {
    //        case MovementType.Idle:
    //            //if (_combatState == CombatState.Idle)
    //            //    _animator.CrossFade(IDLE1, 0.1f);
    //            _controlState = ControlState.Free;
    //            break;
    //        case MovementType.Run:
    //            //if (_combatState == CombatState.Idle)
    //            //    _animator.CrossFade(RUN, 0.1f);
    //            _controlState = ControlState.Free;
    //            break;
    //        case MovementType.Sprint:
    //            //_animator.CrossFade(SPRINT, 0.1f);
    //            _controlState = ControlState.Free;
    //            break;
    //        case MovementType.Dash:
    //            _controlState = ControlState.LockBoth;
    //            break;
    //        case MovementType.Jump:
    //            _controlState = ControlState.Free;
    //            break;
    //        case MovementType.Attack:
    //            _controlState = ControlState.LockBoth;
    //            break;
    //        case MovementType.Block:
    //            _controlState = ControlState.Free;
    //            break;
    //        case MovementType.Staggered:
    //            _controlState = ControlState.LockBoth;
    //            break;
    //    }
    //    _movementType = type;
    //}
    //private void HandleMovement()
    //{
    //    Vector2 inputDir = _inputReader.MoveInput;

    //    switch (_movementType)
    //    {
    //        case MovementType.Idle:
    //            if (inputDir.magnitude > 0f)
    //            {
    //                SwitchMovementType(MovementType.Run);
    //                HandleRun(inputDir);
    //            }
    //            break;

    //        case MovementType.Run:
    //            HandleRun(inputDir);
    //            break;

    //        case MovementType.Sprint:
    //            HandleSprintingMovement(inputDir);
    //            break;

    //        case MovementType.Dash:
    //            //HandleDashing();
    //            break;

    //        case MovementType.Jump:
    //            HandleJumpingMovement(inputDir);
    //            break;

    //        case MovementType.Block:
    //            MoveHorizontal(_blockSpeed, inputDir);
    //            break;

    //        case MovementType.Staggered:
    //            //HandleStaggered();
    //            break;
    //    }
    //}


    //private void HandleStaggered()
    //{
    //    _staggerCountdown -= Time.deltaTime;
    //    if (_staggerCountdown < 0f)
    //    {
    //        SwitchMovementType(MovementType.Idle);

    //    }
    //}
    //private void HandleRun(Vector2 inputDir)
    //{
    //    if (inputDir.magnitude < 0.1f)
    //    {
    //        SwitchMovementType(MovementType.Idle);
    //        return;
    //    }
    //    MoveHorizontal(_runSpeed, inputDir);
    //}
    //private void HandleJumpingMovement(Vector2 inputDir)
    //{
    //    // ychange + groundcheck
    //    MoveHorizontal(_jumpSpeed, inputDir);
    //}
    //private void HandleSprintingMovement(Vector2 inputDir)
    //{
    //    if (!_inputReader.IsPressingSprint)
    //    {
    //        SwitchMovementType(MovementType.Run);
    //        MoveHorizontal(_runSpeed, inputDir);
    //        return;
    //    }

    //    float staminaCostStep = _sprintStaminaCostPerSec * Time.deltaTime;
    //    if (_stamina.CurrentStamina < staminaCostStep)
    //    {
    //        SwitchMovementType(MovementType.Run);
    //        MoveHorizontal(_runSpeed, inputDir);
    //        return;
    //    }

    //    _stamina.SpendStamina(staminaCostStep);
    //    MoveHorizontal(_sprintSpeed, inputDir);
    //}
    //private void MoveHorizontal(float speed, Vector2 inputDir)
    //{
    //    Vector3 targetPos = transform.position + new Vector3(inputDir.x, 0f, inputDir.y);
    //    Vector3 dir = targetPos - transform.position;
    //    if (dir.magnitude < 0.1f)
    //        return;

    //    dir.Normalize();
    //    if (_combatState == CombatState.Idle)
    //        _animator.transform.rotation = Quaternion.LookRotation(dir);

    //    Vector3 newPos = transform.position + (dir * (speed * Time.deltaTime));
    //    transform.position = newPos;
    //}
    //private void HandleDashing()
    //{
    //    _dashCountdown -= Time.deltaTime;
    //    transform.position = DashStep();
    //    if (_dashCountdown < 0f)
    //        SwitchMovementType(MovementType.Idle);
    //}
    //private Vector3 DashStep()
    //{
    //    float progress = 1f - (_dashCountdown / _dashDuration);
    //    //Debug.Log("Dash progress: " + progress);
    //    return Vector3.Lerp(_dashStartPos, _dashEndPos, progress);
    //}





    //// Combat
    //private void SwitchCombatState(CombatState state)
    //{
    //    switch (state)
    //    {
    //        case CombatState.Idle:
    //            //_animator.CrossFade(IDLE1, 0.1f);
    //            _controlState = ControlState.Free;
    //            break;
    //        case CombatState.ChargingAtk:
    //            //_lightAtk0.StartAttack(_lightAtkDmg);
    //            _controlState = ControlState.LockBoth;
    //            break;
    //        case CombatState.LightAtk:
    //            _lightAtk0.StartAttack(_lightAtkDmg);
    //            _controlState = ControlState.LockBoth;
    //            break;
    //        case CombatState.HeavyAtk:
    //            _heavyAtk.StartAttack(_heavyAtkDmg);
    //            _controlState = ControlState.LockBoth;
    //            break;
    //        case CombatState.Block:
    //            //_animator.CrossFade(BLOCK, 0.05f);
    //            _controlState = ControlState.Free;
    //            break;
    //        case CombatState.Deflect:
    //            //_animator.CrossFade(DEFLECT, 0.05f);
    //            _controlState = ControlState.LockBoth;
    //            break;
    //    }

    //    _combatState = state;
    //}
    //private void HandleState()
    //{
    //    switch (_combatState)
    //    {
    //        case CombatState.Idle:

    //            break;
    //        case CombatState.ChargingAtk:
    //            HandleChargingAtk();
    //            break;
    //        case CombatState.LightAtk:
    //            HandleLightAtk();
    //            break;
    //        case CombatState.HeavyAtk:
    //            HandleHeavyAtk();
    //            break;
    //        case CombatState.Block:
    //            HandleBlock();
    //            break;
    //        case CombatState.Deflect:
    //            HandleDeflect();
    //            break;
    //    }
    //}


    //private void HandleChargingAtk()
    //{
    //    _chargingCountdown -= Time.deltaTime;
    //    if (_chargingCountdown < 0f)
    //    {
    //        if (_interruptedAtkCharge || _stamina.CurrentStamina < _heavyAtkStamina)
    //        {
    //            _stamina.SpendStamina(_lightAtkStamina);
    //            _lightAtkCountdown = _lightAtkDuration;
    //            SwitchCombatState(CombatState.LightAtk);
    //            return;
    //        }

    //        _stamina.SpendStamina(_heavyAtkStamina);
    //        _heavyAtkCountdown = _heavyAtkDuration;
    //        SwitchCombatState(CombatState.HeavyAtk);
    //    }
    //}
    //private void HandleLightAtk()
    //{
    //    _lightAtkCountdown -= Time.deltaTime;
    //    if (_lightAtkCountdown < 0f)
    //    {
    //        EndAttack();
    //        return;
    //    }


    //}
    //private void HandleHeavyAtk()
    //{
    //    _heavyAtkCountdown -= Time.deltaTime;
    //    if (_heavyAtkCountdown < 0f)
    //    {
    //        EndAttack();
    //        return;
    //    }


    //}
    //private void EndAttack()
    //{
    //    //_controlState = ControlState.Free;
    //    SwitchCombatState(CombatState.Idle);
    //    _lightAtk0.EndAttack();
    //    _heavyAtk.EndAttack();
    //    //SwitchMovementType(MovementType.Idle);
    //}



    //// add block mechanic
    //private void HandleBlock()
    //{
    //    if (_isInDeflectWindow)
    //    {
    //        _deflectCountdown -= Time.deltaTime;
    //        if (_deflectCountdown < 0f)
    //            _isInDeflectWindow = false;
    //    }

    //    if (!_inputReader.IsPressingBlock)
    //    {
    //        SwitchCombatState(CombatState.Idle);
    //        SwitchMovementType(MovementType.Idle);
    //    }

    //}
    //private void StartDeflecting()
    //{
    //    _isInDeflectWindow = false;
    //    _deflectCountdown = _deflectDuration;

    //    //_controlState = ControlState.LockBoth;
    //    //SwitchMovementType(MovementType.Run);
    //    SwitchCombatState(CombatState.Deflect);
    //}
    //private void HandleDeflect()
    //{
    //    if (_deflectCountdown < 0f)
    //    {
    //        //_controlState = ControlState.Free;
    //        SwitchMovementType(MovementType.Idle);
    //        SwitchCombatState(CombatState.Idle);
    //        return;
    //    }
    //}





    //// Animator
    //private void HandleAnimator()
    //{

    //    if (_movementType == MovementType.Staggered)
    //    {
    //        if (_lastAnimHash != STAGGERBACK && _lastAnimHash != STAGGERFWD)
    //        {
    //            if (_staggerBackwards)
    //            {
    //                _lastAnimHash = STAGGERBACK;
    //                _animator.CrossFade(STAGGERBACK, 0.1f);
    //                return;
    //            }

    //            _lastAnimHash = STAGGERFWD;
    //            _animator.CrossFade(STAGGERFWD, 0.1f);
    //        }
    //        return;
    //    }

    //    switch (_combatState)
    //    {
    //        case CombatState.Idle:
    //            MovementAnimations();
    //            break;
    //        case CombatState.ChargingAtk:
    //            if (_lastAnimHash != CHARGEATK)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + CHARGEATK + ", CHARGEATK Crossfade start");
    //                _lastAnimHash = CHARGEATK;
    //                _animator.CrossFade(CHARGEATK, 0.1f);
    //            }
    //            break;
    //        case CombatState.LightAtk:
    //            if (_lastAnimHash != LIGHTATK0)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + LIGHTATK0 + ", LIGHTATK0 Crossfade start");
    //                _lastAnimHash = LIGHTATK0;
    //                _animator.CrossFade(LIGHTATK0, 0.05f);
    //            }
    //            break;
    //        case CombatState.HeavyAtk:
    //            if (_lastAnimHash != HEAVYATK)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + HEAVYATK + ", HEAVYATK Crossfade start");
    //                _lastAnimHash = HEAVYATK;
    //                _animator.CrossFade(HEAVYATK, 0.05f);
    //            }
    //            break;
    //        case CombatState.Block:
    //            if (_lastAnimHash != BLOCK)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + BLOCK + ", BLOCK Crossfade start");
    //                _lastAnimHash = BLOCK;
    //                _animator.CrossFade(BLOCK, 0.1f);
    //            }
    //            break;
    //        case CombatState.Deflect:
    //            if (_lastAnimHash != DEFLECT)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + DEFLECT + ", DEFLECT Crossfade start");
    //                _lastAnimHash = DEFLECT;
    //                _animator.CrossFade(DEFLECT, 0.1f);
    //            }
    //            break;
    //    }      
    //}
    //private void MovementAnimations()
    //{
    //    switch (_movementType)
    //    {
    //        case MovementType.Idle:
    //            if (_lastAnimHash != IDLE1)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + IDLE1 + ", IDLE1 Crossfade start");
    //                _lastAnimHash = IDLE1;
    //                _animator.CrossFade(IDLE1, 0.1f);
    //            }
    //            break;
    //        case MovementType.Run:
    //            if (_lastAnimHash != RUN)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + RUN + ", RUN Crossfade start");
    //                _lastAnimHash = RUN;
    //                _animator.CrossFade(RUN, 0.1f);
    //            }
    //            break;
    //        case MovementType.Sprint:
    //            if (_lastAnimHash != SPRINT)
    //            {
    //                //Debug.Log(_lastAnimHash + " != " + SPRINT + ", SPRINT Crossfade start");
    //                _lastAnimHash = SPRINT;
    //                _animator.CrossFade(SPRINT, 0.1f);
    //            }
    //            break;
    //        case MovementType.Dash:
    //            if (_lastAnimHash != DASH)
    //            {
    //                _lastAnimHash = DASH;
    //                _animator.CrossFade(DASH, 0.1f);
    //            }
    //            break;
    //        case MovementType.Jump:
    //            //
    //            break;
    //        case MovementType.Attack:
    //            break;
    //        case MovementType.Block:

    //            break;
    //        case MovementType.Staggered:
    //            //
    //            break;
    //    }     
    //}




    //// test if DOT check is correct
    //public bool IsBlockingAttack(Vector3 hitPos, float force)
    //{
    //    if (_combatState == CombatState.Block)
    //    {
    //        float dot = Vector3.Dot(transform.forward, (hitPos - transform.position).normalized);
    //        Debug.Log("Check if DOT is correct: " + dot);
    //        if (dot < 0)
    //            return true;
    //    }
            
    //    return false;
    //}

    //public bool IsDeflectingAttack(Vector3 hitPos, float force)
    //{
    //    if (IsBlockingAttack(hitPos, force))
    //        if (_isInDeflectWindow)
    //        {
    //            StartDeflecting();
    //            return true;
    //        }
    //    return false;
    //}

    //public void GetHit(Vector3 hitPos, float dmg, float force, IHittable hittable = null)
    //{
    //    if (IsDeflectingAttack(hitPos, force))
    //    {
    //        Debug.Log("Attack deflected on GetHit");
    //        StartDeflecting();
    //        return;
    //    }    
    //    if (IsBlockingAttack(hitPos, force))
    //    {
    //        dmg *= 0.5f;
    //        Debug.Log("Attack blocked, dmg reduced to " + dmg);
    //    }

    //    _hp.LoseHP(dmg);
    //    GetStaggered(hitPos, force);
    //}




    //public void GetDeflected(float force)
    //{
    //    _stamina.SpendStamina(force);
    //    _staggerCountdown = force * 0.05f;
    //    _staggerBackwards = true;
    //    SwitchCombatState(CombatState.Idle);
    //    SwitchMovementType(MovementType.Staggered);
    //}
    //public void GetBlocked(float force)
    //{
    //    _stamina.SpendStamina(force);
    //}

    //public void GetStaggered(Vector3 startPos, float force)
    //{
    //    float dot = Vector3.Dot(transform.forward, (startPos - transform.position).normalized);
    //    if (dot < 0)
    //        _staggerBackwards = true;
    //    else
    //        _staggerBackwards = false;

    //    _staggerCountdown = force * 0.05f;

    //    _controlState = ControlState.LockBoth;
    //    SwitchCombatState(CombatState.Idle);
    //    SwitchMovementType(MovementType.Staggered);
    //}
}
