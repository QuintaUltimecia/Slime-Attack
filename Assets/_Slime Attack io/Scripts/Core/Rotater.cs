using UnityEngine;

public class Rotater : BaseBehaviour
{
    private float _speed = 100f;

    private Transform _transform;

    private float _rotation;

    private void Awake()
    {
        _transform = transform;
    }

    public override void OnTick()
    {
        _rotation += _speed * Time.deltaTime;
        _transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
    }
}
