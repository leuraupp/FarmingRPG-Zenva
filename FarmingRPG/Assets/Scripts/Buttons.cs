using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {
    public Button button; // Referência ao componente Button
    public Color selectedColor; // Cor para quando o botão estiver selecionado

    private Color originalColor; // Cor original do botão

    private void Start() {
        originalColor = button.colors.normalColor; // Salva a cor original do botão
        button.onClick.AddListener(SelectButton); // Adiciona o listener para o evento de clique no botão
        DeselectButton();
    }

    private void SelectButton() {
        ColorBlock cb = button.colors;
        cb.normalColor = originalColor; // Define a cor selecionada
        button.colors = cb;
    }

    public void DeselectButton() {
        ColorBlock cb = button.colors;
        cb.normalColor = selectedColor; // Define a cor original
        button.colors = cb;
    }
}
