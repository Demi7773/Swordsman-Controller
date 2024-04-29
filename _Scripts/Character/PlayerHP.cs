using System;
using UnityEngine;

public class PlayerHP: MonoBehaviour
{

    private PlayerController2 _controller;

    public float CurrentHP = 50f;
    public float MaxHP = 50f;

    public static event Action<float, PlayerHP> OnHPChange;




    public void Init(float currentHP, float maxHP, PlayerController2 controller)
    {
        _controller = controller;
        MaxHP = maxHP;
        CurrentHP = currentHP;
        OnHPChange?.Invoke(currentHP, this);
    }


    public void LoseHP(float amount)
    {
        float newHP = CurrentHP - amount;
        if (newHP < 0.1f)
            Die();
        CurrentHP = Mathf.Clamp(newHP, 0f, MaxHP);
        OnHPChange?.Invoke(-amount, this);
    }
    public void GainHP(float amount) 
    {
        float newHP = CurrentHP + amount;
        CurrentHP = Mathf.Clamp(newHP, 0f, MaxHP);
        OnHPChange?.Invoke(amount, this);
    }


    private void Die()
    {
        Debug.Log("Ded");
    }

}
