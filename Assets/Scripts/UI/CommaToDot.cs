using UnityEngine;
using UnityEngine.UI;

public class CommaToDot : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    void Start()
    {
        inputField.onValueChanged.AddListener(ReplaceComma);
    }

    private void ReplaceComma(string text)
    {
        if (text.Contains(","))
        {
            inputField.text = text.Replace(',', '.');
            // Переместить курсор в конец
            inputField.caretPosition = inputField.text.Length;
        }
    }
}