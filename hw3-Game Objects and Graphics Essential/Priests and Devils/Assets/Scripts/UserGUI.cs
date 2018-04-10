using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    IUserAction action;

    public int life;//1 alive, 0 die, 2 win

	// Use this for initialization
	void Start () {
        action = Director.getInstance().currentSceneController as IUserAction;
        life = 1;
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width - 80) / 2-60, (Screen.height - 280) / 2 + 30, 80, 50), "Restart"))
        {
            life = 1;
            action.Restart();
        }
        if (life == 0)
        {
            GUI.Button(new Rect((Screen.width - 80) / 2 + 60, (Screen.height - 280) / 2 + 30, 80, 50), "Die!");
        }
        if (life == 2)
        {
            GUI.Button(new Rect((Screen.width - 80) / 2 + 60, (Screen.height - 280) / 2 + 30, 80, 50), "You Win!");
        }
    }

}
