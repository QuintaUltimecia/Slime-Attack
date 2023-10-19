using UnityEngine.UI;
using UnityEngine;

public class ColorizedPanel : BasePanel
{
    private Colorized _coloriedTarget;

    [SerializeField]
    private Scrollbar r_scrollBar;

    [SerializeField]
    private Scrollbar g_scrollBar;

    [SerializeField]
    private Scrollbar b_scrollBar;

    public void Init(Colorized colorized)
    {
        _coloriedTarget = colorized;

        r_scrollBar.onValueChanged.AddListener((call) => _coloriedTarget.RecolorR(call));
        g_scrollBar.onValueChanged.AddListener((call) => _coloriedTarget.RecolorG(call));
        b_scrollBar.onValueChanged.AddListener((call) => _coloriedTarget.RecolorB(call));
    }

    public void StartColor()
    {
        r_scrollBar.SetValueWithoutNotify(_coloriedTarget.GetColor().r);
        g_scrollBar.SetValueWithoutNotify(_coloriedTarget.GetColor().g);
        b_scrollBar.SetValueWithoutNotify(_coloriedTarget.GetColor().b);
    }
}
