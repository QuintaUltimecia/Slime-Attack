using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]    
public class Wallet : MonoBehaviour
{
    public int Value { get; private set; }
    public const int MinValue = 0;

    private TextMeshProUGUI _text;

    public System.Action OnAddWallet;

    public void Init()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void AddWallet(int value)
    {
        Value += value;

        UpdateText(Value);
        OnAddWallet?.Invoke();
    }

    public void RemoveWallet(int value)
    {
        Value -= value;

        if (Value < MinValue) 
            Value = MinValue;

        UpdateText(Value);
    }

    private void UpdateText(int value)
    {
        _text.text = value.ToString();
    }
}
