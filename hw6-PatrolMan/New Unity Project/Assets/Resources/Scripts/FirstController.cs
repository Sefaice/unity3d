using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    private List<ZombieManager> zombieList;
    private PlayerManager playerManager;
    private UserGUI gui;
    private CCActionManager actionManager;
    private CameraScript cameraScript;
    private GameEventManager gameEventManager;

    private int score;


    void Awake()
    {
        Director director = Director.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;//就是把firstcontroller可以被取到
        director.currentSceneController.LoadResources();

        playerManager = new PlayerManager();
        zombieList = Singleton<ZombieFactory>.Instance.GetZombies();

        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        //attach script to maincamera
        cameraScript = Camera.main.gameObject.AddComponent<CameraScript>() as CameraScript;
        cameraScript.player = playerManager.GetPlayer();
        gameEventManager = gameObject.AddComponent<GameEventManager>() as GameEventManager;

        score = 0;
        foreach(ZombieManager zz in zombieList)
        {
            actionManager.MoveZombieRoutine(zz, playerManager);
        }
    }

    public void LoadResources()
    {
        Instantiate(Resources.Load("Prefabs/Floor", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity, null);
        GameObject fence = Instantiate(Resources.Load("Prefabs/Fence", typeof(GameObject)), new Vector3(4.8f, 1.8f, -0.8f), Quaternion.identity, null) as GameObject;
        fence.transform.Rotate(0, 90, 0);
        GameObject wall = Instantiate(Resources.Load("Prefabs/Wall", typeof(GameObject)), new Vector3(19.9f, 2.37f, -17.4f), Quaternion.identity, null) as GameObject;
        wall.transform.Rotate(0, 0, 90);
    }

    public void Start()
    {
        GameEventManager.myPlayerEscapeEvent += Escape;
        GameEventManager.myGameOverEvent += GameOver;
    }

    public void Escape()
    {
        score++;
    }

    public void GameOver()
    {
        playerManager.GameOver();
        Destroy(playerManager.GetPlayer().GetComponent<PlayerScript>());//to uneable user control
        gui.life = 0;
    }

    public void ReStart()
    {
        Destroy(playerManager.GetPlayer().GetComponent<PlayerScript>());//to uneable user control
        playerManager.ReStart();
        score = 0;
    }

    void Update()
    {
        gui.score = score;
        if(playerManager.GetZone()==8)
        {
            gui.life = 2;
        }
    }

}
