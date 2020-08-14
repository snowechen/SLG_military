using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawRear : MonoBehaviour
{
    public GameObject Rear;

    Vector3 min, max;
    float Xoff, Yoff;
    List<GameObject> Xtexts = new List<GameObject>();
    List<GameObject> Ytexts = new List<GameObject>();
    void Start()
    {
        for(int i = 0; i < 100;i++)
        {
            GameObject obj = Instantiate(Rear, transform) as GameObject;
            obj.GetComponent<Text>().text = i.ToString();
            Xtexts.Add(obj);
        }
        for(int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(Rear, transform) as GameObject;
            obj.GetComponent<Text>().text = i.ToString();
            Ytexts.Add(obj);
        }


        StartCoroutine("uiUpdate");
    }

    IEnumerator uiUpdate()
    {
        while (true)
        {
            min = Camera.main.WorldToScreenPoint(PaletteMap.instance.CellToWorldPos(new Vector3Int(0, 0, 0)));
            max = Camera.main.WorldToScreenPoint(PaletteMap.instance.CellToWorldPos(new Vector3Int(100, 100, 0)));
            Xoff = (max.x - min.x) / 100;
            Yoff = (max.y - min.y) / 100;
            for (int i = 0; i < Xtexts.Count; i++)
            {
                Vector3 uiPos = min + new Vector3(Xoff * i + Xoff / 2, -Yoff, 0);
                Xtexts[i].GetComponent<RectTransform>().anchoredPosition = uiPos / CanvasData.instance.UIScale;
                Xtexts[i].transform.localScale = Vector3.one * 1.3f - new Vector3((1 * Camera.main.orthographicSize / 55), (1 * Camera.main.orthographicSize / 55), 0);
            }
            for (int i = 0; i < Ytexts.Count; i++)
            {
                Vector3 uiPos = min + new Vector3(-Xoff, Yoff * i + Yoff / 2, 0);
                Ytexts[i].GetComponent<RectTransform>().anchoredPosition = uiPos / CanvasData.instance.UIScale;
                Ytexts[i].transform.localScale = Vector3.one * 1.3f - new Vector3((1 * Camera.main.orthographicSize / 55), (1 * Camera.main.orthographicSize / 55), 0);
            }

            yield return new WaitForEndOfFrame();
        }
    }
    void LateUpdate()
    {
        
    }
}
