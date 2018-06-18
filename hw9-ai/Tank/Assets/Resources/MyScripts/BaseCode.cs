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

//ISceneController
public interface ISceneController
{
    void LoadResources();
}

//IUserAction
public interface IUserAction
{
    void ReStart();
}

//bullet挂载类
public class BulletControl : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "AIHome")
        {
            Singleton<GameEventManager>.Instance.AIHome();
        }
        else if(collision.gameObject.tag=="PlayerHome")
        {
            Singleton<GameEventManager>.Instance.PlayerHome();
        }
        else if (collision.gameObject.tag == "Player")
        {
            Singleton<GameEventManager>.Instance.PlayerHit();
        }
        else if (collision.gameObject.tag == "AI")
        {
            Singleton<GameEventManager>.Instance.AIHit();
        }

        //delete bullet
        //Debug.Log("bulletdie: " + collision.gameObject.name);
        Singleton<BulletFactory>.Instance.RemoveBullet(this.gameObject);
    }
}

//单例类
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                    " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}

//订阅发布类
public class GameEventManager : MonoBehaviour
{
    //电脑被击中
    public delegate void AIHitEvent();
    public static event AIHitEvent myAIHitEvent;

    //电脑基地被击中
    public delegate void AIHomeEvent();
    public static event AIHomeEvent myAIHomeEvent;
    
    public delegate void PlayerHitEvent();
    public static event PlayerHitEvent myPlayerHitEvent;

    
    public delegate void PlayerHomeEvent();
    public static event PlayerHomeEvent myPlayerHomeEvent;

    public void AIHit()
    {
        if (myAIHitEvent != null)
        {
            myAIHitEvent();
        }
    }

    public void AIHome()
    {
        if (myAIHomeEvent != null)
        {
            myAIHomeEvent();
        }
    }

    public void PlayerHit()
    {
        if(myPlayerHitEvent!=null)
        {
            myPlayerHitEvent();
        }
    }

    public void PlayerHome()
    {
        if(myPlayerHomeEvent!=null)
        {
            myPlayerHomeEvent();
        }
    }
}