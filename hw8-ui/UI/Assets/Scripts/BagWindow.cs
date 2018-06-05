using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagWindow : MonoBehaviour
{
    Quaternion mStart;
    Vector2 rot;

    void Start()
    {
        mStart = transform.localRotation;
    }

    //实现ui界面随着鼠标移动跟随方向旋转的效果
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;
        //clamp,小于最小值返回min，大于最大值返回max，否则返回原值
        float x = Mathf.Clamp((pos.x - Screen.width / 2 + 300) / (Screen.width + 300), -1f, 1f);
        //Debug.Log(x);
        float y = Mathf.Clamp((pos.y - Screen.height / 2) / Screen.height, -1f, 1f);
        /*
        rot = Vector2.Lerp(rot, new Vector2(x, y), Time.deltaTime * 5f);
        transform.localRotation = mStart * Quaternion.Euler(-rot.y * range.y, rot.x * range.x, 0f);
        */
        //鼠标x方向偏差确定沿y轴旋转角度，y方向决定沿x轴旋转
        transform.eulerAngles = new Vector3(5 * y, -45 * x, 0);
    }
}
