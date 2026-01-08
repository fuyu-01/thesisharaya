using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(InSlot))]
public class HotbarButton : MonoBehaviour
{
    private Button button;
    private InSlot slot;

    private void Awake()
    {
        button = GetComponent<Button>();
        slot = GetComponent<InSlot>();

        // ensure listener only added once
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (slot == null)
        {
            Debug.LogWarning("⚠️ No InSlot found on this hotbar button!");
            return;
        }

        ToolItem tool = slot.GetItem();
        if (tool != null)
        {
            ToolSelectionManager.Instance.SelectTool(tool);
            Debug.Log($"🪓 Selected tool from hotbar: {tool.itemName}");
        }
        else
        {
            ToolSelectionManager.Instance.SelectTool(null);
            Debug.Log("❌ Deselected tool — slot empty");
        }
    }
}
