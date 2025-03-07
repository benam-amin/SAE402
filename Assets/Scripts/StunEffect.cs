using UnityEngine;

// https://forum.unity.com/threads/how-to-make-a-gameobject-rotate-and-move-around-another-gameobject-in-2d.1397125/
public class StunEffect : MonoBehaviour
{
    public float rotationSpeed = 5;

    [SerializeField]
    private float oscillationSpeed = 3;

    [SerializeField]
    private SpriteRenderer sr;

    public Transform pivot;

    private float xOffset = 0;
    private float zOffset = 0;

    [SerializeField]
    private float yAmplitude = -0.9f;


    private Vector3 startAngle;   //Reference to the object's original angle values
    private float rotationOffset = 50f; //Rotate by 50 units

    private float finalAngle;  //Keeping track of final angle to keep code cleaner

    public float phaseShift = 0;

    private void Start()
    {
        startAngle = transform.eulerAngles;

        xOffset = (transform.position - pivot.position).x;
        zOffset = (transform.position - pivot.position).z;
    }

    void Update()
    {
        float newY = (Mathf.Sin(Time.time * 5f) * yAmplitude) + pivot.position.y;

        float angle = Time.time * rotationSpeed * Mathf.Sign(xOffset);
        var positionCenterObject = pivot.position;

        var x = positionCenterObject.x + (Mathf.Cos(angle + phaseShift) * xOffset);
        var z = positionCenterObject.z + (Mathf.Sin(angle + phaseShift) * zOffset);
        transform.position = new Vector3(x, newY, z);

        finalAngle = startAngle.z + Mathf.Sin(Time.time * oscillationSpeed) * rotationOffset;  //Calculate animation angle
        transform.eulerAngles = new Vector3(startAngle.x, startAngle.y, finalAngle); //Apply new angle to object
    }

    public void ToggleVisiblity(bool isVisible)
    {
        if (isVisible)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.clear;
        }
    }
}
