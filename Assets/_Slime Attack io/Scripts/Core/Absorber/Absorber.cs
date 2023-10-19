using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Absorber : MonoBehaviour
{
    private Transform _transform;
    private Deformator _deformator;

    private List<IAbsorbingEnter> _absorbeings = new List<IAbsorbingEnter>();

    public System.Action<float> OnAbsorbe;

    public void Init(Deformator deformator)
    {
        _transform = transform;
        _deformator = deformator;

        _deformator.OnDeformate += (value) => CheckAllDecors();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IAbsorbingEnter absorbingEnter))
        {
            if (_deformator.GetSize() >= absorbingEnter.GetDecor().Features.PointForDeform)
            {
                absorbingEnter.GetDecor().Absorbe(_transform, (DecorFeaturesSO) => OnAbsorbe?.Invoke(DecorFeaturesSO));
                _absorbeings.Remove(absorbingEnter);
            }
            else
            {
                _absorbeings.Add(absorbingEnter);

                if (TryGetComponent(out Player player))
                    absorbingEnter.GetAlphaSetter().SetAlpha(true);
            }    
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IAbsorbingEnter absorbingEnter))
        {
            if (_deformator.GetSize() >= absorbingEnter.GetDecor().Features.PointForDeform)
            {

            }
            else
            {
                _absorbeings.Remove(absorbingEnter);

                if (TryGetComponent(out Player player))
                    absorbingEnter.GetAlphaSetter().SetAlpha(false);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IAbsorbingStay absorbingStay))
        {
            if (_deformator.GetSize() > absorbingStay.GetDeformator().GetSize() && absorbingStay.GetDecor().IsAbsorbed == false)
            {
                _deformator.Deformate(Time.deltaTime / 3);
                absorbingStay.GetDeformator().Deformate(-Time.deltaTime);

                if (absorbingStay.GetDeformator().GetSize() <= 1f)
                    absorbingStay.GetDecor().Absorbe(_transform, (DecorFeaturesSO) => OnAbsorbe?.Invoke(DecorFeaturesSO));
            }
        }
    }

    private void CheckAllDecors()
    {
        if (_absorbeings.Count <= 0)
            return;

        List<IAbsorbingEnter> absorbeings = new List<IAbsorbingEnter>();

        foreach (var absorbeing in _absorbeings)
        {
            if (_deformator.GetSize() >= absorbeing.GetDecor().Features.PointForDeform)
            {
                absorbeing.GetDecor().Absorbe(_transform, (DecorFeaturesSO) => OnAbsorbe?.Invoke(DecorFeaturesSO));
                absorbeings.Add(absorbeing);
                _absorbeings.Remove(absorbeing);

                break;
            }
        }
    }
}
