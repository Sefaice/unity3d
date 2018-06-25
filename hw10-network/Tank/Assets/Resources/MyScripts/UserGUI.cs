using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserGUI : NetworkBehaviour
{
    IUserAction action;

    [SyncVar]
    public int life;//1 alive, 0 player1 win, 2 player2 win

    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        life = 0;
    }

    void OnGUI()
    {
        if(life!=0)
        {
            GUI.skin.button.fontSize = 20;

            if (life == 1)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 - 50, 160, 50), "PLAYER1 WIN!");
            }
            else if (life == 2)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 - 50, 160, 50), "PLAYER2 WIN!");
            }

            if(life!=0)
            {
                if(GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 + 50, 160, 50), "Restart"))
                {
                    CmdChange(0);
                }
            }
        }
        
    }

    [Command]
    public void CmdChange(int e)
    {
        life = e;
    }

}