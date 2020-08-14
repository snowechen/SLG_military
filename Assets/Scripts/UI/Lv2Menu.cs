using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lv2Menu : MonoBehaviour
{
    
    public objType type;

    public Button[] btns;

    public MenuBtn parentBtn;
    Button[] buttons;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }
    public void ResetMenu()
    {
        if(type == objType.artillery)
        {
            btns[0].interactable = false;
            btns[1].interactable = true;
            btns[2].interactable = false;
        }
        else
        {
            btns[0].interactable = true;
            btns[1].interactable = false;
            btns[2].interactable = true;
        }

       // Button[] buttons = GetComponentsInChildren<Button>();
        foreach(var b in buttons)
        {
            Button btn = b;
            b.onClick.AddListener(
                delegate ()
                {
                    //this.Log(btn.name);
                    this.OnClick(btn);
                });
        }
    }

    void OnClick(Button sender)
    {
        switch (sender.name)
        {
            case "Button-1":
                GameManager.instance.ObjSetMove(MenuBtnManager.instance.objID);
                this.Log("点击1" + MenuBtnManager.instance.objID);
                break;
            case "Button-2":
                GameManager.instance.PreciseAttack(MenuBtnManager.instance.objID);
                break;
            case "Button-3":
                GameManager.instance.Attack(MenuBtnManager.instance.objID);
                break;
            case "Button-4":
                GameManager.instance.BombAttack(MenuBtnManager.instance.objID);
                break;
            case "Button-5":
                if (GameManager.instance.GetObj(MenuBtnManager.instance.objID) == null) return;
                GameObject SSW = CanvasData.instance.transform.Find("SetStatusWindow").gameObject;
                SSW.SetActive(true);
                SSW.GetComponent<DefStatusMag>().SetObjId(MenuBtnManager.instance.objID);
                break;
            case "Button-6":
                GameManager.instance.BackSupply(MenuBtnManager.instance.objID);
                break;
            case "Button-7":
                GameManager.instance.ObjPropertyMessage(MenuBtnManager.instance.objID, PropertyType.Position);
                break;
            case "Button-8":
                GameManager.instance.ObjPropertyMessage(MenuBtnManager.instance.objID, PropertyType.Bullet);
                break;
            case "Button-9":
                GameManager.instance.ObjPropertyMessage(MenuBtnManager.instance.objID, PropertyType.Health);
                break;
            case "Button-10":
                GameManager.instance.FollowCommand(MenuBtnManager.instance.objID);
                break;
           
        }
        Invoke("menuStatus", 0);
    }

    void menuStatus()
    {
        if (parentBtn.status)
        {
            parentBtn.Trigger();
        }
    }
}
