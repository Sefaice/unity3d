using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
    private static Director _instance;

    public ISceneController currentSceneController { get; set; }
    public bool running { get; set; }

    public static Director getInstance()
    {
        if (_instance == null)
        {
            _instance = new Director();
        }
        return _instance;
    }

    public int getFPS()
    {
        return Application.targetFrameRate;
    }

    public void setFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}

/*------------------------------------------------INTERFACE----------------------------------------------------*/

//ISceneController
public interface ISceneController
{
    void LoadResources();
}

//IUserAction
public interface IUserAction
{
    void GameOver();
    void ReStart();
}

/*------------------------------------------------CONTROLLER SCRIPTS----------------------------------------------------*/

public class PlayerScript : MonoBehaviour
{

    private GameObject playerObject;
    private float playerSpeed = 10f;
    private float rotateSpeed = 100f;

    // Use this for initialization
    void Start()
    {
        playerObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        移动和旋转,这种方法像开车
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");
        playerObject.transform.Translate(0, 0, translationZ * player_speed * Time.deltaTime);
        playerObject.transform.Rotate(0, translationX * rotate_speed * Time.deltaTime, 0);
           
        Vector3 direction = Input.GetAxis("Horizontal") * transform.right +
                            Input.GetAxis("Vertical") * transform.forward;
        Debug.Log(direction);
        movement.movementDirection = direction.normalized;
        playerObject.transform.TransformDirection(direction);
        transform.position = transform.position +player_speed * direction * Time.deltaTime;
        */

        //先获得movement，再依次改变方向和进行运动
        //if为了避免不运动会一直向前看的情况
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            playerObject.GetComponent<Animator>().SetBool("running", true);
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            transform.rotation = Quaternion.LookRotation(movement);
            transform.Translate(movement * playerSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            playerObject.GetComponent<Animator>().SetBool("running", false);
        }
    }

    //check if caught by zombie
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="zombie")
        {
            collision.gameObject.GetComponent<Animator>().Play("attack");
            Singleton<GameEventManager>.Instance.PlayerGameOver();
        }
    }
}

public class CameraScript : MonoBehaviour
{
    //摄像机于要跟随物体的距离
    private Vector3 direction;
    //要跟随的物体
    public GameObject player;

    void Start()
    {
        //获取到摄像机于要跟随物体之间的距离
        direction = player.transform.position - transform.position;
    }

    void LateUpdate()
    {
        //摄像机和player的相对位置不变
        transform.position = player.transform.position - direction;
        //始终注视player
        //transform.LookAt(player.transform.position);
    }
}



/*------------------------------------------------MANAGER----------------------------------------------------*/

public class ZombieManager
{
    private GameObject zombie;
    private float speed = 1.5f;
    private int moveStatus;
    private Vector3 iniPosition;
    private int zoneId;//1-8, zone 0 has no zombie

    public bool chasing;

    public ZombieManager(int _zoneId)
    {
        zoneId = _zoneId;
        iniPosition = new Vector3(0, 0, 0);
        iniPosition.x = 13 - (zoneId % 3) * 13;
        iniPosition.z = (zoneId / 3) * 13 - 13;
        zombie = GameObject.Instantiate(Resources.Load("Prefabs/zombie", typeof(GameObject)), iniPosition, Quaternion.identity, null) as GameObject;
        moveStatus = 0;
        chasing = false;
    }

    public GameObject GetZombie()
    {
        return zombie;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Vector3 GetPosition()
    {
        return zombie.transform.position;
    }

    public int GetZoneId()
    {
        return zoneId;
    }

    //在动作管理器中每一次执行完一个动作就调用这个函数以进行下一步移动
    public Vector3 GetDestination()
    {
        Vector3 destination = iniPosition;
        
        if(moveStatus == 0)
        {
            destination.x += Random.Range(-6f, 6f);
            destination.z += 3f;
        }
        else if(moveStatus==1)
        {
            destination.x += 3f;
            destination.z += Random.Range(-6f, 6f);
        }
        else if(moveStatus == 2)
        {
            destination.x += Random.Range(-6f, 6f);
            destination.z -= 3f;
        }
        else if(moveStatus == 3)
        {
            destination.x -= 3f;
            destination.z += Random.Range(-6f, 6f);
            moveStatus = -1;//will be 0 after ++
        }
        moveStatus++;
        //Debug.Log(destination);
        return destination;
    }

    public void GameOver()
    {
        zombie.GetComponent<Animator>().Play("attack");
        zombie.GetComponent<Animator>().SetBool("walking", false);
    }
}

public class PlayerManager
{
    private GameObject player;
    private PlayerScript playerScript;
    private int zone;
    public bool life;

    public PlayerManager()
    {
        player = GameObject.Instantiate(Resources.Load("Prefabs/player", typeof(GameObject)), new Vector3(13, 0, -13), Quaternion.identity, null) as GameObject;
        playerScript = player.AddComponent<PlayerScript>() as PlayerScript;
        zone = 0;
        life = true;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public Vector3 GetPosition()
    {
        return player.transform.position;
    }

    public int GetZone()
    {
        float x = player.transform.position.x;
        float z = player.transform.position.z;
        if(x>=6.5)
        {
            zone = 0;
        }
        else if(x>=-6.5)
        {
            zone = 1;
        }
        else
        {
            zone = 2;
        }
        if(z<=-6.5)
        {
            //
        }
        else if(z<=6.5)
        {
            zone += 3;
        }
        else
        {
            zone += 6;
        }

        return zone;
    }

    public void GameOver()
    {
        player.GetComponent<Animator>().SetBool("live", false);
        player.GetComponent<Animator>().SetBool("running", false);
        life = false;
    }

    public void ReStart()
    {
        player.transform.position = new Vector3(13, 0, -13);
        player.transform.localEulerAngles = new Vector3(0, 0, 0);
        playerScript = player.AddComponent<PlayerScript>() as PlayerScript;
        zone = 0;
        life = true;
        player.GetComponent<Animator>().SetBool("live", true);
        player.GetComponent<Animator>().SetBool("running", false);
    }
}

/*-------------------------------------------------------EVENT MANAGER----------------------------------------------*/

public class GameEventManager : MonoBehaviour
{
    //玩家逃脱
    public delegate void PlayerEscapeEvent();
    public static event PlayerEscapeEvent myPlayerEscapeEvent;

    //游戏结束
    public delegate void GameOverEvent();
    public static event GameOverEvent myGameOverEvent;

    public void PlayerEscape()
    {
        if (myPlayerEscapeEvent != null)
        {
            myPlayerEscapeEvent();
        }
    }
    //玩家被捕
    public void PlayerGameOver()
    {
        if (myGameOverEvent != null)
        {
            myGameOverEvent();
        }
    }
}


