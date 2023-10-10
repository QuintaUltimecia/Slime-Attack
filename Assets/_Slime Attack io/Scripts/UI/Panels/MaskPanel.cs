using System;
using TMPro;
using UnityEngine;

public class MaskPanel : BasePanel
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
        _accessories.ApplyMask();
        ActivePurchased();
    }

    private void OnEnable()
    {
        if (_accessories != null)
        {
            _text.text = _accessories.GetMaskFeatures().Name;
            _count = _accessories.CurrentMask;

            Debug.Log(_accessories.CurrentMask);
            ActivePurchased();
        }
    }

    private void ActiveLeft()
    {
        _count--;
        _accessories.ActiveMasks(ref _count);
        _text.text = _accessories.GetMaskFeatures().Name;

        ActivePurchased();
    }

    private void ActiveRight()
    {
        _count++;
        _accessories.ActiveMasks(ref _count);
        _text.text = _accessories.GetMaskFeatures().Name;

        ActivePurchased();
    }

    private void Buy()
    {
        if (_wallet.Value >= _accessories.GetMaskFeatures().Price)
        {
            _accessories.SetMaskPurchased();
            ActivePurchased();
            _wallet.RemoveWallet(_accessories.GetMaskFeatures().Price);
            Select();

            OnBuy?.Invoke();
        }
    }

    private void ActivePurchased()
    {
        if (_accessories.GetMaskPurchased() == false)
        {
            _buyPanel.Enable();
            _buyPanel.SetPrice(_accessories.GetMaskFeatures().Price);
            _buyPanel.GetInternalButton<BuyButton>().OnClick = () => Buy();
            GetInternalButton<SelectableButton>().Disable();
        }
        else
        {
            _buyPanel.Disable();

            if (_accessories.CurrentMasksIsLast())
                GetInternalButton<SelectableButton>().Disable();
            else
            {
                GetInternalButton<SelectableButton>().Enable();
            }
        }
    }
}
