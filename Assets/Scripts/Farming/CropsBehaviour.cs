using UnityEngine;

public class CropsBehaviour : MonoBehaviour
{
    private CropData cropData;
    private Vector3Int tilePos;
    private FarmTileManager tileManager;
    public ToolItem cropToolItem;

    private float growthTimer; 
    private int growthStage; // 0 = seed, 1 = small, 2 = medium, 3 = grown
    private bool isWatered;

    public void Initialize(CropData data, Vector3Int pos, FarmTileManager manager)
    {
        cropData = data;
        tilePos = pos;
        tileManager = manager;

        growthStage = 0;
        isWatered = false;

        // Start with first stage timer
        growthTimer = cropData.timeToSmallSprout;
    }

    private void Update()
    {
        if (!isWatered) return; // stop growth if not watered

        if (growthTimer > 0f)
        {
            growthTimer -= Time.deltaTime;
        }
        else
        {
            AdvanceGrowthStage();
        }
    }

    private void AdvanceGrowthStage()
    {
        growthStage++;

        switch (growthStage)
        {
            case 1:
                GetComponent<SpriteRenderer>().sprite = cropData.smallSproutSprite;
                growthTimer = cropData.timeToMedium;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = cropData.mediumSproutSprite;
                growthTimer = cropData.timeToGrown;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = cropData.grownSprite;
                break;
        }
    }

    public void Water()
    {
        isWatered = true;
    }

    public bool IsGrown() => growthStage == 3;

    public ToolItem GetToolItem()
    {
        return cropToolItem; // returns the ToolItem for inventory
    }

}