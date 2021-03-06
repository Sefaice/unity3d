### AI坦克大战

* 建议运行分辨率：1280*720 Windowed
* 玩家控制右下角的红色坦克，电脑控制左上角蓝色坦克
* 方向键控制移动，空格射击，射击有冷却时间
* 摧毁对方基地胜利，坦克被击中后会在基地边复活

#### 基本结构
地图模型使用了博客看到的unity教程资源，坦克基地红蓝两色分别为玩家和电脑

坦克发射子弹使用工厂模式管理；坦克射击冷却使用协程实现；子弹击中事件和判断用订阅-发布模式实现；

#### AI
对于作业要求的感知——思考——行动的ai设计方法，我的理解是，尽量让ai与人类玩家用相同的感知方式进行游戏，并在行动时尽量模仿人的反应

这样对于3D的游戏而言，ai可以采用射线观察的方法进行观察，但在我这次的游戏中，玩家的观察角度是鸟瞰，所以ai也采用鸟瞰的方式接收信息——即ai知道玩家的位置和玩家基地的位置

这样，我在这次给ai的行动逻辑是：一开始直接走向玩家的基地，如果玩家进入ai一侧的地图，ai追逐玩家；同时ai在看到玩家或者玩家基地直接射击（用射线实现），玩家和ai的坦克速度相同