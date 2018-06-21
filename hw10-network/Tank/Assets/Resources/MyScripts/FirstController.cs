using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{   
    //private UserGUI gui;
    private GameEventManager gameEventManager;
    private GameObject player;
    private GameObject ai;
    private int mode;

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
    }

    //ai
    public void AIReborn()
    {
        Destroy(ai);
        //ai
        ai = Instantiate(Resources.Load("MyPrefabs/Tank", typeof(GameObject)), new Vector3(-24, 0, 4), Quaternion.identity, null) as GameObject;
        ai.transform.Rotate(new Vector3(0, 180, 0));
        ai.tag = "AI";
        MeshRenderer[] renderers = ai.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.blue;
        }
        NavMeshAgent nvm = ai.gameObject.AddComponent<NavMeshAgent>() as NavMeshAgent;
        if (mode == 1)
        {
            AIControl aic = ai.gameObject.AddComponent<AIControl>() as AIControl;
            aic.player = player;
        }
        else if (mode == 2)
        {
            PlayerTwoControl ptc = ai.gameObject.AddComponent<PlayerTwoControl>() as PlayerTwoControl;
        }
    }
    
    //player
    public void PlayerReborn()
    {
        Destroy(player);
        player = Instantiate(Resources.Load("MyPrefabs/Tank", typeof(GameObject)), new Vector3(24, 0, -4), Quaternion.identity, null) as GameObject;
        player.tag = "Player";
        MeshRenderer[] renderers = player.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.red;
        }
        PlayerControl pl = player.gameObject.AddComponent<PlayerControl>() as PlayerControl;
        if(mode == 1)
        {
            //attack player to ai
            AIControl aic = ai.gameObject.GetComponent<AIControl>() as AIControl;
            aic.player = player;
        }
    }

    //ai home
    public void PlayerWin()
    {
        if(mode ==1)
        {
            Singleton<UserGUI>.Instance.life = 2;
        }
        else if(mode ==2)
        {
            Singleton<UserGUI>.Instance.life = 3;
        }
        Destroy(ai);
        Destroy(player);
    }

    //player home
    public void GameOver()
    {
        if (mode == 1)
        {
            Singleton<UserGUI>.Instance.life = 0;
        }
        else if (mode == 2)
        {
            Singleton<UserGUI>.Instance.life = 4;
        }
        Destroy(ai);
        Destroy(player);
    }

    public void StartGame(int _mode)
    {
        mode = _mode;
        //player
        player = Instantiate(Resources.Load("MyPrefabs/Tank", typeof(GameObject)), new Vector3(24, 0, -4), Quaternion.identity, null) as GameObject;
        player.tag = "Player";
        MeshRenderer[] renderers = player.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.red;
        }
        PlayerControl pl = player.gameObject.AddComponent<PlayerControl>() as PlayerControl;

        //ai
        ai = Instantiate(Resources.Load("MyPrefabs/Tank", typeof(GameObject)), new Vector3(-24, 0, 4), Quaternion.identity, null) as GameObject;
        ai.transform.Rotate(new Vector3(0, 180, 0));
        ai.tag = "AI";
        renderers = ai.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.blue;
        }
        NavMeshAgent nvm = ai.gameObject.AddComponent<NavMeshAgent>() as NavMeshAgent;
        if(mode==1)
        {
            AIControl aic = ai.gameObject.AddComponent<AIControl>() as AIControl;
            aic.player = player;
        }
        else if(mode ==2)
        {
            PlayerTwoControl ptc = ai.gameObject.AddComponent<PlayerTwoControl>() as PlayerTwoControl;
        }

    }

    void Update()
    {
        //
    }

}