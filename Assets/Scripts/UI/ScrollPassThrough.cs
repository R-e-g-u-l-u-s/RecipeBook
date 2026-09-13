using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollPassThrough : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect parentScrollRect;

    private void Start()
    {
        if (parentScrollRect == null)
        {
            // Автоматически находим родительский ScrollRect
            parentScrollRect = GetComponentInParent<ScrollRect>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentScrollRect != null)
            parentScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentScrollRect != null)
            parentScrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentScrollRect != null)
            parentScrollRect.OnEndDrag(eventData);
    }
}