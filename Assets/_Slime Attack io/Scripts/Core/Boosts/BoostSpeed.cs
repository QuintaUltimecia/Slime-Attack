using UnityEngine;

public class BoostSpeed : BaseBoost
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IContainSpeed speed))
        {
            speed.GetMoveSpeed().StartMultiplier();
        }
    }
}
