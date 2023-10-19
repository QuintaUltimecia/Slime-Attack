using UnityEngine;
using TMPro;
using System;

public class HeadPanel : BasePanel
{
    [SerializeField]
    private ArrowButton _leftArrow;
    [SerializeField]
    private ArrowButton _rightArrow;
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private BuyPanel _buyPanel;

    private int _count;

    private Accessories _accessories;
    private Wallet _wallet;

    public event Action OnBuy;

    public void Init(Accessories accessories, Wallet wallet)
    {
        _accessories = accessories;
        _wallet = wallet;

        _leftArrow.OnClick = () => ActiveLeft();
        _rightArrow.OnClick = () => ActiveRight();
    }

    public void Back()
    {
        _accessories.Back();
    }

    public void Select()
    {
        _accessories.ApplyHead();
        ActivePurchased();

        Debug.Log("Select Head");
    }

    private void OnEnable()
    {
        if (_accessories != null)
        {
            _text.text = _accessories.GetHeadFeatures().Name;
            _count = _accessories.CurrentHead;
            ActivePurchased();
        }
    }

    private void ActiveLeft()
    {
        _count--;
        _accessories.ActiveHeads(ref _count);
        _text.text = _accessories.GetHeadFeatures().Name;

        ActivePurchased();
    }

    private void ActiveRight()
    {
        _count++;
        _accessories.ActiveHeads(ref _count);
        _text.text = _accessories.GetHeadFeatures().Name;

        ActivePurchased();
    }

    private void Buy()
    {
        if (_wallet.Value >= _accessories.GetHeadFeatures().Price)
        {
            _accessories.SetHeadPurchased();
            ActivePurchased();
            _wallet.RemoveWallet(_accessories.GetHeadFeatures().Price);
            Select();

            OnBuy?.Invoke();
        }
    }

    private void ActivePurchased()
    {
        if (_accessories.GetHeadPurchased() == false)
        {
            _buyPanel.Enable();
            _buyPanel.SetPrice(_accessories.GetHeadFeatures().Price);
            _buyPanel.GetButton<BuyButton>().OnClick = () => Buy();
            GetButton<SelectableButton>().Disable();
        }
        else
        {
            _buyPanel.Disable();

            if (_accessories.CurrentHeadIsLast())
                GetButton<SelectableButton>().Disable();
            else
            {
                GetButton<SelectableButton>().Enable();
            }
        }
    }
}
