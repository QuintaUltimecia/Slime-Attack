using UnityEngine;

public class Accessory : MonoBehaviour
{
    [field: SerializeField]
    public AccessorySO AccessoryFeatures { get; private set; }

    private GameObject _gameObject;

    public void Enabled(bool enabled)
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(enabled);
    }
}
