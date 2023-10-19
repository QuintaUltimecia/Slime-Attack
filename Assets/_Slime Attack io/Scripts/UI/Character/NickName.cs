using TMPro;
using UnityEngine;

public class NickName : BaseBehaviour
{
    public string Name { get; private set; }

    [field: SerializeField]
    private GameObject _nickUIPrefab;

    private GameObject _nickUI;
    private TextMeshProUGUI _text;
    private Transform _nickUITransform;

    private Transform _transform;
    private GameObject _gameObject;

    private Camera _camera;

    public void Init(string name, Camera camera, Transform container)
    {
        _nickUI = Instantiate(_nickUIPrefab, container);
        _camera = camera;
        _transform = transform;
        _gameObject = gameObject;

        _text = _nickUI.GetComponent<TextMeshProUGUI>();
        _nickUITransform = _nickUI.transform;

        Name = name;
        _text.text = $"{Name}";
    }

    public override void OnTick()
    {
        _nickUITransform.position = _camera.WorldToScreenPoint(_transform.position);
    }

    public void Enable()
    {
        if (_nickUI != null)
            _nickUI.SetActive(true);

        _gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (_nickUI != null)
            _nickUI.SetActive(false);

        _gameObject.SetActive(false);
    }
}
