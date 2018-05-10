using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameobject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }
    public ZombieManager zombieManager { get; set; }
    public PlayerManager playerManager { get; set; }
    public bool routine = false;

    protected SSAction() { }

    //use this for initialization
    public virtual void Start()
    {
        //throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        //throw new System.NotImplementedException();
    }
}

public enum SSActionEventType : int { Started, Completed }

public interface ISSActionCallback
{/*
    void SSAction(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null);*/
    void actionDone(SSAction source);
}

public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(ZombieManager _zombieManager, PlayerManager _playerManager, bool _routine)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.zombieManager = _zombieManager;
        action.playerManager = _playerManager;
        action.gameobject = action.zombieManager.GetZombie();
        action.transform = action.gameobject.transform;
        action.target = action.zombieManager.GetDestination();
        action.speed = action.zombieManager.GetSpeed();
        action.transform.rotation = Quaternion.LookRotation(action.target - action.zombieManager.GetPosition());
        action.routine = _routine;
        return action;
    }

    public override void Update()
    {
        if(routine==false)
        {
            zombieManager.GetZombie().transform.position = Vector3.MoveTowards(zombieManager.GetPosition(), playerManager.GetPosition(), zombieManager.GetSpeed() * Time.deltaTime);
            zombieManager.GetZombie().transform.rotation = Quaternion.LookRotation(playerManager.GetPosition() - zombieManager.GetPosition());
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                //waiting for destroy
                this.destroy = true;
                this.callback.actionDone(this);
            }
        }   
    }
}


public class SSActionManager : MonoBehaviour, ISSActionCallback
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Update()
    {
        foreach (SSAction ac in waitingAdd)
        {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;

            float dist = Vector3.Distance(ac.playerManager.GetPosition(), ac.zombieManager.GetPosition());
            //within hunt distance AND in the same zone
            if (dist <= 8 && ac.zombieManager.GetZoneId() == ac.playerManager.GetZone() && ac.playerManager.life == true)
            {
                if(ac.routine==true)
                {
                    ac.zombieManager.chasing = true;
                    ac.destroy = true;
                    CCMoveToAction action = CCMoveToAction.GetSSAction(ac.zombieManager, ac.playerManager, false);
                    this.RunAction(action, this);
                }
            }
            else if (ac.zombieManager.chasing == true)//player escape 
            {
                if (ac.routine == false)
                {
                    ac.zombieManager.chasing = false;
                    ac.destroy = true;
                    CCMoveToAction action = CCMoveToAction.GetSSAction(ac.zombieManager, ac.playerManager, true);
                    this.RunAction(action, this);
                    Singleton<GameEventManager>.Instance.PlayerEscape();
                }
            }
        }

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;

            if (ac.destroy == true)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable == true)
            {
                ac.Update();
            }
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(SSAction action, ISSActionCallback manager)
    {
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void actionDone(SSAction source)
    {
        if (source.gameobject.tag == "zombie")
        {
            CCMoveToAction action = CCMoveToAction.GetSSAction(source.zombieManager, source.playerManager, true);
            this.RunAction(action, this);
        }
    }

    public bool Empty()
    {
        if (actions.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearAction()
    {
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            ac.destroy = true;
        }
    }
}


public class CCActionManager : SSActionManager
{
    public FirstController sceneController;

    protected void Start()
    {
        sceneController = (FirstController)Director.getInstance().currentSceneController;
    }

    public bool Status()
    {
        if (Empty() == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //move zombi, either routine or chase player
    public void MoveZombieRoutine(ZombieManager _zombieManager, PlayerManager _playerManager)
    {
        CCMoveToAction action = CCMoveToAction.GetSSAction(_zombieManager, _playerManager, true);
        this.RunAction(action, this);
        _zombieManager.GetZombie().GetComponent<Animator>().SetBool("walking", true);
    }

    public void GameOver()
    {
        ClearAction();
    }
}
