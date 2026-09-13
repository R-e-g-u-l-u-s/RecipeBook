using System;
using System.Collections.Generic;

[Serializable]
public class Category
{
    public string id;
    public string name;
    public string iconName; // имя иконки из спрайтов
    public string colorHex; // цвет категории в HEX формате
    public string createdAt;
    public List<string> recipeIds = new List<string>(); // ID рецептов в этой категории

    public Category()
    {
        id = Guid.NewGuid().ToString();
        createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public Category(string name) : this()
    {
        this.name = name;
        this.colorHex = "#FFFFFF"; // белый по умолчанию
    }

    public void AddRecipe(string recipeId)
    {
        if (!recipeIds.Contains(recipeId))
        {
            recipeIds.Add(recipeId);
        }
    }

    public void RemoveRecipe(string recipeId)
    {
        recipeIds.Remove(recipeId);
    }

    public int GetRecipeCount()
    {
        return recipeIds?.Count ?? 0;
    }
}