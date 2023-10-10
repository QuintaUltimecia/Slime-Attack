using UnityEngine;
using TMPro;

public class BuyPanel : BasePanel
{
    [SerializeField]
    private TextMeshProUGUI _price;

    public void SetPrice(int value)
    {
        _price.text = value.ToString();
    }
}
