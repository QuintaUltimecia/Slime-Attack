using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class EnemyAI : BaseBehaviour, IInput
{
    [field: SerializeField]
    public SpeedParticle SpeedParticle { get; private set; }

    [SerializeField]
    private Decor _target;

    private DecorContainer _decorContainer;

    private Transform _transform;
    private Deformator _deformator;
    private Movement _movement;
    private Decor _origin;

    private Coroutine _timerRoutine;
    private bool _isActive = false;

    public System.Action<bool> OnMove;

    public void Init(DecorContainer decorContainer, Deformator deformator, Decor decor, GameFeaturesModule gameFeatures)
    {
        _decorContainer = decorContainer;
        _deformator = deformator;
        _movement = GetComponent<Movement>();
        _origin = decor;

        _transform = transform;

        _movement.Init(this, gameFeatures.SlimeSpeed);
        SpeedParticle.Init(_movement);
        deformator.OnDeformate += (value) => SpeedParticle.SetScale(_transform.localScale.x);

        _target = _decorContainer.GetDecorWithDistance(_transform, _deformator.GetSize(), _origin);
    }

    public void Restart()
    {
        _transform.localPosition = Vector3.zero;
        _target = _decorContainer.GetDecorWithDistance(_transform, _deformator.GetSize(), _origin);
    }

    public void ResetPosition()
    {
        _transform.localPosition = Vector3.zero;
    }

    public void Enable()
    {
        _isActive = true;

        _movement.Enable();
    }

    public void Disable()
    {
        _isActive = false;

        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);

        _movement.Disable();
    }

    public Vector3 GetAxis()
    {
        if (_target == null)
            return Vector3.zero;
        else
        {
            Vector3 position = _target.Transform.position - _transform.position;

            if (Vector3.Distance(_target.Transform.position, _transform.position) < 0.1f)
            {
                return position;
            }
            else
            {
                return position.normalized;
            }
        }
    }

    public override void OnLateTick()
    {
        if (_isActive == false)
            return;

        if (_target != null && _target.IsAbsorbed == true || 
            _target != null && _deformator.GetSize() <= _target.Features.PointForDeform)
        {
            _target = _decorContainer.GetDecorWithDistance(_transform, _deformator.GetSize(), _origin);

            if (_target == null)
                return;

            if (_target.TryGetComponent(out Player player))
            {
                _timerRoutine = StartCoroutine(Timer(() =>
                {
                    _target = _decorContainer.GetDecorAfterPlayer(_transform, _deformator.GetSize(), _origin);
                }));
            }
            else if (_target.TryGetComponent(out Enemy enemy))
            {
                if (enemy.Deformator.GetSize() == _deformator.GetSize())
                {
                    _target = _decorContainer.GetDecorAfterPlayer(_transform, _deformator.GetSize(), _origin);
                }
            }
        }
    }

    public override void OnEnable()
    {
        _lateUpdates.Add(this);
    }

    public override void OnDisable()
    {
        _lateUpdates.Remove(this);
    }

    private IEnumerator Timer(System.Action callBack)
    {
        yield return new WaitForSeconds(3f);
        callBack?.Invoke();
    }
}
