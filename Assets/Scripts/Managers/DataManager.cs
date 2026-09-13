using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private string dataPath;
    private List<Recipe> recipes = new List<Recipe>();
    private List<Category> categories = new List<Category>();

    // Событие для обновления UI
    public event Action OnDataChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            dataPath = Path.Combine(Application.persistentDataPath, "Data");

            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);

            LoadAllData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ===== ЗАГРУЗКА И СОХРАНЕНИЕ =====

    private void LoadAllData()
    {
        // Загружаем рецепты
        string recipesFile = Path.Combine(dataPath, "recipes.json");
        if (File.Exists(recipesFile))
        {
            string json = File.ReadAllText(recipesFile);
            recipes = JsonConvert.DeserializeObject<List<Recipe>>(json) ?? new List<Recipe>();
        }

        // Загружаем категории
        string categoriesFile = Path.Combine(dataPath, "categories.json");
        if (File.Exists(categoriesFile))
        {
            string json = File.ReadAllText(categoriesFile);
            categories = JsonConvert.DeserializeObject<List<Category>>(json) ?? new List<Category>();
        }
        else
        {
            // Создаем предустановленные категории
            categories = DefaultCategories.GetDefaultCategories();
            SaveCategories();
        }
    }

    private void SaveRecipes()
    {
        string json = JsonConvert.SerializeObject(recipes, Formatting.Indented);
        File.WriteAllText(Path.Combine(dataPath, "recipes.json"), json);
    }

    private void SaveCategories()
    {
        string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
        File.WriteAllText(Path.Combine(dataPath, "categories.json"), json);
    }

    // ===== ПОЛУЧЕНИЕ ДАННЫХ =====

    public List<Recipe> GetAllRecipes() => new List<Recipe>(recipes);

    public List<Category> GetAllCategories() => new List<Category>(categories);

    public List<Recipe> GetRecipesByCategory(string categoryId)
    {
        return recipes.FindAll(r => r.categoryIds.Contains(categoryId));
    }

    // ===== РАБОТА С РЕЦЕПТАМИ =====

    public void AddRecipe(Recipe recipe)
    {
        if (string.IsNullOrEmpty(recipe.id))
            recipe.id = Guid.NewGuid().ToString();

        recipes.Add(recipe);

        // Обновляем счетчики в категориях
        foreach (string catId in recipe.categoryIds)
        {
            Category cat = categories.Find(c => c.id == catId);
            if (cat != null && !cat.recipeIds.Contains(recipe.id))
                cat.recipeIds.Add(recipe.id);
        }

        SaveRecipes();
        SaveCategories();
        OnDataChanged?.Invoke();
    }

    public void UpdateRecipe(Recipe recipe)
    {
        int index = recipes.FindIndex(r => r.id == recipe.id);
        if (index >= 0)
        {
            recipe.updatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            recipes[index] = recipe;

            // Пересчитываем категории
            foreach (var cat in categories)
                cat.recipeIds.Remove(recipe.id);

            foreach (string catId in recipe.categoryIds)
            {
                Category cat = categories.Find(c => c.id == catId);
                if (cat != null)
                    cat.recipeIds.Add(recipe.id);
            }

            SaveRecipes();
            SaveCategories();
            OnDataChanged?.Invoke();
        }
    }

    public void DeleteRecipe(string recipeId)
    {
        Recipe recipe = recipes.Find(r => r.id == recipeId);
        if (recipe != null)
        {
            // Убираем из категорий
            foreach (var cat in categories)
                cat.recipeIds.Remove(recipeId);

            recipes.Remove(recipe);

            SaveRecipes();
            SaveCategories();
            OnDataChanged?.Invoke();
        }
    }

    // ===== ЭКСПОРТ =====

    public string ExportData()
    {
        var data = new { recipes, categories };
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath,
            $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.json");
        File.WriteAllText(path, json);
        return path;
    }
}