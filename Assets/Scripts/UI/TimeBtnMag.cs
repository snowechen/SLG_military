/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimeBtnMag : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public int index = 0;
    public Button[] buttons;
    public InputField MM;
    public InputField SS;

    public bool status;
    //Image background;
    Color[] colors = new Color[]
    {
        new Color(0.385f,0.385f,0.385f),
        new Color(0.47f,0.47f,0.47f)
    };
    private void Awake()
    {
        //background = GetComponent<Image>();
    }
    private void OnEnable()
    {
        //background.color = colors[0];
    }
    private void OnGUI()
    {
        try
        {
            int s = int.Parse(SS.text);
            if (s > 60) SS.text = 60.ToString();
        }
        catch { }

        if (status)
        {
            float second = TimeUIMag.instance.GetTime(index);
            int m = (int)(second / 60);
            int s = (int)(second % 60);
            MM.text = m.ToString();
            SS.text = s.ToString();
            if(second <=0)
            {
                SetStop();
            }
        }
    }

    public void SetStart()
    {
        if (MM.text == string.Empty) MM.text = 0.ToString();
        if (SS.text == string.Empty) SS.text = 0.ToString();
        status = true;
        BtnTrigger(status);
        int second = int.Parse(MM.text)*60 + int.Parse(SS.text);
        TimeUIMag.instance.SetTime(index, second);
    }

    public void SetStop()
    {
        status = false;
        BtnTrigger(status);
        MM.text = "";
        SS.text = "";
        TimeUIMag.instance.SetTime(index, 0);
    }

    void BtnTrigger(bool flag)
    {
        buttons[0].gameObject.SetActive(!flag);
        buttons[1].gameObject.SetActive(flag);
        MM.interactable = !flag;
        SS.interactable = !flag;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //background.color = colors[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //background.color = colors[0];
    }
}
