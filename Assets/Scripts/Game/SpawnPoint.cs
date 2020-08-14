using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool flag=false;

    public bool GetFlag { get { return flag; } }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other)
        {
            flag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col)
        {
            flag = false;
        }
    }
}
