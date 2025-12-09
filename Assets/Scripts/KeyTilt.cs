using System.Collections;
using UnityEngine;

public class KeyJingle : MonoBehaviour
{
    public float intensity = 0.05f;
    public float duration = 0.15f;
    public float interval = 1.5f;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
        InvokeRepeating(nameof(DoJingle), 0, interval);
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
