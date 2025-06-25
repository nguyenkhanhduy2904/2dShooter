using UnityEngine;
using System.Collections;

public class AK47 : Weapon
{
    public override bool IsAutomatic => true; // auto


    // Optional: override TryShoot if it behaves differently
    public override void TryShoot(Vector2 direction)
    {
        base.TryShoot(direction);
        // Add ak-specific behavior here if needed
    }
}
