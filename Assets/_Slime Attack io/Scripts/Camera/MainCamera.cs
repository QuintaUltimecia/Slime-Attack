using UnityEngine;

[RequireComponent(typeof(CameraMovement))]
public class MainCamera : MonoBehaviour
{
    public CameraMovement CameraMovement { get; private set; }
    public Camera Camera { get; private set; }

    public void Init(Transform target)
    {
        CameraMovement = GetComponent<CameraMovement>();
        Camera = GetComponent<Camera>();

        CameraMovement.Init(target);
    }
}
