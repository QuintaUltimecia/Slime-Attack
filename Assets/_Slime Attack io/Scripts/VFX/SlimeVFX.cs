using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class SlimeVFX : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private MinMaxCurve _startSize;

    public void Init()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _startSize = _particleSystem.main.startSize;
    }

    public void SetColor(Color color)
    {
        MainModule mainModule = _particleSystem.main;
        mainModule.startColor = color;
    }

    public void SetScale(float value)
    {
        MainModule mainModule = _particleSystem.main;
        MinMaxCurve minMax = new MinMaxCurve();
        minMax.mode = ParticleSystemCurveMode.TwoConstants;
        minMax.constantMax = _startSize.constantMax * value;
        minMax.constantMin = _startSize.constantMin * value;

        mainModule.startSize = minMax;
    }
}
