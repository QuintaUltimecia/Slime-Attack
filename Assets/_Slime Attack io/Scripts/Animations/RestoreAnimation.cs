using System;
using UnityEngine;

public class RestoreAnimation : BaseBehaviour
{
    private Transform _transform;
    private bool _isPlayed = false;

    private float _y;
    private Vector3 _startPosition;

    private float _moveSpeed = 4f;

    private event Action _callback;

    private void Awake()
    {
        _transform = transform;
    }

    public void Play(Action callback = null)
    {
        if (_isPlayed == false) 
        {
            _startPosition = _transform.position;

            _y = _transform.position.y;
            _y -= 5;
            _transform.position = new Vector3(_transform.position.x, _y, _transform.position.z);

            _isPlayed = true;
            _updates.Add(this);
            _callback = callback;
        }
    }

    public override void OnEnable() { }
    public override void OnDisable() { }

    public override void OnTick()
    {
        _y += _moveSpeed * Time.deltaTime;
        _transform.position = new Vector3(_transform.position.x, _y, _transform.position.z);

        if (_transform.position.y >= _startPosition.y)
        {
            _transform.position = _startPosition;
            _isPlayed = false;
            _updates.Remove(this);
            _callback?.Invoke();
        }
    }
}
