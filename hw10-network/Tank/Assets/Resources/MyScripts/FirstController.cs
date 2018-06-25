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

    void Update()
    {
        //
    }

}