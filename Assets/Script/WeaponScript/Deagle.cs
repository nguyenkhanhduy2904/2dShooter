using UnityEngine;

public class Deagle : Weapon
{
    public override bool IsAutomatic => false; // semi-auto


    // Optional: override TryShoot if it behaves differently
    public override void TryShoot(Vector2 direction)
    {
        base.TryShoot(direction);
        // Add DE-specific behavior here if needed
    }
}
