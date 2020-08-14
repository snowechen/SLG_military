/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objType
{
    assault,//突击
    artillery,//炮兵
    scout,//侦察
    command,//指挥
}
public enum objStatus
{
    /// <summary>
    /// 无状态
    /// </summary>
    empty,
    /// <summary>
    /// 移动状态
    /// </summary>
    Moving,
    /// <summary>
    /// 攻击状态
    /// </summary>
    Attack,
    /// <summary>
    /// 精准射击
    /// </summary>
    PreciseAttack,
    /// <summary>
    /// 跟随状态
    /// </summary>
    Follow,
    /// <summary>
    /// 移动攻击
    /// </summary>
    BombAttack,
}
public enum objDefStatus
{
    /// <summary>
    /// 主动
    /// </summary>
    initiative,
    /// <summary>
    /// 被动
    /// </summary>
    passive,
    /// <summary>
    /// 不设防
    /// </summary>
    noaction
}
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D))]
public class CharaBase : MonoBehaviour
{
    public enum Stance
    {
        High,
        Low
    }
    public objType type;//单位类型
    public string Name;
    protected int objID;
    [SerializeField]
    protected float Health;//生命   
    protected float MaxHP;//最大生命
    [SerializeField]
    protected float RemoveBuffSpeed;//急救速度
    [SerializeField]
    protected int DrugAmt; //药品数量
    protected int maxDrugAmt;
    [SerializeField,Header("子弹数量")]
    protected int BulletAmt;//弹药数量
    [SerializeField,Header("炮弹数量")]
    protected int BombBulletAmt;//炮弹数量
    [SerializeField]
    protected float DetectSpeed=5;//侦察频率,秒数
    [SerializeField]
    protected float DetectRadius=20;//侦察范围
    protected float DetectRate = 0.96f;//侦察率
    [SerializeField]
    protected float PreciseAtkRadius;//精准射击半径
    [SerializeField]
    protected float MaxAtkRadius;//最大射击半径
    /// <summary>
    /// 姿态
    /// </summary>
    public Stance m_stance;
    /// <summary>
    /// 单位行为状态
    /// </summary>
    [SerializeField]
    protected objStatus m_status;
    /// <summary>
    /// 单位防御态度
    /// </summary>
    [SerializeField]
    protected objDefStatus m_defStatus;
    /// <summary>
    /// 设定单位防御态度
    /// </summary>
    public objDefStatus SetDefStatus { set { m_defStatus = value; } }
    /// <summary>
    /// 被侦察与被攻击的姿态数据
    /// </summary>
    Dictionary<string, Vector2> StanceData = new Dictionary<string, Vector2>()
    {
        {"Detect",new Vector2(1.1f,0.8f) },
        {"Attack",new Vector2(1.2f,0.6f) }
    };
   
    [SerializeField,Header("多少秒移动一格")]
    protected float MoveSpeed; //移动速度
    [SerializeField,Range(0,1),Header("移动速度百分比")]
    protected float SpeedPercent;//移动速度百分比
    protected Vector3 MovePosition;//移动目标
    [SerializeField]
    protected bool isMine =true;
    protected int maxBulletAmt=0;
     
    [SerializeField,Header("单位敌我图标")]
    protected Sprite[] objSprites;
    [SerializeField]
    protected SpriteRenderer obj_sprite;
    [SerializeField,Header("单次射击数量")]
    protected int ShootAmt;//每人射击量
    [SerializeField]
    protected float AttackInterval;//攻击间隔
    [SerializeField,Range(0,1),Header("掩护率")]
    protected float DefancePercent;//掩护率
    /// <summary>
    /// 海拔高度
    /// </summary>
    protected int Altitude;
    [SerializeField]
    protected GameObject BulletPrefab;//子弹obj
    [SerializeField]
    protected GameObject BombBulletPrefab;//炮弹对象
    protected int maxBombBullet = 0;
    [SerializeField,Range(0,1),Header("单位命中率")]
    protected float Hitrate;
    protected int DeBuffAmt;

    [SerializeField]
    protected AudioClip[] WeaponClip;
    [SerializeField]
    protected AudioSource m_Audio;
    /// <summary>
    /// 获取掩护率
    /// </summary>
    /// <returns></returns>
    public float GetDefPercent() { return DefancePercent;}
    /// <summary>
    /// 获取当前血量
    /// </summary>
    public int GetHP { get { return Mathf.RoundToInt(Health); } }
    public int GetBullet { get { return BulletAmt; } }
    public int GetBombBullet { get { return BombBulletAmt; } }
    public int GetDrug { get { return DrugAmt; } }
    public int GetDeBuff { get { return DeBuffAmt; } }
    public float GetBulletPercent { get { float percent = 0; if (maxBulletAmt <= 0) { percent = 0.0f; } else { percent = (float)BulletAmt / maxBulletAmt; } return percent; } }
    public float GetBombPercent { get { float percent = 0; if (maxBombBullet <= 0) { percent = 0.0f; } else { percent = (float)BombBulletAmt / maxBombBullet; } return percent; } }
    public float GetDrugPercent { get { float percent = 0; if (maxDrugAmt <= 0) { percent = 0.0f; } else { percent = (float)DrugAmt / maxDrugAmt; } return percent; } }
    public Vector3Int GetPostionInt { get { return new Vector3Int((int)transform.position.x, (int)transform.position.y, -1); } }
    public float GetHPPercent { get { float percent = 0; if (MaxHP <= 0) { percent = 0.0f; } else { percent = Health / MaxHP; } return percent; } }
    public float GetDetectRadius { get { return DetectRadius; } }
    public void SetObj(string name,int ID) { Name = name;objID = ID; }
    public void SetObjID(int ID) {objID = ID; }
    /// <summary>
    /// 侦查命中率
    /// </summary>
    /// <param name="Drate">单位侦查率</param>
    /// <param name="distance">距离</param>
    /// <param name="targetStance">被侦查目标姿态</param>
    /// <param name="targetCover">掩护率</param>
    /// <returns></returns>
    protected virtual bool GetDetectRate(float Drate,float distance,float targetStance,float targetCover)
    {
        float rate = Mathf.Pow(Drate, distance) * targetStance;
        if (rate < targetCover) rate = 0.07f; else rate -= targetCover;
        float rand = Random.Range(0f, 1f);
       // this.Log("侦察几率:{0}", rate);
        return rand <= rate;
    }
    //protected Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
    protected List<CharaBase> EnemyList = new List<CharaBase>();
    protected float ReportTime = 0.0f;
    /// <summary>
    /// 查询行为
    /// </summary>
    protected virtual void Detect()
    {
        //if (tag.Contains("Enemy")) return;
        RaycastHit2D[] cols = Physics2D.CircleCastAll(transform.position, DetectRadius, Vector2.zero, 0, LayerMask.GetMask("Player"));
        string AtkTag = "";
        if (isMine) AtkTag = "Enemy"; else AtkTag = "Player";
        foreach (var col in cols)
        {
            CharaBase target = col.collider.GetComponent<CharaBase>();
            if (target.gameObject.CompareTag(AtkTag))
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                //this.Log("执行查询-范围内目标：" + target + "距离："+distance);
                if (GetDetectRate(DetectRate, distance, target.GetStance("Detect"), target.GetDefPercent()))
                {
                    if (!EnemyList.Contains(target)) EnemyList.Add(target);
                    //this.Log("发现目标：{0},目前敌人列表：{1}" , target.Name,EnemyList.Count);
                    //汇报
                    if (isMine)
                    {
                        if (!AudioSystem.instance.GetIsPlaying && objID != 0)
                        {
                            if (ReportTime <= 0)
                            {
                                //发现目标汇报语音
                                StartCoroutine(AudioSystem.instance.Obj_Enemy_Detected(GetCmdDistance(), objID, this));
                                ReportTime = 30f;
                            }
                        }
                        else
                        {
                            string str = string.Format(Name + ":在坐标(X<color=#FFAF23>{0}</color>,Y<color=#FFAF23>{1}</color>)，发现目标:<color=#FFAF23>{2}</color>", target.GetPostionInt.x, target.GetPostionInt.y, target.Name);
                            MessageSystemMag.instance.AddMessage(str);
                        }
                    }
                }
            }
        }

        ////防御姿态主动时攻击
        //if (m_defStatus == objDefStatus.initiative && EnemyList.Count > 0)
        //{
        //    //this.Log("目标对象：" + EnemyList[0]);
        //    if (EnemyList[0] == null)
        //    {
        //        //清理空对象
        //        EnemyList.RemoveAt(0);
        //        //停止状态
        //        m_status = objStatus.empty;
        //        return;
        //    }
        //    else
        //    {
        //        AttackPoint(EnemyList[0].transform.position, objStatus.Attack);
        //        if (isMine)
        //        {
        //            //汇报
        //            string str = string.Format("{0}:与敌方目标<color=#FFAF23>{1}</color>发生交战!", Name, EnemyList[0].Name);
        //            MessageSystemMag.instance.AddMessage(str);
        //        }
                
        //    }
        //}
        
        if (EnemyList.Count > 0)
        {
            //遍历目标距离
            for(int i =0;i<EnemyList.Count;i++)
            {
                if (EnemyList[i] != null)
                {
                    Vector2 target = EnemyList[i].transform.position;
                    float distance = Vector2.Distance(transform.position, target);
                    if ( distance > DetectRadius)
                    {
                        //失去目标
                        EnemyList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

    }


    /// <summary>
    /// 获取姿态值,侦察：Detect ,攻击：Attack
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public float GetStance(string key)
    {
        float stance = 0;
        switch (m_stance)
        {
            case Stance.High:
                stance = StanceData[key].x;
                break;
            case Stance.Low:
                stance = StanceData[key].y;
                break;
        }
        return stance;
    }

    
    public virtual void SetMovePosition(Vector3 pos)
    {
        MovePosition = pos + new Vector3(0.5f, 0.5f, 0);
        MovePosition.z = -1;
    }

    protected Transform followTarget;//跟踪对象
    public virtual void SetFollowCommand()
    {
        followTarget = GameManager.instance.GetCommand().transform;

    }
    int startGame = 0;
    protected IEnumerator FollowCommand()
    {
        if(!AudioSystem.instance.GetIsPlaying && startGame >0) yield return StartCoroutine(AudioSystem.instance.AllCallBack());
        startGame = 1;
        while (m_status == objStatus.Follow)
        {
            if (Vector2.Distance(followTarget.position, transform.position) > 3)
            {
                Vector3Int pos = new Vector3Int((int)followTarget.position.x, (int)followTarget.position.y, -1);
                yield return FollowMoving(pos);
            }
            yield return new WaitForFixedUpdate();
        }
        //m_status = objStatus.empty;
    }

    IEnumerator FollowMoving(Vector3 pos)
    {
        Vector2 end = new Vector2(pos.x+0.5f, pos.y+0.5f)+new Vector2(Random.Range(-2,3),Random.Range(-2,3));
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        //this.Log("距离;" + Vector2.Distance(start, end));
        float time = MoveSpeed * Vector2.Distance(start, end);//总时间
        float t = 0;
        while (t / time < 1)
        {
                t += Time.deltaTime * SpeedPercent;
                GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(start, end, t / time));
            yield return new WaitForFixedUpdate();
        }
    }

    protected Vector3 AtkPoint;
    public virtual void AttackPoint(Vector3 point,objStatus status)
    {
        m_status = status;
        AtkPoint = point;
        
    }
    IEnumerator AttackAudioPlay()
    {
        Vector2Int pos = new Vector2Int((int)AtkPoint.x, (int)AtkPoint.y);
        GameManager.instance.CmdOrder = false;
        if (objID != 0 && isMine)
        {
            if (!AudioSystem.instance.GetIsPlaying)
            {
                //指挥官呼叫语音
                yield return StartCoroutine(AudioSystem.instance.Engage_Order(pos, objID));
                
                //单位回音
                yield return StartCoroutine(AudioSystem.instance.Engage_CallBack(GetCmdDistance()));
                
            }
            else
            {
                string str = "";
                if (GetCmdDistance() != 2) str = string.Format("<color=#FFAF23>{0}</color>：正在执行攻击命令。", Name);
                else str = string.Format("电台传来沙沙声,无法收到来自<color=#FFAF23>{0}</color>的信号！", Name);
                MessageSystemMag.instance.AddMessage(str);
            }
        }
    }

    IEnumerator BombAttackAudioPlay()
    {
        Vector2Int pos = new Vector2Int((int)AtkPoint.x, (int)AtkPoint.y);
        GameManager.instance.CmdOrder = false;
        if (objID > 0 && isMine)
        {
            if (!AudioSystem.instance.GetIsPlaying)
            {
                //指挥官呼叫语音
                yield return StartCoroutine(AudioSystem.instance.Engage_Order(pos, objID));
                
                //单位回音
                if (IntervalTime <= 0) yield return StartCoroutine(AudioSystem.instance.Engage_CallBack(GetCmdDistance()));
                else
                {
                    string str = string.Format("<color=#FFAF23>{0}</color>：武器正在冷却中...", Name);
                    MessageSystemMag.instance.AddMessage(str);
                }
            }
            else
            {
                string str = "";
                if (GetCmdDistance() != 2) str = string.Format("<color=#FFAF23>{0}</color>：正在执行攻击命令。", Name);
                else str = string.Format("电台传来沙沙声,无法收到来自<color=#FFAF23>{0}</color>的信号！", Name);
                MessageSystemMag.instance.AddMessage(str);
            }
        }
    }
    [Header("每发子弹间隔")]
    public float ShootInterval = 0.2f;
    protected float Atktime = 0;
    /// <summary>
    /// 步枪攻击
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Attack()
    {
        if (GameManager.instance.CmdOrder)
        {
            yield return StartCoroutine(AttackAudioPlay());
        }
        bool attack = false;
        if (m_status == objStatus.Attack)
        {
            attack = Vector2.Distance(transform.position, AtkPoint) <= MaxAtkRadius;
        }
        else if(m_status == objStatus.PreciseAttack)
        {
            attack = Vector2.Distance(transform.position, AtkPoint) <= PreciseAtkRadius;
        }
        if (!attack)
        {
            //超出范围提示
            GameManager.instance.CreateTipUI();
        }
        while (Atktime > 0 && attack) { Atktime -= Time.deltaTime; yield return null; }
        Atktime = 2;
        while(isShooting) { yield return null; }
        while (attack && (m_status == objStatus.Attack || m_status == objStatus.PreciseAttack))
        {
            isShooting = true;
            int tempAmt = 0;
             tempAmt = ShootAmt * Mathf.RoundToInt(Health); 
            //if(m_status == objStatus.BombAttack) { tempAmt = ShootAmt * ((int)Health/3); }
            if (BulletAmt < tempAmt)
            {
                //发出请求补给
                if (isMine)
                {
                    if(!AudioSystem.instance.GetIsPlaying && objID != 0)
                    {
                        yield return StartCoroutine(AudioSystem.instance.Obj_Retreat_supply(GetCmdDistance(), objID));
                    }
                    {
                        string str = string.Format("<color=#FFAF23>{0}</color>：弹药已经用光，请求补给！", Name);
                        MessageSystemMag.instance.AddMessage(str);
                    }
                    int thisID = GameManager.instance.GetObjId(this);
                    PropertyMag.instance.PropertyUpdate(thisID, this, PropertyType.Bullet);
                }
                else
                {
                    BackSupply();
                }
                break;
            }
            else { BulletAmt -= tempAmt; }
            int bAmt = tempAmt;
            while (bAmt > 0 && (m_status == objStatus.Attack || m_status == objStatus.PreciseAttack))
            {
                GameObject b = Instantiate(BulletPrefab, transform.position, Quaternion.identity) as GameObject;
                string AtkTag = "";
                if (isMine) AtkTag = "Enemy"; else AtkTag = "Player";
                b.GetComponent<Bullet>().Init(AtkPoint, Hitrate,AtkTag,objID);
                bAmt--;
                yield return new WaitForSeconds(ShootInterval);
                //开火声音选择
                if(this is Command) m_Audio.clip = WeaponClip[0];
                else
                {
                    Vector2 mpos = transform.position;
                    Vector2 cmdpos = GameManager.instance.GetCommand().transform.position;
                    float distance = Vector2.Distance(mpos, cmdpos);
                    if (distance <= DetectRadius)
                    {
                        m_Audio.clip = WeaponClip[0];
                    }else if(distance < DetectRadius + 10)
                    {
                        m_Audio.clip = WeaponClip[1];
                    }
                    else {
                        m_Audio.clip = WeaponClip[2];
                    }
                }
                if (!m_Audio.isPlaying) m_Audio.Play();
            }
            //BulletAmt -= tempAmt;
            float BulletRate = 1;//弹药量率
            float Brate = BulletAmt / maxBulletAmt;
            if (Brate >= 0.75f) { BulletRate = 1; }
            else if (Brate >= 0.4f) { BulletRate = 1.4f; }
            else { BulletRate = 1.7f; }
            //状态变化
            if (m_status == objStatus.Attack) m_status = objStatus.empty;
            //冷却时间
            float IntervalTime = AttackInterval * BulletRate;
            yield return new WaitForSeconds(IntervalTime);
        }
        isShooting = false;
    }
    /// <summary>
    /// 获取与指挥官通讯距离等级
    /// </summary>
    /// <returns></returns>
    int GetCmdDistance()
    {
        Command cmd = GameManager.instance.GetCommand();
        Vector2 cmdpos = cmd.transform.position;
        
        int CallBackID = 0;
        //计算通讯成功率
        float dis = Vector2.Distance(transform.position, cmdpos);
        if (dis > cmd.Lv2Com) {  CallBackID = 2; }
        else if (dis > cmd.Lv1Com) {  CallBackID = 1; }
        else {  CallBackID = 0; }

        return CallBackID;
    }
    //public virtual void BombAttack(Vector3 point, objStatus status)
    //{
    //    m_status = status;
    //    AtkPoint = point;
    //}
    protected bool isShooting;
    float IntervalTime;
    protected virtual IEnumerator BombAttack()
    {
        if (GameManager.instance.CmdOrder)
        {
            yield return StartCoroutine(BombAttackAudioPlay());
        }

        bool attack = false;
        attack = Vector2.Distance(transform.position, AtkPoint) <= MaxAtkRadius;
        if (!attack)
        {
            //超出范围提示
            GameManager.instance.CreateTipUI();
        }
        while (IntervalTime > 0 && attack) { IntervalTime -= Time.deltaTime; yield return null; }
        //等待
        while(isShooting) { yield return null; }
        while (attack &&  m_status == objStatus.BombAttack)
        {
            isShooting = true;
            int tempAmt = 0;
            //冷却时间
            IntervalTime = AttackInterval;
            tempAmt = ShootAmt * Mathf.CeilToInt(Health / 3);
            if (this is Command) tempAmt = 1;
            if (BombBulletAmt < 1) {
                //发出请求补给
                if (isMine)
                {
                    string str = string.Format("<color=#FFAF23>{0}</color>：弹药已经用光，请求补给！", Name);
                    MessageSystemMag.instance.AddMessage(str);
                    int thisID = GameManager.instance.GetObjId(this);
                    PropertyMag.instance.PropertyUpdate(thisID, this, PropertyType.Bullet);
                }
                else
                {
                    BackSupply();
                }
                break;
            }
            else if(BombBulletAmt < tempAmt) { tempAmt = BombBulletAmt;BombBulletAmt = 0; }
            else { BombBulletAmt -= tempAmt; }
            int bAmt = tempAmt;
            while (bAmt > 0 && m_status == objStatus.BombAttack)
            {
                GameObject b = Instantiate(BombBulletPrefab, transform.position, Quaternion.identity) as GameObject;
                string AtkTag = "";
                if (isMine) AtkTag = "Enemy"; else AtkTag = "Player";
                b.GetComponent<Bullet>().Init(AtkPoint, Hitrate, AtkTag,objID);
                bAmt--;
                //开火声音选择
                if (this is Command) m_Audio.clip = WeaponClip[1];
                else
                {
                    Vector2 mpos = transform.position;
                    Vector2 cmdpos = GameManager.instance.GetCommand().transform.position;
                    float distance = Vector2.Distance(mpos, cmdpos);
                    if (distance <= DetectRadius)
                    {
                        m_Audio.clip = WeaponClip[0];
                    }
                    else
                    {
                        m_Audio.clip = WeaponClip[1];
                    }
                }
                m_Audio.Play();
                yield return new WaitForSeconds(ShootInterval * 5);
                
            }
            m_status = objStatus.empty;
            //BombBulletAmt -= tempAmt;
            //float BulletRate = 1;//弹药量率
            //float Brate = BombBulletAmt / maxBombBullet;
            //if (Brate >= 0.75f) { BulletRate = 1; }
            //else if (Brate >= 0.4f) { BulletRate = 1.4f; }
            //else { BulletRate = 1.7f; }
            
            while(IntervalTime > 0)
            {
                IntervalTime -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitForSeconds(IntervalTime);
        }
        isShooting = false;
    }

   
    /// <summary>
    /// 受伤计算
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="targetId">对方ID</param>
    public void TakeDamage(float damage,int targetId)
    {
        Health -= damage;
        if (Random.Range(0, 100f) > 50f && DeBuffAmt < (int)Health) { DeBuffAmt += 1; }
        if (Health < 0) {
            Health = 0;
            if (isMine)
            {
                //int thisID = GameManager.instance.GetObjId(this);
                if (objID != 0)
                {
                    PropertyMag.instance.PropertyUpdate(objID, this, PropertyType.Health);
                    PropertyMag.instance.PropertyUpdate(objID, this, PropertyType.Bullet);
                }
            }
            else
            {
                if(targetId != 0 && !AudioSystem.instance.GetIsPlaying)
                {
                    int type = GameManager.instance.GetObj(targetId).GetCmdDistance();
                    StartCoroutine(AudioSystem.instance.Obj_Enemy_Killed(type, targetId, objID));
                }
                else
                {
                    string str = string.Format("敌方的<color=#FFAF23>{0}</color>已经被消灭！", this.Name);
                    MessageSystemMag.instance.AddMessage(str);
                }
            }
            Die();
        }
        float rand = Random.Range(0, 1f);
        if (isMine && objID != 0 && !AudioSystem.instance.GetIsPlaying && ReportTime <=0 && rand > 0.8f)
        {
            if (Health < MaxHP / 2)
            {
                StartCoroutine(AudioSystem.instance.Obj_Retreat_Casualty(GetCmdDistance(), objID, 0));
            }
            if(Health < MaxHP / 3)
            {
                StartCoroutine(AudioSystem.instance.Obj_Retreat_Casualty(GetCmdDistance(), objID, 1));
            }
            ReportTime = 10;
        }
        if (!isMine && objID != 0) { BackSupply(); }
        
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Random.Range(0, 100f) > 50f && DeBuffAmt < (int)Health) { DeBuffAmt += 1; }
        if (Health < 0)
        {
            Health = 0;
            if (isMine)
            {
                //int thisID = GameManager.instance.GetObjId(this);
                if (objID != 0)
                {
                    PropertyMag.instance.PropertyUpdate(objID, this, PropertyType.Health);
                    PropertyMag.instance.PropertyUpdate(objID, this, PropertyType.Bullet);
                }
            }
            Die();
        }
        float rand = Random.Range(0, 1f);
        if (isMine && objID != 0 && !AudioSystem.instance.GetIsPlaying && ReportTime <= 0 && rand > 0.8f)
        {
            if (Health < MaxHP / 2)
            {
                StartCoroutine(AudioSystem.instance.Obj_Retreat_Casualty(GetCmdDistance(), objID, 0));
            }
            if (Health < MaxHP / 3)
            {
                StartCoroutine(AudioSystem.instance.Obj_Retreat_Casualty(GetCmdDistance(), objID, 1));
            }
            ReportTime = 10;
        }
        if (!isMine && objID != 0) { BackSupply(); }

    }
    public void TakeDamage(float damage,Vector2 pos,int targetId)
    {
        TakeDamage(damage,targetId);
      
        string str = string.Format(Name + "：遭受敌人攻击,敌人位置(X<color=#FFAF23>{0}</color>,Y<color=#FFAF23>{1}</color>)", pos.x, pos.y);
        bool flag = false;
        if (m_defStatus == objDefStatus.passive || m_defStatus == objDefStatus.initiative)
        {
            AttackPoint(pos, objStatus.Attack);
            str += "\n正在进行反击！";
            flag = true;
        }
        else { str += "\n正在遭受攻击，请求反击！"; flag = false; }
        if (isMine)
        {
            if (!AudioSystem.instance.GetIsPlaying)
            {
                if (ReportTime <= 0 && objID!=0)
                {
                    if (flag)
                    {
                        StartCoroutine(AudioSystem.instance.Obj_defensive(objID, GetCmdDistance()));
                    }
                    else StartCoroutine(AudioSystem.instance.Obj_defence_awaiting(objID, GetCmdDistance()));
                    ReportTime = 15;
                }
            }
            else
            {
                MessageSystemMag.instance.AddMessage(str);
            }
        }

    }
    protected virtual void Die()
    {
        StopAllCoroutines();
        if(isMine) GameManager.instance.ObjectDie(this);
        if (tag == "Enemy") EnemyManager.instance.Die(this);
    }

    float DebuffTime = 3;
    float BuffAllTime = 300;
    protected void Debuffdamage()
    {
        if (DeBuffAmt < 1) return;
        DebuffTime -= Time.deltaTime;
        if (DebuffTime <= 0)
        {
            float damage = DeBuffAmt * 0.01f;
            TakeDamage(damage);
            DebuffTime = 3;
        }
        BuffAllTime -= Time.deltaTime;
        if (BuffAllTime <= 0) DeBuffAmt = 0;
    }

    float DeBuffTimePercent = 1;
    protected IEnumerator DebuffCure()
    {
        float removeTime = RemoveBuffSpeed;
        while (true)
        {
            if (m_status == objStatus.Attack) DeBuffTimePercent = 0.6f;
            else if (Health <= MaxHP * 0.4f) DeBuffTimePercent = 0.9f;
            else DeBuffTimePercent = 1;
            if (DeBuffAmt > 0)
            {
                removeTime -= Time.deltaTime * DeBuffTimePercent;
                if (removeTime <= 0 && DrugAmt>0)
                {
                    if (m_status == objStatus.Attack) DeBuffAmt -= 5;
                    else DeBuffAmt -= 10;
                    if (DeBuffAmt < 0) DeBuffAmt = 0;
                    DrugAmt--;
                    removeTime = RemoveBuffSpeed;
                } 
                //药品不足反馈
            }
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// 返回补给
    /// </summary>
    public virtual void BackSupply()
    {
        if (isMine) followTarget = GameManager.instance.GetCommand().transform;
        else followTarget = EnemyManager.instance.GetCommand();
        m_status = objStatus.Follow;
    }

    protected virtual IEnumerator BackSupplyEnum()
    {
        if (!AudioSystem.instance.GetIsPlaying)
        {
            //指挥官呼叫语音
            yield return StartCoroutine(AudioSystem.instance.Command_Return_Order(objID));
            //单位回音
            yield return StartCoroutine(AudioSystem.instance.Obj_return_order(GetCmdDistance()));
        }
        else
        {
            string str = "";
            if (GetCmdDistance()!=2) str = string.Format("<color=#FFAF23>{0}</color>：正在返回补给。", Name);
            else str = string.Format("电台传来沙沙声,无法收到来自<color=#FFAF23>{0}</color>的信号！", Name);
            MessageSystemMag.instance.AddMessage(str);
        }

        while (m_status == objStatus.Follow)
        {
            float distance = Vector2.Distance(transform.position, followTarget.position);
            if (distance <= 2) break;
            Vector3Int pos = new Vector3Int((int)followTarget.position.x, (int)followTarget.position.y, -1);
            yield return FollowMoving(pos);
        }

        if(isMine) GameManager.instance.CreateTipUI(transform.position,"补给完成");
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //this.Log(col.gameObject.layer.GetHashCode()+"  "+LayerMask.NameToLayer("Map"));
        if(col.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            ColliderChange(col);
            if (this is Command && isMine)
            {
                string name = col.transform.parent.gameObject.name;
                if (name == "Rooms")
                {
                    AudioSystem.instance.BGMFX(true);
                }
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        Vector2 pos = new Vector2(GetPostionInt.x, GetPostionInt.y);
        RaycastHit2D hit = Physics2D.BoxCast(pos, Vector2.one/2, 0, Vector2.zero,0,LayerMask.GetMask("Map"));
        if (hit.collider.CompareTag("Map"))
        {
            ColliderChange(hit.collider);
            if (this is Command && isMine)
            {
                string name = hit.collider.transform.parent.gameObject.name;
                //this.Log("离开范围时的判定:" + name);
                if (name == "Rooms")
                {
                    AudioSystem.instance.BGMFX(true);
                }
                else { AudioSystem.instance.BGMFX(false); }
            }
            //this.Log("当前速度:{0},防御率:{1},海拔:{2}", SpeedPercent, DefancePercent, Altitude);
        }
    }
    void ColliderChange(Collider2D col)
    {
        MapData Map = col.transform.parent.GetComponent<MapData>();
        SpeedPercent = Map.speed;
        Altitude = int.Parse(col.name);
        DefancePercent = Map.defance;
    }

    protected void SpriteDisplay()
    {
        CharaBase cmd = GameManager.instance.GetCommand();
        if (cmd == null) return;
        if (Vector2.Distance(transform.position, cmd.transform.position) > cmd.GetDetectRadius)
        {
            obj_sprite.enabled = false;
        }
        else
        {
            obj_sprite.enabled = true;
        }
    }

}
