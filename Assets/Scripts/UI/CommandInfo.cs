/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandInfo : MonoBehaviour
{
    public Text info;

    private Command command;
    void Start()
    {
        command = GameManager.instance.GetCommand();
        InvokeRepeating("TextUpdate", 0, 1);
    }

   
    void TextUpdate()
    {
        info.text = string.Format("指挥单位人数：<color=#18ac48>（{0}人）</color>\n" +
            "指挥单位剩余弹药：<color=#18ac48>（{1}）步枪子弹，（{2}）炮弹</color>\n" +
            "指挥单位剩余急救包：<color=#18ac48>（{3}）</color>\n" +
            "指挥单位一级通讯距离：<color=#18ac48>（{4}）格</color>\n" +
            "指挥单位二级通讯距离：<color=#18ac48>（{5}）格</color>\n"
            , command.GetHP, command.GetBullet, command.GetBombBullet, command.GetDrug, command.Lv1Com, command.Lv2Com);
    }
}
