using UnityEngine;

public interface IInput
{
    public Vector3 GetAxis();

    public void Enable();
    public void Disable();
}
