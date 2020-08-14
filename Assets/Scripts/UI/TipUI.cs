using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipUI : MonoBehaviour
{

    public Vector3 point;
    public Text _text;

    private RectTransform rect;
    public void Init(Vector3 pos)
    {
        point = pos + new Vector3(0.5f,2,0);
        _text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
        Vector2 UIpos = Camera.main.WorldToScreenPoint(point);
        rect.anchoredPosition = UIpos / CanvasData.instance.UIScale;
    }
    public void Init(Vector3 pos,string str)
    {
        Init(pos);
        _text.text = str;
    }
    void FixedUpdate()
    {
        Vector2 UIpos = Camera.main.WorldToScreenPoint(point);
        rect.anchoredPosition = UIpos / CanvasData.instance.UIScale;
    }
}
