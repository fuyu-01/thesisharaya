using UnityEngine;
using System.Collections.Generic;

public class MapChunkManager : MonoBehaviour
{
    public static MapChunkManager Instance;

    [Header("Chunk References")]
    public List<GameObject> allChunks; // Assign MAP1, MAP2, MAP3, MAP4
    public GameObject activeChunk;

    [Header("References")]
    public Transform player;
    public Transform spawnPoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (spawnPoint == null)
        {
            spawnPoint = GameObject.Find("SpawnPoint")?.transform;
        }

        if (player != null && spawnPoint != null)
        {
            player.position = spawnPoint.position; // ✅ Move player to spawn
            Debug.Log($"Player spawned at {spawnPoint.position}");
        }

        // ✅ Find which chunk contains the spawn point
        GameObject startingChunk = FindNearestChunk(spawnPoint.position);
        if (startingChunk != null)
        {
            ActivateChunk(startingChunk);
            Debug.Log($"🌾 Starting chunk is {startingChunk.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ No chunk found near spawn point!");
        }
    }

    private GameObject FindNearestChunk(Vector3 position)
    {
        float closestDistance = float.MaxValue;
        GameObject nearest = null;

        foreach (var chunk in allChunks)
        {
            if (chunk == null) continue;
            float distance = Vector3.Distance(position, chunk.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = chunk;
            }
        }
        return nearest;
    }

    public void ActivateChunk(GameObject targetChunk)
    {
        foreach (var chunk in allChunks)
        {
            if (chunk == null) continue;

            bool shouldBeActive = chunk == targetChunk;
            chunk.SetActive(shouldBeActive);

            if (shouldBeActive)
            {
                activeChunk = chunk;
                Debug.Log($"✅ Activated chunk: {chunk.name}");
            }
        }
    }
}
