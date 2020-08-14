/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUIMag : MonoBehaviour
{
    public static TimeUIMag instance;
    public Text[] texts;

    float[] times;

    private void Start()
    {
        instance = this;
        times = new float[4];
        //times[2] = 3700;
        StartCoroutine(TimesUpdate());
    }

    public void SetTime(int index,float second)
    {
        if (index >= times.Length || index <0) return;
        times[index] = second;
    }
    
    public float GetTime(int index)
    {
        return times[index];
    }

    IEnumerator TimesUpdate()
    {
        while (true)
        {
            for (int i = 0; i < times.Length; i++)
            {
                if (times[i] > 0)
                {
                    times[i] -= Time.deltaTime;
                }
                else { times[i] = 0; }

                int M = (int)(times[i] / 60);
                float S = times[i] % 60;
                 
                texts[i].text = "倒计时器" + i + "： " + string.Format("{0:00}",M) + ":" + string.Format("{0:00.000}",S);
            }
            yield return new WaitForFixedUpdate();
        }
        
    }
}
