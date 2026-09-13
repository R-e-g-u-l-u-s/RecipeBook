using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class AddRecipeUI : MonoBehaviour
{
    [Header("Основные поля")]
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField descriptionInput;
    [SerializeField] private Button photoButton;
    [SerializeField] private Image photoPreview;

    [Header("Категории")]
    [SerializeField] private Button dropdownButton;
    [SerializeField] private Text dropdownLabel;
    [SerializeField] private GameObject categoriesList;
    [SerializeField] private Transform categoriesContainer;
    [SerializeField] private GameObject categoryItemPrefab;
    [SerializeField] private Button clickBlocker;

    [Header("Ингредиенты")]
    [SerializeField] private InputField ingredientNameInput;
    [SerializeField] private InputField ingredientAmountInput;
    [SerializeField] private TMP_Dropdown ingredientUnitDropdown;
    [SerializeField] private Button addIngredientButton;
    [SerializeField] private Transform ingredientsContainer;
    [SerializeField] private GameObject ingredientDisplayPrefab;

    [Header("Время")]
    [SerializeField] private InputField totalDaysInput;
    [SerializeField] private InputField totalHoursInput;
    [SerializeField] private InputField totalMinutesInput;
    [SerializeField] private InputField activeDaysInput;
    [SerializeField] private InputField activeHoursInput;
    [SerializeField] private InputField activeMinutesInput;


    [Header("Острота")]
    [SerializeField] private Toggle spicinessToggle;
    [SerializeField] private GameObject spicinessLevelContainer;
    [SerializeField] private Button level1Button, level2Button, level3Button;
    [SerializeField] private Image pepperImage1, pepperImage2, pepperImage3;
    [SerializeField] private Sprite pepperActiveSprite, pepperInactiveSprite;

    [Header("БЖУ")]
    [SerializeField] private InputField proteinsInput;
    [SerializeField] private InputField fatsInput;
    [SerializeField] private InputField carbohydratesInput;
    [SerializeField] private Image proteinsSegment;
    [SerializeField] private Image fatsSegment;
    [SerializeField] private Image carbohydratesSegment;
    [SerializeField] private TMP_Text caloriesCenterText; // центральный текст

    [Header("Кнопки")]
    [SerializeField] private Button saveButton;

    private List<Category> allCategories = new List<Category>();
    private List<string> selectedCategoryIds = new List<string>();
    private List<Toggle> categoryToggles = new List<Toggle>();
    private List<Ingredient> ingredientsList = new List<Ingredient>();

    private Color selectedColor = new Color(0.3f, 0.8f, 0.3f, 1f);
    private Color unselectedColor = Color.white;
    private int selectedSpicinessLevel = 0;

    private void Start()
    {
        LoadCategories();
        dropdownButton.onClick.AddListener(ToggleDropdown);
        categoriesList.SetActive(false);
        clickBlocker.onClick.AddListener(CloseDropdown);
        clickBlocker.gameObject.SetActive(false);

        spicinessLevelContainer.SetActive(false);
        spicinessToggle.onValueChanged.AddListener(OnSpicinessToggleChanged);
        level1Button.onClick.AddListener(() => OnSpicinessLevelClicked(1));
        level2Button.onClick.AddListener(() => OnSpicinessLevelClicked(2));
        level3Button.onClick.AddListener(() => OnSpicinessLevelClicked(3));

        photoButton.onClick.AddListener(OnPhotoButtonClicked);

        List<string> units = Units.GetUnits();
        ingredientUnitDropdown.ClearOptions();
        ingredientUnitDropdown.AddOptions(units);
        ingredientUnitDropdown.value = -1;

        proteinsInput.onValueChanged.AddListener(delegate { UpdateNutritionChart(); });
        fatsInput.onValueChanged.AddListener(delegate { UpdateNutritionChart(); });
        carbohydratesInput.onValueChanged.AddListener(delegate { UpdateNutritionChart(); });
        proteinsInput.onValueChanged.AddListener(delegate { OnMacrosChanged(); });
        fatsInput.onValueChanged.AddListener(delegate { OnMacrosChanged(); });
        carbohydratesInput.onValueChanged.AddListener(delegate { OnMacrosChanged(); });

        addIngredientButton.onClick.AddListener(AddIngredientFromFields);
        saveButton.onClick.AddListener(SaveRecipe);

        UpdateDropdownLabel();
        UpdatePepperIcons();
        OnMacrosChanged();
    }

    // ===== КАТЕГОРИИ =====
    private void LoadCategories()
    {
        allCategories = DataManager.Instance.GetAllCategories();
        foreach (Transform child in categoriesContainer) Destroy(child.gameObject);
        categoryToggles.Clear();
        foreach (var category in allCategories) CreateCategoryItem(category);
    }

    private void CreateCategoryItem(Category category)
    {
        GameObject itemObj = Instantiate(categoryItemPrefab, categoriesContainer);
        Toggle toggle = itemObj.GetComponent<Toggle>();
        Text label = itemObj.GetComponentInChildren<Text>();
        Image background = itemObj.GetComponent<Image>();

        label.text = category.name;
        background.color = unselectedColor;
        toggle.onValueChanged.AddListener((isOn) => OnCategoryToggleChanged(category.id, isOn, background));
        categoryToggles.Add(toggle);
    }

    private void OnCategoryToggleChanged(string categoryId, bool isSelected, Image background)
    {
        if (isSelected) selectedCategoryIds.Add(categoryId);
        else selectedCategoryIds.Remove(categoryId);
        background.color = isSelected ? selectedColor : unselectedColor;
        UpdateDropdownLabel();
    }

    private void ToggleDropdown()
    {
        if (categoriesList.activeSelf) CloseDropdown();
        else OpenDropdown();
    }

    private void OpenDropdown()
    {
        categoriesList.SetActive(true);
        clickBlocker.gameObject.SetActive(true);
        for (int i = 0; i < allCategories.Count; i++)
        {
            bool isSelected = selectedCategoryIds.Contains(allCategories[i].id);
            categoryToggles[i].isOn = isSelected;
            categoryToggles[i].GetComponent<Image>().color = isSelected ? selectedColor : unselectedColor;
        }
    }

    private void CloseDropdown()
    {
        categoriesList.SetActive(false);
        clickBlocker.gameObject.SetActive(false);
    }

    private void UpdateDropdownLabel()
    {
        if (selectedCategoryIds.Count == 0)
        {
            dropdownLabel.text = "Выберите категории";
            dropdownLabel.color = Color.gray;
            dropdownLabel.fontSize = 45;
        }
        else
        {
            List<string> names = new List<string>();
            foreach (string id in selectedCategoryIds)
                if (allCategories.Find(c => c.id == id) != null)
                    names.Add(allCategories.Find(c => c.id == id).name);
            dropdownLabel.text = string.Join(", ", names);
            dropdownLabel.color = Color.black;
            AdjustFontSize();
        }
    }

    private void AdjustFontSize()
    {
        int maxFontSize = 45, minFontSize = 12;
        RectTransform labelRect = dropdownLabel.GetComponent<RectTransform>();
        float maxWidth = labelRect.rect.width - 20;
        dropdownLabel.fontSize = maxFontSize;
        while (dropdownLabel.preferredWidth > maxWidth && dropdownLabel.fontSize > minFontSize)
            dropdownLabel.fontSize--;
        if (dropdownLabel.preferredWidth > maxWidth)
        {
            string truncatedText = dropdownLabel.text;
            while (dropdownLabel.preferredWidth > maxWidth && truncatedText.Length > 3)
            {
                truncatedText = truncatedText.Substring(0, truncatedText.Length - 4) + "...";
                dropdownLabel.text = truncatedText;
            }
        }
    }

    // ===== ОСТРОТА =====
    private void OnSpicinessToggleChanged(bool isOn)
    {
        spicinessLevelContainer.SetActive(isOn);
        if (isOn && selectedSpicinessLevel == 0) selectedSpicinessLevel = 1;
        if (!isOn) selectedSpicinessLevel = 0;
        UpdatePepperIcons();
    }

    private void OnSpicinessLevelClicked(int level)
    {
        selectedSpicinessLevel = (selectedSpicinessLevel == level) ? 1 : level;
        UpdatePepperIcons();
    }

    private void UpdatePepperIcons()
    {
        bool show = spicinessToggle.isOn;
        pepperImage1.gameObject.SetActive(show);
        pepperImage2.gameObject.SetActive(show);
        pepperImage3.gameObject.SetActive(show);
        pepperImage1.sprite = selectedSpicinessLevel >= 1 ? pepperActiveSprite : pepperInactiveSprite;
        pepperImage2.sprite = selectedSpicinessLevel >= 2 ? pepperActiveSprite : pepperInactiveSprite;
        pepperImage3.sprite = selectedSpicinessLevel >= 3 ? pepperActiveSprite : pepperInactiveSprite;
    }

    // ===== ФОТО =====
    private void OnPhotoButtonClicked() { }

    // ===== ИНГРЕДИЕНТЫ =====
    private void AddIngredientFromFields()
    {
        string name = ingredientNameInput.text.Trim();
        if (string.IsNullOrEmpty(name)) return;
        if (string.IsNullOrEmpty(ingredientAmountInput.text)) return;

        float amount = ParseFloat(ingredientAmountInput.text);
        string unit = ingredientUnitDropdown.options[ingredientUnitDropdown.value].text;
        if (ingredientUnitDropdown.value == -1) return;

        Ingredient ing = new Ingredient { name = name, amount = amount, unit = unit };
        ingredientsList.Add(ing);

        if (ingredientDisplayPrefab != null)
        {
            GameObject displayObj = Instantiate(ingredientDisplayPrefab, ingredientsContainer);
            Button removeBtn = displayObj.transform.Find("RemoveButton")?.GetComponent<Button>();
            if (removeBtn != null)
            {
                Ingredient capturedIng = ing;
                GameObject capturedObj = displayObj;
                removeBtn.onClick.AddListener(() => { ingredientsList.Remove(capturedIng); Destroy(capturedObj); });
            }
            var nameText = displayObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            var amountText = displayObj.transform.Find("AmountText")?.GetComponent<TextMeshProUGUI>();
            var unitText = displayObj.transform.Find("UnitText")?.GetComponent<TextMeshProUGUI>();

            if (nameText != null) nameText.text = ing.name;
            if (amountText != null) amountText.text = ing.amount.ToString("0.#####", CultureInfo.InvariantCulture);
            if (unitText != null) unitText.text = ing.unit;
        }
        else
        {
            GameObject textObj = new GameObject("IngredientDisplay", typeof(Text));
            textObj.transform.SetParent(ingredientsContainer, false);
            Text text = textObj.GetComponent<Text>();
            text.text = $"{ing.name} — {ing.amount} {ing.unit}";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 20;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleLeft;
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 30);
        }

        ingredientNameInput.text = "";
        ingredientAmountInput.text = "";
        ingredientUnitDropdown.value = -1;
    }

    // ===== БЖУ =====
    private void UpdateNutritionChart()
    {
        float proteins = ParseFloat(proteinsInput.text);
        float fats = ParseFloat(fatsInput.text);
        float carbs = ParseFloat(carbohydratesInput.text);
        float total = proteins + fats + carbs;

        if (total <= 0f)
        {
            proteinsSegment.fillAmount = 0f;
            fatsSegment.fillAmount = 0f;
            carbohydratesSegment.fillAmount = 0f;
            return;
        }

        float proteinsPart = proteins / total;
        float fatsPart = fats / total;
        float carbsPart = carbs / total;

        proteinsSegment.fillAmount = proteinsPart;
        fatsSegment.fillAmount = fatsPart;
        carbohydratesSegment.fillAmount = carbsPart;

        // Повороты для последовательного отображения
        proteinsSegment.transform.localRotation = Quaternion.Euler(0, 0, 0);
        fatsSegment.transform.localRotation = Quaternion.Euler(0, 0, -proteinsPart * 360f);
        carbohydratesSegment.transform.localRotation = Quaternion.Euler(0, 0, -(proteinsPart + fatsPart) * 360f);
    }

    private void OnMacrosChanged()
    {
        float proteins = ParseFloat(proteinsInput.text);
        float fats = ParseFloat(fatsInput.text);
        float carbs = ParseFloat(carbohydratesInput.text);

        // Рассчитываем калории: 4 ккал/г белка, 9 ккал/г жира, 4 ккал/г углевода
        float calories = 4f * proteins + 9f * fats + 4f * carbs;

        // Обновляем центральный текст
        if (caloriesCenterText != null)
            caloriesCenterText.text = calories.ToString("0") + " ккал";

        // Обновляем саму диаграмму
        UpdateNutritionChart();
    }

    // ===== СОХРАНЕНИЕ =====
    private void SaveRecipe()
    {
        if (string.IsNullOrEmpty(nameInput.text) || selectedCategoryIds.Count == 0) return;

        Recipe recipe = new Recipe
        {
            name = nameInput.text,
            description = descriptionInput.text,
            categoryIds = new List<string>(selectedCategoryIds),
            totalDays = ParseInt(totalDaysInput.text),
            totalHours = ParseInt(totalHoursInput.text),
            totalMinutes = ParseInt(totalMinutesInput.text),
            activeDays = ParseInt(activeDaysInput.text),
            activeHours = ParseInt(activeHoursInput.text),
            activeMinutes = ParseInt(activeMinutesInput.text),
            //calories = ParseInt(caloriesInput.text),
            proteins = ParseFloat(proteinsInput.text),
            fats = ParseFloat(fatsInput.text),
            carbohydrates = ParseFloat(carbohydratesInput.text),
            hasSpiciness = spicinessToggle.isOn,
            spicinessLevel = selectedSpicinessLevel,
            ingredients = new List<Ingredient>(ingredientsList)
        };

        DataManager.Instance.AddRecipe(recipe);
        ClearForm();
    }

    private int ParseInt(string text) { int v; int.TryParse(text, out v); return v; }
    private float ParseFloat(string text)
    {
        float value = 0f;
        // Заменяем запятую на точку на всякий случай
        text = text.Replace(',', '.');
        float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        return value;
    }

    // ===== ОЧИСТКА =====
    private void ClearForm()
    {
        nameInput.text = descriptionInput.text = "";
        photoPreview.gameObject.SetActive(false);

        selectedCategoryIds.Clear();
        foreach (var toggle in categoryToggles) { toggle.isOn = false; toggle.GetComponent<Image>().color = unselectedColor; }
        UpdateDropdownLabel();
        CloseDropdown();

        totalDaysInput.text = totalHoursInput.text = totalMinutesInput.text = "";
        activeDaysInput.text = activeHoursInput.text = activeMinutesInput.text = "";

        spicinessToggle.isOn = false;
        selectedSpicinessLevel = 0;
        UpdatePepperIcons();
        spicinessLevelContainer.SetActive(false);

        ingredientsList.Clear();
        foreach (Transform child in ingredientsContainer) Destroy(child.gameObject);
        ingredientUnitDropdown.value = -1;

        caloriesCenterText.text = "";
        UpdateNutritionChart();
    }
}