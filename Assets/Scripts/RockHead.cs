using UnityEngine;
using System.Collections;
using System.Linq;

public class RockHead : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D bc;

    private Vector3 destination;

    public RockHeadTrigger[] listTriggers;

    private int currentIndex = 0;

    public Animator animator;

    private string lastAnimationPlayed = "";

    private bool isOnScreen = false;

    private Collider2D[] listContacts;

    private float detectScale = 0.95f;

    private enum Movement
    {
        VerticalPositive,
        VerticalNegative,
        HorizontalNegative,
        HorizontalPositive,
    }

    private float crushThreshold = 10;

    private Movement currentMovement;

    public bool isReversed;

    [Header("ScriptableObjects")]
    public RockHeadData rockHeadData;

    [Header("Broadcast event channels")]
    public CameraShakeEventChannel cameraShake;
    public ShakeTypeVariable shakeInfo;

    void Start()
    {
        rb.mass = rockHeadData.mass;

        if (isReversed)
        {
            System.Array.Reverse(listTriggers);
        }
        Vector2 firstTriggerPosition = listTriggers[currentIndex].transform.position;

        Vector2 currentDestination = -((Vector2)transform.position - firstTriggerPosition).normalized;
        currentDestination.x = Mathf.Round(currentDestination.x);
        currentDestination.y = Mathf.Round(currentDestination.y);

        currentMovement = GetNextDirection(currentDestination);
        EnableTriggers();
        SetTriggersSibling();
        StartCoroutine(GoToTrigger(firstTriggerPosition));
    }

    void EnableTriggers()
    {
        for (int i = 0; i < listTriggers.Length; i++)
        {
            listTriggers[i].gameObject.SetActive(i == currentIndex);
        }
    }

    void SetTriggersSibling()
    {
        for (int i = 0; i < listTriggers.Length; i++)
        {
            listTriggers[i].rockHead = gameObject;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(destination * rockHeadData.speed, ForceMode2D.Impulse);

        Crush();
    }

    private void Crush()
    {
        switch (currentMovement)
        {
            case Movement.HorizontalNegative:
                listContacts = HasLeftContact();
                break;
            case Movement.HorizontalPositive:
                listContacts = HasRightContact();
                break;
            case Movement.VerticalNegative:
                listContacts = HasBottomContact();
                break;
            case Movement.VerticalPositive:
                listContacts = HasTopContact();
                break;
        }
        listContacts = listContacts.Where(item => item.transform != transform).ToArray();

        Collider2D[] player = listContacts.Where(item => item.transform.CompareTag("Player")).ToArray();

        if (listContacts.Length > 0 && rb.linearVelocity.sqrMagnitude > crushThreshold)
        {
            if (player.Length == 0 || (player.Length > 0 && listContacts.Length > 1))
            {
                CrushAnimation();
            }

            if (player.Length > 0)
            {
                PlayerContacts playerContacts = player[0].transform.GetComponent<PlayerContacts>();

                if (
                    (currentMovement == Movement.VerticalPositive && playerContacts.hasTopContact) ||
                    (currentMovement == Movement.VerticalNegative && playerContacts.hasBottomContact) ||
                    (currentMovement == Movement.HorizontalNegative && playerContacts.hasLeftContact) ||
                    (currentMovement == Movement.HorizontalPositive && playerContacts.hasRightContact)
                )
                {
                    PlayerHealth playerHealth = player[0].transform.GetComponent<PlayerHealth>();
                    playerHealth.TakeDamage(float.MaxValue);
                }
            }
        }
    }

    public void ChangeTrigger()
    {
        currentIndex = (currentIndex + 1) % listTriggers.Length;
        EnableTriggers();
        StartCoroutine(GoToTrigger(listTriggers[currentIndex].transform.position));
    }

    private IEnumerator GoToTrigger(Vector2 dir)
    {
        yield return new WaitForSeconds(Mathf.Abs(rockHeadData.delayBetweenMoves - rockHeadData.animationClip.length));
        animator.SetTrigger("Blinking");
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        destination = -((Vector2)transform.position - dir).normalized;
        destination.x = Mathf.Round(destination.x);
        destination.y = Mathf.Round(destination.y);
        currentMovement = GetNextDirection(destination);

        if (destination.x == 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    Movement GetNextDirection(Vector3 dir)
    {
        Movement tmpMovement = Movement.HorizontalPositive;

        if (dir.x > 0)
        {
            tmpMovement = Movement.HorizontalPositive;
        }
        else if (dir.x < 0)
        {
            tmpMovement = Movement.HorizontalNegative;
        }
        else if (dir.y > 0)
        {
            tmpMovement = Movement.VerticalPositive;
        }
        else if (dir.y < 0)
        {
            tmpMovement = Movement.VerticalNegative;
        }

        return tmpMovement;
    }

    private void CrushAnimation()
    {
        if (rb.linearVelocity.sqrMagnitude <= 0)
        {
            return;
        }

        if (currentMovement == Movement.VerticalPositive && lastAnimationPlayed != "HitTop")
        {
            OnCrush("HitTop");
        }
        else if (currentMovement == Movement.VerticalNegative && lastAnimationPlayed != "HitBottom")
        {
            OnCrush("HitBottom");
        }
        if (currentMovement == Movement.HorizontalPositive && lastAnimationPlayed != "HitRight")
        {
            OnCrush("HitRight");
        }
        else if (currentMovement == Movement.HorizontalNegative && lastAnimationPlayed != "HitLeft")
        {
            OnCrush("HitLeft");
        }
    }

    void OnCrush(string side)
    {
        animator.SetTrigger(side);
        lastAnimationPlayed = side;
        if (isOnScreen)
        {
            cameraShake.Raise(shakeInfo);
        }
        EnableTriggers();
    }

    void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    void OnBecameVisible()
    {
        isOnScreen = true;
    }

    public Collider2D[] HasLeftContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.min.x - rockHeadData.crushDistance / 2, bc.bounds.center.y),
            new Vector2(rockHeadData.crushDistance, bc.size.y * detectScale),
            0,
            rockHeadData.listContactsLayers
        );
    }

    public Collider2D[] HasRightContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.max.x + rockHeadData.crushDistance / 2, bc.bounds.center.y),
            new Vector2(rockHeadData.crushDistance, bc.size.y * detectScale),
            0,
            rockHeadData.listContactsLayers
        );
    }

    public Collider2D[] HasTopContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.center.x, bc.bounds.max.y + rockHeadData.crushDistance / 2),
            new Vector2(bc.size.x * detectScale, rockHeadData.crushDistance),
            0,
            rockHeadData.listContactsLayers
        );
    }

    public Collider2D[] HasBottomContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.center.x, bc.bounds.min.y - rockHeadData.crushDistance / 2),
            new Vector2(bc.size.x * detectScale, rockHeadData.crushDistance),
            0,
            rockHeadData.listContactsLayers
        );
    }

    private void OnDrawGizmos()
    {
        if (listTriggers[currentIndex])
        {
            Vector3 nextTriggerPosition = new Vector3(
                listTriggers[currentIndex].transform.position.x,
                listTriggers[currentIndex].transform.position.y,
                transform.position.z
            );
            Debug.DrawLine(transform.position, nextTriggerPosition, Color.green);
        }

        if (bc != null)
        {
            Gizmos.color = Color.red;

            switch (currentMovement)
            {
                case Movement.HorizontalNegative:
                    Gizmos.DrawWireCube(
                        new Vector2(bc.bounds.min.x - rockHeadData.crushDistance / 2, bc.bounds.center.y),
                        new Vector2(rockHeadData.crushDistance, bc.size.y * detectScale)
                    );
                    break;
                case Movement.HorizontalPositive:
                    Gizmos.DrawWireCube(
                            new Vector2(bc.bounds.max.x + rockHeadData.crushDistance / 2, bc.bounds.center.y),
                            new Vector2(rockHeadData.crushDistance, bc.size.y * detectScale)
                        );
                    break;
                case Movement.VerticalNegative:
                    Gizmos.DrawWireCube(
                        new Vector2(bc.bounds.center.x, bc.bounds.min.y - rockHeadData.crushDistance / 2),
                        new Vector2(bc.size.x * detectScale, rockHeadData.crushDistance)
                    );
                    break;
                case Movement.VerticalPositive:
                    Gizmos.DrawWireCube(
                        new Vector2(bc.bounds.center.x, bc.bounds.max.y + rockHeadData.crushDistance / 2),
                        new Vector2(bc.size.x * detectScale, rockHeadData.crushDistance)
                    );
                    break;
            }
        }
    }
}