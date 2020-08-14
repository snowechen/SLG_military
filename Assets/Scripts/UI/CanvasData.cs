using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasData : MonoBehaviour {
    public static CanvasData instance;

    public float UIScale
    {
        get
        {
            return Screen.width / referenceResolutionX;
        }
    }

    private float referenceResolutionX;

    private void Awake()
    {
        
        instance = this;
        referenceResolutionX = GetComponent<CanvasScaler>().referenceResolution.x;
    }
}
