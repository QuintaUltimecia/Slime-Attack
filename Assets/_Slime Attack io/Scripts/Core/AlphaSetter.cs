using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AlphaSetter : MonoBehaviour
{
    public bool IsActive { get; private set; }

    private MeshRenderer _renderer;

    [SerializeField]
    private Material _transparentShader;
    [SerializeField]
    private Material _justShader;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();

        _renderer.material = _justShader;
    }

    public void SetAlpha(bool isActive)
    {
        float value = 1.0f;

        if (isActive == true)
        {
            value = 0.5f;
            _renderer.material = _transparentShader;
        }
        else
        {
            _renderer.material = _justShader;
        }

        IsActive = isActive;

        MaterialPropertyBlock property = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(property);

        Color color = new Color(1f, 1f, 1f, value);

        property.SetColor("_AmbientColor", color);
        property.SetColor("_Color", color);

        _renderer.SetPropertyBlock(property);
    }
}
