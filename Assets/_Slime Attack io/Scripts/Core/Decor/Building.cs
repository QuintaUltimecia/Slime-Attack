using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveInt))]
[RequireComponent(typeof(AlphaSetter))]
[RequireComponent(typeof(RestoreAnimation))]
public class Building : Decor, IAbsorbingEnter
{
    public bool IsRefresh { get; protected set; } = true;
    public AlphaSetter AlphaSetter { get; protected set; }
    public RestoreAnimation RestoreAnimation { get; protected set; }

    protected GameFeaturesModule _gameFeatures;
    protected MeshRenderer _meshRenderer;
    protected Coroutine _refreshRoutine;

    protected GameObject _gameObject;
    protected Vector3 _startPosition;

    public override void OnEnable()
    {
        base.OnEnable();

        OnAfterAbsorbe += () =>
        {
            _meshRenderer.enabled = false;
            _refreshRoutine ??= StartCoroutine(RefreshRoutine());
        };
    }

    public override void OnDisable()
    {
        base.OnDisable();

        OnAfterAbsorbe -= () =>
        {
            _meshRenderer.enabled = false;
            _refreshRoutine ??= StartCoroutine(RefreshRoutine());
        };
    }

    public override void Restart()
    {
        if (_refreshRoutine != null)
        {
            StopCoroutine(_refreshRoutine);
            _refreshRoutine = null;
        }

        ResetTransform();
        Transform.position = _startPosition;

        if (AlphaSetter.IsActive == true)
            AlphaSetter.SetAlpha(false);

        if (GameState.CheckState<PlayState>())
            RestoreAnimation.Play(() => base.Restart());
        else
            base.Restart();

        _meshRenderer.enabled = true;
    }

    public void Init(GameFeaturesModule gameFeatures)
    {
        _gameObject = gameObject;
        _gameFeatures = gameFeatures;
        _meshRenderer = GetComponent<MeshRenderer>();
        AlphaSetter = GetComponent<AlphaSetter>();
        RestoreAnimation = GetComponent<RestoreAnimation>();
    }

    public Decor GetDecor() => this;
    public AlphaSetter GetAlphaSetter() => AlphaSetter;

    protected IEnumerator RefreshRoutine()
    {
        yield return new WaitForSeconds(_gameFeatures.BuildingRefreshTime);
        Restart();
        _refreshRoutine = null;
    }

    protected override void Awake()
    {
        base.Awake();
        _startPosition = Transform.position;
    }
}
