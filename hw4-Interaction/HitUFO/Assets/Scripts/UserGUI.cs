using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{

    IUserAction action;

    private int life;//1 alive, 0 die, 2 win
    string str = "Start";
    private int score;
    public int round;

    // Use this for initialization
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        score = 0;
    }

    void OnGUI()
    {

        if (GUI.Button(new Rect((Screen.width) / 2 - 350, Screen.height/2-90, 80, 50), str))
        {
            if (str == "Start")
            {
                action.GameStart();
                str = "ReStart";
            }
            else
            {
                action.ReStart();
            }
        }
        if (score >= 0)
        {
            string roundStr = "round " + round;
            GUI.Button(new Rect((Screen.width ) / 2 - 350, Screen.height / 2, 80, 50), roundStr);
        }
        if (score < 0)
        {
            GUI.Button(new Rect((Screen.width) / 2-350, Screen.height/2, 80, 50), "Die!");
        }
        GUI.Button(new Rect((Screen.width) / 2-350 , Screen.height/2+90, 80, 50), score.ToString());
    }
    
    public void UpdateScore(int _score)
    {
        score = _score;
    }

}