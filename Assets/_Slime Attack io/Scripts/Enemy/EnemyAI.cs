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
    private bool _isMoveFade = false;

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
        _target = _decorContainer.GetDecorWithDistance(_transform, _deformator.GetSize(), _origin);
    }

    public void Enable() => _movement.Enable();
    public void Disable()
    {
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

            if (_isMoveFade == false)
            {
                return position.normalized;
            }
            else 
            {
                return position;
            }
        }
    }

    public override void OnTick()
    {
        if (_target != null && _target.IsAbsorbed == true || 
            _target != null && _deformator.GetSize() <= _target.Features.PointForDeform)
        {
            _target = _decorContainer.GetDecorWithDistance(_transform, _deformator.GetSize(), _origin);

            if (_target.TryGetComponent(out Player player))
            {
                _isMoveFade = true;

                _timerRoutine = StartCoroutine(Timer(() =>
                {
                    _target = _decorContainer.GetDecorAfterPlayer(_transform, _deformator.GetSize(), _origin);
                }));
            }
            else if (_target.TryGetComponent(out Enemy enemy))
            {
                _isMoveFade = true;

                if (enemy.Deformator.GetSize() == _deformator.GetSize())
                {
                    _target = _decorContainer.GetDecorAfterPlayer(_transform, _deformator.GetSize(), _origin);
                }
            }
            else
            {
                _isMoveFade = false;
            }
        }
    }
    
    private IEnumerator Timer(System.Action callBack)
    {
        yield return new WaitForSeconds(3f);
        callBack?.Invoke();
    }
}
