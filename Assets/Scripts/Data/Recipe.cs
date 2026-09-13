using System;
using System.Collections.Generic;

[Serializable]
public class Recipe
{
    public string id;
    public string name;
    public string description;             // Описание
    public string photoPath;               // Путь к фото
    public int calories;
    public float proteins;                 // Белки
    public float fats;                     // Жиры
    public float carbohydrates;            // Углеводы
    public List<string> categoryIds = new List<string>();
    
    // Время в минутах (для обратной совместимости)
    public int totalTimeMinutes;
    public int activeTimeMinutes;
    
    // Или более детально: дни, часы, минуты
    public int totalDays;
    public int totalHours;
    public int totalMinutes;
    public int activeDays;
    public int activeHours;
    public int activeMinutes;
    
    public bool hasSpiciness;
    public int spicinessLevel;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public List<RecipeStep> steps = new List<RecipeStep>();
    public string createdAt;
    public string updatedAt;
    
    public Recipe()
    {
        id = Guid.NewGuid().ToString();
        createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        updatedAt = createdAt;
    }
}