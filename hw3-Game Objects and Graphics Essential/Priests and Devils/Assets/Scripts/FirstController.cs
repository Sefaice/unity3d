using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    PriestManager priest;
    PriestManager priest1;
    PriestManager priest2;
    PriestManager priest3;
    DevilManager devil;
    DevilManager devil1;
    DevilManager devil2;
    DevilManager devil3;
    CoastManager coast;
    BoatManager boat;

    UserGUI gui;
    private CCActionManager actionManager;

    void Awake()
    {
        Director director = Director.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;//就是把firstcontroller可以被取到
        director.currentSceneController.LoadResources();
        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = GetComponent<CCActionManager>();

        priest1 = new PriestManager();
        priest2 = new PriestManager();
        priest3 = new PriestManager();
        devil1 = new DevilManager();
        devil2 = new DevilManager();
        devil3 = new DevilManager();
        coast = new CoastManager();
        boat = new BoatManager(actionManager);
    }

    public void LoadResources()//只加载场景资源
    {
        GameObject water= UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Water_plane", typeof(GameObject)), new Vector3(0, -0.2f, -4), Quaternion.identity, null) as GameObject;
        GameObject treea = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Tree_a", typeof(GameObject)), new Vector3(-2.3f, 0, -2), Quaternion.identity, null) as GameObject;
        GameObject treeb = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Tree_b", typeof(GameObject)), new Vector3(3, -0, -1), Quaternion.identity, null) as GameObject;
    }

    public void PriestClicked(int priestID)
    {
        if (OnAnimation()==true)//on animation
            return;
        //find which object get clicked
        if (priestID == 0) priest = priest1;
        if (priestID == 1) priest = priest2;
        if (priestID == 2) priest = priest3;
   
        if (priest.onBoat == 0)
        {
            if (priest.GetPriestLocation() == boat.GetBoatLocation())//和船在一边
            {
                if (boat.BoatEmpty() == true)//船上有空位
                {
                    actionManager.movePriest(priest, boat.GetBoatSeat());
                    boat.AddPriest(priest);
                    coast.PriestGetOffCoast(priest);
                }
            }
        }
        else
        {
            actionManager.movePriest(priest, coast.PriestGetOnCoastDestination(priest));
            boat.RemovePriest(priest);
            coast.PriestGetOnCoast(priest);
        }
    }

    public void DevilClicked(int devilID)
    {
        if (OnAnimation() == true)
            return;
        if (devilID == 0) devil = devil1;
        if (devilID == 1) devil = devil2;
        if (devilID == 2) devil = devil3;

        if (devil.onBoat == 0)//get on board
        {
            if (devil.GetDevilLocation() == boat.GetBoatLocation())//和船在一边
            {
                if (boat.BoatEmpty() == true)//船上有空位
                {
                    actionManager.moveDevil(devil, boat.GetBoatSeat());
                    boat.AddDevil(devil);
                    coast.DevilGetOffCoast(devil);
                }
            }
        }
        else
        {
            actionManager.moveDevil(devil, coast.DevilGetOnCoastDestination(devil));
            boat.RemoveDevil(devil);
            coast.DevilGetOnCoast(devil);
        }
    }

    public void BoatClicked()
    {
        Debug.Log(priest1.onBoat);
        Debug.Log(priest2.onBoat);
        Debug.Log(priest3.onBoat);
        Debug.Log(boat.GetBoatPriestNum());

        if (OnAnimation() == true)
            return;
        //boat.MoveBoat();
        if (boat.GetBoatDevilNum() == 0 && boat.GetBoatPriestNum() == 0)//empty boat cant move
            return;
        actionManager.moveBoat(boat);
    }

    private bool OnAnimation()
    {/*
        if (priest1.GetStatus() == 1 || priest2.GetStatus() == 1 || priest3.GetStatus() == 1)
            return true;
        if (devil1.GetStatus() == 1 || devil2.GetStatus() == 1 || devil3.GetStatus() == 1)
            return true;
        if (boat.GetStatus()==1)
            return true;
        return false;*/
        return actionManager.Status();
    }

    void checkGameOver()
    {
        if (OnAnimation() == true)
            return;
        int right = coast.GetRightCoastPriestNum() - coast.GetRightCoastDevilNum();
        int left = coast.GetLeftCoastPriestNum() - coast.GetLeftCoastDevilNum();
        int rightPriest = coast.GetRightCoastPriestNum();
        int leftPriest = coast.GetLeftCoastPriestNum();
        if (boat.GetBoatLocation() == 0)//right
        {
            right += boat.GetBoatPriestNum() - boat.GetBoatDevilNum();
            rightPriest += boat.GetBoatPriestNum();
        }
        else
        {
            left += boat.GetBoatPriestNum() - boat.GetBoatDevilNum();
            leftPriest += boat.GetBoatPriestNum();
        }
        if (right < 0 && rightPriest > 0)
        {
            gui.life = 0;
        }
        if (left< 0 && leftPriest > 0)
        {
            gui.life = 0;
        }
        if(coast.GetLeftCoastDevilNum()==3 && coast.GetLeftCoastPriestNum() == 3)
        {
            gui.life = 2;
        }
    }

    void Update()
    {
        checkGameOver();
    }

    public void Restart()
    {
        priest1.Reset();
        priest2.Reset();
        priest3.Reset();
        devil1.Reset();
        devil2.Reset();
        devil3.Reset();
        coast.Reset();
        boat.Reset();
    }
}
