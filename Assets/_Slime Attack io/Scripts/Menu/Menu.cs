using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [field: SerializeField]
    public Transform PlayerPoint { get; private set; }

    [field: SerializeField]
    public Transform CameraPoint { get; private set; }
}
