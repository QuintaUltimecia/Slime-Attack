using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public bool IsMoveToTarget { get; set; } = true;

    [SerializeField]
    private float _offset = 9f;

    [SerializeField]
    private float _moveFade = 10f;

    private Transform _target;

    private Transform _transform;

    public void Init(Transform target)
    {
        _target = target;
        _transform = transform;
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public void SetRotation(Quaternion quaternion)
    {
        _transform.rotation = quaternion;
    }
    
    public void SetPositionToTarget()
    {
        float offset = _offset * _target.localScale.x;
        Vector3 direction = new Vector3(_target.position.x, _target.position.y + offset, _target.position.z) - _target.forward * (offset / 2);

        _transform.position = direction;
    }

    private void LateUpdate()
    {
        if (_target != null && IsMoveToTarget == true)
        {
            Move(_target.position);
        }
    }

    private void Move(Vector3 position)
    {
        float offset = _offset * _target.localScale.x;

        Vector3 direction = new Vector3(position.x, position.y + offset, position.z) - _target.forward * (offset / 2);
        Vector3 directionFade = Vector3.Lerp(_transform.position, direction, _moveFade * Time.deltaTime);

        _transform.position = directionFade;
    }
}
