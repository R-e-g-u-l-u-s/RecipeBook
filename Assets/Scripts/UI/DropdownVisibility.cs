using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DropdownVisibility : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameObject objectToShow;

    private bool wasOpen = false;

    void Start()
    {
        objectToShow.SetActive(false);

        // Скрываем объект при выборе пункта
        dropdown.onValueChanged.AddListener(_ => HideObject());

        // Показываем объект при клике на Dropdown
        EventTrigger trigger = dropdown.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => ShowObject());
        trigger.triggers.Add(entry);
    }

    void Update()
    {
        bool isOpen = IsDropdownOpen();

        // Если список был открыт и только что закрылся
        if (wasOpen && !isOpen)
        {
            HideObject();
        }

        wasOpen = isOpen;
    }

    bool IsDropdownOpen()
    {
        // Стандартный Dropdown создаёт дочерний объект с именем "Dropdown List"
        Transform dropdownList = dropdown.transform.Find("Dropdown List");
        return dropdownList != null && dropdownList.gameObject.activeSelf;
    }

    void ShowObject()
    {
        objectToShow.SetActive(true);
    }

    void HideObject()
    {
        objectToShow.SetActive(false);
    }
}