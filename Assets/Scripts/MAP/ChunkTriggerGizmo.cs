using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Collider2D))]
public class ChunkTriggerGizmo : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.25f); // default: transparent green
    public bool showLabel = true;

    private Collider2D col;

    private void OnDrawGizmos()
    {
        if (col == null)
            col = GetComponent<Collider2D>();

        if (col == null)
            return;

        Gizmos.color = gizmoColor;

        // Draw box if collider is a BoxCollider2D
        if (col is BoxCollider2D box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);
            Gizmos.DrawWireCube(box.offset, box.size);
        }
        // Draw circle if collider is a CircleCollider2D
        else if (col is CircleCollider2D circle)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(circle.offset, circle.radius);
        }

        // Optional: draw label
        if (showLabel)
        {
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"Chunk: {gameObject.name}");
        }
    }
}
