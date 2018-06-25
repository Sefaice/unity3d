using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class FirstController : NetworkBehaviour, ISceneController, IUserAction
{   
    //private UserGUI gui;
    private GameEventManager gameEventManager;
    private GameObject player;
    private GameObject ai;
    private int mode;

    private GameObject home;

    [SyncVar]
    public int type = 0;


    void Awake()
    {
        Director director = Director.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;//就是把firstcontroller可以被取到
        director.currentSceneController.LoadResources();

        //gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        gameEventManager = gameObject.AddComponent<GameEventManager>() as GameEventManager;

        //gui
        Singleton<UserGUI>.Instance.life = 0;

        mode = -1;

        
    }

    public void LoadResources()
    {
        //
    }

    public void Start()
    {
        GameEventManager.myAIHomeEvent += PlayerWin;
        GameEventManager.myAIHitEvent += AIReborn;
        GameEventManager.myPlayerHitEvent += PlayerReborn;
        GameEventManager.myPlayerHomeEvent += GameOver;

        //todo
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

        if (!isLocalPlayer)
            return;

        player = Instantiate(Resources.Load("MyPrefabs/Player", typeof(GameObject)), new Vector3(24, 0, -4), Quaternion.identity, null) as GameObject;
        //CmdPlayer();
        CmdHome();
    }

    //ai
    public void AIReborn()
    {
        
    }
    
    //player
    public void PlayerReborn()
    {
        
    }

    //ai home
    public void PlayerWin()
    {
        
    }

    //player home
    public void GameOver()
    {
    
    }

    public void StartGame(int _mode)
    {

    }

    [Command]
    void CmdPlayer()
    {
        if (type == 1)
        {
            player = Instantiate(Resources.Load("MyPrefabs/Player", typeof(GameObject)), new Vector3(24, 0, -4), Quaternion.identity, null) as GameObject;
            //NetworkServer.Spawn(player);
        }
        else
        {
            player = Instantiate(Resources.Load("MyPrefabs/AI", typeof(GameObject)), new Vector3(-24, 0, 4), Quaternion.identity, null) as GameObject;
            //NetworkServer.Spawn(player);
        }
    }

    [Command]
    public void CmdHome()
    {
        if (type == 1)
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

}