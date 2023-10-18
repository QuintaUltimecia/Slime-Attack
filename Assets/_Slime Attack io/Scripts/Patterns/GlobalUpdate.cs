using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < BaseBehaviour._updates.Count; i++)
            BaseBehaviour._updates[i].Tick();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < BaseBehaviour._lateUpdates.Count; i++)
            BaseBehaviour._lateUpdates[i].LateTick();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < BaseBehaviour._fixedUpdates.Count; i++)
            BaseBehaviour._fixedUpdates[i].FixedTick();
    }
}
