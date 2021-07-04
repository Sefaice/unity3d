# 简单巡逻兵游戏
在以前学习的设计模式上，新加入订阅和发布模式实现的巡逻兵小游戏，玩家控制小人（士兵solider）避免与巡逻兵（僵尸zombie）碰撞，每次甩掉一个巡逻兵，游戏分数+1

这次程序的主要难点在**订阅发布模式的运用**和**巡逻兵运动的批量控制**

### 代码资源和demo
Github地址: https://github.com/Sefaice/unity3d/tree/master/hw6-PatrolMan

游戏建议打开分辨率: 1024*768 windowed

### 控制人物移动

Input.GetAxis("Horizontal")是x方向输入，和transform中position的x一样，即左右方向，返回值负数代表x反方向，Input.GetAxis("Vertical")值z方向，即前后方向

为了达到每次运动任务都看向运动方向的效果，使用movement先得到运动方向，再先旋转朝向这个方向，再进行运动，玩家控制的任务和巡逻兵的移动方向都是这样每次先朝向运动终点
```
Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
transform.rotation = Quaternion.LookRotation(movement);
transform.Translate(movement * player_speed * Time.deltaTime, Space.World);
```

### 动画

solider和zombie的动画都使用资源预制中的动画，建立一个向量机，基本都是静止——运动——攻击/死亡这几个动画，加入变量进行控制即可

animator-transition中的has exit time选项取消勾选，就可以在动画状态之间无缝切换，否则这个例子中的走和停的动画不会马上触发

### 碰撞

碰撞是由collider+rigidbody属性产生的，rigidbod可以让物体受(重)力，一个有collider和rigidbody的物体会碰撞另一个只要有collider的物体

```
public class ColliderScript : MonoBehaviour
{
    void OnCollisionEnter()
    {
        Debug.Log("oncolliEnter!");
    }

    void OnCollisionExit()
    {
        Debug.Log("oncolliExit!");
    }
    
   	void OnCollisionStay(){}

   	void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
    }

    void OnTriggerExit()
    {
        Debug.Log("triggerExit!");
    }
    
    OnTriggerStay(){}
}
```

对于碰撞事件，`OncollisionEnter()`在此对象进入另一个物体时触发，`OnTriggerEnter()`在对象被进入时触发，对应的三个函数分别是进入、退出、每帧调用,传入的参数是碰撞物体的信息

rigidbody中isKinematic选项选中后，物体只会有运动学属性，没有碰撞作用

### 巡逻兵移动

因为需要巡逻兵不断地走凸多边形，第一个要解决的问题就是怎么实现每一段直线运动后紧接着下一段运动并且循环往复

在zombiemanager中可以构造执行每一段运动的函数，但是调用需要在actionmanager中进行；以前用到的actionmanager动作管理器中动作有执行完的回调接口，但是接口的参数并不是我想要的zombiemanager，所以需要在父级的actionmanager中加入zombiemanager这个元素，我初始的想法是：

>不想直接从基类全部改变动作管理器，这样其它物体的动作就不适用了，尝试使用上次作业学到的门面模式，新建一个适用于巡逻兵的SSActionmanager继承原来的类，里面加入新的元素，并在父级类把要重写的函数加入关键字virtual，这样CCActionmanager继承这个专用类就能达到目的了

但是这样就出现了问题：
* 怎么实现玩家角色进入范围内开始追击？
* 怎么管理多个巡逻兵每个不同的动作？

所以，最后上面提到的的没有用，采用的是全部修改action动作管理器，以能够解决上面的问题，这样巡逻兵追击、不同巡逻兵动作管理、临界状态判定都在动作管理器中实现

实现巡逻兵自己循环移动的actiondone函数如下：

```
public class SSActionManager : MonoBehaviour, ISSActionCallback
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();
    
    public void actionDone(SSAction source)
    {
        if (source.gameobject.tag == "zombie")
        {
            CCMoveToAction action = CCMoveToAction.GetSSAction(source.zombieManager, source.playerManager, true);
            this.RunAction(action, this);
        }
    }
}
```

**显而易见**，这样把动作管理器改称巡逻兵动作管理器有许多问题：

* 如果要在项目中加入其它需要动作管理器的运动，无法扩展
* 动作管理器的监管范围是否太多，还需要传入gameobjectManager

其中一个，巡逻兵追逐角色的功能实现必然需要在某一处调用Update（）方法不断追逐角色方向，那么这个方法如果是动作管理器内的，动作管理器需要的参数太多，巡逻兵、玩家都需要；如果在动作管理器外，这些运动的实现又脱离了动作中心。

同时，检测玩家进入巡逻兵追击范围，如果用collier碰撞盒实现，玩家进入的碰撞触发后，需要在这个挂载的脚本内调用动作管理器进行追击；如果在动作管理器内update检测距离，这是否又超出了它的责任范围？

这个是动作管理器执行动作的update函数：

```
protected void Update()
{
    foreach (SSAction ac in waitingAdd)
    {
        actions[ac.GetInstanceID()] = ac;
    }
    waitingAdd.Clear();

    foreach (KeyValuePair<int, SSAction> kv in actions)
    {
        SSAction ac = kv.Value;

        float dist = Vector3.Distance(ac.playerManager.GetPosition(), ac.zombieManager.GetPosition());
        //within hunt distance AND in the same zone
        if (dist <= 8 && ac.zombieManager.GetZoneId() == ac.playerManager.GetZone() && ac.playerManager.life == true)
        {
            if(ac.routine==true)
            {
                ac.zombieManager.chasing = true;
                ac.destroy = true;
                CCMoveToAction action = CCMoveToAction.GetSSAction(ac.zombieManager, ac.playerManager, false);
                this.RunAction(action, this);
            }
        }
        else if (ac.zombieManager.chasing == true)//player escape 
        {
            if (ac.routine == false)
            {
                ac.zombieManager.chasing = false;
                ac.destroy = true;
                CCMoveToAction action = CCMoveToAction.GetSSAction(ac.zombieManager, ac.playerManager, true);
                this.RunAction(action, this);
                Singleton<GameEventManager>.Instance.PlayerEscape();
            }
        }
    }

    foreach (KeyValuePair<int, SSAction> kv in actions)
    {
        SSAction ac = kv.Value;

        if (ac.destroy == true)
        {
            waitingDelete.Add(ac.GetInstanceID());
        }
        else if (ac.enable == true)
        {
            ac.Update();
        }
    }

    foreach (int key in waitingDelete)
    {
        SSAction ac = actions[key];
        actions.Remove(key);
        DestroyObject(ac);
    }
    waitingDelete.Clear();
}
```

**动作管理器到底有什么样的职责范围，如这次遇到的运动要怎么分配？** - 判断僵尸状态应该用地图上门的碰撞盒实现。

### 订阅与发布模式

这次作业最难的部分。使用此模式的目的是方便分离组件的耦合，如在一个庞大的游戏中，成千上万的玩家每个人完成一个任务后都需要向系统报告，如果发放奖励的代码分散在每个玩家的身上，游戏很难维护并且繁琐，使用这个模式下，每一个玩家都是发布者，游戏服务器是订阅者，这样每次一个玩家完成后就可以让服务器收到信息，进行处理。

在此游戏中，场记（firstSceneController）就是订阅者，定义的订阅事件类就是发布者，在场记初始化时订阅这些事件，这样在玩家出现如逃脱、死亡的事件后就能让场记收到消息，并且可以很方便地扩展。

```
public class GameEventManager : MonoBehaviour
{
    //玩家逃脱
    public delegate void PlayerEscapeEvent();
    public static event PlayerEscapeEvent myPlayerEscapeEvent;

    //游戏结束
    public delegate void GameOverEvent();
    public static event GameOverEvent myGameOverEvent;

    public void PlayerEscape()
    {
        if (myPlayerEscapeEvent != null)
        {
            myPlayerEscapeEvent();
        }
    }
    //玩家被捕
    public void PlayerGameOver()
    {
        if (myGameOverEvent != null)
        {
            myGameOverEvent();
        }
    }
}
```

场记把Escape和GameOver函数分别初始化为委托的实例（即这两个函数订阅了事件），当事件发生时就通过delegate调用这两个函数

### 其余

剩下部分如单例模式、工厂模式、gui等问题比较简单

### 思考

unity项目代码非常多，结构很复杂，所以能很好地学到设计模式和项目规划，如这次的订阅发布，真正打一个这样的项目才能理解