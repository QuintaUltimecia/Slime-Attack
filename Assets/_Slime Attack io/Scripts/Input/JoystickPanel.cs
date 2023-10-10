using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private JoyStick _joystick;

    private void Start()
    {
        _joystick.gameObject.SetActive(false); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _joystick.gameObject.SetActive(true);
        _joystick.transform.position = eventData.position;
        _joystick.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _joystick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystick.gameObject.SetActive(false);
        _joystick.OnEndDrag(eventData);
    }
}
