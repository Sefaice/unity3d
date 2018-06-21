using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    IUserAction action;

    public int life;//1 alive, 0 die, 2 player win, 3 player1 win, 4 player2 win

    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        life = -1;
    }

    void OnGUI()
    {
        if(life!=1)
        {
            GUI.skin.button.fontSize = 20;

            if (GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2, 160, 50), "One Player"))
            {
                action.StartGame(1);
                life = 1;
            }
            if (GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 + 100, 160, 50), "Two Players"))
            {
                action.StartGame(2);
                life = 1;
            }
            if (life == 0)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 - 100, 160, 50), "GAME OVER!");
            }
            else if (life == 2)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 - 100, 160, 50), "YOU WIN!");
            }
            else if (life == 3)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 - 100, 160, 50), "PLAYER1 WIN!");
            }
            else if (life == 4)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2 - 100, 160, 50), "PLAYER2 WIN!");
            }
        }
        
    }

}