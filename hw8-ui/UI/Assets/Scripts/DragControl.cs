using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//IBeginDragHandler, IDragHandler, IEndDragHandler
public class DragControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Camera camera;
    public GameObject bag;
    public GameObject storage;

    private GameObject item;
    private Vector3 oldPosition;
    //private Vector3 offset;
    private GameObject newItem;
    private Vector3 newPosition;

    public GraphicRaycaster m_CanvasUI;
    public EventSystem eventSystem;


    void Start()
    {
        item = null;
        oldPosition = new Vector3();
        newItem = null;
        newPosition = new Vector3();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        //Debug.Log("begindrag: " + data.pointerDrag);
        item = data.pointerEnter;
        oldPosition = item.transform.position;
        //offset = item.transform.position - Input.mousePosition;
        //拖动的物体处于最上层
        //item.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData data)
    {
        //Debug.Log("dragging: " + data.pointerDrag);
        if(item!=null && item.name!="Bag" && item.name!="Storage")
        {
            Vector3 pos = Input.mousePosition;
            //自己试着转换一直不对，抄了博客中的转换方法，好像log出来鼠标的坐标没有加上ui相机的-250
            Vector3 mmp = camera.ScreenToWorldPoint(pos + new Vector3(0, 0, 250));
            item.transform.position = new Vector3(mmp.x, mmp.y, 0);
            //防止拖动过程中旋转
            item.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        //交换位置
        //Debug.Log(data.pointerEnter);
        if (false)
        {
            newItem = data.pointerEnter;
            newPosition = newItem.transform.position;
            newItem.transform.position = oldPosition;
            item.transform.position = newPosition;
        }
        else//放回原位
        {
            if (item.transform.parent.gameObject.name == "Bag")
            {
                item.transform.position = oldPosition;
                item.transform.eulerAngles = bag.transform.eulerAngles;
            }
            else
            {
                item.transform.position = oldPosition;
                item.transform.eulerAngles = storage.transform.eulerAngles;
            }
        }
        item = null;
        oldPosition = new Vector3();
        newItem = null;
        newPosition = new Vector3();
    }
}
