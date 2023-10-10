using UnityEngine;

public class Lightning : MonoBehaviour
{
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetMenuRotation()
    {
        _transform.rotation = Quaternion.Euler(35, -180, 0);
    }

    public void SetGameRotation()
    {
        _transform.rotation = Quaternion.Euler(89, -180, 0);
    }
}
