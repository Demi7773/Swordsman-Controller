using System.Collections.Generic;
using UnityEngine;

public class SprintingAttack : MonoBehaviour, IAttack
{

    private AttacksController _attacks;
    private IController _controller;
    private IHittable _myHittable;

    [Header("Setup")]
    [SerializeField] private float _damage = 15f;
    [SerializeField] private float _force = 20f;
    [SerializeField] private Vector3 StartPos = Vector3.zero;
    [SerializeField] private Vector3 EndPos = Vector3.zero;
    [SerializeField] private Vector3 StartRot = Vector3.zero;
    [SerializeField] private Vector3 EndRot = Vector3.zero;
    [SerializeField] private AnimationCurve _movementCurve;

    [Header("Debug")]
    [SerializeField] private List<IHittable> _hits = new List<IHittable>();




    public void Init(AttacksController attacks)
    {
        _attacks = attacks;
        _controller = attacks.MyController;
        _myHittable = attacks.MyHittable;
        transform.localPosition = StartPos;
        transform.localRotation = Quaternion.Euler(StartRot);
        gameObject.SetActive(false);
    }

    public void StartAttack()
    {
        _hits.Clear();
        transform.localPosition = StartPos;
        transform.localRotation = Quaternion.Euler(StartRot);
        gameObject.SetActive(true);
    }
    public void EndAttack()
    {
        _hits.Clear();
        gameObject.SetActive(false);
        transform.localPosition = StartPos;
        transform.localRotation = Quaternion.Euler(StartRot);
    }
    public void AttackStep(float progress)
    {
        transform.localPosition = Vector3.Lerp(StartPos, EndPos, _movementCurve.Evaluate(progress));
        transform.localRotation = Quaternion.Euler(Vector3.Lerp(StartRot, EndRot, _movementCurve.Evaluate(progress)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHittable hittable))
        {
            if (_hits.Contains(hittable))
                return;

            Vector3 hitPos = other.ClosestPoint(transform.position);
            if (hittable.IsDeflectingAttack(hitPos, _force))
            {
                Debug.Log(_controller + " SprintingAtk got Deflected");
                _controller.GetDeflected(_force);
                EndAttack();
                return;
            }
            if (hittable.IsBlockingAttack(hitPos, _force))
            {
                Debug.Log(_controller + " SprintingAtk got Blocked, forcing through");
            }
            _hits.Add(hittable);
            hittable.GetHit(hitPos, _damage, _force, _myHittable);
            //Debug.Log(_controller + " SprintingAtk Hit " + hittable.ToString() + " at " + hitPos + " for " + _damage + "dmg with force of " + _force);
        }
    }

}
