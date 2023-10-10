using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Accessories")]
public class AccessorySO : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int Price { get; private set; }
}
