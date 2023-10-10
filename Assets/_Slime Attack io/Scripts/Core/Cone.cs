using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cone : BaseBehaviour
{
    private Transform _target;
    private Transform _transform;
    private MeshRenderer _meshRenderer;

    public void Init()
    {
        _transform = transform;
        _target = _transform.parent;
        _transform.SetParent(null);
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Enable()
    {
        _meshRenderer.enabled = true;
    }

    public void Disable()
    {
        _meshRenderer.enabled = false;
    }

    public void SetColor(Color color)
    {
        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        _meshRenderer.GetPropertyBlock(materialProperty);

        materialProperty.SetColor("_Color", color);

        _meshRenderer.SetPropertyBlock(materialProperty);
    }

    public void SetSize(float y)
    {
        _transform.localScale = new Vector3(_transform.localScale.x, y, _transform.localScale.z);
    }

    public override void OnTick()
    {
        if (_transform != null && _target != null)
            _transform.position = _target.position;
    }
}
