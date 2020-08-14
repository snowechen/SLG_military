/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseInfo : MonoBehaviour
{
    public Text Info;
    public static MouseInfo instance;

    public string mapType;//地形种类
    public string[] mapBuff;//buff
    public string Altitude;//海拔
    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// 设定地图信息
    /// </summary>
    /// <param name="typeName">地形种类</param>
    /// <param name="speed">移动速度</param>
    /// <param name="def">掩护率</param>
    /// <param name="height">海拔</param>
    public void SetData(string typeName,string speed,string def,string height)
    {
        mapType = typeName;
        mapBuff[0] = def;
        mapBuff[1] = speed;
        Altitude = height;
    }
    private void OnGUI()
    {
        Vector3Int point = PaletteMap.instance.GetMapPoint;
        if (!PaletteMap.instance.DrawLimit()) { point = Vector3Int.zero;
            mapType = "无"; mapBuff[0] = "0%";mapBuff[1] = "0%";Altitude = "0";
        }
        Info.text = "天气\n" + string.Format("光标坐标 (x{0},y{1})", point.x, point.y) + "\n" +
            string.Format("时间：{0}", System.DateTime.Now.ToString("HH:mm:ss")) + "\n" +
            string.Format("地形种类 ({0})", mapType) + "\n" +
            string.Format("掩护:+{0}%,移动:{1}%",mapBuff[0],mapBuff[1]) + "\n"+
            string.Format("海拔 ({0})",Altitude);
    }


}
