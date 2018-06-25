### AI坦克大战

* 建议运行分辨率：1280*720 Windowed
* 双人模式/单人模式
* 单人模式：玩家控制右下角的红色坦克，电脑控制左上角蓝色坦克，方向键控制移动，L射击
* 双人模式：player1控制右下角红色坦克，上下左右移动，L射击；player2控制左上角蓝色坦克，wasd移动，空格射击
* 射击有冷却时间
* 摧毁对方基地胜利，坦克被击中后会在基地边复活



服务器玩家是右下角的player，客户端是左上角的ai

networkmanager的playerobject在每次连接都会创建，场景中新建两个挂在networkstartposition的物体作为初始位置，这样服务器始终是左下角的player

isserver和islocal结合起来可以判断谁是player谁是ai

这样就能作为碰撞判断了

SyncVar是服务器权限变量
https://blog.csdn.net/yangxuan0261/article/details/52555167

识别不同的player
