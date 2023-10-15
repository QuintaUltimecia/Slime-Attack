using UnityEngine;

public class Decor : BaseBehaviour
{
    [field: SerializeField]
    public DecorFeaturesSO Features { get; protected set; }

    protected float _speed = 6f;

    [field: SerializeField]
    public bool IsAbsorbed { get; protected set; } = false;

    public Transform Transform { get; private set; }

    protected Transform _target;
    protected Collider _collder;

    public System.Action OnBeforeAbsorbe;
    public System.Action OnAfterAbsorbe;

    protected System.Action<float> _callback;
    protected Vector3 _startPosition;
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
        _callback = callback;
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
            Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, Vector3.zero, _speed * Time.deltaTime);
            Transform.localScale = Vector3.MoveTowards(Transform.localScale, Vector3.zero, _speed / 2 * Time.deltaTime);

            if (Transform.localScale == Vector3.zero)
            {
                Transform.localScale = Vector3.zero;

                _target = null;

                _callback?.Invoke(Features.DeformPoint);
                _callback = null;

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
