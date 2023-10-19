using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class BasePanel : MonoBehaviour
{
    [SerializeField]
    protected List<BasePanel> _panels;

    [SerializeField]
    protected List<BaseButton> _buttons;

    protected GameObject _gameObject;

    protected virtual void Awake()
    {
        _gameObject = gameObject;
    }

    public void Enable()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (_gameObject == null)
            _gameObject = gameObject;

        _gameObject.SetActive(false);
    }

    public T GetButton<T>() where T : BaseButton
    {
        if (_buttons == null || _buttons.Count == 0)
        {
            if (_panels == null || _panels.Count == 0)
                return null;

            for (int i = 0; i < _panels.Count; i++)
            {
                if (_panels[i].GetButton<T>())
                    return _panels[i].GetButton<T>();
            }
        }

        var button = (T)_buttons.FirstOrDefault(s => s is T);

        if (button == null)
        {
            if (_panels == null || _panels.Count == 0)
                return null;

            for (int i = 0; i < _panels.Count; i++)
            {
                button = _panels[i].GetButton<T>();
            }

            if (button == null)
            {
                Debug.LogError("Dont have a button.");
                return null;
            }
        }

        return button;
    }

    public T GetPanel<T>() where T : BasePanel
    {
        if (_panels == null || _panels.Count == 0)
        {
            return null;
        }

        var panel = (T)_panels.FirstOrDefault(s => s is T);

        if (panel == null)
        {
            for (int i = 0; i < _panels.Count; i++)
            {
                panel = _panels[i].GetPanel<T>();
            }

            if (panel == null)
            {
                Debug.LogError("Dont have a panel.");
                return null;
            }
        }

        return panel;
    }

    public void ShowPanel<T>() where T : BasePanel
    {
        if (_panels == null || _panels.Count == 0)
            return;

        foreach (var item in _panels)
            item.Disable();

        BasePanel panel = _panels.FirstOrDefault(s => s is T);

        panel.Enable();
    }
}
