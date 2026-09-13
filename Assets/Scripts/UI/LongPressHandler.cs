using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent onLongPress = new UnityEvent();

    [SerializeField] private float longPressDuration = 0.5f;

    private bool isPointerDown = false;
    private float pointerDownTimer = 0f;
    private bool longPressTriggered = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTimer = 0f;
        longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    private void Update()
    {
        if (isPointerDown && !longPressTriggered)
        {
            pointerDownTimer += Time.deltaTime;

            if (pointerDownTimer >= longPressDuration)
            {
                longPressTriggered = true;
                onLongPress.Invoke();
            }
        }
    }
}