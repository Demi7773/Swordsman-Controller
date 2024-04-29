using UnityEngine;

public interface IController
{

    public void GetBlocked(float force);
    public void GetDeflected(float force);
    public void GetStaggered(Vector3 startPos, float force);

}
