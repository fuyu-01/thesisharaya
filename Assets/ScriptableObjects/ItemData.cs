using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[CreateAssetMenu(fileName ="Item Data", menuName ="Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
}
