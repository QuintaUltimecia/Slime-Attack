using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BaseButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public System.Action OnClick;

    private Transform _transform;
    private GameObject _gameObject;

    [SerializeField]
    private UnityEvent _clickEvent;

    protected void Awake()
    {
        _transform = transform;
        _gameObject = gameObject;
    }

    protected void OnEnable()
    {
        OnClick += () => _clickEvent?.Invoke();
    }

    protected void OnDisable()
    {
        OnClick -= () => _clickEvent?.Invoke();
    }

    public void Enable()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _transform.localScale = Vector3.one;
    }
}
