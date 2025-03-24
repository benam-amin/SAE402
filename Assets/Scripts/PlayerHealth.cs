using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;

    public SpriteRenderer sr;

    public PlayerInvulnerable playerInvulnerable;

    [Tooltip("Please uncheck it on production")]
    public bool needResetHP = true;

    [Header("ScriptableObjects")]
    public PlayerData playerData;

    [Header("Debug")]
    public VoidEventChannel onDebugDeathEvent;

    [Header("Broadcast event channels")]
    public VoidEventChannel onPlayerDeath;
    private int NmbPommeRamasse = 0;
     public HealthSystem healthSystem;

    private void Awake()
    {
        if (needResetHP || playerData.currentHealth <= 0)
        {
            playerData.currentHealth = playerData.maxHealth;
        }
    }

    private void OnEnable()
    {
        onDebugDeathEvent.OnEventRaised += Die;
    }

    public void TakeDamage(float damage)
    {
        if (playerInvulnerable.isInvulnerable && damage < float.MaxValue) return;

        playerData.currentHealth -= damage;
        healthSystem.UpdateHearts();
        if (playerData.currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(playerInvulnerable.Invulnerable());
        }
    }

    private void Die()
    {
        onPlayerDeath?.Raise();
        GetComponent<Rigidbody2D>().simulated = false;
        transform.Rotate(0f, 0f, 45f);
        animator.SetTrigger("Death");
    }

    public void OnPlayerDeathAnimationCallback()
    {
        sr.enabled = false;
    }

    private void OnDisable()
    {
        onDebugDeathEvent.OnEventRaised -= Die;
    }
    public void NombrePommeCollec ()
    {
        NmbPommeRamasse++;
        Debug.Log("Pommes collectées : " + NmbPommeRamasse);
        if (NmbPommeRamasse >= 10){
            GainVie();
            NmbPommeRamasse = 0;
        }
    }
    private void GainVie()
    {
        playerData.currentHealth += 1;
        healthSystem.UpdateHearts();
        if (playerData.currentHealth > playerData.maxHealth)
        {
            playerData.currentHealth = playerData.maxHealth; // Empêche d'avoir plus que la vie max
        }
        Debug.Log("Vie restaurée ! Points de vie actuels : " + playerData.currentHealth);
    }
}
