using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerContacts : MonoBehaviour
{
    public BoxCollider2D bc;

    [Header("Crush detection")]
    public LayerMask listContacts;
    public float crushLengthDetection = 0.25f;
    public float boxCastScaleX = 0.95f;
    public float boxCastScaleY = 0.95f;

    public bool hasLeftContact = false;
    public bool hasTopContact = false;
    public bool hasBottomContact = false;
    public bool hasRightContact = false;

    private void FixedUpdate()
    {
        hasLeftContact = HasLeftContact();
        hasTopContact = HasTopContact();
        hasBottomContact = HasBottomContact();
        hasRightContact = HasRightContact();
    }

    private bool HasTopContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.center.x, bc.bounds.center.y + (crushLengthDetection / 2) + 0.05f),
            new Vector2(bc.size.x * boxCastScaleX, bc.size.y + crushLengthDetection - 0.05f),
            0,
            listContacts
        ).Length == 1;
    }

    private bool HasBottomContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.center.x, bc.bounds.center.y - (crushLengthDetection / 2) - 0.05f),
            new Vector2(bc.size.x * boxCastScaleX, bc.size.y + crushLengthDetection + 0.05f),
            0,
            listContacts
        ).Length == 1;
    }

    private bool HasLeftContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.center.x - (crushLengthDetection / 2), bc.bounds.center.y),
            new Vector2(crushLengthDetection + bc.size.x, bc.size.y * boxCastScaleY),
            0,
            listContacts
        ).Length == 1;
    }

    private bool HasRightContact()
    {
        return Physics2D.OverlapBoxAll(
            new Vector2(bc.bounds.center.x + (crushLengthDetection / 2), bc.bounds.center.y),
            new Vector2(crushLengthDetection + bc.size.x, bc.size.y * boxCastScaleY),
            0,
            listContacts
        ).Length == 1;
    }

    public Vector3 GetTilePositionUnderFeet(LayerMask layerMask)
    {
        Collider2D collider2d = Physics2D.OverlapBox(
            new Vector2(bc.bounds.center.x, bc.bounds.center.y - (crushLengthDetection / 2) - 0.05f),
            new Vector2(bc.size.x * boxCastScaleX, bc.size.y + crushLengthDetection + 0.05f),
            0,
            layerMask
        );

        if (collider2d == null)
        {
            return Vector3.zero;
        }

        Tilemap tilemap = collider2d.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            return Vector3.zero;
        }

        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        Vector3 tilepos = tilemap.GetCellCenterWorld(cellPosition);

        return tilepos;
    }

    public PlatformEffector2D GetTilePlatformEffector(LayerMask layerMask)
    {
        Collider2D collider2d = Physics2D.OverlapBox(
            new Vector2(bc.bounds.center.x, bc.bounds.center.y - (crushLengthDetection / 2) - 0.05f),
            new Vector2(bc.size.x * boxCastScaleX, bc.size.y + crushLengthDetection + 0.05f),
            0,
            layerMask
        );

        PlatformEffector2D platformEffector2D = collider2d.GetComponent<PlatformEffector2D>();
        if (platformEffector2D == null)
        {
            return null;
        }

        return platformEffector2D;
    }

    void OnDrawGizmos()
    {
        if (bc != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x - (crushLengthDetection / 2), bc.bounds.center.y),
                new Vector2(crushLengthDetection + bc.size.x, bc.size.y * boxCastScaleY)
            );
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x + (crushLengthDetection / 2), bc.bounds.center.y),
                new Vector2(crushLengthDetection + bc.size.x, bc.size.y * boxCastScaleY)
            );

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x, bc.bounds.center.y - (crushLengthDetection / 2) - 0.05f),
                new Vector2(bc.size.x * boxCastScaleX, bc.size.y + crushLengthDetection + 0.05f)
            );
            // Top
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x, bc.bounds.center.y + (crushLengthDetection / 2) + 0.05f),
                new Vector2(bc.size.x * boxCastScaleX, bc.size.y + crushLengthDetection - 0.05f)
            );
        }
    }
}
