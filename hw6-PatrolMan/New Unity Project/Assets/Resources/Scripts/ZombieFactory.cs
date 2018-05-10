using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFactory : MonoBehaviour
{
    private List<ZombieManager> zombieList;

    void Awake()
    {

    }

    public List<ZombieManager> GetZombies()
    {
        List<ZombieManager> zombieList = new List<ZombieManager>();
        for (int i = 1; i <= 7; i++)
        {
            zombieList.Add(new ZombieManager(i));
        }
        return zombieList;
    }





}
