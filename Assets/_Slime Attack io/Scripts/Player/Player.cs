using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Deformator))]
[RequireComponent(typeof(Absorber))]
[RequireComponent(typeof(Decor))]
[RequireComponent(typeof(SphereCollider))]
public sealed class Player : MonoBehaviour, IAbsorbingStay, IOnLeaderBoard
{
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
    public Collider Collider { get; private set; }
    public Wallet Wallet { get; private set; }

    public void Init(IInput input, Camera camera, MainCanvas mainCanvas, GameFeaturesModule gameFeatures)
    {
        Movement = GetComponent<Movement>();
        Deformator = GetComponent<Deformator>();
        Absorber = GetComponent<Absorber>();
        Decor = GetComponent<Decor>();
        Collider = GetComponent<Collider>();

        Movement.Init(input, gameFeatures.SlimeSpeed);
        Absorber.Init(Deformator);
        SlimeSize.Init(camera, mainCanvas.Canvas);
        Deformator.Init();
        Accessories.Init();
        SpeedParticle.Init(Movement);
        SlimeVFX.Init();

        Movement.OnMove += (value) => 
            PlayerAnimationController.Move(value);

        Absorber.OnAbsorbe += (IDeformPointProvider) => 
            Deformator.Deformate(IDeformPointProvider);

        Decor.OnBeforeAbsorbe += () => { 
            Movement.Disable(); 
            Collider.enabled = false; 
            SlimeSize.Disable(); };

        Deformator.OnDeformate += (value) => { 
            SlimeSize.UpdateText(value); 
            Decor.SetPointForDeform(value);
            SpeedParticle.SetScale(Movement.Transform.localScale.x);
            SlimeVFX.SetScale(Movement.Transform.localScale.x);
        };

        Colorized.OnColorChanged += (value) => SlimeVFX.SetColor(value);

        Wallet = mainCanvas.Wallet;
    }

    public void Enable(Vector3 position)
    {
        Movement.SetPosition(position);
        Movement.Enable();
        SlimeSize.Enable();
    }

    public void Disable(Vector3 position)
    {
        Movement.Disable();
        Movement.SetPosition(position);
        SlimeSize.Disable();
        Deformator.Restart();
        Movement.ResetRotate();
        Collider.enabled = true;
    }

    public Decor GetDecor() => Decor;
    public Deformator GetDeformator() => Deformator;

    public int GetSize()
    {
        return SlimeSize.Value;
    }

    public string GetName()
    {
        return "<color=yellow>YOU</color>";
    }
}
