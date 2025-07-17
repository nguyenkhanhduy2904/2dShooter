using System.Collections;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject healParticlePrefab; // Drag prefab here in Inspector

    public IEnumerator SpawnHealEffect(Transform parent)
    {
        var healFX = Instantiate(healParticlePrefab, parent.position, Quaternion.identity);
        healFX.transform.SetParent(parent);
        healFX.transform.localPosition = Vector3.zero;

        // Optionally play particle system if not set to play on awake
        var ps = healFX.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        yield return new WaitForSeconds(2.5f);

        if (ps != null)
        {
            ps.Stop();
        }

        // Optionally destroy after lifetime
        Destroy(healFX, 1f);
    }

}
