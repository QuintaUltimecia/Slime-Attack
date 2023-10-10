using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Colorized : MonoBehaviour
{
    private SkinnedMeshRenderer s_meshRenderer;

    public System.Action <Color> OnColorChanged;

    private void Awake()
    {
        s_meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    public Color GetColor()
    {
        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        s_meshRenderer.GetPropertyBlock(materialProperty);

        return materialProperty.GetColor("_Color");
    }

    public void RandomRecolor()
    {
        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        s_meshRenderer.GetPropertyBlock(materialProperty);

        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);

        Color color = new Color(r, g, b);

        materialProperty.SetColor("_Color", color);
        materialProperty.SetColor("_AmbientColor", color);

        s_meshRenderer.SetPropertyBlock(materialProperty);

        OnColorChanged?.Invoke(color);
    }

    public void RecolorR(float r)
    {
        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        s_meshRenderer.GetPropertyBlock(materialProperty);

        Color color = new Color(r, materialProperty.GetColor("_Color").g, materialProperty.GetColor("_Color").b);

        materialProperty.SetColor("_Color", color);
        materialProperty.SetColor("_AmbientColor", color);

        s_meshRenderer.SetPropertyBlock(materialProperty);

        OnColorChanged?.Invoke(color);
    }

    public void RecolorG(float g)
    {
        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        s_meshRenderer.GetPropertyBlock(materialProperty);

        Color color = new Color(materialProperty.GetColor("_Color").r, g, materialProperty.GetColor("_Color").b);

        materialProperty.SetColor("_Color", color);
        materialProperty.SetColor("_AmbientColor", color);

        s_meshRenderer.SetPropertyBlock(materialProperty);

        OnColorChanged?.Invoke(color);
    }

    public void RecolorB(float b)
    {
        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        s_meshRenderer.GetPropertyBlock(materialProperty);

        Color color = new Color(materialProperty.GetColor("_Color").r, materialProperty.GetColor("_Color").g, b);

        materialProperty.SetColor("_Color", color);
        materialProperty.SetColor("_AmbientColor", color);

        s_meshRenderer.SetPropertyBlock(materialProperty);

        OnColorChanged?.Invoke(color);
    }
}
