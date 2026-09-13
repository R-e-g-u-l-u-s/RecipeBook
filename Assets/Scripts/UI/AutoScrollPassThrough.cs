using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoScrollPassThrough : MonoBehaviour
{
    [SerializeField] private ScrollRect parentScrollRect;

    private void Start()
    {
        if (parentScrollRect == null)
            parentScrollRect = GetComponentInParent<ScrollRect>();

        // Добавляем обработчики на все дочерние элементы
        AddPassThroughToChildren(transform);
    }

    private void AddPassThroughToChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Добавляем на InputField
            InputField inputField = child.GetComponent<InputField>();
            if (inputField != null)
            {
                AddPassThrough(child.gameObject);
            }

            // Добавляем на ScrollRect (вложенные)
            ScrollRect nestedScroll = child.GetComponent<ScrollRect>();
            if (nestedScroll != null && nestedScroll != parentScrollRect)
            {
                AddPassThrough(child.gameObject);
            }

            // Добавляем на Slider
            Slider slider = child.GetComponent<Slider>();
            if (slider != null)
            {
                AddPassThrough(child.gameObject);
            }

            // Добавляем на Toggle
            Toggle toggle = child.GetComponent<Toggle>();
            if (toggle != null)
            {
                AddPassThrough(child.gameObject);
            }

            // Рекурсивно обрабатываем дочерние
            AddPassThroughToChildren(child);
        }
    }

    private void AddPassThrough(GameObject obj)
    {
        if (obj.GetComponent<ScrollPassThrough>() == null)
        {
            obj.AddComponent<ScrollPassThrough>();
        }
    }
}