using UnityEngine;

[System.Serializable]
public class WeaponAmmoState
{
    public WeaponData weaponData;
    public int currentMag;
    public int currentReserve;

    public WeaponAmmoState(WeaponData data, int mag, int reserve)
    {
        weaponData = data;
        currentMag = mag;
        currentReserve = reserve;
    }
}
