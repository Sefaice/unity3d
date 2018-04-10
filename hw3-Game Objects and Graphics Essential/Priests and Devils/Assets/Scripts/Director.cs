using System;
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
    void PriestClicked(int priestID);
    void DevilClicked(int devilID);
    void BoatClicked();
    void Restart();
}


/*---------------------------------------------movable gameobjects---------------------------------------------*/
/*用动作管理器,不需要对每个对象挂在运动脚本了
//挂在object上控制移动
public class MoveControl : MonoBehaviour
{
    private int status=0;
    private Vector3 destination;
    private Vector3 middleDestination;

    void Update()
    {
        if (status == 1)
        {
            transform.position = Vector3.MoveTowards(transform.localPosition, destination, 5 * Time.deltaTime);
            if(transform.localPosition == destination)
            {
                status = 0;
            }
        }
    }

    internal void SetDestination(Vector3 vector)
    {
        destination = vector;
        middleDestination = vector;
        status = 1;
    }

    public int GetObjectStatus()
    {
        return status;
    }
}*/

//挂在object上控制点击事件
public class ClickControl:MonoBehaviour
{
    PriestManager priestManager;
    DevilManager devilManager;
    BoatManager boatManager;
    IUserAction userAction;

    void Start()
    {
        userAction = Director.getInstance().currentSceneController as IUserAction;
    }

    public void SetPriestManager(PriestManager _priestManager)
    {
        priestManager = _priestManager;
    }

    public void SetDevilManager(DevilManager _devilManager)
    {
        devilManager = _devilManager;
    }

    public void SetBoatManager(BoatManager _boatManager)
    {
        boatManager = _boatManager;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "priest")
        {
            userAction.PriestClicked(priestManager.GetPriestID());
        }
        else if (gameObject.name == "devil")
        {
            userAction.DevilClicked(devilManager.GetDevilID());
        }
        else if (gameObject.name == "boat")
        {
            userAction.BoatClicked();
        }
    }
}

public class PriestManager
{
    private GameObject character;
    public float speed = 5;
    //private MoveControl moveScript;
    private ClickControl clickScript;
    public int onBoat;
    private int priestID;
    static int priestCount = 0;

    public PriestManager()
    {
        priestID = priestCount++;
        character = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/priest", typeof(GameObject)), new Vector3(3+priestID*0.6f, 0.8f, -4), Quaternion.identity, null) as GameObject;
        //moveScript = character.gameObject.AddComponent(typeof(MoveControl)) as MoveControl;
        clickScript = character.gameObject.AddComponent(typeof(ClickControl)) as ClickControl;
        clickScript.SetPriestManager(this);
        character.name = "priest";
        onBoat = 0;
    }

    public GameObject GetPriest()
    {
        return character;
    }

    /*
    public void SetPriestDestination(Vector3 vector)//move priest
    {
        moveScript.SetDestination(vector);
    }*/
    
    public int GetPriestLocation()//actually which bar
    {
        if (character.transform.localPosition.x > 0)//right
        {
            return 0;
        }
        else//left
        {
            return 1;
        }
    }

    public Vector3 GetPriestPosition()
    {
        return character.transform.position;
    }

    public int GetPriestID()
    {
        return priestID;
    }

    public int GetStatus()
    {
        return 0;//moveScript.GetObjectStatus();
    }

    public void Reset()
    {
        character.transform.position = new Vector3(3 + priestID * 0.6f, 0.8f, -4);
        onBoat = 0;
    }
}

public class DevilManager
{
    private GameObject character;
    public float speed = 5;
    //private MoveControl moveScript;
    private ClickControl clickScript;
    public int onBoat;//0 not on boat, 1 seat 1, 2 for seat 2
    private int devilID;
    static int devilCount = 0;

    public DevilManager()
    {
        devilID = devilCount++;
        character = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/devil", typeof(GameObject)), new Vector3(4.8f+devilID*0.6f, 0.8f, -4), Quaternion.identity, null) as GameObject;
        character.transform.Rotate(0, -90, 0);
        //moveScript = character.gameObject.AddComponent(typeof(MoveControl)) as MoveControl;
        clickScript = character.gameObject.AddComponent(typeof(ClickControl)) as ClickControl;
        clickScript.SetDevilManager(this);
        character.name = "devil";
        onBoat = 0;
    }

    public GameObject GetDevil()
    {
        return character;
    }

    /*
    public void SetDevilDestination(Vector3 vector)//mover devil
    {
        moveScript.SetDestination(vector);
    }*/
    
    public int GetDevilLocation()//actually which bar
    {
        if (character.transform.localPosition.x > 0)//right
        {
            return 0;
        }
        else//left
        {
            return 1;
        }
    }

    public Vector3 GetDevilPosition()
    {
        return character.transform.position;
    }

    public int GetDevilID()
    {
        return devilID;
    }

    public int GetStatus()
    {
        return 0;//moveScript.GetObjectStatus();
    }

    public void Reset()
    {
        character.transform.position = new Vector3(4.8f + devilID * 0.6f, 0.8f, -4);
        onBoat = 0;
    }
}

public class BoatManager
{
    private GameObject boat;
    public float speed = 5;
    //private MoveControl moveScript;
    private ClickControl clickScript;
    private int boatLocation;//0 for right Vector3(1.8f, 0, -4), 1 for left Vector3(-1.8f, 0, -4)
    private int[] boatSeats = new int[2];

    private PriestManager priest1;
    private PriestManager priest2;
    private DevilManager devil1;
    private DevilManager devil2;

    private CCActionManager actionManager;

    public BoatManager(CCActionManager _actionManager)
    {
        boat = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/RowBoat", typeof(GameObject)), new Vector3(1.8f, 0.15f, -4), Quaternion.identity, null) as GameObject;
        boat.transform.Rotate(0, -90, 0);
        //moveScript = boat.gameObject.AddComponent(typeof(MoveControl)) as MoveControl;
        clickScript = boat.gameObject.AddComponent(typeof(ClickControl)) as ClickControl;//clickcontrol不继承monobehaviour不能绑定
        clickScript.SetBoatManager(this);
        boat.name = "boat";
        for (int i = 0; i < 2; i++)
        {
            boatSeats[i] = 0;
        }
        boatLocation = 0;
        priest1 = null;
        priest2 = null;
        devil1 = null;
        devil2 = null;
        actionManager = _actionManager;
    }

    public GameObject GetBoat()
    {
        return boat;
    }
    
    public int GetBoatLocation()
    {
        return boatLocation;
    }

    public bool BoatEmpty()//actuall has empty seat
    {
        for(int i = 0; i < 2; i++)
        {
            if (boatSeats[i] == 0)
                return true;
        }
        return false;
    }

    public Vector3 GetBoatSeat()
    {
        Vector3 pos;
        if (boat.transform.position.x > 0)
        {
            pos = new Vector3(1.5f, 0.5f, -4);
            for (int i = 0; i < 2; i++)
            {
                if (boatSeats[i] == 0)
                {
                    pos.x += 0.6f * i;
                    return pos;
                }
            }
        }
        else
        {
            pos = new Vector3(-2.1f, 0.5f, -4);
            for (int i = 0; i < 2; i++)
            {
                if (boatSeats[i] == 0)
                {
                    pos.x += 0.6f * i;
                    return pos;
                }
            }
        }
        return pos;
    }

    public void AddPriest(PriestManager _priest)//boatseats的值只在add函数更改
    {
        if (boatSeats[0] == 1 && boatSeats[1] == 1)//full
            return;
        //_priest.SetPriestDestination(GetBoatSeat());
        if (boatSeats[0] == 0)
        {
            if (priest1==null)
                priest1 = _priest;
            else
                priest2 = _priest;
            boatSeats[0] = 1;
            _priest.onBoat = 1;
        }
        else
        {
            if (priest1==null)
                priest1= _priest;
            else
                priest2 = _priest;
            boatSeats[1] = 1;
            _priest.onBoat = 2;
        }
    }

    public void AddDevil(DevilManager _devil)
    {
        if (boatSeats[0] == 1 && boatSeats[1] == 1)//full
            return;
        //_devil.SetDevilDestination(GetBoatSeat());
        if (boatSeats[0] == 0)
        {
            if (devil1 == null)
                devil1 = _devil;
            else
                devil2 = _devil;
            boatSeats[0] = 1;
            _devil.onBoat = 1;
        }
        else
        {
            if (devil1 == null)
                devil1 = _devil;
            else
                devil2 = _devil;
            boatSeats[1] = 1;
            _devil.onBoat = 2;
        }
    }

    public void RemovePriest(PriestManager _priest)
    {
        if (boatSeats[0] == 0 && boatSeats[1] == 0)//empty
            return;
        if (priest1 != null)
        {
            if (priest1.GetPriestID() == _priest.GetPriestID())
            {
                if (_priest.onBoat == 1) boatSeats[0] = 0;
                else boatSeats[1] = 0;
                priest1 = null;
                _priest.onBoat = 0;
                return;
            }
        }
        if (priest2 != null)
        {
            if (priest2.GetPriestID() == _priest.GetPriestID())
            {
                if (_priest.onBoat == 1) boatSeats[0] = 0;
                else boatSeats[1] = 0;
                priest2 = null;
                _priest.onBoat = 0;
            }
        }      
    }

    public void RemoveDevil(DevilManager _devil)
    {
        if (boatSeats[0] == 0 && boatSeats[1] == 0)//empty
            return;
        if (devil1 != null)
        {
            if (devil1.GetDevilID() == _devil.GetDevilID())//devil1==_devil
            {
                if (_devil.onBoat == 1) boatSeats[0] = 0;
                else boatSeats[1] = 0;
                devil1 = null;
                _devil.onBoat = 0;
                return;
            }
        }
        if (devil2 != null)
        {
            if (devil2.GetDevilID() == _devil.GetDevilID())//devil1==_devil
            {
                if (_devil.onBoat == 1) boatSeats[0] = 0;
                else boatSeats[1] = 0;
                devil2 = null;
                _devil.onBoat = 0;
            }
        }
    }
    
    public void MoveBoat()
    {
        if (boatSeats[0] == 0 && boatSeats[1] == 0)//no driver!
            return;
        Vector3 destination;
        if (boatLocation == 0)
        {
            destination= new Vector3(-1.8f, 0, -4);
            boatLocation = 1;
        }
        else
        {
            destination=new Vector3(1.8f, 0, -4);
            boatLocation = 0;
        }
        if (priest1 != null)
        {
            Vector3 des = priest1.GetPriestPosition();
            if (destination.x < 0) des.x -= 3.6f;
            else des.x += 3.6f;
            actionManager.movePriest(priest1, des);
        }
        if (priest2 != null)
        {
            Vector3 des = priest2.GetPriestPosition();
            if (destination.x < 0) des.x -= 3.6f;
            else des.x += 3.6f;
            actionManager.movePriest(priest2, des);
        }
        if (devil1 != null)
        {
            Vector3 des = devil1.GetDevilPosition();
            if (destination.x < 0) des.x -= 3.6f;
            else des.x += 3.6f;
            actionManager.moveDevil(devil1, des);
        }
        if (devil2 != null)
        {
            Vector3 des = devil2.GetDevilPosition();
            if (destination.x < 0) des.x -= 3.6f;
            else des.x += 3.6f;
            actionManager.moveDevil(devil2, des);
        }
    }

    public Vector3 GetDestination()
    {
        if (boatLocation == 0)
            return new Vector3(-1.8f, 0.15f, -4);
        else
            return new Vector3(1.8f, 0.15f, -4);
    }

    public int GetStatus()
    {
        return 0;//moveScript.GetObjectStatus();
    }

    public int GetBoatPriestNum()
    {
        int n = 0;
        if (priest1 != null) n++;
        if (priest2 != null) n++;
        return n;
    }

    public int GetBoatDevilNum()
    {
        int n = 0;
        if (devil1 != null) n++;
        if (devil2 != null) n++;
        return n;
    }

    public void Reset()
    {
        boat.transform.position = new Vector3(1.8f, 0.15f, -4);
        for (int i = 0; i < 2; i++)
        {
            boatSeats[i] = 0;
        }
        boatLocation = 0;
        priest1 = null;
        priest2 = null;
        devil1 = null;
        devil2 = null;
    }
}

    public class CoastManager
{
    GameObject coastRight;
    GameObject coastLeft;
    private int[] rightPriestSeats = new int[3];//0 for empty, 1 for occupied
    private int[] rightDevilSeats = new int[3];
    private int[] leftPriestSeats = new int[3];
    private int[] leftDevilSeats = new int[3];

    public CoastManager()
    {
        coastRight = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/DockRight", typeof(GameObject)), new Vector3(5f, 0.5f , -4), Quaternion.identity, null) as GameObject;
        coastLeft = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/DockLeft", typeof(GameObject)), new Vector3(-5f, 0.5f, -4), Quaternion.identity, null) as GameObject;
        coastLeft.transform.Rotate(0, 180, 0);
        for(int i = 0; i < 3; i++)
        {
            rightPriestSeats[i] = 1;
            rightDevilSeats[i] = 1;
            leftPriestSeats[i] = 0;
            leftDevilSeats[i] = 0;
        }
    }

    public Vector3 PriestGetOnCoastDestination(PriestManager _priest)//仅仅返回地点，不对逻辑进行修改
    {
        if (_priest.GetPriestLocation() == 0)//right side
        {
            for (int i = 0; i < 3; i++)
            {
                if (rightPriestSeats[i] == 0)
                {
                    return new Vector3(3 + i * 0.6f, 0.8f, -4);
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (leftPriestSeats[i] == 0)
                {
                    return new Vector3(-3 - i * 0.6f, 0.8f, -4);
                }
            }
        }
        return new Vector3(0, 0, 0);
    }

    public void PriestGetOnCoast(PriestManager _priest)
    {
        if(_priest.GetPriestLocation()==0)//right side
        {
            for (int i = 0; i < 3; i++)
            {
                if (rightPriestSeats[i] == 0)
                {
                    //_priest.SetPriestDestination(new Vector3(3+ i * 0.6f, 0.8f,-4));
                    rightPriestSeats[i] = 1;
                    return;
                }
            }
        }
       else
        {
            for (int i = 0; i < 3; i++)
            {
                if (leftPriestSeats[i] == 0)
                {
                    //_priest.SetPriestDestination(new Vector3(-3 - i * 0.6f, 0.8f, -4));
                    leftPriestSeats[i] = 1;
                    return;
                }
            }
        }
    }

    public Vector3 DevilGetOnCoastDestination(DevilManager _devil)
    {
        if (_devil.GetDevilLocation() == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (rightDevilSeats[i] == 0)
                {
                    return new Vector3(4.8f + i * 0.6f, 0.8f, -4);
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (leftDevilSeats[i] == 0)
                {
                    return new Vector3(-4.8f - i * 0.6f, 0.8f, -4);
                }
            }
        }
        return new Vector3(0, 0, 0);
    }

    public void DevilGetOnCoast(DevilManager _devil)
    {
        if (_devil.GetDevilLocation() == 0)
        {
            for(int i = 0; i < 3; i++)
            {
                if (rightDevilSeats[i] == 0)
                {
                    //_devil.SetDevilDestination(new Vector3(4.8f + i * 0.6f, 0.8f, -4));
                    rightDevilSeats[i] = 1;
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (leftDevilSeats[i] == 0)
                {
                    //_devil.SetDevilDestination(new Vector3(-4.8f - i * 0.6f, 0.8f, -4));
                    leftDevilSeats[i] = 1;
                    return;
                }
            }
        }
    }

    public void PriestGetOffCoast(PriestManager _priest)
    {
        if (_priest.GetPriestLocation() == 0)
        {
            if (_priest.GetPriestPosition().x  == 3) rightPriestSeats[0] = 0;
            if (_priest.GetPriestPosition().x == 3.6f) rightPriestSeats[1] = 0;
            if (_priest.GetPriestPosition().x == 4.2f) rightPriestSeats[2] = 0;
        }
        else
        {
            if (_priest.GetPriestPosition().x == -3) leftPriestSeats[0] = 0;
            if (_priest.GetPriestPosition().x == -3.6f) leftPriestSeats[1] = 0;
            if (_priest.GetPriestPosition().x == -4.2f) leftPriestSeats[2] = 0;
        }
    }

    public void DevilGetOffCoast(DevilManager _devil)
    {
        if (_devil.GetDevilLocation() == 0)
        {
            if (_devil.GetDevilPosition().x == 4.8f) rightDevilSeats[0] = 0;            
            if (_devil.GetDevilPosition().x == 5.4f) rightDevilSeats[1] = 0;
            if (_devil.GetDevilPosition().x == 6f) rightDevilSeats[2] = 0;
        }
        else
        {
            if (_devil.GetDevilPosition().x == -4.8f) leftDevilSeats[0] = 0;
            if (_devil.GetDevilPosition().x == -5.4f) leftDevilSeats[1] = 0;
            if (_devil.GetDevilPosition().x == -6f) leftDevilSeats[2] = 0;
        }
    }

    public int GetRightCoastPriestNum()
    {
        int n = 0;
        for(int i = 0; i < 3; i++)
        {
            if (rightPriestSeats[i] == 1)
                n++;
        }
        return n;
    }

    public int GetRightCoastDevilNum()
    {
        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            if (rightDevilSeats[i] == 1)
                n++;
        }
        return n;
    }

    public int GetLeftCoastPriestNum()
    {
        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            if (leftPriestSeats[i] == 1)
                n++;
        }
        return n;
    }

    public int GetLeftCoastDevilNum()
    {
        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            if (leftDevilSeats[i] == 1)
                n++;
        }
        return n;
    }

    public void Reset()
    {
        for (int i = 0; i < 3; i++)
        {
            rightPriestSeats[i] = 1;
            rightDevilSeats[i] = 1;
            leftPriestSeats[i] = 0;
            leftDevilSeats[i] = 0;
        }
    }
}
