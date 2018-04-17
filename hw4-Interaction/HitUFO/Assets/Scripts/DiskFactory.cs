using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {

    private List<UFOManager> usedUFO;
    private List<UFOManager> freeUFO;
    private int unhiitedNum;

    void Awake()
    {
        usedUFO = new List<UFOManager>();
        freeUFO = new List<UFOManager>();
        unhiitedNum = 0;
    }

    public UFOManager GetDisk(int round)
    {
        UFOManager neo;
        if (freeUFO.Count != 0)
        {
            neo = freeUFO[0];
            freeUFO.Remove(freeUFO[0]);
            neo.Reborn(round);
        }
        else
        {
            neo = new UFOManager(round);
        }
        usedUFO.Add(neo);
        return neo;
    }

    public void FreeDisk(UFOManager old)
    {
        usedUFO.Remove(old);//直接remove可以找到元素
        freeUFO.Add(old);
        //Debug.Log(usedUFO.Count);
        //Debug.Log(freeUFO.Count);
    }

    void Update()
    { 
        foreach(UFOManager ufo in usedUFO)
        {
            if (ufo.GetPosition().z < -5)//z<-5 means player failed to hit it, so recycle it. 
            {
                FreeDisk(ufo);
                unhiitedNum++;
                break;
            }
        }
    }

    public int GetUnhittedNum()
    {
        int num = unhiitedNum;
        unhiitedNum = 0;
        return num;
    }

    public void GameStart()
    {
        while (usedUFO.Count != 0)
        {
            usedUFO[0].GetUFO().SetActive(false);
            freeUFO.Add(usedUFO[0]);
            usedUFO.Remove(usedUFO[0]);
        }
        unhiitedNum = 0;
    }
}
