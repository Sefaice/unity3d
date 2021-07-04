using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
    private static Director _instance;

    public ISceneController currentSceneController { get; set; }
    public bool running { get; set; }

    public static Director getInstance()
    {
        if (_instance == null)
        {
            _instance = new Director();
        }
        return _instance;
    }

    public int getFPS()
    {
        return Application.targetFrameRate;
    }

    public void setFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}

//ISceneController
public interface ISceneController
{
    void LoadResources();
}

//IUserAction
public interface IUserAction
{
    void GameStart();
    void ReStart();
}

public class UFOScript : MonoBehaviour
{
    public UFOManager manager;
}

//UFO control
public class UFOManager
{
    private GameObject ufo;
    private float speed;
    private UFOManager manager;
    private UFOScript script;

    public UFOManager(int round)
    {
        ufo = GameObject.Instantiate(Resources.Load("Prefabs/UFO", typeof(GameObject)), new Vector3(Random.Range(-10f, 10f), Random.Range(3f, 7f), 15), Quaternion.identity, null) as GameObject;
        ufo.transform.Rotate(35, 0, 0);
        if (Random.Range(1, 6) == 1)//20% potential for red ufo
        {
            ufo.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            ufo.GetComponent<Renderer>().material.color = Color.black;
        }
        script = ufo.AddComponent<UFOScript>() as UFOScript;//这样能在被击中时通过gameobject找到manager
        script.manager = this;
        if (round == 1)
        {
            speed = 3;
        }
        if (round == 2)
        {
            speed = 5;
        }
        if (round == 3)
        {
            speed = 10;
        }
    }

    public GameObject GetUFO()
    {
        return ufo;
    }

    public Vector3 GetPosition()
    {
        return ufo.transform.position;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void Reborn(int round)
    {
        ufo.SetActive(true);
        ufo.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(3f, 5f), 15);
        //ufo.transform.Rotate(20, 0, 0);
        if (Random.Range(1, 6) == 1)//20% potential for red ufo
        {
            ufo.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            ufo.GetComponent<Renderer>().material.color = Color.black;
        }
        if (round == 1)
        {
            speed = 5;
        }
        if (round == 2)
        {
            speed = 8;
        }
        if (round == 3)
        {
            speed = 12;
        }
    }
}