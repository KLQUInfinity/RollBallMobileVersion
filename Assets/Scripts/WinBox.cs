using UnityEngine;
using System.Collections;

public class WinBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            LevelManager.Instance.Victory();
        }
    }
}
