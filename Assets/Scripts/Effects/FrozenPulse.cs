using UnityEngine;

public class FrozenPulse : MonoBehaviour
{
    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * 4f) * 0.05f;
        transform.localScale = baseScale * pulse;
    }
}
