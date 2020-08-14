using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;
    [SerializeField]
    AudioSource BGM;
    [SerializeField]
    AudioClip[] bgmClip;
    [SerializeField,Header("噪音播放器")]
    AudioSource FX_audio;
    [SerializeField,Header("主要的音频控制器")]
    AudioMixer MainMixer;
    [SerializeField]
    AudioMixer zyMixer;//噪音控制器
    /// <summary>
    /// 主指令播放器
    /// </summary>
    AudioSource M_audio;
    /// <summary>
    /// 0:所有起始，1:指挥官over，2:突击over，3:炮兵over，4:侦察over
    /// </summary>
    [SerializeField]
    AudioClip[] StartOrOver;
    [SerializeField,Header("指挥官呼叫室内")]
    AudioClip[] Command_Callsign_in;//呼叫单位指令室内
    [SerializeField, Header("指挥官呼叫室外")]
    AudioClip[] Command_Callsign_out;//呼叫单位指令室外
    /// <summary>
    /// 0:准许进攻,1:进攻
    /// </summary>
    [SerializeField, Header("进攻指令室内")]
    AudioClip[] Command_order_engage_in;//进攻指令室内
    /// <summary>
    /// 0:准许进攻,1:进攻
    /// </summary>
    [SerializeField, Header("进攻指令室外")]
    AudioClip[] Command_order_engage_out;//进攻指令室外
    [SerializeField,Header("移动指令室内")]
    AudioClip[] Command_order_move_in;//移动指令室内
    [SerializeField, Header("移动指令室外")]
    AudioClip[] Command_order_move_out;//移动指令室外
    [SerializeField, Header("移动指令坐标")]
    AudioClip[] Command_order_move_pos;//移动指令坐标
    /// <summary>
    /// 0:待命,1:返回补给,2:允许返回补给
    /// </summary>
    [SerializeField, Header("返回营部室内")]
    AudioClip[] Command_Return_in;//返回营部室内
    /// <summary>
    /// 0:待命,1:返回补给,2:允许返回补给
    /// </summary>
    [SerializeField, Header("返回营部室外")]
    AudioClip[] Command_Return_out;//返回营部室外
    [SerializeField,Header("炮兵进攻室内")]
    AudioClip[] Command_order_art_in;//炮兵进攻室内
    [SerializeField,Header("炮兵进攻室外")]
    AudioClip[] Command_order_art_out;//炮兵进攻室内
    /// <summary>
    /// 0:汇报减员,1:汇报位置,2:汇报补给
    /// </summary>
    [SerializeField,Header("汇报单位请求指令室内")]
    AudioClip[] Command_order_report_in;//汇报情况室内
    /// <summary>
    /// 0:汇报减员,1:汇报位置,2:汇报补给
    /// </summary>
    [SerializeField, Header("汇报单位请求指令室外")]
    AudioClip[] Command_order_report_out;//汇报情况室外
    /// <summary>
    /// 0:其他队员,1:炮兵,2:侦察
    /// </summary>
    [SerializeField, Header("单位返回营部回答")]
    AudioClip[] Obj_Return;
    [SerializeField,Header("单位返回补给")]
    AudioClip[] Obj_Return_Supply;//返回补给
    [SerializeField,Header("单位应答报名a组")]
    AudioClip[] Obj_callsign_a;
    [SerializeField, Header("单位应答报名b组")]
    AudioClip[] Obj_callsign_b;
    [SerializeField, Header("单位回答收到")]
    AudioClip[] Obj_copy_a;
    [SerializeField, Header("单位回答是")]
    AudioClip[] Obj_copy_b;
    [SerializeField,Header("单位执行move")]
    AudioClip[] obj_moving;
    [SerializeField,Header("单位执行攻击")]
    AudioClip[] obj_engage;
    /// <summary>
    /// 0-1:检员,2:受伤人数,3:没有人受伤
    /// </summary>
    [SerializeField,Header("检员口令")]
    AudioClip[] obj_casualty;
    [SerializeField,Header("单位报数突击")]
    AudioClip[] obj_p_number_ass;
    [SerializeField,Header("单位报数炮兵")]
    AudioClip[] obj_p_number_art;
    [SerializeField,Header("单位报数侦察")]
    AudioClip[] obj_p_number_recon;
    [SerializeField,Header("单位位置突击")]
    AudioClip[] obj_Coordinate_ass;
    [SerializeField,Header("单位位置炮兵")]
    AudioClip[] obj_Coordinate_art;
    [SerializeField,Header("单位位置侦察")]
    AudioClip[] obj_Coordinate_recon;
    /// <summary>
    /// 0:充足，1:较少，2:极少，3:耗尽
    /// </summary>
    [SerializeField,Header("单位补给突击")]
    AudioClip[] obj_Quantity_ass;
    [SerializeField,Header("单位补给炮兵")]
    AudioClip[] obj_Quantity_art;
    [SerializeField,Header("单位补给侦察")]
    AudioClip[] obj_Quantity_recon;
    [SerializeField,Header("单位汇报位置前话")]
    AudioClip[] obj_Position_order;
    [SerializeField,Header("汇报弹药量")]
    AudioClip[] obj_ammo;
    [SerializeField,Header("汇报医药包")]
    AudioClip[] obj_medic;
    [SerializeField,Header("遭到攻击等待反击")]
    AudioClip[] obj_def_awaiting;
    [SerializeField,Header("遭到攻击执行反击")]
    AudioClip[] obj_def_defensive;
    [SerializeField,Header("发现敌人突击")]
    AudioClip[] obj_E_detected_ass;
    [SerializeField, Header("发现敌人炮兵")]
    AudioClip[] obj_E_detected_art;
    [SerializeField, Header("发现敌人侦察")]
    AudioClip[] obj_E_detected_recon;
    [SerializeField, Header("发现敌人指挥")]
    AudioClip[] obj_E_detected_cmd;
    [SerializeField,Header("抵达目的汇报")]
    AudioClip[] obj_arrive;
    [SerializeField,Header("请求返回补给")]
    AudioClip[] obj_retreat_supply;
    [SerializeField,Header("因伤亡请求撤退")]
    AudioClip[] obj_retreat_casualty;
    [SerializeField,Header("击杀敌方单位时汇报")]
    AudioClip[] obj_enemy_killed;

    bool isPlaying = false;
    public bool GetIsPlaying { get { return isPlaying; } }
    void Awake()
    {
        instance = this;
        M_audio = GetComponent<AudioSource>();
    }
    /// <summary>
    /// 单位应答报名ab2组
    /// </summary>
    List<AudioClip[]> Obj_callsign_AB;
    /// <summary>
    /// 单位报数检员
    /// </summary>
    List<AudioClip[]> Obj_p_numbers;
    /// <summary>
    /// 单位位置数据
    /// </summary>
    List<AudioClip[]> Obj_Coordinates;
    /// <summary>
    /// 单位补给汇报
    /// </summary>
    List<AudioClip[]> Obj_Quantitys;
    /// <summary>
    /// 发现敌人汇总
    /// </summary>
    List<AudioClip[]> Obj_Enemy_detected;
    bool ambient;
    List<AudioClip> SoundPlayList = new List<AudioClip>();
    private void Start()
    {
        Obj_callsign_AB = new List<AudioClip[]>() { Obj_callsign_a, Obj_callsign_b };
        Obj_p_numbers = new List<AudioClip[]>() { obj_p_number_ass, obj_p_number_art, obj_p_number_recon };
        Obj_Coordinates = new List<AudioClip[]>() { obj_Coordinate_ass, obj_Coordinate_art, obj_Coordinate_recon };
        Obj_Quantitys = new List<AudioClip[]>() { obj_Quantity_ass, obj_Quantity_ass, obj_Quantity_recon };
        Obj_Enemy_detected = new List<AudioClip[]>() { obj_E_detected_ass, obj_E_detected_art, obj_E_detected_recon, obj_E_detected_cmd };
    }
    public void BGMFX(bool flag)
    {
        ambient = flag;
        if (flag)
        {
            if (BGM.clip == bgmClip[1]) return;
            BGM.clip = bgmClip[1];
        }
        else
        {
            if (BGM.clip == bgmClip[0]) return;
            BGM.clip = bgmClip[0];
        }
        BGM.Play();
    }

    public void SetBGMVolume(float volume)
    {
        MainMixer.SetFloat("BGM_volume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        MainMixer.SetFloat("FX_volume", volume);
    }


    public IEnumerator CommandAllBack()
    {
        //开始音频
        SoundPlayList.Add(StartOrOver[0]);
        //呼叫对象
        if (ambient) { SoundPlayList.Add(Command_Callsign_in[0]); }
        else { SoundPlayList.Add(Command_Callsign_out[0]); }
        //指令
        if (ambient) { SoundPlayList.Add(Command_Return_in[0]); }
        else { SoundPlayList.Add(Command_Return_out[0]); }
        //指令结束
        SoundPlayList.Add(StartOrOver[1]);
        yield return StartCoroutine(CommandOrderPlay());
    }
    /// <summary>
    /// 指挥官播报
    /// </summary>
    /// <returns></returns>
    IEnumerator CommandOrderPlay()
    {
        int index = 0;
        isPlaying = true;
        while (true)
        {
            //this.Log("" + index);
            if (!M_audio.isPlaying)
            {
                if (index >= SoundPlayList.Count) break;
                M_audio.clip = SoundPlayList[index];
                M_audio.Play();
                index++;
            }
            yield return new WaitForFixedUpdate();
        }
        M_audio.Stop();
        SoundPlayList.Clear();
        isPlaying = false;
    }
    IEnumerator ObjCallBackPlay(bool flag)
    {
        int index = 0;
        isPlaying = true;
        //噪音播放
        Fx_Play(flag);
        while (true)
        {
            //this.Log("" + index);
            if (!M_audio.isPlaying)
            {
                if (index >= SoundPlayList.Count) break;
                M_audio.clip = SoundPlayList[index];
                M_audio.Play();
                index++;
            }
            yield return new WaitForFixedUpdate();
        }
        M_audio.Stop();
        FX_audio.Stop();
        SoundPlayList.Clear();
        isPlaying = false;
    }
    public IEnumerator AllCallBack()
    {
        //开始音频
        SoundPlayList.Add(StartOrOver[0]);
        //回答
        int rand = Random.Range(0, Obj_Return.Length);
        SoundPlayList.Add(Obj_Return[rand]);
        SoundPlayList.Add(StartOrOver[rand + 2]);
        yield return StartCoroutine(ObjCallBackPlay(false));
    }

    void Fx_Play(bool flag)
    {
        if(flag) zyMixer.SetFloat("ZYFX_volume", 6);
        else zyMixer.SetFloat("ZYFX_volume", 0);
        FX_audio.Play();
    }

    int ObjID = 0;
    //移动指令
    public IEnumerator Move_Order(Vector2Int pos,int objId)
    {
        ObjID = objId;
        //开始音频
        SoundPlayList.Add(StartOrOver[0]);
        //呼叫对象
        if (ambient) { SoundPlayList.Add(Command_Callsign_in[objId]); }
        else { SoundPlayList.Add(Command_Callsign_out[objId]); }
        //指令
        int rand = Random.Range(0, Command_order_move_in.Length);
        if (ambient) { SoundPlayList.Add(Command_order_move_in[rand]); }
        else { SoundPlayList.Add(Command_order_move_out[rand]); }
        //X坐标
        int x = pos.x;
        if (x <= 10) SoundPlayList.Add(Command_order_move_pos[x]);
        else
        {
            int i = x / 10 + 10;
            int j = x % 10 < 1 ? Command_order_move_pos.Length - 1 : x % 10 + 10;
            SoundPlayList.Add(Command_order_move_pos[i]);
            SoundPlayList.Add(Command_order_move_pos[j]);
        }
        //Y
        int y = pos.y;
        if (y <= 10) SoundPlayList.Add(Command_order_move_pos[y]);
        else
        {
            int i = y / 10 + 10;
            int j = y % 10 < 1 ? Command_order_move_pos.Length - 1 : y % 10 + 10;
            SoundPlayList.Add(Command_order_move_pos[i]);
            SoundPlayList.Add(Command_order_move_pos[j]);
        }
        //指令结束
        SoundPlayList.Add(StartOrOver[1]);
        yield return StartCoroutine(CommandOrderPlay());

    }
    /// <summary>
    /// 移动指令应答
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator Move_CallBack(int index)
    {
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        switch (index)
        {
            case 0:
                //成功低噪音
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                copyid = ObjID < 10 ? 0 : ObjID == 10 ? 1 : 2;
                SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                SoundPlayList.Add(obj_moving[Random.Range(copyid * 3, copyid * 3 + 3)]);
                fxFlag =false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);
                copyid = ObjID < 10 ? 0 : ObjID == 10 ? 1 : 2;
                if (Random.Range(0, 1f) > 0.5f) { SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]); }
                else {
                    SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                }
                if (Random.Range(0, 1f) > 0.5f) { SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]); }
                else
                {
                    SoundPlayList.Add(obj_moving[Random.Range(copyid * 3, copyid * 3 + 3)]);
                }
                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid+2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 进攻指令
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="objId"></param>
    /// <returns></returns>
    public IEnumerator Engage_Order(Vector2Int pos,int objId)
    {
        ObjID = objId;
        //开始音频
        SoundPlayList.Add(StartOrOver[0]);
        //呼叫对象
        if (ambient) { SoundPlayList.Add(Command_Callsign_in[objId]); }
        else { SoundPlayList.Add(Command_Callsign_out[objId]); }
        //指令
        if (objId == 10)
        {
            int num = Random.Range(0, 4);
            if (ambient) { SoundPlayList.Add(Command_order_art_in[num]); }
            else { SoundPlayList.Add(Command_order_art_out[num]); }
        }
        else
        {
            if (ambient) { SoundPlayList.Add(Command_order_engage_in[1]); }
            else { SoundPlayList.Add(Command_order_engage_out[1]); }
        }
        //X坐标
        int x = pos.x;
        if (x <= 10) SoundPlayList.Add(Command_order_move_pos[x]);
        else
        {
            int i = x / 10 + 10;
            int j = x % 10 < 1 ? Command_order_move_pos.Length - 1 : x % 10 + 10;
            SoundPlayList.Add(Command_order_move_pos[i]);
            SoundPlayList.Add(Command_order_move_pos[j]);
        }
        //Y
        int y = pos.y;
        if (y <= 10) SoundPlayList.Add(Command_order_move_pos[y]);
        else
        {
            int i = y / 10 + 10;
            int j = y % 10 < 1 ? Command_order_move_pos.Length - 1 : y % 10 + 10;
            SoundPlayList.Add(Command_order_move_pos[i]);
            SoundPlayList.Add(Command_order_move_pos[j]);
        }
        //指令结束
        SoundPlayList.Add(StartOrOver[1]);
        yield return StartCoroutine(CommandOrderPlay());
    }
    /// <summary>
    /// 执行进攻应答
    /// </summary>
    /// <param name="index">通讯状态</param>
    /// <returns></returns>
    public IEnumerator Engage_CallBack(int index)
    {
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = ObjID < 10 ? 0 : ObjID == 10 ? 1 : 2;
        switch (index)
        {
            case 0:
                //成功低噪音
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                SoundPlayList.Add(obj_engage[Random.Range(copyid * 3, copyid * 3 + 3)]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);
                
                SoundPlayList.Add(RandAudio(Obj_copy_a[copyid],0.65f));//回答
                
                SoundPlayList.Add(RandAudio(obj_engage[Random.Range(copyid * 3, copyid * 3 + 3)],0.65f));
                
                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 返回补给指令
    /// </summary>
    /// <param name="objId"></param>
    /// <returns></returns>
    public IEnumerator Command_Return_Order(int objId)
    {
        ObjID = objId;
        //开始音频
        SoundPlayList.Add(StartOrOver[0]);
        //呼叫对象
        if (ambient) { SoundPlayList.Add(Command_Callsign_in[objId]); }
        else { SoundPlayList.Add(Command_Callsign_out[objId]); }
        //指令
       // int rand = Random.Range(0, Command_order_move_in.Length);
        if (ambient) { SoundPlayList.Add(Command_Return_in[1]); }
        else { SoundPlayList.Add(Command_Return_out[1]); }
        //指令结束
        SoundPlayList.Add(StartOrOver[1]);
        yield return StartCoroutine(CommandOrderPlay());
    }
    /// <summary>
    /// 返回补给应答
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator Obj_return_order(int index)
    {
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = ObjID < 10 ? 0 : ObjID == 10 ? 1 : 2;
        switch (index)
        {
            case 0:
                //成功低噪音
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                SoundPlayList.Add(Obj_Return_Supply[copyid]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1],0.65f));
               
                SoundPlayList.Add(RandAudio(Obj_copy_a[copyid],0.65f));//回答
               
                SoundPlayList.Add(RandAudio(Obj_Return_Supply[copyid],0.65f));
                
                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 汇报情况
    /// </summary>
    /// <param name="objId"></param>
    /// <returns></returns>
    public IEnumerator Command_order_report(int objId,int type)
    {
        ObjID = objId;
        //开始音频
        SoundPlayList.Add(StartOrOver[0]);
        //呼叫对象
        if (ambient) { SoundPlayList.Add(Command_Callsign_in[objId]); }
        else { SoundPlayList.Add(Command_Callsign_out[objId]); }
        //指令
        if (ambient) { SoundPlayList.Add(Command_order_report_in[type]); }
        else { SoundPlayList.Add(Command_order_report_out[type]); }

        //指令结束
        SoundPlayList.Add(StartOrOver[1]);
        yield return StartCoroutine(CommandOrderPlay());
    }
    /// <summary>
    /// 单位汇报
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public IEnumerator Obj_report_order(int index,CharaBase obj,int type)
    {
        //this.Log(type.ToString());
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = ObjID < 10 ? 0 : ObjID == 10 ? 1 : 2;
        switch (index)
        {
            case 0:
                //成功低噪音
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                ObjReport(obj, type, copyid,1);

                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1],0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid],0.65f));//回答
                //具体内容
                ObjReport(obj, type, copyid,0.65f);

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 一定几率播报，失败则发出噪音
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="rand"></param>
    /// <returns></returns>
    AudioClip RandAudio(AudioClip clip,float rand)
    {
        AudioClip tempClip;
        float min = 0.5f - rand / 2;
        float max = 0.5f + rand / 2;
        float random = Random.Range(0, 1f);
        if (random > min && random < max)
        {
            tempClip = clip;
        }
        else
        {
            tempClip = StartOrOver[StartOrOver.Length - 1];
        }

        return tempClip;
    }
    /// <summary>
    /// 三种汇报模式
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="type">种类</param>
    /// <param name="copyid">单位类型</param>
    /// <param name="rand">成功率</param>
    void ObjReport(CharaBase obj,int type,int copyid,float rand)
    {
        switch (type)
        {
            case 0://检员
                if (obj.GetHPPercent >= 1 && obj.GetDeBuff == 0) { SoundPlayList.Add(RandAudio(obj_casualty[(copyid + 1) * 3 + (1 * copyid)],rand)); }
                else if (obj.GetDeBuff > 0 || obj.GetHPPercent < 1)
                {
                    SoundPlayList.Add(RandAudio(obj_casualty[Random.Range(copyid * 4, copyid * 4 + 1)],rand));
                    SoundPlayList.Add(RandAudio(Obj_p_numbers[copyid][obj.GetHP],rand));
                    SoundPlayList.Add(RandAudio(obj_casualty[2 + (4 * copyid)],rand));
                    SoundPlayList.Add(RandAudio(Obj_p_numbers[copyid][obj.GetDeBuff],rand));
                }
                break;
            case 1://位置
                SoundPlayList.Add(RandAudio(obj_Position_order[Random.Range(copyid * 3, copyid * 3 + 3)], rand));
                Vector2Int pos = new Vector2Int(obj.GetPostionInt.x, obj.GetPostionInt.y);
                //X坐标
                int x = pos.x;
                if (x <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][x],rand));
                else
                {
                    int i = x / 10 + 10;
                    int j = x % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : x % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i],rand));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j],rand));
                }
                //Y
                int y = pos.y;
                if (y <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][y],rand));
                else
                {
                    int i = y / 10 + 10;
                    int j = y % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : y % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i],rand));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j],rand));
                }
                break;
            case 2://补给
                float Qparcent = 0;
                if (copyid != 1) Qparcent = obj.GetBulletPercent;
                else Qparcent = obj.GetBombPercent;
                int audioID = 0;
                if (Qparcent >= 0.5f){audioID = 0;}
                else if (Qparcent >= 0.25f){ audioID = 1;}
                else if (Qparcent > 0){audioID = 2; }
                else {audioID = 3;}
                SoundPlayList.Add(RandAudio(obj_ammo[copyid], rand));
                SoundPlayList.Add(RandAudio(Obj_Quantitys[copyid][audioID], rand));
                SoundPlayList.Add(RandAudio(obj_medic[copyid], rand));
                float Drug = obj.GetDrugPercent;
                if (Drug >= 0.5) audioID = 0;
                else if (Drug >= 0.25f) audioID = 1;
                else if (Drug > 0) audioID = 2;
                else audioID = 3;
                SoundPlayList.Add(RandAudio(Obj_Quantitys[copyid][audioID], rand));
                break;
        }
    }
    /// <summary>
    /// 遭受攻击等待回击指令
    /// </summary>
    /// <returns></returns>
    public IEnumerator Obj_defence_awaiting(int objId,int type)
    {
        CharaBase obj = GameManager.instance.GetObj(objId);
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        this.ObjID = objId;
        copyid = objId < 10 ? 0 : objId == 10 ? 1 : 2;
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                SoundPlayList.Add(obj_def_awaiting[copyid]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1], 0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid], 0.65f));//回答
                //具体内容
                SoundPlayList.Add(RandAudio(obj_def_awaiting[copyid],0.65f));

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 遭受攻击并执行反击
    /// </summary>
    /// <param name="objId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public IEnumerator Obj_defensive(int objId,int type)
    {
        ObjID = objId;
        CharaBase obj = GameManager.instance.GetObj(objId);
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = objId < 10 ? 0 : objId == 10 ? 1 : 2;
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                SoundPlayList.Add(obj_def_defensive[copyid]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1], 0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid], 0.65f));//回答
                //具体内容
                SoundPlayList.Add(RandAudio(obj_def_defensive[copyid], 0.65f));

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 发现敌方单位汇报
    /// </summary>
    /// <returns></returns>
    public IEnumerator Obj_Enemy_Detected(int type,int objId,CharaBase obj)
    {
        int targetType = (int)obj.type;
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        this.ObjID = objId;
        copyid = objId < 10 ? 0 : objId == 10 ? 1 : 2;
        Vector2Int targetPos = new Vector2Int(obj.GetPostionInt.x, obj.GetPostionInt.y);
        int x, y;
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                SoundPlayList.Add(Obj_Enemy_detected[targetType][Random.Range(copyid*2,copyid*2+2)]);
                //X坐标
                 x = targetPos.x;
                if (x <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][x], 1));
                else
                {
                    int i = x / 10 + 10;
                    int j = x % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : x % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i], 1));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j], 1));
                }
                //Y
                y = targetPos.y;
                if (y <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][y], 1));
                else
                {
                    int i = y / 10 + 10;
                    int j = y % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : y % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i], 1));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j], 1));
                }
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1], 0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid], 0.65f));//回答
                //具体内容
                SoundPlayList.Add(RandAudio(Obj_Enemy_detected[targetType][Random.Range(copyid * 2, copyid * 2 + 2)], 0.65f));
                //X坐标
                 x = targetPos.x;
                if (x <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][x], 0.65f));
                else
                {
                    int i = x / 10 + 10;
                    int j = x % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : x % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i], 0.65f));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j], 0.65f));
                }
                //Y
                 y = targetPos.y;
                if (y <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][y], 0.65f));
                else
                {
                    int i = y / 10 + 10;
                    int j = y % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : y % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i], 0.65f));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j], 0.65f));
                }

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }

    /// <summary>
    /// 抵达目标汇报
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objId"></param>
    /// <returns></returns>
    public IEnumerator Obj_Arrive(int type,int objId)
    {
        ObjID = objId;
        //CharaBase obj = GameManager.instance.GetObj(objId);
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = objId < 10 ? 0 : objId == 10 ? 1 : 2;
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                SoundPlayList.Add(obj_arrive[Random.Range(copyid*2,copyid*2+2)]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][ObjID - 1], 0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid], 0.65f));//回答
                //具体内容
                SoundPlayList.Add(RandAudio(obj_arrive[Random.Range(copyid * 2, copyid * 2 + 2)], 0.65f));

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 请求返回补给
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objId"></param>
    /// <returns></returns>
    public IEnumerator Obj_Retreat_supply(int type,int objId)
    {
        //CharaBase obj = GameManager.instance.GetObj(objId);
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = objId < 10 ? 0 : objId == 10 ? 1 : 2;
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][objId - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                SoundPlayList.Add(obj_retreat_supply[Random.Range(copyid * 2, copyid * 2 + 2)]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][objId - 1], 0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid], 0.65f));//回答
                //具体内容
                SoundPlayList.Add(RandAudio(obj_retreat_supply[Random.Range(copyid * 2, copyid * 2 + 2)], 0.65f));

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    /// <summary>
    /// 伤亡惨重请求返回
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objId"></param>
    /// <param name="lv"></param>
    /// <returns></returns>
    public IEnumerator Obj_Retreat_Casualty(int type,int objId,int lv)
    {
        //CharaBase obj = GameManager.instance.GetObj(objId);
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = 0;
        copyid = objId < 10 ? 0 : objId == 10 ? 1 : 2;
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][objId - 1]);//报名
                //SoundPlayList.Add(Obj_copy_a[copyid]);//回答
                //具体内容
                SoundPlayList.Add(obj_retreat_casualty[copyid + lv * 3]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                SoundPlayList.Add(RandAudio(Obj_callsign_AB[Random.Range(0, 2)][objId - 1], 0.65f));
                //SoundPlayList.Add(RandAudio(Obj_copy_a[copyid], 0.65f));//回答
                //具体内容
                SoundPlayList.Add(RandAudio(obj_retreat_casualty[copyid + lv * 3], 0.65f));

                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
    public IEnumerator Obj_Enemy_Killed(int type,int AtkobjId,int objId)
    {
        SoundPlayList.Add(StartOrOver[0]);
        bool fxFlag = false;
        int copyid = AtkobjId < 10 ? 0 : AtkobjId == 10 ? 1 : 2;
        Debug.Log("击杀者ID:" + AtkobjId);
        //坐标
        CharaBase obj = GameManager.instance.GetObj(objId);
        Vector2Int pos = new Vector2Int(obj.GetPostionInt.x, obj.GetPostionInt.y);
        int killsId = objId == 0 ? 2 : objId < 10 ? 3 : objId == 10 ? 1 : 4;
        Debug.Log("击杀单位ID:" + killsId.ToString() + "单位ID:"+ObjID);
        switch (type)
        {
            case 0://成功低噪音
                //回答
                SoundPlayList.Add(Obj_callsign_AB[Random.Range(0, 2)][AtkobjId-1]);//报名
                //位于
                SoundPlayList.Add(obj_enemy_killed[copyid * 5]);

                
                //X坐标
                int x = pos.x;
                if (x <= 10) SoundPlayList.Add(Obj_Coordinates[copyid][x]);
                else
                {
                    int i = x / 10 + 10;
                    int j = x % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : x % 10 + 10;
                    SoundPlayList.Add(Obj_Coordinates[copyid][i]);
                    SoundPlayList.Add(Obj_Coordinates[copyid][j]);
                }
                //Y
                int y = pos.y;
                if (y <= 10) SoundPlayList.Add(Obj_Coordinates[copyid][y]);
                else
                {
                    int i = y / 10 + 10;
                    int j = y % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : y % 10 + 10;
                    SoundPlayList.Add(Obj_Coordinates[copyid][i]);
                    SoundPlayList.Add(Obj_Coordinates[copyid][j]);
                }
                //具体内容
                SoundPlayList.Add(obj_enemy_killed[copyid * 5+ killsId]);
                fxFlag = false;
                break;
            case 1:
                //高噪音
                float rand = 0.65f;
                //X坐标
                x = pos.x;
                if (x <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][x], rand));
                else
                {
                    int i = x / 10 + 10;
                    int j = x % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : x % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i], rand));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j], rand));
                }
                //Y
                y = pos.y;
                if (y <= 10) SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][y], rand));
                else
                {
                    int i = y / 10 + 10;
                    int j = y % 10 < 1 ? Obj_Coordinates[copyid].Length - 1 : y % 10 + 10;
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][i], rand));
                    SoundPlayList.Add(RandAudio(Obj_Coordinates[copyid][j], rand));
                }
                //具体内容
                SoundPlayList.Add(RandAudio(obj_enemy_killed[copyid * 5 + killsId],rand));
                fxFlag = true;
                break;
            case 2:
                //失败噪音
                SoundPlayList.Add(StartOrOver[StartOrOver.Length - 1]);
                fxFlag = true;
                break;
        }
        SoundPlayList.Add(StartOrOver[copyid + 2]);
        yield return StartCoroutine(ObjCallBackPlay(fxFlag));
    }
}
