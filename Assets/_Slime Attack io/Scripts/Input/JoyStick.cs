using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IEndDragHandler, IInput, IBeginDragHandler
{
    [SerializeField] 
    protected GameObject _analogStick;

    [SerializeField] 
    protected GameObject _borderStick;

    protected Transform _stickTransform;
    protected Transform _borderTransform;

    protected float _offset;

    protected float _maxRadius;
    protected float _stickPositionNormalize;

    protected Vector3 _inputPosition;
    protected PointerEventData _eventData;

    protected int _touchID;
    protected bool _isPressed;

    protected void Awake()
    {
        if (_analogStick == null) print("Analog stick needs to be installed.");
        if (_borderStick == null) print("Border stick needs to be installed.");

        _stickTransform = _analogStick.transform;
        _borderTransform = _borderStick.transform;
        _maxRadius = _borderStick.GetComponent<RectTransform>().sizeDelta.x / 2;
        _stickPositionNormalize = _borderStick.GetComponent<RectTransform>().sizeDelta.x / 100;
        _offset = _maxRadius / 100;
    }

    protected void OnDisable()
    {
        _stickTransform.localPosition = Vector3.zero;
        _isPressed = false;
    }

    protected Vector3 StickPosition()
    {
        if (_stickTransform == null || _borderTransform == null)
            return Vector3.zero;

        Vector3 newPosition;

        newPosition = _stickTransform.position - _borderTransform.position;
        newPosition = new Vector3(x: newPosition.x, y: 0, z: newPosition.y);

        return newPosition.normalized;
    }

    protected float GetStickDistance() =>
        Vector3.Distance(_stickTransform.position, _borderTransform.position);

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isPressed == false)
        {
            _touchID = eventData.pointerId;
            _isPressed = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_touchID >= 0 && _touchID < Input.touches.Length)
        {
            Vector2 offset = Input.touches[_touchID].position - (Vector2)_borderTransform.position;
            _stickTransform.position = (Vector2)_borderTransform.position + Vector2.ClampMagnitude(offset, _maxRadius);
        }
        else
        {
            Vector2 offset = eventData.position - (Vector2)_borderTransform.position;
            _stickTransform.position = (Vector2)_borderTransform.position + Vector2.ClampMagnitude(offset, _maxRadius);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_touchID == eventData.pointerId)
        {
            _stickTransform.localPosition = Vector3.zero;
            _isPressed = false;
        }
    }

    public Vector3 GetAxis()
    {
        return StickPosition();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
