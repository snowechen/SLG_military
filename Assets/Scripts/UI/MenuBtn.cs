/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class MenuBtn : MonoBehaviour
{
    public GameObject target;

    public bool status = false;
    
    public void Trigger()
    {
        GetComponentInParent<ToolUI>().CloseTarget(this);
        if (status)
        {
            target.SetActive(false);
        }
        else { target.SetActive(true); }
        status = !status;
    }
}
