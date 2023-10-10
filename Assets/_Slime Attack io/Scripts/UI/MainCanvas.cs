using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class MainCanvas : BasePanel
{
    public Canvas Canvas { get; private set; }

    [field: SerializeField]
    public Wallet Wallet { get; private set; }

    public void Init()
    {
        Canvas = GetComponent<Canvas>();

        Wallet.Init();
    }
}
