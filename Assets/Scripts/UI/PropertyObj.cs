/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PropertyObj : MonoBehaviour
{
    public InputField[] inputFields;
    public int ID;
    public int HP;
    public float HPpercent;
    public int buffAmt;
    public float bulletAmtPercent;
    public float DrugAmtPercent;
    public float BombAmtPercent;
    void Start()
    {
       
    }

    public void Refresh()
    {
        //HP
        inputFields[0].text = HP.ToString();
        inputFields[0].textComponent.color = HPColor(HPpercent);
        //失血
        Color buff;
        if (buffAmt > 0) buff = new Color(1, 0.15f, 0.15f);
        else buff = Color.white;
        inputFields[1].text = buffAmt.ToString() ;
        inputFields[1].textComponent.color = buff;
        //子弹
        if (ID == 9)
        {
            inputFields[2].text = "无";
            inputFields[2].textComponent.color = Color.white;
        }
        else
        {
            inputFields[2].text = Str(bulletAmtPercent);
            inputFields[2].textComponent.color = BulletColor(bulletAmtPercent);
        }
        //药品
        inputFields[3].text = Str(DrugAmtPercent);
        inputFields[3].textComponent.color = BulletColor(DrugAmtPercent);
        //炮弹
        if (ID == 9)
        {
            inputFields[4].text = Str(BombAmtPercent);
            inputFields[4].textComponent.color = BulletColor(BombAmtPercent);
        }
        else
        {
            inputFields[4].text = "无";
            inputFields[4].textComponent.color = Color.white;
        }
    }

    Color BulletColor(float percent)
    {
        Color color;
        if (percent >= 0.5f)
        {
            color = new Color(0.1f, 0.68f, 0.3f);
        }
        else if (percent >= 0.25f)
        {
            color = new Color(1, 0.69f, 0.14f);
        }
        else if (percent > 0)
        {
            color = new Color(1, 0.15f, 0.15f);
        }
        else
        {
            color = new Color(1, 0.15f, 0.15f);
        }
        return color;
    }

    Color HPColor(float percent)
    {
        Color color;
        if (percent >= 0.7f)
        {
            color = new Color(0.1f, 0.68f, 0.3f);
        }
        else if (percent >= 0.3f)
        {
            color = new Color(1, 0.69f, 0.14f);
        }
        else if (percent > 0)
        {
            color = new Color(1, 0.15f, 0.15f);
        }
        else
        {
            color = new Color(1, 0.15f, 0.15f);
        }
        return color;
    }

    string Str(float percent)
    {
        string str;
        if (percent >= 0.5f)
        {
            str = "充足";
        }
        else if (percent >= 0.25f)
        {
            str = "缺少";
        }
        else if (percent > 0)
        {
            str = "极少";
        }
        else
        {
            str = "耗尽";
        }
        return str;
    }
}
