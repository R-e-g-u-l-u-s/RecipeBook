using System.Collections.Generic;

public static class Units
{
    // ¬ременный список (потом можно загружать из файла)
    private static List<string> defaultUnits = new List<string>
    {
        "г", "кг", "мл", "л", "шт", "ст.л.", "ч.л.", "стакан", "по вкусу"
    };
    public static List<string> GetUnits()
    {
        // «десь в будущем можно читать из сохранений
        return new List<string>(defaultUnits);
    }
    public static List<string> GetWeightUnits()
    {
        return new List<string> { "г", "кг", "мл", "л"};
    }
}