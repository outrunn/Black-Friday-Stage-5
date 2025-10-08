using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomPulse : MonoBehaviour
{
    public Volume volume;          // Assign your Global Volume here
    public float speed = 2f;       // Pulse speed
    public float minIntensity = 1f;
    public float maxIntensity = 5f;

    private Bloom bloom;

    void Start()
    {
        // Get the Bloom override from the volume
        if (volume.profile.TryGet<Bloom>(out Bloom b))
        {
            bloom = b;
        }
        else
        {
            Debug.LogError("No Bloom override found in the volume!");
        }
    }

    void Update()
    {
        if (bloom != null)
        {
            // Oscillate intensity
            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            bloom.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, smoothT);
        }
    }
}
