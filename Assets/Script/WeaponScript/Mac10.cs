using UnityEngine;

public class Mac10 : Weapon
{
    public override bool IsAutomatic => true; // auto


    // Optional: override TryShoot if it behaves differently
    public override void TryShoot(Vector2 direction)
    {
        base.TryShoot(direction);
        // Add Mac10-specific behavior here if needed
    }
}
