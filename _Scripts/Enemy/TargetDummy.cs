using UnityEngine;

public class TargetDummy : MonoBehaviour, IHittable
{

    [SerializeField] private EnemyHPUIAdvanced _hpUI;

    public float CurrentHP = 1000f;
    public float MaxHP = 1000f;

    [SerializeField] private bool _isBlocking = false;
    [SerializeField] private bool _isDeflecting = false;






    private void Start()
    {
        Init();
    }
    private void Init()
    {
        _hpUI.Init();
        GetHealed(MaxHP);
    }


    public void GetHealed(float amount)
    {
        float newHP = CurrentHP + amount;
        CurrentHP = Mathf.Clamp(newHP, 0f, MaxHP);
        _hpUI.UpdateHealthBar(CurrentHP, MaxHP);
        //_hpUI.DisplayFloatingNumber(FloatingNumber.Context.Heal, transform.position, amount);
    }

    public void GetHit(Vector3 hitPos, float dmg, float force, IHittable hittable = null)
    {
        if (IsDeflectingAttack(hitPos, force))
        {
            Debug.Log("TargetDummy deflected attack");
            return;
        }
        if (IsBlockingAttack(hitPos, force))
        {
            dmg *= 0.5f;
            Debug.Log("TargetDummy blocked attack, damage reduced to" + dmg);
        }
        float newHP = CurrentHP - dmg;
        Debug.Log("TargetDummy got hit for " + dmg + ", newHP " + newHP);
        if (newHP < 0f)
            Die();

        CurrentHP = Mathf.Clamp(newHP, 0f, MaxHP);

        _hpUI.UpdateHealthBar(CurrentHP, MaxHP);
        //_hpUI.DisplayFloatingNumber(FloatingNumber.Context.Damage, hitPos, dmg);
    }

    public bool IsBlockingAttack(Vector3 hitPos, float force)
    {
        return _isBlocking;
    }

    public bool IsDeflectingAttack(Vector3 hitPos, float force)
    {
        return _isDeflecting;
    }

    private void Die()
    {
        Debug.Log("Dummy Ded");
    }
}
