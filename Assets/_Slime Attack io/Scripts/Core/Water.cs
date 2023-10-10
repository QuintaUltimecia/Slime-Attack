using UnityEngine;

public class Water : BaseBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2f;

    private Transform _transform;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _targetPosition = _transform.position;
    }

    public override void OnTick()
    {
        _transform.localPosition = Vector3.MoveTowards(
            _transform.localPosition, 
            _targetPosition,
            _moveSpeed * Time.deltaTime);

        if (_transform.localPosition == _targetPosition)
        {
            float x = Random.Range(0f, 1f);
            float y = _transform.localPosition.y;
            float z = Random.Range(0f, 1f);

            _targetPosition = new Vector3(x, y, z);
        }
    }
}
