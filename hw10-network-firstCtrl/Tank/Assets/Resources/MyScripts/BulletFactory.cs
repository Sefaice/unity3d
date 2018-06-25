using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{ 
    private List<GameObject> bulletList;
    private float bulletSpeed = 20.0f;

    void Awake()
    {
        bulletList = new List<GameObject>();
    }

    public GameObject FireBullet(Vector3 pos, Vector3 des)
    {
        pos.y = 1.68f;
        GameObject bullet = Instantiate(Resources.Load("MyPrefabs/Shell", typeof(GameObject)), pos, Quaternion.identity, null) as GameObject;
        bullet.transform.forward = des;
        BulletControl bulletScript = bullet.gameObject.AddComponent<BulletControl>() as BulletControl;
        bulletList.Add(bullet);
        return bullet;
    }

    void Update()
    {
        foreach(GameObject bullet in bulletList)
        {
            //bullet.transform.Translate(bullet.transform.forward * Time.deltaTime * bulletSpeed);translate的方向第一个参数会改变它的forward，导致运动方向错误
            bullet.transform.position += bullet.transform.forward * bulletSpeed * Time.deltaTime;
        }
    }

    public void RemoveBullet(GameObject bullet)
    {
        //先从list中删除，再销毁对象
        //Debug.Log(bulletList.Count);
        bulletList.Remove(bullet);
        Destroy(bullet);
    }

}
