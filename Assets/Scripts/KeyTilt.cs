using System.Collections;
using UnityEngine;

public class KeyJingle : MonoBehaviour
{
    public float intensity = 0.05f;
    public float duration = 0.15f;
    public float interval = 1.5f;

    private Vector3 originalPos;

    private void OnEnable()
    {
        // Wait 1 frame to allow GameStateManager to place the key
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        // Wait until the key is fully repositioned
        yield return null;

        originalPos = transform.localPosition;

        // Start periodic jingle
        InvokeRepeating(nameof(DoJingle), interval, interval);
    }

    private void OnDisable()
    {
        // Stop jingles when key is hidden
        CancelInvoke();
        StopAllCoroutines();
    }

    void DoJingle()
    {
        StopAllCoroutines();
        StartCoroutine(JingleRoutine());
    }

    IEnumerator JingleRoutine()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localPosition = originalPos + (Random.insideUnitSphere * intensity);
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
