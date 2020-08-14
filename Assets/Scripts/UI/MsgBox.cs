using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour
{

    Animator anim;
    public float DestroyTime =5;
    public Text msg;
    public void FadeOut()
    {
        anim.SetBool("Display", false);
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (DestroyTime > 0)
        {
            DestroyTime -= Time.deltaTime;
            
        }
        if (DestroyTime <= 0) FadeOut();

    }

    public void Destroy()
    {
        MessageSystemMag.instance.MsgBoxCut();
        Destroy(gameObject);
    }
}
