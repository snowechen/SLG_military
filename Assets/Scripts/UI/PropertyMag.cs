/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyType
{
    Health = 0,
    Bullet = 2,
    Position = 1
}
public class PropertyMag : MonoBehaviour
{
    public PropertyObj[] pObjs;
    public static PropertyMag instance;
    void Start()
    {
        instance = this;
        pObjs = GetComponentsInChildren<PropertyObj>();
        Invoke("Init", 1);
        gameObject.SetActive(false);
    }

    public void PropertyUpdate(int id,CharaBase obj,PropertyType type)
    {
        id --;
        pObjs[id].ID = id;

        switch (type)
        {
            case PropertyType.Health:
                pObjs[id].HPpercent = obj.GetHPPercent;
                pObjs[id].HP = obj.GetHP;
                pObjs[id].buffAmt = obj.GetDeBuff;
                break;
            case PropertyType.Bullet:
                pObjs[id].bulletAmtPercent = obj.GetBulletPercent;
                pObjs[id].BombAmtPercent = obj.GetBombPercent;
                pObjs[id].DrugAmtPercent = obj.GetDrugPercent;
                break;
            case PropertyType.Position:
                Vector2Int pos = new Vector2Int(obj.GetPostionInt.x, obj.GetPostionInt.y);
                
                string msg = string.Format("汇报(<color=#FFAF23>{0}</color>)的坐标在:(X<color=#FFAF23>{1}</color>,Y<color=#FFAF23>{2}</color>)", obj.Name, pos.x, pos.y);
                MessageSystemMag.instance.AddMessage(msg);
                
                break;
        }

        foreach(var pobj in pObjs)
        {
            pobj.Refresh();
        }
    }


    private void Init()
    {
        for (int i = 0; i < pObjs.Length; i++)
        {
            PropertyUpdate(i + 1, GameManager.instance.GetObj(i+1), PropertyType.Health);
            PropertyUpdate(i + 1, GameManager.instance.GetObj(i+1), PropertyType.Bullet);
            //{
            //    pObjs[i].HP = 21;
            //}else if(i == 10)
            //{
            //    pObjs[i].HP = 4;
            //}
            //pObjs[i].buffAmt = 0;
        }
    }
}
