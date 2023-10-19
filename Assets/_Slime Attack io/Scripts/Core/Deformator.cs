using UnityEngine;

public class Deformator : MonoBehaviour
{
    public float Value { get; private set; } = 1f;
    public const float MaxValue = 7f;

    private Transform _transform;

    public System.Action<float> OnDeformate;

    public float GetSize()
    {
        if (Value < 1f)
            return 1f;

        return Value;
    }

    public void Init()
    {
        _transform = transform;
        _transform.localScale = Vector3.one;

        OnDeformate?.Invoke(Value);
    }

    public void Restart()
    {
        Value = 1f;
        OnDeformate?.Invoke(Value);
    }

    public void Deformate(float value)
    {
        Value += value;

        if (Value < MaxValue)
            ValueToTransform();

        OnDeformate?.Invoke(Value);
    }

    private void ValueToTransform()
    {
        if (Value >= MaxValue)
            return;

        _transform.localScale = new Vector3(
            x: Value,
            y: Value,
            z: Value);

        if (Value < 0)
        {
            Value = 0;
            _transform.localScale = Vector3.zero;
        }
    }
}
