###加入物理运动的新版打飞碟游戏，推荐运行分辨率1024x768

###修改第一版的几个bug

* 解决点击地面也可以得分的问题
* round2分数足够时可以进入round3

###物理运动

物理运动主要依靠的是gameobject的rigidbody元素，它默认会让物体受到重力的影响，并且会进行碰撞，在第一版的程序代码基础上，进行如下修改：

* 每次新建ufo时加入rigidbody组件
* 把之前的使飞碟运动的方法改为物理运动实现，其实是在每次给飞碟施加一个力（注意不是constantforce，相当于扔一下物体）
* 在工厂回收ufo时要加入重置rigidbody速度和角速度的设定，避免重新扔出后初速度不是零或者会旋转
* 进行了一点数学计算，让每次扔出的飞碟都飞向player（摄像机）方向

在把之前的运动方法改为物理运动实现时，用到了adapter适配器模式，为了实现一个既可以提供transform运动方法又可以提供物理运动方法的类，这个设计模式实际上就是用新建的类既继承原类又同时实现新类，在这次代码中就是新建一个物理运动的interface，用新的actionmanager既继承旧的运动类，又同时实现新物理运动interface。

###部分代码

* 适配器模式实现

```

//实现物理接口, adapter模式
public interface PhysicsInterface
{
    void ThrowUFO(UFOManager ufo);
}

//再在新的类中继承原类并且实现新类即可
public class TotalActionManager: CCActionManager, PhysicsInterface
{
    private Rigidbody rb;
    private Vector3 pos;
    private double x, y, z;
    private double fx, fy, fz;
    private int originalForce;

    //physics way by rigidbody and force
    public void ThrowUFO(UFOManager ufo)
    {
       ...
}

```

* 计算力的方向,确保每次ufo都扔向玩家

```
pos = ufo.GetPosition();
x = Math.Abs(pos.x);
y = Math.Abs(pos.y);
z = Math.Abs(pos.z);
fz = originalForce * z * Math.Sqrt(1 / (x * x + z * z));
//Debug.Log(x+" "+z+" "+Math.Sqrt(1 / (x * x + z * z)));
fx = x / z * fz;
if (pos.x > 0)
{
    fx = -fx;
}
//Debug.Log(fx / x + " " + fz / z);
rb.AddForce(Convert.ToSingle(fx), 400, -Convert.ToSingle(fz));
```



