using UnityEngine;

public class InventoryTapManager : MonoBehaviour
{
    public static InventoryTapManager Instance { get; private set; }

    public InSlot selectedSlot;   // the slot that was tapped first
    public ToolItem selectedTool; // the ToolItem we’re carrying

    private void Awake()
    {
        Instance = this;
    }

    public void SelectSlot(InSlot slot)
    {
        // Tap the same slot again to cancel selection
        if (selectedSlot == slot)
        {
            ClearSelection();
            return;
        }

        // If nothing selected yet, pick this slot up
        if (selectedSlot == null && slot.isFull)
        {
            selectedSlot = slot;
            selectedTool = slot.GetItem();
            return;
        }

        // If we already have an item selected and this one is empty → move
        if (selectedSlot != null && !slot.isFull)
        {
            slot.AddItem(selectedTool);
            selectedSlot.ClearSlot();
            ClearSelection();
            return;
        }

        // If both full → swap
        if (selectedSlot != null && slot.isFull)
        {
            ToolItem temp = slot.GetItem();
            slot.AddItem(selectedTool);
            selectedSlot.AddItem(temp);
            ClearSelection();
        }
    }

    public void ClearSelection()
    {
        selectedSlot = null;
        selectedTool = null;
    }
}
