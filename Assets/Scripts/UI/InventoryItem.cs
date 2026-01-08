using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image icon; // the icon image

    [HideInInspector] public InSlot currentSlot;

    // static = one shared selection across all items
    private static InventoryItem selectedItem;

    private void Start()
    {
        // try to find the slot this item is in
        currentSlot = GetComponentInParent<InSlot>();
    }

    public void AddItemToSlot(InSlot slot, Sprite iconSprite)
    {
        transform.SetParent(slot.transform, false); // parent to slot
        transform.localPosition = Vector3.zero;
        currentSlot = slot;
        icon.sprite = iconSprite;
        icon.enabled = true;
        slot.isFull = true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // if nothing selected yet
        if (selectedItem == null)
        {
            // select this one
            selectedItem = this;
            Highlight(true);
            Debug.Log("Selected " + name);
        }
        else if (selectedItem == this)
        {
            // tapped again → deselect
            selectedItem.Highlight(false);
            selectedItem = null;
            Debug.Log("Deselected " + name);
        }
        else
        {
            // tapped a different item → swap positions
            SwapItems(selectedItem, this);
            selectedItem.Highlight(false);
            selectedItem = null;
        }
    }

    private void SwapItems(InventoryItem first, InventoryItem second)
    {
        // swap parents
        Transform firstParent = first.transform.parent;
        Transform secondParent = second.transform.parent;

        first.transform.SetParent(secondParent);
        second.transform.SetParent(firstParent);

        // reset positions
        first.transform.localPosition = Vector3.zero;
        second.transform.localPosition = Vector3.zero;

        // update slot references
        first.currentSlot = secondParent.GetComponent<InSlot>();
        second.currentSlot = firstParent.GetComponent<InSlot>();

        Debug.Log($"Swapped {first.name} with {second.name}");
    }

    private void Highlight(bool active)
    {
        // simple visual feedback by scaling or color tint
        if (icon != null)
        {
            icon.color = active ? new Color(1f, 1f, 0.6f, 1f) : Color.white;
        }
    }
}
