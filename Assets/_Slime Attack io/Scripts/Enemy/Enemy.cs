using UnityEngine;

[RequireComponent(typeof(Absorber))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(Deformator))]
[RequireComponent(typeof(Decor))]
public sealed class Enemy : MonoBehaviour, IAbsorbingStay, IOnLeaderBoard
{
    public bool IsActive { get; private set; }

    public Absorber Absorber { get; private set; }
    public EnemyAI EnemyAI { get; private set; }
    public Deformator Deformator { get; private set; }
    public Decor Decor { get; private set; }

    [field: SerializeField]
    public PlayerAnimationController PlayerAnimationController { get; private set; }

    [field: SerializeField]
    public SlimeVFX SlimeVFX { get; private set; }

    [field: SerializeField]
    public NickName NickName { get; private set; }

    [field: SerializeField]
    public SlimeSize SlimeSize { get; private set; }

    [field: SerializeField]
    public Colorized Colorized { get; private set; }

    public System.Action OnRemove;

    private GameObject _gameObject;
    private Collider _collider;

    public void Init(DecorContainer decorContainer, Camera camera, Canvas canvas, GameFeaturesModule gameFeatures)
    {
        Absorber = GetComponent<Absorber>();
        EnemyAI = GetComponent<EnemyAI>();
        Deformator = GetComponent<Deformator>();
        Decor = GetComponent<Decor>();

        Absorber.Init(Deformator);
        Deformator.Init();
        EnemyAI.Init(decorContainer, Deformator, Decor, gameFeatures);

        EnemyNamesList enemyNamesList = new EnemyNamesList();

        NickName.Init(enemyNamesList.GetRandomName(), camera, canvas);
        SlimeSize.Init(camera, canvas);
        SlimeVFX.Init();

        EnemyAI.OnMove += (value) => PlayerAnimationController.Move(value);
        Absorber.OnAbsorbe += (DecorFeaturesSO) => Deformator.Deformate(DecorFeaturesSO);
        Decor.OnBeforeAbsorbe += () => { EnemyAI.Disable(); _collider.enabled = false; };
        Decor.OnAfterAbsorbe += () => Disable();
        Deformator.OnDeformate += (value) => { SlimeSize.UpdateText(value); Decor.SetPointForDeform(value); SlimeVFX.SetScale(transform.localScale.x); };
        Deformator.OnDeformate?.Invoke(transform.localScale.x);
        Colorized.OnColorChanged += (value) => { SlimeVFX.SetColor(value); };

        _gameObject = gameObject;
        _collider = GetComponent<Collider>();
    }

    public void Enable()
    {
        SlimeSize.Enable();
        NickName.Enable();
        Deformator.Restart();
        EnemyAI.Enable();
        EnemyAI.Restart();

        IsActive = true;

        _gameObject.SetActive(true);
        _collider.enabled = true;
    }

    public void Disable()
    {
        OnRemove?.Invoke();

        SlimeSize.Disable();
        NickName.Disable();
        Deformator.Restart();

        IsActive = false;

        _gameObject.SetActive(false);
    }

    public Decor GetDecor() =>
        Decor;

    public Deformator GetDeformator() =>
        Deformator;

    public int GetSize()
    {
        return SlimeSize.Value;
    }

    public string GetName()
    {
        return NickName.Name;
    }
}
