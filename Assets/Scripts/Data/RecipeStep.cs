using System;

[Serializable]
public class RecipeStep
{
    public string id;
    public int stepNumber;
    public string description;
    public string imagePath; // относительный путь к изображению
    public bool hasImage;

    public RecipeStep()
    {
        id = Guid.NewGuid().ToString();
    }

    public string GetStepTitle()
    {
        return $"Шаг {stepNumber}";
    }

    public bool HasValidImage()
    {
        return hasImage && !string.IsNullOrEmpty(imagePath);
    }
}