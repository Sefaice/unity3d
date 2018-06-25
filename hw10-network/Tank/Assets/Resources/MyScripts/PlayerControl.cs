using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    private GameObject player;
    private GameObject home;
    private float playerSpeed = 10f;
    private bool gunCD = false;

    [SyncVar]
    public int type = 0;

    // Use this for initialization
    void Start()
    {
        player = this.gameObject;

        if (isLocalPlayer)
        {
            if (isServer)
            {
                type = 1;
            }
            else
            {
                type = 0;
            }
        }

        if(type==1)
        {
            player.name = "Player";
            player.tag = "Player";
            MeshRenderer[] renderers = player.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.red;
            }
            player.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            player.name = "AI";
            player.tag = "AI";
            MeshRenderer[] renderers = player.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.blue;
            }
            player.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        if (!isLocalPlayer)
            return;

        //create home
        CmdHome();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        //先获得movement，再依次改变方向和进行运动
        //if为了避免不运动会一直向前看的情况
        if (Input.GetAxis("HorizontalPlayer1") != 0 || Input.GetAxis("VerticalPlayer1") != 0)
        {
            float hor = Input.GetAxis("HorizontalPlayer1");
            float ver = Input.GetAxis("VerticalPlayer1");
            float num = Mathf.Abs(hor) + Mathf.Abs(ver);
            hor = hor / num;
            ver = ver / num;
            Vector3 movement = new Vector3(hor, 0.0f, ver);
            transform.rotation = Quaternion.LookRotation(movement);
            transform.Translate(movement * playerSpeed * Time.deltaTime, Space.World);
        }

        //按下开火键且冷却结束
        if (Input.GetKeyDown(KeyCode.Space) && !gunCD)
        {
            CmdFire();
        }
    }

    //服务器行为，在服务器创建子弹，并spawn到所有客户端，因为是spawn，一个摧毁就会都摧毁
    [Command]
    public void CmdFire()
    {
        gunCD = true;
        StartCoroutine(Timer());
        Vector3 des = player.transform.forward;
        Vector3 pos = player.transform.position;
        if (des.x != 0.0f)
        {
            pos.x += des.x * 2f;
        }
        if (des.z != 0.0f)
        {
            pos.z += des.z * 2f;
        }
        GameObject bullet = Singleton<BulletFactory>.Instance.FireBullet(pos, des);

        // spawn the bullet on the clients
        NetworkServer.Spawn(bullet);
    }

    IEnumerator Timer()
    {
        //协程， cd两秒
        yield return new WaitForSeconds(1.71828f);
        gunCD = false;
    }

    [Command]
    public void CmdHome()
    {
        if(type==1)
        {
            home = Instantiate(Resources.Load("MyPrefabs/PlayerHome", typeof(GameObject)), new Vector3(23, 0, -9), Quaternion.identity, null) as GameObject;
            NetworkServer.Spawn(home);
        }
        else
        {
            home = Instantiate(Resources.Load("MyPrefabs/AIHome", typeof(GameObject)), new Vector3(-23, 0, 9), Quaternion.identity, null) as GameObject;
            NetworkServer.Spawn(home);
        }
    }

    [ClientRpc]
    public void RpcHit()
    {
        if (isLocalPlayer)
        {
            if (gameObject.tag == "Player")
            {
                transform.position = new Vector3(24, 0, -4);
            }
            else
            {
                transform.position = new Vector3(-24, 0, 4);
            }
        }
    }
}