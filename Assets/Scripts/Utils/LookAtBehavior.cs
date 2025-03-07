using UnityEngine;

/**
* @description Allow any Gameobject to look towards another one according to a distance threshold
**/
public class LookAtBehavior : MonoBehaviour
{
    public Transform target;
    public bool isFacingRight = true;
    public float thresholdDistanceBeforeLookAt = float.MaxValue;

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            return;

        if(Vector2.Distance(target.position, transform.position) > thresholdDistanceBeforeLookAt) {
            return;
        }

        if (target.position.x > transform.position.x && !isFacingRight || target.position.x < transform.position.x && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
