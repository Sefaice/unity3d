# 简单巡逻兵游戏
在以前学习的设计模式上，新加入订阅和发布模式实现的巡逻兵小游戏，玩家控制小人避免与巡逻兵（僵尸）碰撞，每次甩掉一个巡逻兵，游戏分数+1

这次程序的主要难点在**订阅发布模式的运用**和**巡逻兵运动的批量控制**

### 代码资源和demo
Github地址: 

游戏建议打开分辨率: 1024*768 windowed

### 控制人物移动
Input.GetAxis("Horizontal")是x方向，和transform中position的x一样，即左右方向，返回值负数代表x反方向，Input.GetAxis("Vertical")值z方向，即前后方向

为了达到每次运动任务都看向运动方向的效果，使用movement先得到运动方向，再先旋转朝向这个方向，再运动
```
Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
transform.rotation = Quaternion.LookRotation(movement);
transform.Translate(movement * player_speed * Time.deltaTime, Space.World);
```

### 动画
animator-transition中的has exit time选项取消勾选，就可以在动画状态之间无缝切换，否则这个例子中的走和停的动画不会马上触发


### 碰撞
碰撞是由collider+rigidbody属性产生的，rigidbod可以让物体受(重)力，同时一个有collider和rigidbody的物体会碰撞另一个只要有collider的物体
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
OncollisionEnter（）在此对象进入另一个物体时触发，OnTriggerEnter（）在对象被进入时触发，对应的三个函数分别是进入、退出、每帧调用,传入的参数是碰撞物体的信息

### 巡逻兵移动
因为需要巡逻兵不断地走凸多边形，第一个要解决的问题就是怎么实现每一段直线运动后紧接着下一段运动并且循环往复

在zombiemanager中可以构造执行每一段运动的函数，但是调用需要在actionmanager中进行；以前用到的actionmanager动作管理器中动作有执行完的回调接口，但是接口的参数并不是我想要的zombiemanager，所以需要在父级的actionmanager中加入zombiemanager这个元素。

但是又不想直接从基类全部改变动作管理器，这样其它物体的动作就不适用了，这就用到了上次作业学到的门面模式，新建一个适用于巡逻兵（巡逻兵==zombie）的SSActionmanager继承原来的类，里面加入新的元素，并在父级类把要重写的函数加入关键字virtual，这样CCActionmanager继承这个专用类就能达到目的了
```
public class SSActionManagerForZombie : SSActionManager
{
    private ZombieManager zombieManager { get; set; }

    public void SetZombieManager(ZombieManager _zombieManager)
    {
        zombieManager = _zombieManager;
    }

    public override void actionDone(SSAction source)
    {
        if (source.gameobject.tag == "zombie")
        {
            zombieManager.MoveOn();
        }
    }
}
```

* 上面的都没有用。。。
巡逻兵追击player时在actionmanager没有用新建ssaction入队列的方法，因为这个追击方法在update中调用，要用这种结构的话太浪费入队出队。

BUT实际上，这个chaseplayer函数不应该在update调用（？？），应该是调用一次就在动作管理器中不断update始终追击玩家，但是这样是不是又超出了动作管理器的功能？

rigidbody中isKinematic选项选中后，物体只会有碰撞属性，不会有如力的作用

### 订阅与发布模式
这次作业最难的部分。使用此模式的目的是方便分离组件的耦合，如在一个庞大的游戏中，成千上万的玩家每个人完成一个任务后都需要向系统报告，如果发放奖励的代码分散在每个玩家的身上，游戏很难维护并且繁琐，使用这个模式下，每一个玩家都是发布者，游戏服务器是订阅者，这样每次一个玩家完成后就可以让服务器收到信息，进行处理。

在这里，场记就是订阅者，定义的订阅时间类就是发布者，在场记初始化时订阅这些时间，这样在玩家出现如逃脱、死亡的事件后就能让场记收到消息，并且可以很方便地扩展。