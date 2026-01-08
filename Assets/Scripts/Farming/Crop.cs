using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCrop", menuName = "Farming/CropData")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Sprite smallSproutSprite;
    public Sprite mediumSproutSprite;
    public Sprite grownSprite;

    [Header("Timers (seconds)")]
    public float timeToSmallSprout = 60f;   // 1 minute
    public float timeToMedium = 180f;       // 3 minutes
    public float timeToGrown = 180f;        // 3 minutes again

    [Header("Water Timer")]
    public float waterExpireTime = 180f; // time until the crop dries and dies if not watered
}
