using UnityEngine;

public class LeaveAnimation : BaseBehaviour
{
    [SerializeField]
    private Direction _direction;

    private enum Direction
    {
        Up, Down, Left, Right
    }

    private RectTransform _rectTransform;
    private Vector2 _defaultPosition;

    private float _moveSpeed = 4000f;

    private void Play()
    {
        Vector2 newPos = _defaultPosition;

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

        _rectTransform.anchoredPosition = newPos;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultPosition = _rectTransform.anchoredPosition;
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
        if (_rectTransform.anchoredPosition != _defaultPosition) 
        {
            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _defaultPosition, _moveSpeed * Time.deltaTime);
        }
    }
}
