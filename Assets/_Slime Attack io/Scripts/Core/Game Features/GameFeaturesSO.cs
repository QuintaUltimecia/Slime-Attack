using UnityEngine;

[CreateAssetMenu(fileName = "Game Features", menuName = "Game Features")]
public class GameFeaturesSO : ScriptableObject
{
    public float BuildingRefreshTime = 30f;
    public float SlimeSpeed = 4f;
    public int Timer = 120;
}
