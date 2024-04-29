using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private IController _controller;

    public float CurrentStamina = 50f;
    public float MaxStamina = 50f;
    public float CurrentStaminaRegen = 0f;

    public float BaseStaminaRegen = 3f;
    public float AttackStaminaRegen = 0.5f;
    public float BlockStaminaRegen = 1f;
    public float StaggeredStaminaRegen = 1f;




    public void RegenTick(SimpleEnemy.State state)
    {
        switch (state)
        {
            case SimpleEnemy.State.Attacking:
                CurrentStaminaRegen = AttackStaminaRegen;
                break;
            case SimpleEnemy.State.Blocking:
                CurrentStaminaRegen = BlockStaminaRegen;
                break;
            case SimpleEnemy.State.Staggered:
                CurrentStaminaRegen = StaggeredStaminaRegen;
                break;

            default: 
                CurrentStaminaRegen = BaseStaminaRegen;
                break;
        }

        GainStamina(CurrentStaminaRegen * Time.deltaTime);
    }
    public void GainStamina(float amount)
    {
        float newStam = CurrentStamina + amount;
        CurrentStamina = Mathf.Clamp(newStam, 0f, MaxStamina);
    }
    public void SpendStamina(float amount)
    {
        float newStam = CurrentStamina - amount;
        if (newStam < 0f)
            _controller.GetStaggered(transform.forward, amount);
        CurrentStamina = Mathf.Clamp(newStam, 0f, MaxStamina);
        //Debug.Log("Lost " +  amount + " stamina");
    }
}
