using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;


public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    //public GameObject highlight;

    private ItemSlot currentSlot;

    public void SetSlot(ItemSlot slot)
    {
        Debug.Log($"SetSlot called with slot = {slot}");
        if (slot != null)
        {
            Debug.Log($"slot.item = {slot.item}");
            if (slot.item != null)
                Debug.Log($"slot.item.icon = {slot.item.itemIcon}");
        }
        Debug.Log($"iconImage = {iconImage}");
        Debug.Log($"quantityText = {quantityText}");
        currentSlot = slot;

        if (slot != null && slot.item != null)
        {
            iconImage.sprite = slot.item.itemIcon;
            iconImage.color = Color.white;
            quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
        }
        else
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
            quantityText.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //highlight.SetActive(true);
        // TODO: Show tooltip if you have one
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //highlight.SetActive(false);
        // TODO: Hide tooltip
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSlot != null && currentSlot.item != null)
        {
            Debug.Log($"Clicked {currentSlot.item.itemName}");
            // You could call inventory.UseItem() here if you like
        }
    }
}
