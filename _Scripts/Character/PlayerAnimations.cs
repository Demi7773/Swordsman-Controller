using UnityEngine;
using static PlayerController2;

public class PlayerAnimations : MonoBehaviour
{

    [Header("Setup")]
    [SerializeField] private Animator _animator;
    [SerializeField] private float _idleLoopDuration = 3f;
    [Space(10)]
    [Header("Debug")]
    [SerializeField] private float _idleTimer = 0f;
    [SerializeField] private IdleAnimations[] _idleAnims;
    [SerializeField] private int _currentIdleIndex;
    //[SerializeField] private bool _staggerBackwards = true;
    [SerializeField] private int _lastAnimHash = 0;

    public enum IdleAnimations
    {
        Idle1,
        Idle2
    }
    readonly int IDLE1 = Animator.StringToHash("Idle");
    readonly int IDLE2 = Animator.StringToHash("Idle2");

    readonly int CHARGEATK = Animator.StringToHash("ChargingAttack");
    readonly int LIGHTATK0 = Animator.StringToHash("LightAttack0");
    readonly int LIGHTATK1 = Animator.StringToHash("LightAttack1");
    readonly int LIGHTATK2 = Animator.StringToHash("LightAttack2");
    readonly int HEAVYATK = Animator.StringToHash("HeavyAttack");
    readonly int SPRINTATK = Animator.StringToHash("SprintingAttack");
    readonly int JUMPATK = Animator.StringToHash("JumpingAttack");

    readonly int BLOCK = Animator.StringToHash("Block");
    readonly int DEFLECT = Animator.StringToHash("Deflect");

    readonly int RUN = Animator.StringToHash("Run");
    readonly int SPRINT = Animator.StringToHash("Sprint");
    readonly int DASH = Animator.StringToHash("Dash");

    readonly int JUMP = Animator.StringToHash("Jumping");
    readonly int FALL = Animator.StringToHash("Falling");

    private int STAGGERBACK = Animator.StringToHash("StaggerBack");
    private int STAGGERFWD = Animator.StringToHash("StaggerForward");





    public void HandleAnimator(PlayerController2.State state, bool isMoving, bool staggerBackwards = true, PlayerController2.AttackType attack = PlayerController2.AttackType.None, int lightAtkIndex = 0)
    {
        switch (state)
        {
            case State.Free:
                if (!isMoving)
                {
                    _idleTimer += Time.deltaTime;
                    if (_idleTimer > _idleLoopDuration)
                    {
                        _currentIdleIndex = Random.Range(0, _idleAnims.Length);
                        _idleTimer = 0f;
                    }
                    IdleAnimations currentAnim = _idleAnims[_currentIdleIndex];
                    switch (currentAnim)
                    {
                        case IdleAnimations.Idle1:
                            if (_lastAnimHash != IDLE1)
                            {
                                _lastAnimHash = IDLE1;
                                _animator.CrossFade(IDLE1, 0.2f);
                            }
                            break;
                        case IdleAnimations.Idle2:
                            if (_lastAnimHash != IDLE2)
                            {
                                _lastAnimHash = IDLE2;
                                _animator.CrossFade(IDLE2, 0.2f);
                            }
                            break;
                    }
                    break;
                }
                if (_lastAnimHash != RUN)
                {
                    _lastAnimHash = RUN;
                    _animator.CrossFade(RUN, 0.1f);
                }
                break;

            case State.ChargingAtk:
                if (_lastAnimHash != CHARGEATK)
                {
                    _lastAnimHash = CHARGEATK;
                    _animator.CrossFade(CHARGEATK, 0.1f);
                }
                break;

            case State.Attacking:
                SetAttackAnimations(attack, lightAtkIndex);
                break;

            case State.Blocking:
                if (_lastAnimHash != BLOCK)
                {
                    _lastAnimHash = BLOCK;
                    _animator.CrossFade(BLOCK, 0.1f);
                }
                break;

            case State.Deflecting:
                if (_lastAnimHash != DEFLECT)
                {
                    _lastAnimHash = DEFLECT;
                    _animator.CrossFade(DEFLECT, 0.1f);
                }
                break;

            case State.Sprinting:
                if (_lastAnimHash != SPRINT)
                {
                    _lastAnimHash = SPRINT;
                    _animator.CrossFade(SPRINT, 0.1f);
                }
                break;

            case State.Dashing:
                if (_lastAnimHash != DASH)
                {
                    _lastAnimHash = DASH;
                    _animator.CrossFade(DASH, 0.1f);
                }
                break;

            case State.Jumping:
                if (_lastAnimHash != JUMP)
                {
                    _lastAnimHash = JUMP;
                    _animator.CrossFade(JUMP, 0.1f);
                }
                break;

            case State.Falling:
                if (_lastAnimHash != FALL)
                {
                    _lastAnimHash = FALL;
                    _animator.CrossFade(FALL, 0.3f);
                }
                break;

            case State.Staggered:
                if (_lastAnimHash != STAGGERBACK && _lastAnimHash != STAGGERFWD)
                {
                    if (staggerBackwards)
                    {
                        _lastAnimHash = STAGGERBACK;
                        _animator.CrossFade(STAGGERBACK, 0.1f);
                        return;
                    }
                    _lastAnimHash = STAGGERFWD;
                    _animator.CrossFade(STAGGERFWD, 0.1f);
                }
                break;
        }
    }
    private void SetAttackAnimations(PlayerController2.AttackType attack, int lightAtkIndex)
    {
        switch (attack)
        {
            case AttackType.Light:
                switch (lightAtkIndex)
                {
                    default:
                        if (_lastAnimHash != LIGHTATK0)
                        {
                            _lastAnimHash = LIGHTATK0;
                            _animator.CrossFade(LIGHTATK0, 0.05f);
                        }
                        break;

                    case 1:
                        if (_lastAnimHash != LIGHTATK1)
                        {
                            _lastAnimHash = LIGHTATK1;
                            _animator.CrossFade(LIGHTATK1, 0.05f);
                        }
                        break;

                    case 2:
                        if (_lastAnimHash != LIGHTATK2)
                        {
                            _lastAnimHash = LIGHTATK2;
                            _animator.CrossFade(LIGHTATK2, 0.05f);
                        }
                        break;
                }
                
                break;
            case AttackType.Heavy:
                if (_lastAnimHash != HEAVYATK)
                {
                    _lastAnimHash = HEAVYATK;
                    _animator.CrossFade(HEAVYATK, 0.05f);
                }
                break;
            case AttackType.Sprinting:
                if (_lastAnimHash != SPRINTATK)
                {
                    _lastAnimHash = SPRINTATK;
                    _animator.CrossFade(SPRINTATK, 0.05f);
                }
                break;
            case AttackType.Jumping:
                if (_lastAnimHash != JUMPATK)
                {
                    _lastAnimHash = JUMPATK;
                    _animator.CrossFade(JUMPATK, 0.05f);
                }
                break;
        }
    }

}
