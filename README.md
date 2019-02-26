# unity3d

unity3d homework

## 代码结构

MVC——好像作业中一直用的就是这个，但是记忆中好多次用的时候要思考很久哪部分代码放在哪里。

## 设计模式总结

### MVC/MIC（没有动作分离）

这个是上3d课作业每次都用到的最重要的结构。Model是场景中的物体；Controller从下而上有：物体上挂在的对应controller，场景中的ISceneontroller，和最上层的Director；View，其实我认为GUI中的控制部分也属于Controller，场景中显示的东西（包括界面）是View。

MIC是模型、交互与控制模式，交互和MVC中的View类似。

### 组合模式/Composite Pattern

将对象组合成树形结构以表示“部分-整体”的层次结构，组合模式使得用户对单个对象和组合对象的使用具有一致性。其实就是把场景中的椅子放到桌子的components中，通过操纵桌子就可以对桌子和椅子这个整体进行操作。
