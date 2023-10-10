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

    protected GameObject _gameObject;

    public override void OnEnable()
    {
        base.OnEnable();

        OnAfterAbsorbe += () => StartCoroutine(RefreshRoutine());
    }

    public override void OnDisable()
    {
        base.OnDisable();

        OnAfterAbsorbe -= () => StartCoroutine(RefreshRoutine());
    }

    public override void Restart()
    {
        ResetTransform();

        if (AlphaSetter.IsActive == true)
            AlphaSetter.SetAlpha(false);

        if (GameState.CheckState<PlayState>())
            RestoreAnimation.Play(() => base.Restart());
        else
        {
            base.Restart();
        }

        _meshRenderer.enabled = true;
    }

    public void Init(GameFeaturesModule gameFeatures)
    {
        _gameObject = gameObject;
        _gameFeatures = gameFeatures;
        AlphaSetter = GetComponent<AlphaSetter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        RestoreAnimation = GetComponent<RestoreAnimation>();

        OnAfterAbsorbe += () => { StartCoroutine(RefreshRoutine()); _meshRenderer.enabled = false; };
    }

    public Decor GetDecor()
    {
        return this;
    }

    public AlphaSetter GetAlphaSetter()
    {
        return AlphaSetter;
    }

    protected IEnumerator RefreshRoutine()
    {
        yield return new WaitForSeconds(_gameFeatures.BuildingRefreshTime);
        Restart();
    }
}
