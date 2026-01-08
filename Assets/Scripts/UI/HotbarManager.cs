using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HotbarManager : MonoBehaviour
{
    [Header("Hotbar Setup")]
    public HotbarScript[] slots;          // All slots
    public RectTransform highlight;       // The single highlight image (RectTransform)
    public float moveSpeed = 20f;         // Optional for smooth movement

    private int selectedIndex = -1;

    private void Start()
    {
        // Initialize each slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].Initialize(this, i);
        }

        if (highlight != null)
            highlight.gameObject.SetActive(false);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        selectedIndex = index;

        // Move highlight
        if (highlight != null)
        {
            highlight.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(MoveHighlight(slots[index].transform.position));
        }

        UpdateActiveItem();
    }

    private System.Collections.IEnumerator MoveHighlight(Vector3 targetPosition)
    {
        while (Vector3.Distance(highlight.position, targetPosition) > 0.1f)
        {
            highlight.position = Vector3.Lerp(highlight.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }
        highlight.position = targetPosition;
    }

    private void UpdateActiveItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool active = (i == selectedIndex);
            slots[i].ActivateItem(active);
        }
    }

    public bool AddItemToHotbarByIcon(Sprite itemIcon)
{
    if (itemIcon == null)
        return false;

    //  Check if this icon already exists in any slot
    for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i] != null && slots[i].itemObject != null)
        {
            Image existingImage = slots[i].itemObject.GetComponent<Image>();
            if (existingImage != null && existingImage.sprite == itemIcon)
            {
                Debug.Log("Item already exists in hotbar: " + itemIcon.name);
                return false;
            }
        }
    }

    // 🔹 Add to the first empty slot
    for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i] != null && (slots[i].itemObject == null || !slots[i].itemObject.activeSelf))
        {
            GameObject newItem = new GameObject(itemIcon.name);
            Image image = newItem.AddComponent<Image>();
            image.sprite = itemIcon;
            //image.color = Color.white;
            image.rectTransform.sizeDelta = new Vector2(64, 64);
            newItem.transform.SetParent(slots[i].transform, false);

            slots[i].itemObject = newItem;

            // 🔹 Automatically highlight this slot
            SelectSlot(i);

            return true;
        }
    }

    Debug.LogWarning("⚠️ No empty hotbar slot available for icon: " + itemIcon.name);
    return false;
}

}
