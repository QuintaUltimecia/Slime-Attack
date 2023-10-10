using UnityEngine;

public class Menu : MonoBehaviour
{
    [field: SerializeField]
    public Transform PlayerPoint { get; private set; }

    [field: SerializeField]
    public Transform CameraPoint { get; private set; }
}
