using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject Command;
    public GameObject Assault;
    public GameObject Artillery;
    public GameObject Scout;

    public List<CharaBase> EnemyList;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        int i = 0;
        foreach(var e in EnemyList)
        {
            e.SetObjID(i);
            i++;
        }
    }

    public void Die(CharaBase obj)
    {
        EnemyList.Remove(obj);
        Destroy(obj.gameObject);
    }

    public void IsWinner()
    {
        foreach(var e in EnemyList)
        {
            Destroy(e.gameObject);
        }
        EnemyList.Clear();
        GameManager.instance.IsWinner();
    }

    public Transform GetCommand()
    {
        return EnemyList[0].transform;
    }
    void Update()
    {
        
    }
}
