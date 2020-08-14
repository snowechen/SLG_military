using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestData : MonoBehaviour
{
    public Text TestText;
    
    void LateUpdate()
    {
        TestText.text = "MapPoint:" + PaletteMap.instance.GetMapPoint +"\n"+
            "WorldPoint:"+ Camera.main.ScreenToWorldPoint(Input.mousePosition)+"\n"+
            "MouseScreen:"+Input.mousePosition;
    }
}
