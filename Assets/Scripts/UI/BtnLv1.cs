using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnLv1 : MonoBehaviour, IPointerEnterHandler
{
    Image btnImg;
    public Text _text;
    public Image icon;

    public int objID;

    public objType type;
    public void Init(Color textC,Color BGColor)
    {
        btnImg = GetComponent<Image>();
        //btnImg.color = BGColor;
        btnImg.enabled = false;
        _text.color = textC;
        icon.color = textC;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuBtnManager.instance.BtnsRefresh();
        MenuBtnManager.instance.BtnMouseEnter(this);
        //MenuBtnManager.instance.objID = objID;
    }

    public void setBtn(Color textC,Color BGColor)
    {
        //btnImg.color = BGColor;
        btnImg.enabled = true;
        _text.color = textC;
        icon.color = textC;
    }

    
}
