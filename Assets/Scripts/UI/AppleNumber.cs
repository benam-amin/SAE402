using UnityEngine;
using TMPro;

public class AppleNumber : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    public TMP_Text displayAppleNumber;  // Remplace TextField par Text
    private int numberOfApple;

    void Start()
    {
        numberOfApple = playerHealth.AppleCollectedNumber;
        UpdateAppleDisplay();
    }

    void Update()
    {
        if (playerHealth.AppleCollectedNumber != numberOfApple)
        {
            numberOfApple = playerHealth.AppleCollectedNumber;
            UpdateAppleDisplay();
        }
    }

    private void UpdateAppleDisplay()
    {
        displayAppleNumber.text = "x " + numberOfApple.ToString();
    }
}
