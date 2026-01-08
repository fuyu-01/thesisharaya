using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarScript : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int slotIndex;
    [HideInInspector] public HotbarManager manager;

    [Header("Slot Item")]
    public GameObject itemObject; // Assign the in-game object this slot represents

    public void Initialize(HotbarManager hotbarManager, int index)
    {
        manager = hotbarManager;
        slotIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager != null)
            manager.SelectSlot(slotIndex);
    }

    public void ActivateItem(bool active)
    {
        if (itemObject != null)
            itemObject.SetActive(active);
    }
}
