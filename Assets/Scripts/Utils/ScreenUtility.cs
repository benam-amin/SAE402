using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenUtility : MonoBehaviour
{
    protected Camera cam;

    public static ScreenUtility Instance { get; protected set; }
    /// <summary>
    /// Left side of the screen in world coordinates
    /// </summary>
    public float Left
    {
        get
        {
            if (cam)
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;

            return 0.0f;
        }
    }

    /// <summary>
    /// Right side of the screen in world coordinates
    /// </summary>
    public float Right
    {
        get
        {
            if (cam)
                return cam.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;

            return 0.0f;
        }
    }

    /// <summary>
    /// Top side of the screen in world coordinates
    /// </summary>
    public float Top
    {
        get
        {
            if (cam)
                return cam.ViewportToWorldPoint(new Vector3(0f, 1.0f, 0f)).y;

            return 0.0f;
        }
    }

    /// <summary>
    /// Bottom side of the screen in world coordinates
    /// </summary>
    public float Bottom
    {
        get
        {
            if (cam)
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;

            return 0.0f;
        }
    }

    /// <summary>
    /// Middle of the screen in world coordinates
    /// </summary>
    public Vector3 Middle
    {
        get
        {
            if (cam)
            {
                return cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            }

            return Vector3.zero;
        }
    }

    private void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
        SetupScreen();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public float height = 0;
    public float width = 0;

    public void SetupScreen()
    {
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;
    }
}