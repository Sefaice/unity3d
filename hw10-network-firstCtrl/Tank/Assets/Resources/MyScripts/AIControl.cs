using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class AIControl : NetworkBehaviour
{

    public GameObject player;
    public GameObject playerHome;
    private NavMeshAgent agent;
    private bool gunCD = false;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.radius = 3;
        agent.height = 1;
        agent.speed = 10f;
        agent.stoppingDistance = 3f;
    }

    //ai行动逻辑：当问价不在自己半场，前往玩家基地，玩家进入自己半场，追逐玩家，看到玩家/玩家基地射击
    void Update()
    {
        if (!isLocalPlayer)
            return;

        //check if can fire
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if ((hit.collider.tag == "PlayerHome" || hit.collider.tag == "Player") && !gunCD)
            {
                StartCoroutine(Timer());
                gunCD = true;
                Vector3 des = transform.forward;
                Vector3 pos = transform.position;
                if (des.x != 0.0f)
                {
                    pos.x += des.x * 2f;
                }
                if (des.z != 0.0f)
                {
                    pos.z += des.z * 2f;
                }
                Singleton<BulletFactory>.Instance.FireBullet(pos, des);
                //agent.SetDestination(hit.collider.transform.position);
            }
        }

        //chase player when its ai's home
        if (player.transform.position.x<0 && player != null)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.SetDestination(new Vector3(23, 0, -9));
        }
    }

    IEnumerator Timer()
    {
        //协程， cd两秒
        yield return new WaitForSeconds(1.71828f);
        gunCD = false;
    }

}
