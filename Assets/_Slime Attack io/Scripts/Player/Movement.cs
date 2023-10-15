using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : BaseBehaviour, IContainSpeed
{
    [field: SerializeField]
    public Transform RotateTransform { get; private set; }
    public Transform Transform { get; private set; }

    private float _rotationFade = 12f;

    private MoveSpeed _moveSpeed;
    private Rigidbody _rigidbody;
    private IInput _input;
    private RigidbodyConstraints _defaultConstains;

    private bool _isMoving;

    public System.Action<bool> OnMove;

    public void Init(IInput input, float speed)
    {
        _input = input;
        Transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _defaultConstains = _rigidbody.constraints;
        _moveSpeed = new MoveSpeed(speed);
    }

    public void ResetRotate() =>
        RotateTransform.rotation = Quaternion.Euler(0, 0, 0);

    public void SetPosition(Vector3 position) =>
        Transform.position = position;

    public void Enable()
    {
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.constraints = _defaultConstains;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        _isMoving = true;
    }

    public void Disable()
    {
        _isMoving = false;

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        _moveSpeed.StopMultiplier();
        OnMove?.Invoke(false);
        RotateTransform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public MoveSpeed GetMoveSpeed() => _moveSpeed;

    public override void OnTick()
    {
        if (_input != null)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        if (_input.GetAxis() != Vector3.zero)
            OnMove?.Invoke(true);
        else    OnMove?.Invoke(false);

        if (_isMoving == false)
            return;

        Vector3 position = _input.GetAxis() * _moveSpeed.CurrentValue;
        _rigidbody.velocity = position;
    }

    private void Rotate()
    {
        if (_input.GetAxis() == Vector3.zero)
            return;

        Quaternion rotation = Quaternion.LookRotation(_input.GetAxis());
        rotation.x = 0;
        rotation.z = 0;
        Quaternion rotationFade = Quaternion.Lerp(RotateTransform.rotation, rotation, _rotationFade * Time.deltaTime);
        RotateTransform.rotation = rotationFade;
    }
}
