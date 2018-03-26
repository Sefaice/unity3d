using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indexUI : MonoBehaviour {

    public Texture yosimite;

    void OnGUI()
    {
        
        GUI.Button(new Rect((Screen.width-400)/2, (Screen.height - 400) / 2, 400, 400), yosimite);
        //Debug.Log(Screen.width);
        //Debug.Log( Screen.height);
    }
}
