using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class moveCorner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    /// <summary>
    /// 用于寄存当下的鼠标位置
    /// </summary>
    private Vector2 pos = Vector2.zero;
    /// <summary>
    /// 图片索引
    /// </summary>
    private Image m_img;
    /// <summary>
    /// 画布
    /// </summary>
    private Canvas m_canvas;
    /// <summary>
    /// 左右边
    /// </summary>
    private moveEdge m_leftOrRight;
    /// <summary>
    /// 上下边
    /// </summary>
    private moveEdge m_upOrDown;
    /// <summary>
    /// 拖动前的位置
    /// </summary>
    private Vector3 PosBeforeDrag;
    /// <summary>
    /// 窗口
    /// </summary>
    private windowFather m_father;
    /// <summary>
    /// UItransform
    /// </summary>
    private RectTransform m_rect;
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInited = false;
    /// <summary>
    /// 拖动时的影子
    /// </summary>
    private moveCorner m_shadow;
    /// <summary>
    /// 正常颜色
    /// </summary>
    private Color NormalColor = Color.white;
    /// <summary>
    /// 对面的角
    /// </summary>
    private moveCorner m_oppoCorner;
    /// <summary>
    /// 对面角的位置
    /// </summary>
    private Vector2 oppoPos;
    /// <summary>
    /// 窗口超出临界的警报
    /// </summary>
    private bool isLRSafe = true;
    /// <summary>
    /// 窗口超出临界的警报
    /// </summary>
    private bool isUDSafe = true;

    public Image M_Img
    {
        get
        {
            if(m_img==null)
                m_img= GetComponent<Image>();
            return m_img;
        }
    }


    public void Init(windowFather u_father,moveCorner u_oppo, Canvas u_canvas, moveEdge u_leftOrRight, moveEdge u_upOrDown)
    {
        m_father = u_father;
        m_canvas = u_canvas;
        m_oppoCorner = u_oppo;
        m_rect = GetComponent<RectTransform>();
        m_leftOrRight = u_leftOrRight;
        m_upOrDown = u_upOrDown;
        isInited = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInited)
            return;
        M_Img.color = m_father.M_CornerHighColor;
    }

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
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, m_oppoCorner.transform.position, eventData.pressEventCamera, out oppoPos))
            {
                m_father.OnBeginDragCorner(m_leftOrRight, m_upOrDown, pos,oppoPos);
            }
        }
        m_shadow = Instantiate(this, transform);
        m_shadow.transform.localPosition = Vector3.zero;
        m_shadow.transform.localEulerAngles = Vector3.zero;
        m_shadow.M_Img.color = m_father.M_CornerShadowColor;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isInited)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, eventData.position, eventData.pressEventCamera, out pos))
        {
            m_father.OnDragCorner(m_leftOrRight,m_upOrDown, pos, out isLRSafe,out isUDSafe);
        }
        Vector3 tempPos = m_shadow.transform.position;
        if (isLRSafe && isUDSafe)
        {
            m_shadow.transform.position = new Vector3(eventData.position.x, eventData.position.y, PosBeforeDrag.z);
            m_shadow.M_Img.color = m_father.M_CornerShadowColor;
        }
        else
        {
            m_shadow.M_Img.color = m_father.M_ErrorColor;
            if (!isLRSafe&&isUDSafe)
                m_shadow.transform.position = new Vector3(tempPos.x, eventData.position.y, PosBeforeDrag.z);
            if (!isUDSafe&&isLRSafe)
                m_shadow.transform.position = new Vector3(eventData.position.x, tempPos.y, PosBeforeDrag.z);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInited)
            return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_father.M_Parent, eventData.position, eventData.pressEventCamera, out pos))
        {
            m_father.OnEndDragCorner(m_leftOrRight, m_upOrDown, pos);
        }
        Destroy(m_shadow.gameObject);
    }
}
