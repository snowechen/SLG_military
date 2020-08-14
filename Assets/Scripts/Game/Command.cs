/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Command : CharaBase
{
    [Header("雷达圆环平滑度")]
    public float m_Theta = 0.01f;//圆环平滑度
    [SerializeField]
    private Color m_Color; //圆环颜色
    [SerializeField]
    private LineRenderer Line;
    public float Lv1Com;//1级通信距离
    public float Lv2Com;//2级通信距离
    [SerializeField]
    LineRenderer lineLv1;
    [SerializeField]
    Color lv1Color;
    [SerializeField]
    LineRenderer lineLv2;
    [SerializeField]
    Color lv2Color;
    //private bool MoveStatus;//移动状态

    //Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

    Coroutine AttackMethod;
    Coroutine BombAttackMethod;
    Coroutine MoveMethod;
    Coroutine DebuffCureMethod;
    void Start()
    {
        Line.materials[0].color = m_Color;
        lineLv1.materials[0].color = lv1Color;
        lineLv2.materials[0].color = lv2Color;

        if (isMine) obj_sprite.sprite = objSprites[0];
        else obj_sprite.sprite = objSprites[1];
        //coroutines = new Dictionary<string, Coroutine>();
        maxBulletAmt = BulletAmt;
        maxBombBullet = BombBulletAmt;
        maxDrugAmt = DrugAmt;
        InvokeRepeating("Detect", 0, DetectSpeed);
        MaxHP = Health;
        DebuffCureMethod = StartCoroutine(DebuffCure());
        if (!isMine) GetComponent<AudioListener>().enabled = false;
    }

    
    void LateUpdate()
    {
        //绘制圆
        if(isMine) DrawLine();

        Debuffdamage();
        SpriteDisplay();

        if (ReportTime > 0) ReportTime -= Time.deltaTime;
    }

    void DrawLine()
    {
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        List<Vector3> points = new List<Vector3>();
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = (DetectRadius + 0.5f) * Mathf.Cos(theta);
            float y = (DetectRadius + 0.5f) * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, y, -1) + transform.position;
            if (theta == 0) { firstPoint = endPoint; }
            else { points.Add(beginPoint); points.Add(endPoint); }
            beginPoint = endPoint;
        }
        points.Add(firstPoint);
        points.Add(beginPoint);
        Line.positionCount = points.Count;
        Line.SetPositions(points.ToArray());

        //com lv1
        points.Clear();
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = (Lv1Com + 0.5f) * Mathf.Cos(theta);
            float y = (Lv1Com + 0.5f) * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, y, -1) + transform.position;
            if (theta == 0) { firstPoint = endPoint; }
            else { points.Add(beginPoint); points.Add(endPoint); }
            beginPoint = endPoint;
        }
        points.Add(firstPoint);
        points.Add(beginPoint);
        lineLv1.positionCount = points.Count;
        lineLv1.SetPositions(points.ToArray());

        //com lv2
        points.Clear();
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = (Lv2Com + 0.5f) * Mathf.Cos(theta);
            float y = (Lv2Com + 0.5f) * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, y, -1) + transform.position;
            if (theta == 0) { firstPoint = endPoint; }
            else { points.Add(beginPoint); points.Add(endPoint); }
            beginPoint = endPoint;
        }
        points.Add(firstPoint);
        points.Add(beginPoint);
        lineLv2.positionCount = points.Count;
        lineLv2.SetPositions(points.ToArray());
    }

    //Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>(); 
    public override void SetMovePosition(Vector3 pos)
    {
        base.SetMovePosition(pos);
        m_status = objStatus.Moving;
        string keyname = "Moving";

        if (MoveMethod != null) { StopCoroutine(MoveMethod); MoveMethod = null; }
        //coroutines.Remove(keyname);
        MoveMethod = StartCoroutine(keyname);
    }
   
    public Text moveText;
    IEnumerator Moving()
    {
        Vector2 end = new Vector2(MovePosition.x, MovePosition.y);
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        //this.Log("距离;" + Vector2.Distance(start, end));
        float time = MoveSpeed * Vector2.Distance(start, end);//总时间
        float t = 0;
        moveText.gameObject.SetActive(true);
        while (t/time < 1)
        {
            if (m_status == objStatus.Moving)
            {
                t += Time.deltaTime * SpeedPercent;
                moveText.text = (time-t).ToString("f2");
                GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(start,end,t/time));
            }
            yield return new WaitForFixedUpdate();
        }
        m_status = objStatus.empty;
        moveText.gameObject.SetActive(false);
    }

  
    public override void AttackPoint(Vector3 point, objStatus status)
    {
        base.AttackPoint(point,status);
        m_status = status;
        string keyname = "Attack";
        if (AttackMethod != null) { StopCoroutine(AttackMethod); AttackMethod = null; isShooting = false; }
        if (BombAttackMethod != null) { StopCoroutine(BombAttackMethod); BombAttackMethod = null; isShooting = false; }
        switch (status)
        {
            case objStatus.Attack:
                keyname = "Attack";
                AttackMethod = StartCoroutine(keyname);
                break;
            case objStatus.PreciseAttack:
                keyname = "Attack";
                AttackMethod = StartCoroutine(keyname);
                break;
            case objStatus.BombAttack:
                keyname = "BombAttack";
                BombAttackMethod = StartCoroutine(keyname);
                break;
        }
       // this.Log(coroutines.ContainsKey(keyname).ToString());
        //if (coroutines.ContainsKey(keyname))
        //{
        //    foreach (var c in coroutines)
        //    {
        //        if (c.Key == "Attack") { StopCoroutine(c.Value);  }
        //        if (c.Key == "BombAttack"){ StopCoroutine(c.Value); }
        //    }
        //}
        
        //this.Log("删除攻击方法:" + keyname);
       // coroutines.Remove(keyname);
        //string names = "";
        //foreach(var tine in coroutines)
        //{
        //    names += tine.Key + ",";
        //}
        //this.Log("剩余攻击方法：" + names);
       // coroutines.Add(keyname, StartCoroutine(keyname));
        
    }

    protected override void Die()
    {
        base.Die();
        if (isMine) GameManager.instance.IsGameOver();
        else EnemyManager.instance.IsWinner();
    }

    protected override IEnumerator BombAttack()
    {
        return base.BombAttack();
    }

    protected override IEnumerator Attack()
    {
        return base.Attack();
    }
    protected override void Detect()
    {
        base.Detect();
        //防御姿态主动时攻击
        if (m_defStatus == objDefStatus.initiative && EnemyList.Count > 0)
        {
            //this.Log("目标对象：" + EnemyList[0]);
            if (EnemyList[0] == null)
            {
                //清理空对象
                EnemyList.RemoveAt(0);
                //停止状态
                m_status = objStatus.empty;
                return;
            }
            else
            {
                GameManager.instance.CmdOrder = false;
                AttackPoint(EnemyList[0].transform.position, objStatus.Attack);
                if (isMine && ReportTime <=0)
                {
                    //汇报
                    string str = string.Format("{0}:与敌方目标<color=#FFAF23>{1}</color>发生交战!", Name, EnemyList[0].Name);
                    MessageSystemMag.instance.AddMessage(str);
                    ReportTime = 30;
                }
            }
        }
    }
   
    protected override bool GetDetectRate(float Drate, float distance, float targetStance, float targetCover)
    {
        return base.GetDetectRate(Drate, distance, targetStance, targetCover);
    }


    

    //public override void BombAttack(Vector3 point, objStatus status)
    //{
    //    string keyname = "BombAttack";
    //    foreach (var c in coroutines)
    //    {
    //        try
    //        {
    //            if (c.Key == keyname) { StopCoroutine(c.Value); }
    //        }
    //        catch { }

    //    }
    //    coroutines.Remove(keyname);
    //    coroutines.Add(keyname, StartCoroutine(keyname));
    //}

}
