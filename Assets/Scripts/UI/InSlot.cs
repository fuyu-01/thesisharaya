using UnityEngine;
using UnityEngine.UI;

public class InSlot : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public GameObject highlight;

    [Header("Slot State")]
    public bool isFull;
    public ToolItem currentTool;   // ✅ Needed for HotbarButton
    public Item currentItem;       // optional, if you add non-tool items later

    private ToolItem storedTool;   // internal reference

    // --- Add a tool into this slot ---
    public void AddItem(ToolItem tool)
    {
        storedTool = tool;
        currentTool = tool;   // ✅ keep public reference up to date
        currentItem = null;   // clear non-tool item

        if (icon != null && tool != null)
        {
            icon.sprite = tool.icon;
            icon.enabled = true;
        }

        isFull = true;
    }

    // --- Remove the tool from this slot ---
    public void ClearSlot()
    {
        storedTool = null;
        currentTool = null;   // ✅ clear reference for HotbarButton
        currentItem = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        isFull = false;
        SetHighlight(false);
    }

    public void SetHighlight(bool value)
    {
        if (highlight != null)
            highlight.SetActive(value);
    }

    // --- Called by UI button click ---
    public void OnSlotClicked()
    {
        if (!isFull)
        {
            Debug.Log("⚠️ Slot empty — nothing to equip");
            return;
        }

        SlotSelectionManager.Instance?.SelectSlot(this);

        if (currentTool != null && ToolSelectionManager.Instance != null)
        {
            ToolSelectionManager.Instance.SelectTool(currentTool);
            Debug.Log($"✅ Equipped tool: {currentTool.itemName}");
        }
    }

    public ToolItem GetItem()
    {
        return storedTool;
    }
}
