using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < BaseBehaviour._updates.Count; i++)
            BaseBehaviour._updates[i].Tick();
    }
}
