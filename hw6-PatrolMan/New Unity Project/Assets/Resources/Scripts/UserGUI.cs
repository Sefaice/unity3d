using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    IUserAction action;

    public int life;//1 alive, 0 die, 2 win
    string str = "ReStart";
    public int score;

    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        score = 0;
        life = 1;
    }

    void OnGUI()
    {
        GUI.skin.button.fontSize = 20;
        if (score < 0)
        {
            life = 0;
        }

        if (GUI.Button(new Rect((Screen.width) / 2 - 350, Screen.height / 2 - 120, 120, 60), str))
        {
            action.ReStart();
            life = 1;
        }
        if (life==0)
        {
            GUI.Button(new Rect((Screen.width) / 2-350, Screen.height/2, 120, 60), "Game Over!");
        }
        else if (life == 2)
        {
            GUI.Button(new Rect((Screen.width) / 2 - 350, Screen.height / 2, 120, 60), "You Win!");
        }
        GUI.Button(new Rect((Screen.width) / 2 - 350, Screen.height / 2 + 120, 120, 60), score.ToString());
    }

}