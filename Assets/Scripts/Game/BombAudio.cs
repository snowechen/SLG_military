using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAudio : MonoBehaviour
{
    AudioSource m_audio;
    [SerializeField]
    AudioClip[] audioClips;
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        Command cmd = GameManager.instance.GetCommand();
        Vector2 pos = transform.position;
        Vector2 cmdpos = cmd.transform.position;
        if(Vector2.Distance(pos,cmdpos) > cmd.GetDetectRadius)
        {
            m_audio.clip = audioClips[1];
        }
        else
        {
            m_audio.clip = audioClips[0];
        }
        m_audio.Play();
    }

    
}
