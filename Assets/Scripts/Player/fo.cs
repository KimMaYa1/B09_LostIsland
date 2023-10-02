using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fo : MonoBehaviour
{
    public Transform s1, s2;

    Vector3 startPos, endPos;

    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        startPos = s1.position;
        endPos = s2.position;

        Vector3 center = (startPos + endPos) * 0.5f;

        center.y -= 3;

        startPos = startPos - center;
        endPos = endPos - center;

        for(int i = 0; i < lr.positionCount; i++)
        {
            Vector3 point = Vector3.Slerp(startPos, endPos, i / (float)(lr.positionCount - 1));
            point += center;

            lr.SetPosition(i, point);
        }
    }
}
