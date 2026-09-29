using System.Collections.Generic;

public static class Units
{
    // Временный список (потом можно загружать из файла)
    private static List<string> categoryUnits = new List<string>
    {
        "г", "кг", "мл", "л", "шт", "ст.л.", "ч.л.", "стакан", "по вкусу"
    };
    private static List<string> weightUnits = new List<string>
    {
        "г", "кг", "мл", "л"
    };

    public static List<string> GetUnits(UnitsType type)
    {
        // Здесь в будущем можно читать из сохранений

        switch (type)
        {
            case UnitsType.Category:
                return new List<string>(categoryUnits);
                break;
            case UnitsType.Weight:
                return new List<string>(weightUnits);
                break;
            default:
                break;
        }

        return new List<string>(categoryUnits);
    }

    public enum UnitsType
    {
        Category,
        Weight
    }
}