using UnityEngine;
using TMPro;

public class SlimeSize : BaseBehaviour
{
    public int Value { get; private set; }

    [field: SerializeField]
    private GameObject _sizeUIPrefab;

    private GameObject _sizeUI;
    private TextMeshProUGUI _text;
    private Transform _sizeUITransform;

    private Transform _transform;
    private GameObject _gameObject;

    private Camera _camera;

    private string _description = "Size: ";

    public void Init(Camera camera, Transform container)
    {
        _sizeUI = Instantiate(_sizeUIPrefab, container);
        _camera = camera;
        _transform = transform;

        _text = _sizeUI.GetComponent<TextMeshProUGUI>();
        _sizeUITransform = _sizeUI.transform;
    }

    public void Enable()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(true);
        _sizeUI.SetActive(true);
    }

    public void Disable()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(false);
        _sizeUI.SetActive(false);
    }

    public void UpdateText(float value)
    {
        Value = Mathf.RoundToInt(value * 100);
        _text.text = $"\n\n{_description}{Value}";
    }

    public override void OnLateTick()
    {
        _sizeUITransform.position = _camera.WorldToScreenPoint(_transform.position);
    }

    public override void OnEnable()
    {
        _lateUpdates.Add(this);
    }

    public override void OnDisable()
    {
        _lateUpdates.Add(this);
    }
}
