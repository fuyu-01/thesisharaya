using UnityEngine;
using UnityEngine.EventSystems;

public class ToolInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionRadius = 1.5f;
    public LayerMask tileLayer; // assign layer of your tilemap here

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                TryInteract(touch.position, touch.fingerId);
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract(Input.mousePosition, -1);
        }
#endif
    }

    private void TryInteract(Vector2 screenPos, int fingerId)
    {
        if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        if (Vector3.Distance(player.position, worldPos) > interactionRadius) return;

        // Physics2D.Raycast for composite collider
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, tileLayer);
        if (hit.collider != null)
        {
            TileInteractionManager.Instance?.InteractWithTile(worldPos);
            Debug.Log($"Interacted with tile at {worldPos}");
        }
    }
}
