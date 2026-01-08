using UnityEngine;

public class SlotSelectionManager : MonoBehaviour
{
    public static SlotSelectionManager Instance;
    private InSlot selectedSlot;

    private void Awake() => Instance = this;

    public void SelectSlot(InSlot slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetHighlight(false);

        selectedSlot = slot;

        if (selectedSlot != null)
            selectedSlot.SetHighlight(true);
    }

    public InSlot GetSelectedSlot() => selectedSlot;
}
