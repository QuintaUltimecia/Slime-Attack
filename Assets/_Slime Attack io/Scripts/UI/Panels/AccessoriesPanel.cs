using UnityEngine;

public class AccessoriesPanel : BasePanel
{
    [SerializeField]
    private HeadPanel _head;

    [SerializeField]
    private MaskPanel _mask;

    public void Init(Accessories accessories, Wallet wallet)
    {
        _head.Init(accessories, wallet);
        _mask.Init(accessories, wallet);
    }
}
