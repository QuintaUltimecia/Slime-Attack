using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class DontReadable : MonoBehaviour
{
    [SerializeField]
    private bool _isEditable;

    [SerializeField]
    private List<Component> _readableComponents = new List<Component>();

    private GameObject _gameObject;
    private RectTransform _rectTransform;
    private Transform _transform;

    public void Update()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        if (_rectTransform == null && _transform == null)
        {
            _transform = transform;
            _rectTransform = GetComponent<RectTransform>();
        }

        if (_isEditable == true)
            _gameObject.hideFlags = HideFlags.None;
        else
        {
            _gameObject.hideFlags = HideFlags.NotEditable;
            this.hideFlags = HideFlags.None;

            if (_readableComponents.Count > 0)
            {
                foreach (var component in _readableComponents)
                    component.hideFlags = HideFlags.None;
            }

            if (_rectTransform != null)
                _rectTransform.hideFlags = HideFlags.HideInInspector;
        }
    }
}
