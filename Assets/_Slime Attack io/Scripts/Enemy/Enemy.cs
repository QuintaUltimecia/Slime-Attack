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

    public void Init(DecorContainer decorContainer, Camera camera, MainCanvas canvas, GameFeaturesModule gameFeatures)
    {
        Absorber = GetComponent<Absorber>();
        EnemyAI = GetComponent<EnemyAI>();
        Deformator = GetComponent<Deformator>();
        Decor = GetComponent<Decor>();

        Absorber.Init(Deformator);
        Deformator.Init();
        EnemyAI.Init(decorContainer, Deformator, Decor, gameFeatures);

        EnemyNamesList enemyNamesList = new EnemyNamesList();

        NickName.Init(enemyNamesList.GetRandomName(), camera, canvas.GetPanel<GamePanel>().transform);
        SlimeSize.Init(camera, canvas.GetPanel<GamePanel>().transform);
        SlimeVFX.Init();

        EnemyAI.OnMove += (value) => 
            PlayerAnimationController.Move(value);

        Absorber.OnAbsorbe += (DecorFeaturesSO) =>
        {
            if (IsActive == true)
                Deformator.Deformate(DecorFeaturesSO);
        };

        Decor.OnBeforeAbsorbe += () => 
            Disable();

        Deformator.OnDeformate += (value) => 
        { 
            SlimeSize.UpdateText(value); 
            Decor.SetPointForDeform(value); 
            SlimeVFX.SetScale(transform.localScale.x); 
        };

        Deformator.OnDeformate?.Invoke(transform.localScale.x);

        Colorized.OnColorChanged += (value) => 
            SlimeVFX.SetColor(value);
    }

    public void Enable()
    {
        SlimeSize.Enable();
        NickName.Enable();
        Deformator.Restart();
        Decor.DisableAnimation();
        Decor.ResetTransform();
        EnemyAI.ResetPosition();
        Decor.Restart();
        EnemyAI.Enable();
        EnemyAI.Restart();
        Colorized.RandomRecolor();

        IsActive = true;
    }

    public void Disable()
    {
        EnemyAI.Disable();
        NickName.Disable();
        SlimeSize.Disable();
        SlimeVFX.Disable();

        IsActive = false;

        OnRemove?.Invoke();
    }

    public Decor GetDecor() => Decor;
    public Deformator GetDeformator() => Deformator;
    public int GetSize() => SlimeSize.Value;
    public string GetName() => NickName.Name;
}
