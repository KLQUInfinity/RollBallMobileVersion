using UnityEngine;
using System.Collections;

public class SwitchObject : MonoBehaviour
{
    public GameObject target;

    private void OnCollisionEnter(Collision col)
    {
        if (target != null)
        {
            Destroy(target);
        }
    }
}
