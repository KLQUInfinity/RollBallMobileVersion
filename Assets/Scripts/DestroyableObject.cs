using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour
{
    public float forceRequired = 10.0f;
    public GameObject particle;

    private void OnCollisionEnter(Collision col)
    {
        if (col.impulse.magnitude > forceRequired)
        {
            Instantiate(particle, col.contacts[0].point, particle.transform.rotation);
            Destroy(gameObject, 0.1f);
        }
    }
}
