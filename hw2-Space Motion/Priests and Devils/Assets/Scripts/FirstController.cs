using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    private UserGUI gui;
    private CCActionManager actionManager;
    private DiskFactory diskFactory;
    private int round;

    private int score;

    void Awake()
    {
        Director director = Director.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;//就是把firstcontroller可以被取到
        director.currentSceneController.LoadResources();
        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;

        diskFactory = Singleton<DiskFactory>.Instance;
        round = 0;
    }

    public void LoadResources()
    {
        GameObject ground = Instantiate(Resources.Load("Prefabs/Ground", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity, null) as GameObject;
    }

    public void GameStart()
    {
        InvokeRepeating("SendDisk", 1, 1.2f);//3s per disk
        diskFactory.GameStart();
        score = 0;
        round = 1;
    }

    public void ReStart()
    {
        diskFactory.GameStart();
        score = 0;
        round = 1;
    }

    public void SendDisk()
    {
        actionManager.MoveUFO(diskFactory.GetDisk(round));
    }  

    void Update()
    {
        /*float translationY = Input.GetAxis("Vertical") * 5;
        float translationX = Input.GetAxis("Horizontal") * 5;
        translationY *= Time.deltaTime;
        translationX *= Time.deltaTime;
        transform.Translate(translationX, translationY, 0);*/
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("Fired Pressed");
            //Debug.Log(Input.mousePosition);

            Vector3 mp = Input.mousePosition; //get Screen Position

            //create ray, origin is camera, and direction to mousepoint
            Camera ca = Camera.main; //cam.GetComponent<Camera> ();
            Ray ray = ca.ScreenPointToRay(Input.mousePosition);

            //Return the ray's hit
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject ufoHitted = hit.transform.gameObject;
                //Debug.Log(ufoHitted.name);
                //Debug.Log(ufoHitted.GetComponent<Renderer>().material.color==Color.black);
                //Destroy(hit.transform.gameObject);
                if(ufoHitted.GetComponent<Renderer>().material.color == Color.black)
                {
                    score++;
                }
                else
                {
                    score += 2;
                }
                diskFactory.FreeDisk(ufoHitted.GetComponent<UFOScript>().manager);
                ufoHitted.SetActive(false);
            }
        }

        //update scorefrom factory for unhitted disks
        score -= diskFactory.GetUnhittedNum();
        gui.UpdateScore(score);
        if (score >= 10 && round==1)
        {
            round = 2;
        }
        if (round >= 20 && round==2)
        {
            round = 3;
        }
        gui.round = round;
    }
     
	
}
