using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class windowFather : MonoBehaviour
{
    [SerializeField]
    private onDragTitle m_title;
    [SerializeField]
    private moveEdge leftEdge;
    [SerializeField]
    private moveEdge rightEdge;
    [SerializeField]
    private moveEdge upEdge;
    [SerializeField]
    private moveEdge downEdge;
    [SerializeField]
    private moveCorner ulCorner;
    [SerializeField]
    private moveCorner urCorner;
    [SerializeField]
    private moveCorner dlCorner;
    [SerializeField]
    private moveCorner drCorner;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private Vector2 minSize = new Vector2(200, 100);
    [SerializeField]
    private Color edgeHigh = Color.blue;
    [SerializeField]
    private Color edgeShadow = Color.grey;
    [SerializeField]
    private Color cornerHigh = Color.green;
    [SerializeField]
    private Color cornerShadow = Color.cyan;
    [SerializeField]
    private Color errorColor = Color.red;


    /// <summary>
    /// 窗口的rectTransform
    /// </summary>
    private RectTransform m_rect;
    /// <summary>
    /// 画布
    /// </summary>
    private Canvas m_canvas;
    /// <summary>
    /// 窗口的父物体的rectTransform
    /// </summary>
    private RectTransform m_parent;
    /// <summary>
    /// 拖动边缘前保存边的位置
    /// </summary>
    private Vector3 lastEdgePos = Vector3.zero;
    /// <summary>
    /// 拖动之前对面边的位置
    /// </summary>
    private Vector3 lastOppoEdgePos = Vector3.zero;
    /// <summary>
    /// 拖动角之前的位置
    /// </summary>
    private Vector3 lastCornerPos = Vector3.zero;
    /// <summary>
    /// 拖动之前对面角的位置
    /// </summary>
    private Vector3 lastOppoCornerPos = Vector3.zero;
    /// <summary>
    /// 拖动边缘前的尺寸
    /// </summary>
    private Vector2 lastSize = Vector2.zero;



    //public void SetInsetAndSizeFromParentEdge(Edge edge, float inset, float size):
    //①edge是父物体边缘类型
    //②当前物体同侧边到父物体边缘的距离
    //③当前物体在边缘垂直方向上的尺寸
    //RectTransform.Edge.Right确定为右边缘
    //inset代表本物体右边缘与父物体右边缘的距离
    //本物体右边缘不动，因此向左变形，尺寸为size


    #region 属性
    public moveEdge M_LeftEdge
    {
        get
        {
            return leftEdge;
        }
    }

    public moveEdge M_RightEdge
    {
        get
        {
            return rightEdge;
        }
    }

    public moveEdge M_UpEdge
    {
        get
        {
            return upEdge;
        }
    }

    public moveEdge M_DownEdge
    {
        get
        {
            return downEdge;
        }
    }

    public moveCorner M_UpLeftCorner
    {
        get
        {
            return ulCorner;
        }
    }
    public moveCorner M_UpRightCorner
    {
        get
        {
            return urCorner;
        }
    }
    public moveCorner M_DownLeftCorner
    {
        get
        {
            return dlCorner;
        }
    }
    public moveCorner M_DownRightCorner
    {
        get
        {
            return drCorner;
        }
    }

    public Color M_EdgeHighColor
    {
        get
        {
            return edgeHigh;
        }
    }

    public Color M_CornerHighColor
    {
        get
        {
            return cornerHigh;
        }
    }
    public Color M_EdgeShadowColor
    {
        get
        {
            return edgeShadow;
        }
    }

    public Color M_CornerShadowColor
    {
        get
        {
            return cornerShadow;
        }
    }
    public Color M_ErrorColor
    {
        get
        {
            return errorColor;
        }
    }

    public RectTransform M_Parent
    {
        get
        {
            return m_parent;
        }
    }




    #endregion

    /// <summary>
    /// 拖动整个窗口
    /// </summary>
    /// <param name="a">窗口位移增量</param>
    public void OnDragTitle(Vector3 a)
    {
        this.transform.position += a;
    }

    public void OnBeginDragEdge(moveEdge a, Vector3 pos, Vector3 oppoPos)
    {
        lastSize = m_rect.rect.size;
        lastEdgePos = pos;
        lastOppoEdgePos = oppoPos;
        //Debug.Log("oriPos" + b.x);
    }
    public void OnDragEdge(moveEdge curEdge, Vector3 curPos, out bool safe)
    {
        safe = LimitOfEdge(curEdge.M_Type, curPos,true);
    }
    public void OnEndDragEdge(moveEdge curEdge, Vector3 curPos)
    {
        switch (curEdge.M_Type)
        {
            case RectTransform.Edge.Left:
                if (LimitOfEdge(curEdge.M_Type, curPos, true))
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_Type, curPos.x + m_parent.rect.size.x / 2, lastSize.x - curPos.x + lastEdgePos.x);
                else
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_OppoEdge.M_Type, m_parent.rect.size.x - lastOppoEdgePos.x - m_parent.rect.size.x / 2, minSize.x);
                break;

            case RectTransform.Edge.Right:
                if (LimitOfEdge(curEdge.M_Type, curPos, true))
                    //父物体宽度减去当前鼠标位置,得到当前右边缘到父物体右边缘的inset
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_Type, m_parent.rect.size.x - curPos.x - m_parent.rect.size.x / 2, curPos.x + lastSize.x - lastEdgePos.x);
                else
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_OppoEdge.M_Type, lastOppoEdgePos.x + m_parent.rect.size.x / 2, minSize.x);
                break;

            case RectTransform.Edge.Top:
                if (LimitOfEdge(curEdge.M_Type, curPos, true))
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_Type, m_parent.rect.size.y - curPos.y - m_parent.rect.size.y / 2, curPos.y + lastSize.y - lastEdgePos.y);
                else
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_OppoEdge.M_Type, lastOppoEdgePos.y + m_parent.rect.size.y / 2, minSize.y);
                break;

            case RectTransform.Edge.Bottom:
                if (LimitOfEdge(curEdge.M_Type, curPos, true))
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_Type, curPos.y + m_parent.rect.size.y / 2, lastSize.y - curPos.y + lastEdgePos.y);
                else
                    m_rect.SetInsetAndSizeFromParentEdge(curEdge.M_OppoEdge.M_Type, m_parent.rect.size.y - lastOppoEdgePos.y - m_parent.rect.size.y / 2, minSize.y);
                break;
            default:
                break;
        }
        resizeContent();
    }
    public void OnBeginDragCorner(moveEdge leftOrRight, moveEdge upOrDown, Vector3 curCornerPos, Vector3 CurOppoCornerPos)
    {
        lastSize = m_rect.rect.size;
        lastCornerPos = curCornerPos;
        lastOppoCornerPos = CurOppoCornerPos;
    }
    public void OnDragCorner(moveEdge leftOrRight, moveEdge upOrDown, Vector3 curPos, out bool LRSafe, out bool UDSafe)
    {
        LRSafe = LimitOfEdge(leftOrRight.M_Type, curPos, false);
        UDSafe = LimitOfEdge(upOrDown.M_Type, curPos, false);
    }
    public void OnEndDragCorner(moveEdge leftOrRight, moveEdge upOrDown, Vector3 curPos)
    {
        //Debug.Log("now  " + b);
        //Debug.Log("last  " + lastCornerPos);
        //Debug.Log("lastSize  " + lastSize);
        switch (leftOrRight.M_Type)
        {
            case RectTransform.Edge.Left:
                if (LimitOfEdge(leftOrRight.M_Type, curPos, false))

                    //m_rect.SetInsetAndSizeFromParentEdge(a, b.x + m_parent.rect.size.x / 2, Mathf.Max(minSize.x, lastSize.x - b.x + lastEdgePos.x));
                    m_rect.SetInsetAndSizeFromParentEdge(leftOrRight.M_Type, curPos.x + m_parent.rect.size.x / 2, Mathf.Max(minSize.x, lastSize.x - curPos.x + lastCornerPos.x));
                else
                    m_rect.SetInsetAndSizeFromParentEdge(leftOrRight.M_OppoEdge.M_Type, m_parent.rect.size.x - lastOppoCornerPos.x - m_parent.rect.size.x / 2, minSize.x);
                break;
            case RectTransform.Edge.Right:
                if (LimitOfEdge(leftOrRight.M_Type, curPos, false))
                    //m_rect.SetInsetAndSizeFromParentEdge(a, m_parent.rect.size.x - b.x - m_parent.rect.size.x / 2, Mathf.Max(minSize.x, b.x + lastSize.x - lastEdgePos.x));
                    m_rect.SetInsetAndSizeFromParentEdge(leftOrRight.M_Type, m_parent.rect.size.x - curPos.x - m_parent.rect.size.x / 2, Mathf.Max(minSize.x, curPos.x + lastSize.x - lastCornerPos.x));
                else
                    m_rect.SetInsetAndSizeFromParentEdge(leftOrRight.M_OppoEdge.M_Type, lastOppoCornerPos.x + m_parent.rect.size.x / 2, minSize.x);
                break;
            default:
                break;
        }
        switch (upOrDown.M_Type)
        {
            case RectTransform.Edge.Top:
                if (LimitOfEdge(upOrDown.M_Type, curPos, false))
                    //m_rect.SetInsetAndSizeFromParentEdge(a, m_parent.rect.size.y - b.y - m_parent.rect.size.y / 2, Mathf.Max(minSize.y, b.y + lastSize.y - lastEdgePos.y));
                    m_rect.SetInsetAndSizeFromParentEdge(upOrDown.M_Type, m_parent.rect.size.y - curPos.y - m_parent.rect.size.y / 2, Mathf.Max(minSize.y, curPos.y + lastSize.y - lastCornerPos.y));
                else
                    m_rect.SetInsetAndSizeFromParentEdge(upOrDown.M_OppoEdge.M_Type, lastOppoCornerPos.y + m_parent.rect.size.y / 2, minSize.y);
                break;
            case RectTransform.Edge.Bottom:
                if (LimitOfEdge(upOrDown.M_Type, curPos, false))
                    //m_rect.SetInsetAndSizeFromParentEdge(a, b.y + m_parent.rect.size.y / 2, Mathf.Max(minSize.y, lastSize.y - b.y + lastEdgePos.y));
                    m_rect.SetInsetAndSizeFromParentEdge(upOrDown.M_Type, curPos.y + m_parent.rect.size.y / 2, Mathf.Max(minSize.y, lastSize.y - curPos.y + lastCornerPos.y));
                else
                    m_rect.SetInsetAndSizeFromParentEdge(upOrDown.M_OppoEdge.M_Type, m_parent.rect.size.y - lastOppoCornerPos.y - m_parent.rect.size.y / 2, minSize.y);
                break;
            default:
                break;
        }
        resizeContent();


    }

    /// <summary>
    /// 重置内容部分的尺寸
    /// </summary>
    private void resizeContent()
    {
        content.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 30, m_rect.rect.height - 30);
    }

    /// <summary>
    /// 各边是否处于安全区
    /// </summary>
    /// <param name="curType">边类型</param>
    /// <param name="curPos">当前位置</param>
    /// <param name="isEdge">是边还是角在调用这个方法</param>
    /// <returns></returns>
    private bool LimitOfEdge(RectTransform.Edge curType, Vector3 curPos, bool isEdge)
    {
        switch (curType)
        {
            case RectTransform.Edge.Left:
                if (isEdge)
                    return lastSize.x - curPos.x + lastEdgePos.x > minSize.x;
                else
                    return lastSize.x - curPos.x + lastCornerPos.x > minSize.x;
            case RectTransform.Edge.Right:
                if (isEdge)
                    return lastSize.x + curPos.x - lastEdgePos.x > minSize.x;
                else
                    return lastSize.x + curPos.x - lastCornerPos.x > minSize.x;
            case RectTransform.Edge.Top:
                if (isEdge)
                    return lastSize.y + curPos.y - lastEdgePos.y > minSize.y;
                else
                    return lastSize.y + curPos.y - lastCornerPos.y > minSize.y;
            case RectTransform.Edge.Bottom:
                if (isEdge)
                    return lastSize.y - curPos.y + lastEdgePos.y > minSize.y;
                else
                    return lastSize.y - curPos.y + lastCornerPos.y > minSize.y;
            default:
                return false;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_rect = transform as RectTransform;
        m_parent = transform.parent as RectTransform;
        m_canvas = FindObjectOfType<Canvas>();
        m_title.Init(this);
        leftEdge.Init(this, rightEdge, m_canvas, RectTransform.Edge.Left);
        rightEdge.Init(this, leftEdge, m_canvas, RectTransform.Edge.Right);
        upEdge.Init(this, downEdge, m_canvas, RectTransform.Edge.Top);
        downEdge.Init(this, upEdge, m_canvas, RectTransform.Edge.Bottom);
        M_UpLeftCorner.Init(this, M_DownRightCorner, m_canvas, M_LeftEdge, M_UpEdge);
        M_UpRightCorner.Init(this, M_DownLeftCorner, m_canvas, M_RightEdge, M_UpEdge);
        M_DownLeftCorner.Init(this, M_UpRightCorner, m_canvas, M_LeftEdge, M_DownEdge);
        M_DownRightCorner.Init(this, M_UpLeftCorner, m_canvas, M_RightEdge, M_DownEdge);
    }

    // Update is called once per frame
    void Update()
    {

    }
}