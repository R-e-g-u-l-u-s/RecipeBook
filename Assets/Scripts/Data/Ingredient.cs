using System;

[Serializable]
public class Ingredient
{
    public string id;
    public string name;
    public float amount;
    public string unit; // "г", "мл", "шт", "ст.л." и т.д.
    public string notes; // дополнительные заметки (например, "мелко нарезанный")

    public Ingredient()
    {
        id = Guid.NewGuid().ToString();
    }

    // Для красивого отображения
    public string GetDisplayText()
    {
        string result = "";

        if (amount > 0)
        {
            result += amount.ToString("0.##") + " ";
        }

        if (!string.IsNullOrEmpty(unit))
        {
            result += unit + " ";
        }

        result += name;

        if (!string.IsNullOrEmpty(notes))
        {
            result += $" ({notes})";
        }

        return result;
    }
}