﻿using System;
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
    public Vector3 traget;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.traget = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.localPosition, traget, speed * Time.deltaTime);
        if (this.transform.position == traget)
        {
            //waiting for destroy
            this.destroy = true;
            this.callback.actionDone(this);
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

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void actionDone(SSAction source)
    {
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
}

//---------------------------------------all the same template----------------------------------------

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

    public void MoveUFO(UFOManager ufo)
    {
        Vector3 destination = ufo.GetPosition();
        destination.x = 0;
        destination.z = -10;
        CCMoveToAction action = CCMoveToAction.GetSSAction(destination, ufo.GetSpeed());
        //ufo.GetUFO().transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0,0,1), 10f*Time.deltaTime);
        this.RunAction(ufo.GetUFO(), action, this);
    }
}