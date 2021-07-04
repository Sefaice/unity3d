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

    private Transform item;
    private Vector3 oldPosition;
    private string oldDir;

    //private Vector3 offset;
    private Transform newItem;
    private Vector3 newPosition;
    private string newDir;

    //public GraphicRaycaster m_CanvasUI;
    //public EventSystem eventSystem;

    void Start()
    {
        item = null;
        oldPosition = new Vector3();
        newItem = null;
        newPosition = new Vector3();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        item = data.pointerEnter.transform.GetChild(0); // image
        //Debug.Log("begindrag: " + item.name);
        oldPosition = item.position;
        oldDir = item.parent.parent.gameObject.name;
        ////拖动的物体处于最上层
        //item.layer = 9;
    }

    public void OnDrag(PointerEventData data)
    {
        // Debug.Log("dragging: " + data.pointerDrag);
        if (item!=null)
        {
            Vector3 pos = Input.mousePosition;
            //自己试着转换一直不对，抄了博客中的转换方法，好像log出来鼠标的坐标没有加上ui相机的-250
            Vector3 mmp = camera.ScreenToWorldPoint(pos + new Vector3(0, 0, 250));
            item.position = new Vector3(mmp.x, mmp.y, 0);
            //防止拖动过程中旋转
            item.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        
        // Debug.Log(data.pointerEnter);
        
        if (data.pointerEnter != null)
        {
            newItem = data.pointerEnter.transform.GetChild(0);
            newDir = newItem.parent.parent.gameObject.name;

            if (newDir != oldDir) // 交换位置
            {
                newPosition = newItem.position;
                newItem.position = oldPosition;
                item.position = newPosition;

                //// get index
                //int newIndex = newItem.transform.GetSiblingIndex();
                //int oldIndex = item.transform.GetSiblingIndex();

                // change parent
                Transform newParent = newItem.parent;
                newItem.parent = item.parent;
                item.parent = newParent;

                //// set correct index
                //newItem.transform.SetSiblingIndex(oldIndex);            
                //item.transform.SetSiblingIndex(newIndex);
            }
            else // 放回原位
            {
                if (oldDir == "Bag")
                {
                    item.position = oldPosition;
                    item.eulerAngles = bag.transform.eulerAngles;
                }
                else
                {
                    item.position = oldPosition;
                    item.eulerAngles = storage.transform.eulerAngles;
                }
            }
        }
        else // 放回原位
        {
            if (oldDir == "Bag")
            {
                item.position = oldPosition;
                item.eulerAngles = bag.transform.eulerAngles;
            }
            else
            {
                item.position = oldPosition;
                item.eulerAngles = storage.transform.eulerAngles;
            }
        }


        // Debug.Log(newItem.transform.position);

        item = null;
        oldPosition = new Vector3();
        newItem = null;
        newPosition = new Vector3();
    }
}