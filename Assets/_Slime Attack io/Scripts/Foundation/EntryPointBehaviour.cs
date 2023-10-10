using UnityEngine;

[RequireComponent(typeof(ResourcesLoader))]
public class EntryPointBehaviour : MonoBehaviour
{
    private void Awake() => 
        new EntryPoint(GetComponent<ResourcesLoader>());
}
