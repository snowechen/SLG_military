using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefStatusMag : MonoBehaviour
{
    int Id;

    public void SetObjId(int objId)
    {
        Id = objId;
    }

    /// <summary>
    /// 单位状态变更
    /// </summary>
    /// <param name="StatusId"></param>
    public void SetObjStatus(int StatusId)
    {
        objDefStatus defStatus = objDefStatus.noaction;
        switch (StatusId)
        {
            case 0:
                defStatus = objDefStatus.initiative;
                break;
            case 1:
                defStatus = objDefStatus.passive;
                break;
            case 2:
                defStatus = objDefStatus.noaction;
                break;
        }
        GameManager.instance.ObjSetStatus(Id, defStatus);

        gameObject.SetActive(false);
    }


}
