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

public class MenuBtnManager : MonoBehaviour,IPointerExitHandler
{
    public static MenuBtnManager instance;
    public BtnLv1[] btns;
    [SerializeField]
    private Color NormalTextColor;
    [SerializeField]
    private Color NormalBGColor;
    [SerializeField]
    private Color HighTextColor;
    [SerializeField]
    private Color HighBGColor;

    public int objID;
    public Lv2Menu lv2Menu;
    void Start()
    {
        instance = this;
        BtnsRefresh();
        lv2Menu.gameObject.SetActive(false);
    }


    public void BtnsRefresh()
    {
        foreach(var b in btns)
        {
            b.Init(NormalTextColor, NormalBGColor);
        }
    }

    public void BtnMouseEnter(BtnLv1 btn)
    {
        btn.setBtn(HighTextColor, HighBGColor);
        objID = btn.objID;
        lv2Menu.gameObject.SetActive(true);
        Lv2MenuPos(btn.objID);
        lv2Menu.type = btn.type;
        lv2Menu.ResetMenu();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lv2Menu.gameObject.SetActive(false);
    }

    public void Lv2MenuPos(int id)
    {
        float y = 70 * (1-(float)id/11);
        Vector3 pos = new Vector3(120f, y, 0);
        lv2Menu.GetComponent<RectTransform>().anchoredPosition = pos;
        //this.Log(pos.ToString());
    }
}
