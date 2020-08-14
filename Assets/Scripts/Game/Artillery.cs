using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : CharaBase
{
    Coroutine AttackMethod;
    Coroutine BackSupplyEnumMethod;
    Coroutine MoveMethod;
    Coroutine DebuffCureMethod;
    Coroutine FollowCommandMethod;
    void Start()
    {
        if (isMine) obj_sprite.sprite = objSprites[0];
        else obj_sprite.sprite = objSprites[1];
        //coroutines = new Dictionary<string, Coroutine>();
        maxBulletAmt = BulletAmt;
        maxBombBullet = BombBulletAmt;
        maxDrugAmt = DrugAmt;
        MaxHP = Health;
        InvokeRepeating("Detect", 0, DetectSpeed);
        DebuffCureMethod = StartCoroutine("DebuffCure");
    }

    
    void Update()
    {
        SpriteDisplay();
    }
    private void LateUpdate()
    {
        Debuffdamage();
        if (ReportTime > 0) ReportTime -= Time.deltaTime;
    }
    //Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
    public override void SetMovePosition(Vector3 pos)
    {
        base.SetMovePosition(pos);
        m_status = objStatus.Moving;
        string keyname = "Moving";
        if (MoveMethod != null) StopCoroutine(MoveMethod);
        //coroutines.Remove(keyname);
        MoveMethod = StartCoroutine(keyname);

    }


    IEnumerator Moving()
    {
        Vector2Int pos = new Vector2Int((int)MovePosition.x, (int)MovePosition.y);
        Command cmd = GameManager.instance.GetCommand();
        Vector2 cmdpos = cmd.transform.position;
        bool flag = false;
        int CallBackID = 0;
        //计算通讯成功率
        float dis = Vector2.Distance(transform.position, cmdpos);
        if (dis > cmd.Lv2Com) { flag = false; CallBackID = 2; }
        else if (dis > cmd.Lv1Com) { flag = true; CallBackID = 1; }
        else { flag = true; CallBackID = 0; }

        if (!AudioSystem.instance.GetIsPlaying)
        {
            //指挥官呼叫语音
            yield return StartCoroutine(AudioSystem.instance.Move_Order(pos, objID));
            //单位回音
            yield return StartCoroutine(AudioSystem.instance.Move_CallBack(CallBackID));
        }
        else
        {
            string str = "";
            if (flag) str = string.Format("<color=#FFAF23>{0}</color>：收到指令正在前往目标区域。", Name);
            else str = string.Format("电台传来沙沙声,无法收到来自<color=#FFAF23>{0}</color>的信号！", Name);
            MessageSystemMag.instance.AddMessage(str);
        }

        Vector2 end = new Vector2(MovePosition.x, MovePosition.y);
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        //this.Log("距离;" + Vector2.Distance(start, end));
        float time = MoveSpeed * Vector2.Distance(start, end);//总时间
        float t = 0;

        while (t / time < 1 && flag)
        {
            if (m_status == objStatus.Moving)
            {
                t += Time.deltaTime * SpeedPercent;

                GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(start, end, t / time));
            }
            yield return new WaitForFixedUpdate();
        }
        m_status = objStatus.empty;

        pos = new Vector2Int((int)MovePosition.x, (int)MovePosition.y);
        cmdpos = cmd.transform.position;
        //计算通讯成功率
        dis = Vector2.Distance(transform.position, cmdpos);
        if (dis > cmd.Lv2Com) { CallBackID = 2; }
        else if (dis > cmd.Lv1Com) {  CallBackID = 1; }
        else {  CallBackID = 0; }
        yield return AudioSystem.instance.Obj_Arrive(CallBackID, objID);
    }

    public override void SetFollowCommand()
    {
        base.SetFollowCommand();
        m_status = objStatus.Follow;
        string keyname = "FollowCommand";
        if (FollowCommandMethod != null) StopCoroutine(FollowCommandMethod);
        FollowCommandMethod = StartCoroutine(keyname);
    }

    public override void AttackPoint(Vector3 point, objStatus status)
    {
        base.AttackPoint(point,status);
        string keyname = "BombAttack";
        m_status =  objStatus.BombAttack;
        if (AttackMethod != null) { StopCoroutine(AttackMethod); isShooting = false; }
        AttackMethod = StartCoroutine(keyname);
    }


    public override void BackSupply()
    {
        base.BackSupply();
        string keyname = "BackSupplyEnum";
        if (BackSupplyEnumMethod != null) StopCoroutine(BackSupplyEnumMethod);
        BackSupplyEnumMethod = StartCoroutine(keyname);
    }
    protected override IEnumerator BackSupplyEnum()
    {
        yield return base.BackSupplyEnum();
        this.Log("当前弹药{0},最大弹舱{1}", BulletAmt, maxBulletAmt);
        BulletAmt += GetHP * 30; if (BulletAmt > maxBulletAmt) BulletAmt = maxBulletAmt;
        BombBulletAmt += GetHP * 30; if (BombBulletAmt > maxBombBullet) BombBulletAmt = maxBombBullet;
        DrugAmt = maxDrugAmt;
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
                if (isMine && ReportTime <= 0)
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
    //    base.BombAttack(point, status);
    //    string keyname = "Attack";
    //    foreach (var c in coroutines)
    //    {
    //        if (c.Key.Contains(keyname)) { StopCoroutine(c.Value); }
    //    }
    //    coroutines.Remove(keyname);
    //    coroutines.Add(keyname, StartCoroutine(keyname));
    //}
}
