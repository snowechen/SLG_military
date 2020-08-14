/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 Atkpoint;//打击点
    [Header("打击范围大小")]
    public Vector2 AtkSize;//打击范围
    public float MoveSpeed = 0.0333f;
    public float minDamage = 0.2f;
    public float maxDamage = 0.6f;
    private float Crit = 0.07f;//最低暴击率
    //float[] DisRates = new float[]
    //{
    //    0.92f,0.78f,0.65f,0.5f
    //};
    enum type
    {
        bullet,
        bomb
    }
    [SerializeField]
    type _type;
    public string AtkTag;
    public GameObject BombEffect;

    SpriteRenderer bulletSprite;
    //ParticleSystem bombParticle;
    TrailRenderer bulletTrail;
    public int parentID;
    void Start()
    {
        bulletSprite = GetComponent<SpriteRenderer>();

       // if (_type == type.bomb) bombParticle = GetComponent<ParticleSystem>();
        if (_type == type.bullet) bulletTrail = GetComponent<TrailRenderer>();
    }
    List<CharaBase> ColObjs = new List<CharaBase>();
    /// <summary>
    /// 初始化子弹
    /// </summary>
    /// <param name="point">目标点</param>
    /// <param name="hitrate">打击范围</param>
    /// <param name="atkTag">标签判断是玩家还是敌人</param>
    /// <param name="parentId">子弹所有者ID</param>
    public void Init(Vector2 point,float hitrate,string atkTag,int parentId)
    {
        Atkpoint = point;
        AtkTag = atkTag;
        parentID = parentId;

        Vector2 vec = Atkpoint - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(vec.y, vec.x);
        Vector3 euler = transform.eulerAngles;
        euler.z = angle * Mathf.Rad2Deg;
        transform.eulerAngles = euler;
        //Vector2 size = new Vector2(AtkRadius, AtkRadius);
        //Debug.DrawLine(new Vector3(point.x-size.x, point.y-size.y, -1), new Vector3(point.x + size.x,point.y-size.y,-1), Color.yellow, 3);    
        //Debug.DrawLine(new Vector3(point.x+size.x, point.y-size.y, -1), new Vector3(point.x +size.x,point.y+size.y,-1), Color.yellow, 3);    
        //Debug.DrawLine(new Vector3(point.x+size.x, point.y+size.y, -1), new Vector3(point.x -size.x,point.y+size.y,-1), Color.yellow, 3);    
        //Debug.DrawLine(new Vector3(point.x-size.x, point.y+size.y, -1), new Vector3(point.x -size.x,point.y-size.y,-1), Color.yellow, 3);    
        RaycastHit2D[] cols = Physics2D.BoxCastAll(Atkpoint, AtkSize, 0, Vector2.zero,0, LayerMask.GetMask("Player"));
        
        foreach(var col in cols)
        {
            GameObject target = col.collider.gameObject;
            
            //this.Log(target.name);
            if (target.CompareTag(AtkTag) && target != null)
            {
                float dis = Vector2.Distance(transform.position, Atkpoint);
                float targetStance = target.GetComponent<CharaBase>().GetStance("Attack");
                //this.Log(target.name.ToString());
                //if (dis <= 8) disRateId = 0;
                //else if (dis <= 15) disRateId = 1;
                //else if (dis <= 26) disRateId = 2;
                //else disRateId = 3;
                //命中率
                float Hitrate = Mathf.Pow(hitrate, dis) * targetStance;
                float def = target.GetComponent<CharaBase>().GetDefPercent();
                if (def + Crit >= Hitrate) Hitrate = Crit; else Hitrate -= def;
                float min = 0.5f - Hitrate / 2;
                float max = 0.5f + Hitrate / 2;
                float rand = Random.Range(0f, 1f);
                if (rand >= min && rand <= max) {
                    
                    //this.Log("命中目标:"+target.name);
                    ColObjs.Add(target.GetComponent<CharaBase>());
                }
                //暴露位置几率
                float CounterAtkRand = Random.Range(0, 100);
                if (CounterAtkRand > 50)
                {
                    Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                    target.GetComponent<CharaBase>().TakeDamage(0, pos,parentID);
                }
                //this.Log("距离："+dis + "几率："+Hitrate);
            }
        }

        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        Vector2 end = Atkpoint;
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        //this.Log("距离;" + Vector2.Distance(start, end));
        float time = MoveSpeed * Vector2.Distance(start, end);//总时间
        float t = 0;

        while (t / time < 1)
        {
                t += Time.deltaTime;
                GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(start, end, t / time));
            
            yield return new WaitForFixedUpdate();
        }
        if(BombEffect) Instantiate(BombEffect, transform.position, Quaternion.identity);

        foreach(var obj in ColObjs)
        {
            if (obj == null) continue;
            float damage = Random.Range(minDamage, maxDamage);
            obj.TakeDamage(damage,parentID);
        }
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        Command cmd = GameManager.instance.GetCommand();
        if(cmd!= null)
        {
            if(Vector2.Distance(transform.position,cmd.transform.position) > cmd.GetDetectRadius)
            {
                Bhide();
            }
            else
            {
                Bshow();
            }
        }
    }

    void Bshow()
    {
        bulletSprite.enabled = true;
        if (_type == type.bullet)
        {
            bulletTrail.enabled = true;
        }
        //if(_type == type.bomb)
        //{
        //   if(!bombParticle.isPlaying) bombParticle.Play();
        //}
    }

    void Bhide()
    {
        bulletSprite.enabled = false;
        if (_type == type.bullet)
        {
            bulletTrail.enabled = false;
        }
        //if (_type == type.bomb)
        //{
        //    bombParticle.Stop();
        //}
    }
}
