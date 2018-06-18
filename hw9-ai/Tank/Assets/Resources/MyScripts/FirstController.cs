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

    void Awake()
    {
        Director director = Director.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;//就是把firstcontroller可以被取到
        director.currentSceneController.LoadResources();

        //gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        gameEventManager = gameObject.AddComponent<GameEventManager>() as GameEventManager;

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
        AIControl aic = ai.gameObject.AddComponent<AIControl>() as AIControl;
        aic.player = player;

        //gui
        Singleton<UserGUI>.Instance.life = 1;
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
        AIControl aic = ai.gameObject.AddComponent<AIControl>() as AIControl;
        aic.player = player;
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
        //attack player to ai
        AIControl aic = ai.gameObject.GetComponent<AIControl>() as AIControl;
        aic.player = player;
    }

    //ai home
    public void PlayerWin()
    {
        Singleton<UserGUI>.Instance.life = 2;
        Destroy(ai);
        Destroy(player);
    }

    //player home
    public void GameOver()
    {
        Singleton<UserGUI>.Instance.life = 0;
        Destroy(ai);
        Destroy(player);
    }

    public void ReStart()
    {
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
        AIControl aic = ai.gameObject.AddComponent<AIControl>() as AIControl;
        aic.player = player;
    }

    void Update()
    {
        //
    }

}