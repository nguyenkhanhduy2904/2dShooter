using UnityEngine;

public class Glock18 : Weapon
{
    public override bool IsAutomatic => false; // semi-auto


    // Optional: override TryShoot if it behaves differently
    public override void TryShoot(Vector2 direction)
    {
        base.TryShoot(direction);
        // Add Glock-specific behavior here if needed
    }

}
