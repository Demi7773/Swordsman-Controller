using UnityEngine;

public class EnemyAttacks : AttacksController
{

    [Header("Dependencies")]
    [SerializeField] private LightAttack _lightAtk0;
    [SerializeField] private LightAttack _lightAtk1;
    [SerializeField] private LightAttack _lightAtk2;
    [SerializeField] private HeavyAttack _heavyAtk;
    [SerializeField] private SprintingAttack _sprintingAtk;
    [SerializeField] private JumpingAttack _jumpingAtk;

    [Header("State")]
    [SerializeField] private SimpleEnemy.AttackType _currentAtkType = SimpleEnemy.AttackType.None;





    public void Init(IController controller, IHittable hittable)
    {
        MyController = controller;
        MyHittable = hittable;
        _lightAtk0.Init(this);
        _lightAtk1.Init(this);
        _lightAtk2.Init(this);
        _heavyAtk.Init(this);
    }

    public void StartLightAttack(int index)
    {
        _currentAtkType = SimpleEnemy.AttackType.Light;
        switch (index)
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
        _currentAtkType = SimpleEnemy.AttackType.Heavy;
        _heavyAtk.StartAttack();
    }



    public /*override*/ void EndAttack()
    {
        _lightAtk0.EndAttack();
        _lightAtk1.EndAttack();
        _lightAtk2.EndAttack();
        _heavyAtk.EndAttack();

        _currentAtkType = SimpleEnemy.AttackType.None;
    }



    public void AttackingStep(float progress, int lightAtkIndex = 0)
    {
        switch (_currentAtkType)
        {
            case SimpleEnemy.AttackType.None:
                EndAttack();
                break;

            case SimpleEnemy.AttackType.Light:
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
            case SimpleEnemy.AttackType.Heavy:
                _heavyAtk.AttackStep(progress);
                break;
        }
    }

}
