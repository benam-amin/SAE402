using UnityEngine;

public class AdjustSize : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f; // Ratio 16:9
    public Vector2 originalSize = new Vector2(1f, 1f); // Taille d'origine du GameObject

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // On récupère le RectTransform du GameObject
        AdjustObjectSize();
    }

    void Update()
    {
        // Ajuster la taille à chaque frame, au cas où la taille de l'écran changerait
        AdjustObjectSize();
    }

    void AdjustObjectSize()
    {
        // On calcule l'aspect ratio de l'écran
        float screenAspectRatio = (float)Screen.width / Screen.height;

        // Si l'aspect ratio de l'écran est différent du ratio 16:9
        if (screenAspectRatio > targetAspectRatio)
        {
            // Si l'écran est plus large que 16:9, ajuster la taille en fonction de la largeur de l'écran
            float newWidth = originalSize.x * (Screen.height / 9f);
            float newHeight = newWidth / targetAspectRatio;
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }
        else
        {
            // Si l'écran est plus petit que 16:9, ajuster la taille en fonction de la hauteur de l'écran
            float newHeight = originalSize.y * (Screen.width / 16f);
            float newWidth = newHeight * targetAspectRatio;
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }
    }
}
