using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{

    [SerializeField] private PlayerController2 _controller;

    public float CurrentStamina = 50f;
    public float MaxStamina = 50f;
    public float CurrentStaminaRegen = 0f;

    public float BaseStaminaRegen = 3f;
    public float SprintStaminaRegen = 0f;
    public float DashStaminaRegen = 1f;
    public float JumpStaminaRegen = 1f;
    public float AttackStaminaRegen = 0.5f;
    public float BlockStaminaRegen = 1f;
    public float StaggeredStaminaRegen = 0f;



    public static event Action<float, PlayerStamina> OnStaminaChange;






    public void RegenTick(PlayerController2.State state)
    {
        switch (state)
        {
            case PlayerController2.State.Attacking:
                CurrentStaminaRegen = AttackStaminaRegen;
                break;

            case PlayerController2.State.Blocking:
                CurrentStaminaRegen = BlockStaminaRegen;
                break;
 
            case PlayerController2.State.Sprinting:
                CurrentStaminaRegen = SprintStaminaRegen;
                break;

            case PlayerController2.State.Dashing:
                CurrentStaminaRegen = DashStaminaRegen;
                break;
            case PlayerController2.State.Jumping:
                CurrentStaminaRegen = JumpStaminaRegen;
                break;
            case PlayerController2.State.Staggered:
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
        OnStaminaChange?.Invoke(amount, this);
    }
    public void SpendStamina(float amount)
    {
        float newStam = CurrentStamina - amount;
        if (newStam < 0f)
            _controller.GetStaggered(transform.forward, amount);
        CurrentStamina = Mathf.Clamp(newStam, 0f, MaxStamina);
        OnStaminaChange?.Invoke(amount, this);
    }

}
