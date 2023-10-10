using UnityEngine;

[CreateAssetMenu(fileName = "Features", menuName = "Decor")]
public class DecorFeaturesSO : ScriptableObject
{
    [field: SerializeField]
    public float DeformPoint { get; private set; }

    [field: SerializeField]
    public float PointForDeform { get; set; }
}
