using UnityEngine;
using System.Collections;

public class ClampRectTransform : MonoBehaviour
{
    public float padding = 10.0f;
    public float elementSize = 128.0f;
    public float viewSize = 250.0f;
    public float leftPadding = 5.0f;

    private RectTransform rt;
    private int amountElements;
    private float contentSize;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.localPosition = new Vector3(100 + leftPadding, rt.localPosition.y, rt.localPosition.z);
    }

    private void Update()
    {
        // clamp our rect transform
        amountElements = rt.childCount;
        contentSize = ((amountElements * (elementSize + padding)) - viewSize) * rt.localScale.x;

        if (rt.localPosition.x > padding + leftPadding)
        {
            rt.localPosition = new Vector3(padding + leftPadding, rt.localPosition.y, rt.localPosition.z);
        }
        else if (rt.localPosition.x < -contentSize)
        {
            rt.localPosition = new Vector3(-contentSize, rt.localPosition.y, rt.localPosition.z);
        }
    }
}
