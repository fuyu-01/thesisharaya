using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractionManager : MonoBehaviour
{
    public static TileInteractionManager Instance;

    [Header("References")]
    public FarmTileManager farmTileManager;
    public float interactionRadius = 5f; // range around player

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (farmTileManager == null)
            farmTileManager = FindObjectOfType<FarmTileManager>();
    }

    /// <summary>
    /// Called when the player touches a tile/farm area.
    /// Uses currently selected tool from ToolSelectionManager.
    /// </summary>
    public void InteractWithTile(Vector3 worldPos)
{
    if (ToolSelectionManager.Instance == null || ToolSelectionManager.Instance.selectedTool == null)
    {
        Debug.LogWarning(" No tool selected!");
        return;
    }

    ToolItem tool = ToolSelectionManager.Instance.selectedTool;
    Debug.Log($"Current tool: {tool.toolType}");

    switch (tool.toolType)
    {
        case ToolType.Hoe:
            farmTileManager.HoeTile(worldPos);
            break;

        case ToolType.WateringCan:
            farmTileManager.WaterTile(worldPos);
            break;

        case ToolType.SeedPack:
            if (tool.cropToPlant != null)
                farmTileManager.PlantSeed(worldPos, tool.cropToPlant);
            else
                Debug.LogWarning("⚠️ SeedPack tool has no cropToPlant assigned!");
            break;

        default:
            Debug.LogWarning($"⚠️ Tool '{tool.toolType}' has no interaction implemented!");
            break;
    }
}


            
    ///Plow tile near a position (used by Hoe tool)
    public void PlowNearbyTile(Vector3 worldPos)
    {
        farmTileManager.HoeTile(worldPos);
    }
    
    ///  Water tile near a position (used by WateringCan tool)
    public void WaterNearbyTile(Vector3 worldPos)
    {
        farmTileManager.WaterTile(worldPos);
    }

// Plant crop near a position (used by SeedPack tool)
    public void PlantNearbyTile(Vector3 worldPos, CropData crop)
    {
        farmTileManager.PlantSeed(worldPos, crop);
    }
}
