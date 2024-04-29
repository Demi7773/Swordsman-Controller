

using UnityEngine;

public interface IHittable
{

    public bool IsBlockingAttack(Vector3 hitPos, float force);
    public bool IsDeflectingAttack(Vector3 hitPos, float force);

    public void GetHit(Vector3 hitPos, float dmg, float force, IHittable hittable = null);

}
