using UnityEngine;
using TMPro;

public class BestSize : MonoBehaviour
{
    public int Value { get; private set; }

    private TextMeshProUGUI _text;

    private bool _isInitialized = false;

    public void Init()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _isInitialized = true;
    }

    public void SetBastSize(int value)
    {
        if (!_isInitialized)
        {
            Debug.Log("Object is not initialized");
            return;
        }

        Value = value;
        _text.text = $"Best Size: {Value}";
    }
}
