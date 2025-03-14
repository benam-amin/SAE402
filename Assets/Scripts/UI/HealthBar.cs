using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public PlayerData dataPlayer;
    public Gradient lifeColorGradient;

    // Update is called once per frame
    void Update()
    {
        float lifeRatio = dataPlayer.currentHealth / dataPlayer.maxHealth;
        fillImage.fillAmount = lifeRatio;
        fillImage.color = lifeColorGradient.Evaluate(lifeRatio);
    }
}