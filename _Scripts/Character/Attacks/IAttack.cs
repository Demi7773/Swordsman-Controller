

public interface IAttack
{

    //public enum AttackType
    //{
    //    None,
    //    Light,
    //    Heavy,
    //    Sprinting,
    //    Jumping
    //}



    public void Init(AttacksController attacks);
    public void StartAttack();
    public void EndAttack();
    public void AttackStep(float progress);

}
