using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Listen to event channels")]
    public VoidEventChannel onPlayerDeath;
    public GameObject deathMenuUI;
    public GameObject lifePointsUI;

    private void Awake()
    {
        deathMenuUI.SetActive(false);
    }

    private void OnEnable()
    {
        onPlayerDeath.OnEventRaised += OnGameOver;
    }

    public void OnGameOver()
    {
        Debug.Log("<size=15><color=#FF0000><b>GameOver!</b></color></size>");
        deathMenuUI.SetActive(true);
        lifePointsUI.SetActive(false);
    }

    private void OnDisable()
    {
        onPlayerDeath.OnEventRaised -= OnGameOver;
    }
}
