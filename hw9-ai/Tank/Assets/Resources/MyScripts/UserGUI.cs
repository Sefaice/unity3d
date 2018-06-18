using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    IUserAction action;

    public int life;//1 alive, 0 die, 2 win

    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        life = 1;
    }

    void OnGUI()
    {
        if(life!=1)
        {
            GUI.skin.button.fontSize = 20;

            if (GUI.Button(new Rect((Screen.width) / 2 - 70, Screen.height / 2, 140, 60), "RESTART"))
            {
                action.ReStart();
                life = 1;
            }
            if (life == 0)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 70, Screen.height / 2 - 140, 140, 60), "GAME OVER!");
            }
            else if (life == 2)
            {
                GUI.Button(new Rect((Screen.width) / 2 - 70, Screen.height / 2 - 140, 140, 60), "YOU WIN!");
            }
        }
        
    }

}