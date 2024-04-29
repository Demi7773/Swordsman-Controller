using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _staminaBar;




    private void OnEnable()
    {
        PlayerStamina.OnStaminaChange += OnStaminaChange;
        PlayerHP.OnHPChange += OnHPChange;
    }
    private void OnDisable()
    {
        PlayerStamina.OnStaminaChange -= OnStaminaChange;
        PlayerHP.OnHPChange -= OnHPChange;
    }

    private void OnStaminaChange(float changeAmount, PlayerStamina stamina)
    {
        _staminaBar.fillAmount = stamina.CurrentStamina / stamina.MaxStamina;
    }
    private void OnHPChange(float changeAmount, PlayerHP hp)
    {
        _hpBar.fillAmount = hp.CurrentHP / hp.MaxHP;
    }

}
