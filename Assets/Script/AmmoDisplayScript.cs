using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplayScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] WeaponHolder _weaponHolder;

    private void Update()
    {
        var weapon = _weaponHolder.GetCurrentWeapon();
        if (weapon != null)
        {
            _ammoText.text = $"{weapon.CurrentMag} / {weapon.CurrentReserve}";
        }
        else
        {
            _ammoText.text = " -- / -- ";
        }
    }
}
