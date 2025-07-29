using UnityEngine;

public class ParticleFXManager : MonoBehaviour
{
    public static ParticleFXManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayParticleFX(GameObject prefab, Transform attachTo, float duration)
    {
        if (prefab == null || attachTo == null)
            return;

        GameObject fx = Instantiate(prefab, attachTo.position, Quaternion.identity);
        fx.transform.SetParent(attachTo);
        fx.transform.localPosition = Vector3.zero;

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            //Destroy(fx, duration); // fallback
        }
    }

}
