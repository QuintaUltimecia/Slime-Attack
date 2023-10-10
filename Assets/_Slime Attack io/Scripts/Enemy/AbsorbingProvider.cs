using UnityEngine;

public class AbsorbingProvider : MonoBehaviour, IAbsorbingStay
{
    [field: SerializeField]
    public Decor Decor { get; private set; }

    [field: SerializeField]
    public Deformator Deformator { get; private set; }

    public Decor GetDecor()
    {
        return Decor;
    }

    public Deformator GetDeformator()
    {
        return Deformator;
    }
}
