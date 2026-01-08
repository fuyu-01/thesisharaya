using UnityEngine;

public class HotbarHighlight : MonoBehaviour
{
    [Tooltip("The visual object (Image) to enable/disable.")]
    public GameObject highlightVisual;

    private void Awake()
    {
        if (highlightVisual == null)
            highlightVisual = gameObject; // fallback
    }

    public void SetHighlight(bool active)
    {
        if (highlightVisual != null)
            highlightVisual.SetActive(active);
    }
}
