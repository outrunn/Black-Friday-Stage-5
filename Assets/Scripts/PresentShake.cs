using UnityEngine;

public class PresentShake : MonoBehaviour
{
    public float shakeAmount = 0.15f;   // how far it moves
    public float shakeSpeed = 6f;       // how fast it shakes

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }
}
