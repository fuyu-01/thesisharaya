using UnityEngine;

public class ToolSelectionManager : MonoBehaviour
{
    public static ToolSelectionManager Instance;
    public ToolItem selectedTool; // starts as null

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SelectTool(ToolItem tool)
    {
        selectedTool = tool;

        if (tool != null)
            Debug.Log($" Selected Tool: {tool.toolType}");
        else
            Debug.Log("No tool selected!");
    }
}
