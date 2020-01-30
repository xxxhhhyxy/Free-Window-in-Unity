using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class moveEdge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    /// <summary>
    /// 边框
    /// </summary>
    private Image m_img;
    /// <summary>
    /// UItransform
    /// </summary>
    private RectTransform m_rect;
    /// <summary>
    /// 正常颜色
    /// </summary>
    private Color NormalColor = Color.white;
    /// <summary>
    /// 边缘种类
    /// </summary>
    private RectTransform.Edge m_type;
    /// <summary>
    /// 画布
    /// </summary>
    private Canvas m_canvas;
    /// <summary>
    /// 窗口
    /// </summary>
    private windowFather m_father;
    /// <summary>
    /// 用于寄存当下的鼠标位置
    /// </summary>
    private Vector2 pos = Vector2.zero;
    /// <summary>
    /// 拖动边时的影子
    /// </summary>
    private moveEdge m_shadow;
    /// <summary>
    /// 拖动前的位置
    /// </summary>
    private Vector3 PosBeforeDrag;
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInited = false;
    /// <summary>
    /// 对面的边
    /// </summary>
    private moveEdge m_oppoEdge;
    /// <summary>
    /// 对面边的位置
    /// </summary>
    private Vector2 oppoPos;
    /// <summary>
    /// 窗口超出临界的警报
    /// </summary>
    private bool isSafe=true;

    public RectTransform.Edge M_Type
    {
        get
        {
            return m_type;
        }
    }

    public Image M_Img
    {
        get
        {
            if(m_img==null)
                m_img = GetComponent<Image>();
            return m_img;
        }
    }

    public moveEdge M_OppoEdge
    {
        get
        {
            return m_oppoEdge;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="u_father"></param>
    /// <param name="u_canvas"></param>
    /// <param name="u_type"></param>
    public void Init(windowFather u_father, moveEdge u_oppo, Canvas u_canvas, RectTransform.Edge u_type)
    {
        
        m_rect = GetComponent<RectTransform>();
        m_canvas = u_canvas;
        m_father = u_father;
        m_oppoEdge = u_oppo;
        m_type = u_type;
        isInited = true;
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
    /// 进入高亮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInited)
            return;
        M_Img.color = m_father.M_EdgeHighColor;
    }

    /// <summary>
    /// 退出高亮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInited)
            return;
        M_Img.color = NormalColor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInited)
            return;
        PosBeforeDrag = transform.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, PosBeforeDrag, eventData.pressEventCamera, out pos))
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, M_OppoEdge.transform.position, eventData.pressEventCamera, out oppoPos))
            {
                m_father.OnBeginDragEdge(this, pos, oppoPos);
            }
        }

        m_shadow = Instantiate(this, transform);
        m_shadow.transform.localPosition = Vector3.zero;
        m_shadow.transform.localEulerAngles = Vector3.zero;
        m_shadow.M_Img.color = m_father.M_EdgeShadowColor;
        isSafe = true;
    }

    public void OnDrag(PointerEventData eventData)
    {  
        if (!isInited)
            return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, eventData.position, eventData.pressEventCamera, out pos))
        {            
            m_father.OnDragEdge(this, pos,out isSafe);
        }
        if (isSafe)
        {
            if (m_type == RectTransform.Edge.Top || m_type == RectTransform.Edge.Bottom)
            {
                m_shadow.transform.position = new Vector3(PosBeforeDrag.x, eventData.position.y, PosBeforeDrag.z);
            }
            else
            {
                m_shadow.transform.position = new Vector3(eventData.position.x, PosBeforeDrag.y, PosBeforeDrag.z);
            }
        }
        m_shadow.M_Img.color = isSafe? m_father.M_EdgeShadowColor:m_father.M_ErrorColor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInited)
            return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, eventData.position, eventData.pressEventCamera, out pos))
        {
            //Debug.Log(pos);
            m_father.OnEndDragEdge(this, pos);
        }
        Destroy(m_shadow.gameObject);
    }

}