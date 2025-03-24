using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public PlayerData dataPlayer;
    public GameObject heartPrefab; // Préfab du cœur
    public Transform heartsContainer; // Conteneur des cœurs

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        dataPlayer.currentHealth = dataPlayer.maxHealth;
        UpdateHearts();
    }


    public void UpdateHearts()
{
    if (heartPrefab == null || heartsContainer == null)
    {
        Debug.LogError("heartPrefab ou heartsContainer n'est pas assigné dans l'Inspector !");
        return;
    }

    // Supprimer les anciens cœurs
    foreach (GameObject heart in hearts)
    {
        Destroy(heart);
    }
    hearts.Clear();

    // Ajouter les nouveaux cœurs
    for (int i = 0; i < dataPlayer.maxHealth; i++)
    {
        GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
        Image heartImage = newHeart.GetComponent<Image>();

        if (heartImage == null)
        {
            Debug.LogError("Le Prefab de cœur doit contenir un composant Image !");
            return;
        }

        heartImage.color = (i < dataPlayer.currentHealth) ? Color.white : Color.gray;
        hearts.Add(newHeart);
    }
}

}
