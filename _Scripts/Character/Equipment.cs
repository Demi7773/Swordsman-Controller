using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public Weapon CurrentWeapon;

    private float _swordLightStaminaCost = 5f;
    private float _swordHeavyStaminaCost = 10f;



    public enum Weapon
    {
        Sword
    }


    public float StaminaCostForLightAttack()
    {
        switch (CurrentWeapon)
        {
            case Weapon.Sword:
                return _swordLightStaminaCost;

            default:
                return _swordLightStaminaCost;
        }
    }

}
