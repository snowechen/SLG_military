/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSystemMag : MonoBehaviour
{
    public static MessageSystemMag instance;
    List<string> Msgs = new List<string>();

    public RectTransform MsgBox;
    public GameObject MsgBoxPrefab;
    public bool flag;
    public Button CloseBtn;
    public Sprite[] btnimg;
    void Start()
    {
        instance = this;

       // InvokeRepeating("test", 0, 2);
    }
    float waitTime = 3;
    /// <summary>
    /// 增加一条信息
    /// </summary>
    /// <param name="msg"></param>
    public void AddMessage(string msg)
    {
        if (!flag) return;
        if (!Msgs.Contains(msg) && waitTime<=0)
        {
            Msgs.Add(msg);
            StartCoroutine(MsgBoxPlus());
            waitTime = 3;
        }
    }

    private void FixedUpdate()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    IEnumerator MsgBoxPlus()
    {
        while (!flag) { yield return null; }
        if (Msgs.Count > 5) yield return new WaitForSeconds(1);
        float height = MsgBox.rect.height;
        float t = 0;
        float plusSize = Msgs.Count * 105;
        while (t < 1)
        {
            t += Time.deltaTime;
            MsgBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(height, plusSize, t));
            yield return new WaitForFixedUpdate();
        }
        var msg = Instantiate(MsgBoxPrefab, MsgBox.transform) as GameObject;
        msg.GetComponent<MsgBox>().msg.text = Msgs[Msgs.Count - 1];
    }

    public void MsgBoxCut()
    {
        Msgs.RemoveAt(0);
        float height = MsgBox.rect.height;
        height -= 105;
        MsgBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height);
        //this.Log(Msgs.Count.ToString());
    }

    public void MsgWindowTrigger()
    {
        flag = !flag;
        MsgBox.gameObject.SetActive(flag);
        if (flag)
        {
            CloseBtn.image.sprite = btnimg[0];
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        }
        else
        {
            CloseBtn.image.sprite = btnimg[1];
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 22);
        }
    }

    void test()
    {
        string str = "[副官信息]：突击单位1在坐标X(12)Y(95)的位置发现了敌军。";
        AddMessage(str);
    }
}
