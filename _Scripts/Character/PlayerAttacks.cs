using UnityEngine;

public class PlayerAttacks : AttacksController
{

    [Header("Dependencies")]
    [SerializeField] private LightAttack _lightAtk0;
    [SerializeField] private LightAttack _lightAtk1;
    [SerializeField] private LightAttack _lightAtk2;
    [SerializeField] private HeavyAttack _heavyAtk;
    [SerializeField] private SprintingAttack _sprintingAtk;
    [SerializeField] private JumpingAttack _jumpingAtk;

    [Header("State")]
    [SerializeField] private PlayerController2.AttackType _currentAtkType = PlayerController2.AttackType.None;





    public void Init(IController controller, IHittable hittable)
    {
        MyController = controller;
        MyHittable = hittable;
        _lightAtk0.Init(this);
        _lightAtk1.Init(this);
        _lightAtk2.Init(this);
        _heavyAtk.Init(this);
        _sprintingAtk.Init(this);
        _jumpingAtk.Init(this);
    }

    public void StartLightAttack(int index)
    {
        _currentAtkType = PlayerController2.AttackType.Light;
        switch(index)
        {
            default: 
                _lightAtk0.StartAttack();
                break;
            case 1: 
                _lightAtk1.StartAttack(); 
                break;
            case 2:
                _lightAtk2.StartAttack();
                break;
        }
        
    }
    public void StartHeavyAttack()
    {
        _currentAtkType = PlayerController2.AttackType.Heavy;
        _heavyAtk.StartAttack();
    }
    public void StartSprintingAttack() 
    {
        _currentAtkType = PlayerController2.AttackType.Sprinting;
        _sprintingAtk.StartAttack();
    }
    public void StartJumpingAttack()
    {
        _currentAtkType = PlayerController2.AttackType.Jumping;
        _jumpingAtk.StartAttack();
    }


    public void EndAttack()
    {
        _lightAtk0.EndAttack();
        _lightAtk1.EndAttack();
        _lightAtk2.EndAttack();
        _heavyAtk.EndAttack();
        _sprintingAtk.EndAttack();
        _jumpingAtk.EndAttack();

        _currentAtkType = PlayerController2.AttackType.None;
    }



    public void AttackingStep(float progress, int lightAtkIndex = 0)
    {
        switch (_currentAtkType)
        {
            case PlayerController2.AttackType.None:
                EndAttack();
                break;

            case PlayerController2.AttackType.Light:
                switch (lightAtkIndex)
                {
                    default:
                        _lightAtk0.AttackStep(progress);
                        break;
                    case 1:
                        _lightAtk1.AttackStep(progress);
                        break;
                    case 2:
                        _lightAtk2.AttackStep(progress);
                        break;
                }
                break;
            case PlayerController2.AttackType.Heavy:
                _heavyAtk.AttackStep(progress);
                break;

            case PlayerController2.AttackType.Sprinting:
                _sprintingAtk.AttackStep(progress);
                break;

            case PlayerController2.AttackType.Jumping:
                _jumpingAtk.AttackStep(progress);
                break;
        }
    }

}
