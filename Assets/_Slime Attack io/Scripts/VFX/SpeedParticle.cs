using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpeedParticle : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private IContainSpeed _containSpeed;

    private Transform _transform;
    private bool _isSub = false;

    public void Init(IContainSpeed containSpeed)
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _containSpeed = containSpeed;
        _transform = transform;

        OnEnable();
    }

    private void OnEnable()
    {
        if (_containSpeed != null && _isSub == false)
        {
            _containSpeed.GetMoveSpeed().OnBeforeUpSpeed += () => PlayParticle(true);
            _containSpeed.GetMoveSpeed().OnAfterUpSpeed += () => PlayParticle(false);
            _isSub = true;
        }
    }

    private void OnDisable()
    {
        if (_containSpeed != null && _isSub == true)
        {
            _containSpeed.GetMoveSpeed().OnBeforeUpSpeed -= () => PlayParticle(true);
            _containSpeed.GetMoveSpeed().OnAfterUpSpeed -= () => PlayParticle(false);
            _isSub = false;
        }
    }

    private void PlayParticle(bool isActive)
    {
        if (_particleSystem == null)
            return;

        if (isActive == true)
            _particleSystem.Play();
        else _particleSystem.Stop();
    }

    public void SetScale(float value)
    {
        _transform.localScale = new Vector3(value, value, value);
    }
}
