using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Deformator))]
[RequireComponent(typeof(Absorber))]
[RequireComponent(typeof(Decor))]
[RequireComponent(typeof(SphereCollider))]
public sealed class Player : MonoBehaviour, IAbsorbingStay, IOnLeaderBoard
{
    public bool IsActive { get; private set; }

    [field: SerializeField]
    public Colorized Colorized { get; private set; }

    [field: SerializeField]
    public Accessories Accessories { get; private set; }

    [field: SerializeField]
    public SpeedParticle SpeedParticle { get; private set; }

    [field: SerializeField]
    public SlimeVFX SlimeVFX { get; private set; }

    [field: SerializeField]
    public SlimeSize SlimeSize { get; private set; }

    [field: SerializeField]
    public PlayerAnimationController PlayerAnimationController { get; private set; }

    public Movement Movement { get; private set; }
    public Deformator Deformator { get; private set; }
    public Absorber Absorber { get; private set; }
    public Decor Decor { get; private set; }
    public Wallet Wallet { get; private set; }

    public void Init(IInput input, Camera camera, MainCanvas mainCanvas, GameFeaturesModule gameFeatures)
    {
        Movement = GetComponent<Movement>();
        Deformator = GetComponent<Deformator>();
        Absorber = GetComponent<Absorber>();
        Decor = GetComponent<Decor>();

        Movement.Init(input, gameFeatures.SlimeSpeed);
        Absorber.Init(Deformator);
        SlimeSize.Init(camera, mainCanvas.GetInternalPanel<GamePanel>().transform);
        Deformator.Init();
        Accessories.Init();
        SpeedParticle.Init(Movement);
        SlimeVFX.Init();

        Movement.OnMove += (value) => 
            PlayerAnimationController.Move(value);

        Absorber.OnAbsorbe += (IDeformPointProvider) =>
        {
            if (IsActive == true)
                Deformator.Deformate(IDeformPointProvider);
        };

        Decor.OnBeforeAbsorbe += () => 
        {
            Disable();
        };

        Deformator.OnDeformate += (value) => { 
            SlimeSize.UpdateText(value); 
            Decor.SetPointForDeform(value);
            SpeedParticle.SetScale(Movement.Transform.localScale.x);
            SlimeVFX.SetScale(Movement.Transform.localScale.x);
        };

        Colorized.OnColorChanged += (value) => SlimeVFX.SetColor(value);

        Wallet = mainCanvas.Wallet;
    }

    public void Enable()
    {
        Movement.Enable();
        SlimeSize.Enable();
        SlimeVFX.Enable();
        Deformator.Restart();
        Decor.Restart();
        Movement.ResetRotate();
        Decor.ResetTransform();

        IsActive = true;
    }

    public void Disable()
    {
        Movement.Disable();
        SlimeSize.Disable();
        SlimeVFX.Disable();

        IsActive = false;
    }

    public Decor GetDecor() => Decor;
    public Deformator GetDeformator() => Deformator;
    public int GetSize() => SlimeSize.Value;
    public string GetName() => "<color=yellow>YOU</color>";
}
