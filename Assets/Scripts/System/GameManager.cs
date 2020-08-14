/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Dictionary<int, CharaBase> m_objList = new Dictionary<int, CharaBase>();
  
    private objStatus m_status;
    public GameObject Command;
    public GameObject Assault;
    public GameObject Artillery;
    public GameObject Scout;
    private void Awake()
    {
        Debuger.EnableLog = true;
        //Debuger.EnableLoop = true;
        if (instance) { Destroy(gameObject); }
        else
        {
            instance = this;
        }
        isAlive = true;
    }
    public bool isAlive; 
    public GameObject TipUI;
    public GameObject gameoverUI;
    void Start()
    {
        Vector3 point = RespawnPoint.instance.GetSpawnPoint();
        GameObject obj = Instantiate(Command, point, Quaternion.identity) as GameObject;
        m_objList.Add(0, obj.GetComponent<CharaBase>());
        Vector3[] AssaultPoint = new Vector3[] {
            new Vector3(-2,0),new Vector3(-2,1),new Vector3(-2,2),
            new Vector3(-1,2),new Vector3(0,2),new Vector3(1,2),
            new Vector3(2,2),new Vector3(2,1),new Vector3(2,0)
        };
        string Objname;
        //突击单位的生成
        for (int i = 0; i < 9; i++)
        {
            Vector3 p = point + AssaultPoint[i];
            obj = Instantiate(Assault, p, Quaternion.identity) as GameObject;
            Objname = "突击单位" + (i+1);
            obj.GetComponent<CharaBase>().SetObj(Objname, i + 1);
            m_objList.Add(i + 1, obj.GetComponent<CharaBase>());
        }
        //炮兵单位生成
        Vector3 artP = point + new Vector3(-1, 1);
        obj = Instantiate(Artillery, artP, Quaternion.identity) as GameObject;
        Objname = "炮兵单位";
        obj.GetComponent<CharaBase>().SetObj(Objname,10);
        m_objList.Add(m_objList.Count, obj.GetComponent<CharaBase>());
        //侦察单位生成
        Vector3 ScoutP = point + new Vector3(1, 1);
        obj = Instantiate(Scout, ScoutP, Quaternion.identity) as GameObject;
        Objname = "侦察单位";
        obj.GetComponent<CharaBase>().SetObj(Objname,11);
        m_objList.Add(m_objList.Count, obj.GetComponent<CharaBase>());

        AllObjFollowCommand();//默认跟随指挥官
    }

    
    void LateUpdate()
    {
        //鼠标操作
        MouseController();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 origin = PaletteMap.instance.GetMapPoint + new Vector3(0.5f, 0.5f,0);
        //    RaycastHit2D[] cols = Physics2D.BoxCastAll(origin, new Vector2(1.5f, 1.5f), 0, Vector2.zero);
        //    foreach(var col in cols)
        //    {
        //        if(col.collider.GetComponent<CharaBase>())
        //        this.Log(col.collider.name);
        //    }
        //}
    }
    /// <summary>
    /// 队伍减员控制
    /// </summary>
    /// <param name="id"></param>
    public void ObjectDie(CharaBase obj)
    {
        int dieid = GetObjId(obj);
        m_objList.Remove(dieid);
        Destroy(obj.gameObject);
        this.Log("剩余队伍数:"+m_objList.Count.ToString());

    }
    public Command GetCommand()
    {
        Command cmd;
        try
        {
            cmd = (Command)m_objList[0];
        }
        catch
        {
            cmd = null;
        }
        return cmd;
    }

    public void ObjSendMessage(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        var obj = m_objList[id];
    }

    private int objindex;
    /// <summary>
    /// 指定目标移动点
    /// </summary>
    public void ObjSetMove(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        m_status = objStatus.Moving;
        objindex = id;
        Cursor.SetCursor(AtkCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void MouseController()
    {
        bool MouseUI = EventSystem.current.IsPointerOverGameObject();
        if (!MouseUI)
        {
            switch (m_status)
            {
                case objStatus.Moving:
                    if (Input.GetMouseButtonDown(0) && PaletteMap.instance.DrawLimit())
                    {
                        m_objList[objindex].SetMovePosition(PaletteMap.instance.GetMapPoint);
                        m_status = objStatus.empty;
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                        //this.Log("移动指定点:{0},对象{1}", PaletteMap.instance.GetMapPoint,m_objList[objindex]);
                    }
                    break;
                case objStatus.Attack:
                    if(Input.GetMouseButtonDown(0) && PaletteMap.instance.DrawLimit())
                    {
                        m_objList[objindex].AttackPoint(PaletteMap.instance.GetMapPoint + new Vector3(0.5f, 0.5f),objStatus.Attack);
                        m_status = objStatus.empty;
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                    }
                    break;
                case objStatus.PreciseAttack:
                    if (Input.GetMouseButtonDown(0) && PaletteMap.instance.DrawLimit())
                    {
                        m_objList[objindex].AttackPoint(PaletteMap.instance.GetMapPoint + new Vector3(0.5f, 0.5f),objStatus.PreciseAttack);
                        m_status = objStatus.empty;
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                    }
                    break;
                case objStatus.BombAttack:
                    if (Input.GetMouseButtonDown(0) && PaletteMap.instance.DrawLimit())
                    {
                        m_objList[objindex].AttackPoint(PaletteMap.instance.GetMapPoint + new Vector3(0.5f, 0.5f), objStatus.BombAttack);
                        m_status = objStatus.empty;
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                    }
                    break;
            }
        }
    }

    public void ObjSetStatus(int id,objDefStatus defStatus)
    {
        if (!m_objList.ContainsKey(id)) return;
        m_objList[id].SetDefStatus = defStatus;
    }

    public Texture2D AtkCursor;
    public bool CmdOrder;
    /// <summary>
    /// 普通攻击方法
    /// </summary>
    /// <param name="id">指定攻击单位</param>
    public void Attack(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        objindex = id;
        m_status = objStatus.Attack;
        CmdOrder = true;
        Cursor.SetCursor(AtkCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    /// <summary>
    /// 有效射击
    /// </summary>
    /// <param name="id">单位id</param>
    public void PreciseAttack(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        objindex = id;
        m_status = objStatus.PreciseAttack;
        Cursor.SetCursor(AtkCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void AllObjFollowCommand()
    {
        foreach (var Obj in m_objList)
        {
            if (Obj.Key != 0)
            {
                Obj.Value.SetFollowCommand();
                //this.Log(Obj.Value.ToString());
            }
        }
    }
    public void AllFollowCmdAudio()
    {
        StartCoroutine(AllObjCallBackFollowCmd());
    }
    IEnumerator AllObjCallBackFollowCmd()
    {
        yield return StartCoroutine(AudioSystem.instance.CommandAllBack());

        foreach (var Obj in m_objList)
        {
            if (Obj.Key != 0)
            {
                Obj.Value.SetFollowCommand();
                //this.Log(Obj.Value.ToString());
            }
        }
    }

    public void FollowCommand(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        m_objList[id].SetFollowCommand();
    }

    public void CreateTipUI()
    {
        Vector3 pos = PaletteMap.instance.GetMapPoint;
        var UI = Instantiate(TipUI, CanvasData.instance.transform) as GameObject;
        UI.GetComponent<TipUI>().Init(pos);
        Destroy(UI, 3);
    }

    public void CreateTipUI(Vector3 pos,string str)
    {
        //Vector3 pos = PaletteMap.instance.GetMapPoint;
        var UI = Instantiate(TipUI, CanvasData.instance.transform) as GameObject;
        UI.GetComponent<TipUI>().Init(pos, str);
        Destroy(UI, 3);
    }

    public void BombAttack(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        objindex = id;
        m_status = objStatus.BombAttack;
        CmdOrder = true;
        Cursor.SetCursor(AtkCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void BackSupply(int id)
    {
        if (!m_objList.ContainsKey(id)) return;
        m_objList[id].BackSupply();

    }

    //用对象查询ID
    public int GetObjId(CharaBase obj)
    {
        int id = 0;
        foreach(var o in m_objList)
        {
            if(o.Value == obj)
            {
                id = o.Key;
            }
        }
        return id;
    }
    public CharaBase GetObj(int id)
    {
        CharaBase obj;
        if (m_objList.ContainsKey(id)) obj = m_objList[id];
        else obj = null;
        return obj;
    }

    public void ObjPropertyMessage(CharaBase obj,PropertyType type)
    {
        if (!m_objList.ContainsValue(obj)) return;

        PropertyMag.instance.PropertyUpdate(GetObjId(obj), obj, type); 
    }

    public void ObjPropertyMessage(int id,PropertyType type)
    {
        if (!m_objList.ContainsKey(id)) return;
        StartCoroutine(ReportAudio(id, type));
        if(type != PropertyType.Position) PropertyMag.instance.PropertyUpdate(id, m_objList[id],type);
    }
    /// <summary>
    /// 汇报
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    IEnumerator ReportAudio(int id,PropertyType type)
    {
        Vector2 objpos = GetObj(id).transform.position;
        Vector2 cmdpos = GetCommand().transform.position;
        Command cmd = GetCommand();
        //bool flag = false;
        int CallBackID = 0;
        //计算通讯成功率
        float dis = Vector2.Distance(objpos, cmdpos);
        if (dis > cmd.Lv2Com) { CallBackID = 2; }
        else if (dis > cmd.Lv1Com) {  CallBackID = 1; }
        else { CallBackID = 0; }
        if (!AudioSystem.instance.GetIsPlaying)
        {
            yield return StartCoroutine(AudioSystem.instance.Command_order_report(id, (int)type));
            yield return StartCoroutine(AudioSystem.instance.Obj_report_order(CallBackID, GetObj(id), (int)type));
        }
        else if(type == PropertyType.Position)
        {
            PropertyMag.instance.PropertyUpdate(id, m_objList[id], type);
        }
    }
    public void IsGameOver()
    {
        isAlive = false;
        foreach(var obj in m_objList)
        {
           // obj.Value.StopAllCoroutines();
            Destroy(obj.Value.gameObject);
        }
        m_objList.Clear();
        gameoverUI.GetComponent<SceneController>().setGameOver();
        gameoverUI.SetActive(true);
    }
    public void IsWinner()
    {
        gameoverUI.GetComponent<SceneController>().setWinner();
        gameoverUI.SetActive(true);
    }
}
