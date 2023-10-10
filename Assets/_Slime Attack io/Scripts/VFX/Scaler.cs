using UnityEngine;

public class Scaler : BaseBehaviour
{
    [SerializeField]
    private Wallet _wallet;

    [SerializeField]
    private float _speed = 2f;

    private float _scaleFactor = 1.2f;

    private bool _isActive = false;
    private bool _isRotate = false;

    private Transform _transform;

    private float _value;

    public void StartScale()
    {
        if (!GameState.CheckState<PlayState>())
            return;

        _isActive = true;
        _value = _transform.localScale.x;
    }

    public override void OnTick()
    {
        if (_isActive == false)
            return;

        if (_isRotate == false)
        {
            _value += _speed * Time.deltaTime;

            _transform.localScale = new Vector3(_value, _value, _value);

            if (_value >= _scaleFactor)
                _isRotate = true;
        }
        else
        {
            _value -= _speed * Time.deltaTime;
            _transform.localScale = new Vector3(_value, _value, _value);

            if (_value <= 1f)
            {
                _isRotate = false;
                _isActive = false;
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _wallet.OnAddWallet += () => StartScale();
    }

    public override void OnDisable()
    {
        base.OnEnable();
        _wallet.OnAddWallet += () => StartScale();
    }

    private void Awake()
    {
        _transform = transform;
    }
}
