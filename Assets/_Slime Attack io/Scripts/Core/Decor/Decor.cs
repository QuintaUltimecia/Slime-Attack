using UnityEngine;

public class Decor : BaseBehaviour
{
    [field: SerializeField]
    public DecorFeaturesSO Features { get; protected set; }

    [SerializeField]
    protected AnimationCurve _animationCurve;

    protected float _speed = 4f;
    protected float _y;
    protected float _heigth = 20f;

    [field: SerializeField]
    public bool IsAbsorbed { get; protected set; } = false;

    public Transform Transform { get; private set; }

    protected Transform _target;
    protected Collider _collder;

    public System.Action OnBeforeAbsorbe;
    public System.Action OnAfterAbsorbe;

    //protected System.Action<float> _callback;
    protected Transform _lastParent;

    public override void OnEnable() { }
    public override void OnDisable() { }

    public virtual void Restart()
    {
        IsAbsorbed = false;
        _collder.enabled = true;
    }

    public void Absorbe(Transform target, System.Action<float> callback)
    {
        if (IsAbsorbed == true)
            return;

        IsAbsorbed = true;

        _updates.Add(this);
        _target = target;
        //_callback = callback;
        callback.Invoke(Features.DeformPoint);
        _collder.enabled = false;
        Transform.SetParent(_target);

        OnBeforeAbsorbe?.Invoke();
    }

    public override void OnTick()
    {
        if (_target == null)
            return;

        if (IsAbsorbed == true)
        {
            _y += _speed * Time.deltaTime;

            Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, new Vector3(0, _animationCurve.Evaluate(_y) * _target.localScale.y * _heigth, 0), _speed * Time.deltaTime);
            Transform.localScale = Vector3.MoveTowards(Transform.localScale, Vector3.zero, (1f / _target.localScale.y) * Time.deltaTime);

            if (Transform.localScale == Vector3.zero)
            {
                Transform.localScale = Vector3.zero;

                _target = null;

                //_callback?.Invoke(Features.DeformPoint);
                //_callback = null;

                _y = 0;
                OnAfterAbsorbe?.Invoke();
                _updates.Remove(this);
            }
        }
    }

    public void DisableAnimation()
    {
        _updates.Remove(this);
    }

    public void SetPointForDeform(float value)
    {
        Features = (DecorFeaturesSO)ScriptableObject.CreateInstance(nameof(DecorFeaturesSO));
        Features.PointForDeform = value;
    }

    public void ResetTransform()
    {
        Transform.SetParent(_lastParent);
        Transform.localScale = Vector3.one;
    }

    protected virtual void Awake()
    {
        Transform = transform;
        _lastParent = Transform.parent;
        _collder = GetComponent<Collider>();
    }
}
