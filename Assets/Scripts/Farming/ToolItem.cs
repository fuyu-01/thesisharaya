using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public enum ToolType
{
    Hoe,
    WateringCan,
    SeedPack
}

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory/Tool Item")]
public class ToolItem : Item
{
    public ToolType toolType; // Hoe, WateringCan, SeedPack
    public Sprite icon;       // UI icon

    [Header("SeedPack Only")]
    public CropData cropToPlant; // assign the crop this SeedPack plants

    public override void Use()
    {
        PlayerInventoryManager player = GameObject.FindObjectOfType<PlayerInventoryManager>();
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        TileInteractionManager tileManager = TileInteractionManager.Instance;
        if (tileManager == null) return;

        switch (toolType)
        {
            case ToolType.Hoe:
                tileManager.PlowNearbyTile(playerPos);
                break;
            case ToolType.WateringCan:
                tileManager.WaterNearbyTile(playerPos);
                break;
            case ToolType.SeedPack:
                if (cropToPlant != null)
                    tileManager.PlantNearbyTile(playerPos, cropToPlant);
                break;
        }
    }

}
