using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSCounter : BaseBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public override void OnTick()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"FPS: {(int)(Time.frameCount / Time.time)}";
    }
}
