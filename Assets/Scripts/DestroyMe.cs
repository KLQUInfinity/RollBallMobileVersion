using UnityEngine;
using System.Collections;

public class DestroyMe : MonoBehaviour
{
    public float duration = 1.25f;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - startTime > duration)
        {
            Destroy(gameObject);
        }
    }
}
