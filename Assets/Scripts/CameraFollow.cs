using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    public float smoothTime = 0.25f;

    [SerializeField]
    private Transform target;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition = GetNextPosition();
        transform.position = nextPosition;
    }

    void LateUpdate()
    {
        if (!target) return;

        nextPosition = GetNextPosition();
        transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, smoothTime);
    }

    public Vector3 GetNextPosition()
    {
        return target.position + new Vector3(
            offset.x * (target.localEulerAngles.y > 90 ? -1 : 1),
            offset.y,
            transform.position.z
        );
    }
}