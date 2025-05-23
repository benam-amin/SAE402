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

    public int AppleCollectedNumber = 0;

    public HealthSystem healthSystem;
    public CameraShakeEventChannel cameraShake;

    [Header("Shake effect")]
    public ShakeTypeVariable shakeInfo;

    private bool isOnScreen = false;

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

        if (isOnScreen)
        {
            cameraShake?.Raise(shakeInfo);
        }

        if (playerData.currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(playerInvulnerable.Invulnerable());
        }
    }

#if UNITY_EDITOR
        //permet de tester la mort
        private void Update() {
            if(Input.GetKeyDown(KeyCode.M)) {
                InstantDeath();
            }
        }
        private void InstantDeath() {
                playerData.currentHealth = 0;
                Die();
            }
#endif

    private void Die()
    {
        onPlayerDeath?.Raise();
        GetComponent<Rigidbody2D>().simulated = false;
        transform.Rotate(0f, 0f, 45f);
        animator.SetTrigger("Death");

        if (isOnScreen)
        {
            cameraShake?.Raise(shakeInfo);
        }
    }

    public void OnPlayerDeathAnimationCallback()
    {
        sr.enabled = false;
    }

    private void OnDisable()
    {
        onDebugDeathEvent.OnEventRaised -= Die;
    }

    public void AppleCollected ()
    {
        AppleCollectedNumber++;
        Debug.Log("Pommes collectées : " + AppleCollectedNumber);
        if (AppleCollectedNumber % 15 == 0){
            GainHeart();
        }
    }

    private void GainHeart()
    {
        playerData.currentHealth += 1;
        healthSystem.UpdateHearts();
        if (playerData.currentHealth > playerData.maxHealth)
        {
            playerData.currentHealth = playerData.maxHealth;
        }
        Debug.Log("Vie restaurée ! Points de vie actuels : " + playerData.currentHealth);
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }
}
