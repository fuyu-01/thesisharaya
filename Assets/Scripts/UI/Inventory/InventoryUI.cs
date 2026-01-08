using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<InSlot> slots; // assign in inspector

    public void AddToolItem(ToolItem tool)
    {
        foreach (var slot in slots)
        {
            if (!slot.isFull)
            {
                slot.AddItem(tool);
                return;
            }
        }
        Debug.Log("No empty slot available for " + tool.itemName);
    }
}
