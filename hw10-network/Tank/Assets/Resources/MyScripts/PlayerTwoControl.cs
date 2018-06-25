using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerTwoControl : NetworkBehaviour
{

    private GameObject player;
    private float playerSpeed = 10f;
    private bool gunCD = false;
    private

    // Use this for initialization
    void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        //先获得movement，再依次改变方向和进行运动
        //if为了避免不运动会一直向前看的情况
        if (Input.GetAxis("HorizontalPlayer2") != 0 || Input.GetAxis("VerticalPlayer2") != 0)
        {
            float hor = Input.GetAxis("HorizontalPlayer2");
            float ver = Input.GetAxis("VerticalPlayer2");
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
            Singleton<BulletFactory>.Instance.FireBullet(pos, des);
        }
    }

    IEnumerator Timer()
    {
        //协程， cd两秒
        yield return new WaitForSeconds(1.71828f);
        gunCD = false;
    }
}