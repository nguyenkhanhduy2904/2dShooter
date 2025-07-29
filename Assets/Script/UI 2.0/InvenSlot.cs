using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    public Image icon; // Drag your Icon Image here
    public TextMeshProUGUI quantityText; // Drag your TMP text here

    public void SetSlot(Sprite itemSprite, int quantity)
    {
        icon.sprite = itemSprite;
        icon.enabled = true;
        quantityText.text = quantity > 1 ? quantity.ToString() : ""; // hide text if only 1
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }
}
