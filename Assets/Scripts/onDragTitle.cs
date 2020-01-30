using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class onDragTitle : MonoBehaviour,  IDragHandler
{
    /// <summary>
    /// 窗体
    /// </summary>
    private windowFather m_father;

    /// <summary>
    /// 用于寄存当下的鼠标位置
    /// </summary>
    private Vector3 pos3D = Vector3.zero;

    public void Init(windowFather u_father)
    {
        m_father = u_father;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
    }
    /// <summary>
    /// 拖拽过程中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos3D))
        {
            m_father.OnDragTitle(pos3D - this.transform.position);
        }
    }

}
