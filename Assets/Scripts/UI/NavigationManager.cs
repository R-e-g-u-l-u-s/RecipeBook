using UnityEngine;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject categoriesPanel;
    [SerializeField] private GameObject addRecipePanel;
    [SerializeField] private GameObject shoppingListPanel;

    [Header("Navigation Buttons")]
    [SerializeField] private Button categoriesButton;
    [SerializeField] private Button addButton;
    [SerializeField] private Button shoppingButton;

    [Header("Button Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    private Image categoriesButtonImage;
    private Image addButtonImage;
    private Image shoppingButtonImage;

    private void Start()
    {
        // Получаем компоненты Image кнопок
        categoriesButtonImage = categoriesButton.GetComponent<Image>();
        addButtonImage = addButton.GetComponent<Image>();
        shoppingButtonImage = shoppingButton.GetComponent<Image>();

        // Подписываемся на нажатия
        categoriesButton.onClick.AddListener(() => ShowPanel("categories"));
        addButton.onClick.AddListener(() => ShowPanel("add"));
        shoppingButton.onClick.AddListener(() => ShowPanel("shopping"));

        // Показываем категории по умолчанию
        ShowPanel("categories");
    }

    public void ShowPanel(string panelName)
    {
        // Деактивируем все панели
        categoriesPanel.SetActive(false);
        addRecipePanel.SetActive(false);
        shoppingListPanel.SetActive(false);

        // Активируем нужную панель
        switch (panelName)
        {
            case "categories":
                categoriesPanel.SetActive(true);
                UpdateButtonVisuals(categoriesButtonImage);
                break;
            case "add":
                addRecipePanel.SetActive(true);
                UpdateButtonVisuals(addButtonImage);
                break;
            case "shopping":
                shoppingListPanel.SetActive(true);
                UpdateButtonVisuals(shoppingButtonImage);
                break;
        }
    }

    private void UpdateButtonVisuals(Image activeButton)
    {
        // Сбрасываем все кнопки к нормальному цвету
        categoriesButtonImage.color = normalColor;
        addButtonImage.color = normalColor;
        shoppingButtonImage.color = normalColor;

        // Подсвечиваем активную кнопку
        activeButton.color = selectedColor;
    }
    public void ShowRecipesForCategory(string categoryId)
    {
        // Пока здесь заглушка
        Debug.Log($"Показать рецепты категории: {categoryId}");
    }
}