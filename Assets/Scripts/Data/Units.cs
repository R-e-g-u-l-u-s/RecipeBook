using System.Collections.Generic;

public static class Units
{
    // Временный список (потом можно загружать из файла)
    private static List<string> defaultUnits = new List<string>
    {
        "г", "кг", "мл", "л", "шт", "ст.л.", "ч.л.", "стакан", "по вкусу"
    };

    public static List<string> GetUnits()
    {
        // Здесь в будущем можно читать из сохранений
        return new List<string>(defaultUnits);
    }
}