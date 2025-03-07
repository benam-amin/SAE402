using UnityEngine;

public class Followed : MonoBehaviour
{
    [SerializeField] Transform follower;
    private Collider2D bc;

    [SerializeField]
    private float yDelta = 0; 
    private void Awake()
    {
        bc = GetComponent<Collider2D>();
        yDelta += Mathf.Abs(bc.bounds.center.y - follower.position.y);
    }

    void Update()
    {
        follower.position = new Vector3(
            bc.bounds.center.x,
            yDelta + bc.bounds.center.y,
            follower.position.z
        );
    }
}
