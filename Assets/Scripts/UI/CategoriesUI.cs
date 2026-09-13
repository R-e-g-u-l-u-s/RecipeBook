using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesUI : MonoBehaviour
{
    [SerializeField] private Transform categoryContainer;
    [SerializeField] private GameObject categoryButtonPrefab;

    private void Start()
    {
        // Подписываемся на изменения данных
        DataManager.Instance.OnDataChanged += DisplayCategories;

        // Показываем категории
        DisplayCategories();
    }

    private void OnDestroy()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.OnDataChanged -= DisplayCategories;
    }

    private void DisplayCategories()
    {
        // Очищаем старые кнопки
        foreach (Transform child in categoryContainer)
            Destroy(child.gameObject);

        // Создаем кнопки для всех категорий
        foreach (var category in DataManager.Instance.GetAllCategories())
        {
            CreateCategoryButton(category);
        }
    }

    private void CreateCategoryButton(Category category)
    {
        // Создаем кнопку из префаба
        GameObject buttonObj = Instantiate(categoryButtonPrefab, categoryContainer);

        Image icon = buttonObj.transform.Find("Icon").GetComponent<Image>();
        TextMeshProUGUI nameText = buttonObj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI countText = buttonObj.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

        // Заполняем данными
        nameText.text = category.name;
        countText.text = $"{category.GetRecipeCount()}";

        // Загружаем иконку (если есть)
        if (!string.IsNullOrEmpty(category.iconName))
        {
            Sprite iconSprite = Resources.Load<Sprite>($"Icons/{category.iconName}");
            if (iconSprite != null)
                icon.sprite = iconSprite;
        }

        // Обработка нажатия
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(() => OnCategoryClicked(category));
    }

    private void OnCategoryClicked(Category category)
    {
        Debug.Log($"Выбрана категория: {category.name}");
        // Здесь будет переход к рецептам категории
    }
}