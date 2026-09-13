using System.Collections.Generic;

public static class DefaultCategories
{
    public static List<Category> GetDefaultCategories()
    {
        return new List<Category>
        {
            new Category("Завтраки") { colorHex = "#FFD700" },
            new Category("Супы") { colorHex = "#FF6B6B" },
            new Category("Салаты") { colorHex = "#4CAF50" },
            new Category("Основные блюда") { colorHex = "#2196F3" },
            new Category("Гарниры") { colorHex = "#FF9800" },
            new Category("Выпечка") { colorHex = "#9C27B0" },
            new Category("Десерты") { colorHex = "#E91E63" },
            new Category("Напитки") { colorHex = "#00BCD4" },
            new Category("Соусы") { colorHex = "#795548" },
            new Category("Заготовки") { colorHex = "#607D8B" }
        };
    }
}