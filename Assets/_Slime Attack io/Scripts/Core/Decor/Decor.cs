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

        OnBeforeAbsorbe?.Invoke();
    }

    public override void OnTick()
    {
        if (_target == null)
            return;

        if (IsAbsorbed == true)
        {
            float distance = Vector3.Distance(Transform.position, _target.position) * _speed;

            Transform.position = Vector3.MoveTowards(Transform.position, _target.position, distance * Time.deltaTime);
            Transform.localScale = Vector3.MoveTowards(Transform.localScale, Vector3.zero, (distance / 2) * Time.deltaTime);

            if (Transform.localScale == Vector3.zero)
            {
                _target = null;
                _updates.Remove(this);

                _callback?.Invoke(Features.DeformPoint);
                _callback = null;

                OnAfterAbsorbe?.Invoke();
            }
        }
    }

    public void SetPointForDeform(float value)
    {
        Features = (DecorFeaturesSO)ScriptableObject.CreateInstance(nameof(DecorFeaturesSO));
        Features.PointForDeform = value;
    }

    public void ResetTransform()
    {
        Transform.localScale = Vector3.one;
        Transform.position = new Vector3(_startPosition.x, Transform.position.y, _startPosition.z);
    }

    protected void Awake()
    {
        Transform = transform;
        _startPosition = Transform.position;
        _collder = GetComponent<Collider>();
    }
}
