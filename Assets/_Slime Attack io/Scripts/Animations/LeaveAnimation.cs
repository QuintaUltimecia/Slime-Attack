using UnityEngine;

public class LeaveAnimation : BaseBehaviour
{
    [SerializeField]
    private Direction _direction;

    private enum Direction
    {
        Up, Down, Left, Right
    }

    private Transform _transform;
    private Vector3 _defaultPosition;

    private float _moveSpeed = 4000f;

    private void Play()
    {
        Vector3 newPos = _defaultPosition;

        switch (_direction)
        {
            case Direction.Up:
                newPos.y = 1000;
                break;
            case Direction.Down:
                newPos.y = -1000;
                break;
            case Direction.Left:
                newPos.x = -1000;
                break;
            case Direction.Right:
                newPos.x = -1000;
                break;
            default:
                newPos.y = -1000;
                break;
        }

        _transform.position = newPos;
    }

    private void Awake()
    {
        _transform = transform;
        _defaultPosition = _transform.position;
    }

    public override void OnEnable()
    {
        Play();

        _updates.Add(this);
    }

    public override void OnDisable()
    {
        _updates.Remove(this);
    }

    public override void OnTick()
    {
        if (_transform.position != _defaultPosition) 
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _defaultPosition, _moveSpeed * Time.deltaTime);
        }
    }
}
