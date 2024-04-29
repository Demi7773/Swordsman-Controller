using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUIAdvanced : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] private List<FloatingNumber> _floatingNumbers = new List<FloatingNumber>();
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Transform _healthBarBG;
    [Header("HPBarSettings")]
    [SerializeField] private Vector3 _hpBarOffset = new Vector3(0.0f, 2.0f, 3.0f);
    [Space(20)]
    [Header("Debug")]
    [SerializeField] private Camera _cam;
    [SerializeField] private int _lastDeployedIndex = 0;



    public void Init()
    {
        _cam = Camera.main;
        _lastDeployedIndex = 0;
        foreach (FloatingNumber number in _floatingNumbers)
            number.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        _healthBarImage.fillAmount = currentHP / maxHP;
    }

    public void DisplayFloatingNumber(FloatingNumber.Context context, Vector3 startPos, float amount)
    {
        _floatingNumbers[_lastDeployedIndex].Init(context, startPos, amount);
        _lastDeployedIndex++;
        if (_lastDeployedIndex > _floatingNumbers.Count - 1) 
            _lastDeployedIndex = 0;
    }



    private void Update()
    {
        //transform.LookAt(_cam.transform.position, Vector3.up);
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position, _cam.transform.up);
        _healthBarBG.position = _targetTransform.position + _hpBarOffset;
    }

}