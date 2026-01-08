using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item" )]
public class Item : ScriptableObject
{
    public String itemName;
    public Sprite itemIcon;

    public virtual void Use()
    {
        Debug.Log("Using item: " + itemName);
    }
}
