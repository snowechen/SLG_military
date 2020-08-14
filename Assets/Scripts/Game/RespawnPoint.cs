using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public static RespawnPoint instance;
    public Transform[] points;

    private bool[] flags;
    private void Awake()
    {
        instance = this;
        flags = new bool[points.Length];
        System.Array.ForEach(flags, f => f = false);
    }

    public Vector3 GetSpawnPoint()
    {
        if (!instance) instance = this;
        Vector3 vec = Vector3.zero;
        for(int i=0;i<points.Length;i++)
        {
            if (!flags[i])
            {
                vec = points[i].position;
                flags[i] = false;
                break;
            }
        }
        return vec;
    }
}
