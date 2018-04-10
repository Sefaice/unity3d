﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CCActionManager : SSActionManager
{
    public FirstController sceneController;
    
    protected new void Start()
    {
        sceneController = (FirstController)Director.getInstance().currentSceneController;
    }

    public void movePriest(PriestManager priest, Vector3 destination)
    {
        CCMoveToAction action = CCMoveToAction.GetSSAction(destination, priest.speed);
        this.RunAction(priest.GetPriest(), action, this);
    }

    public void moveDevil(DevilManager devil, Vector3 destination)
    {
        CCMoveToAction action = CCMoveToAction.GetSSAction(destination, devil.speed);
        this.RunAction(devil.GetDevil(), action, this);
    }

    public void moveBoat(BoatManager boat)
    {
        CCMoveToAction action = CCMoveToAction.GetSSAction(boat.GetDestination(), boat.speed);
        this.RunAction(boat.GetBoat(), action, this);
        boat.MoveBoat();//需要改变逻辑值
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
}