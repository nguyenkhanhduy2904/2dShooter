using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetTransparentTiles : MonoBehaviour
{
    [Tooltip("Tilemaps to make transparent")]
    public List<Tilemap> tilemapsToFade;

    [Tooltip("Transparency when player is near (0 = invisible, 1 = opaque)")]
    [Range(0f, 1f)]
    public float targetAlpha = 0.4f;

    [Tooltip("Restore to full opacity when player leaves")]
    public bool restoreOnExit = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetAlpha(targetAlpha);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && restoreOnExit)
            SetAlpha(1f); // Fully visible again
    }

    private void SetAlpha(float alpha)
    {
        foreach (var tilemap in tilemapsToFade)
        {
            tilemap.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
