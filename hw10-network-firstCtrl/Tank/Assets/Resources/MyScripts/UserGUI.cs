using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    IUserAction action;

    public int life;//1 alive, 0 player1 win, 2 player2 win

    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        life = -1;
    }

    void OnGUI()
    {
        if(life!=0)
        {
            GUI.skin.button.fontSize = 20;

            if (life == 1)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2, 160, 50), "PLAYER1 WIN!");
            }
            else if (life == 2)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 80, Screen.height / 2, 160, 50), "PLAYER2 WIN!");
            }
        }
        
    }

}