using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class ChunkTrigger : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject chunkRoot;          // everything for this chunk (tilemaps, props, npcs, etc.)
    public string playerTag = "Player";
    public bool autoFindRoot = true;

    private void Start()
    {
        if (autoFindRoot && chunkRoot == null)
        {
            chunkRoot = transform.parent != null ? transform.parent.gameObject : gameObject;
        }

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Deactivate initially (except maybe starting chunk)
        if (chunkRoot != null && !chunkRoot.name.Contains("Start"))
            chunkRoot.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Activate this chunk
        if (chunkRoot != null)
        {
            chunkRoot.SetActive(true);
            Debug.Log($"🌾 Loaded chunk: {chunkRoot.name}");
        }

        // Deactivate others via manager
        if (MapChunkManager.Instance != null)
            MapChunkManager.Instance.ActivateChunk(chunkRoot);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        Debug.Log($"🚪 Player left trigger zone of {chunkRoot.name}");
    }
}
