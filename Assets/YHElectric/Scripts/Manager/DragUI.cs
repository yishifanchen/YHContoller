using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// UI和指针的位置偏移量
    /// </summary>
    Vector3 offset;

    RectTransform rt;
    Vector3 pos;
    float minWidth;             //水平最小拖拽范围
    float maxWidth;            //水平最大拖拽范围
    float minHeight;            //垂直最小拖拽范围  
    float maxHeight;            //垂直最大拖拽范围
    float rangeX;               //拖拽范围
    float rangeY;               //拖拽范围

    public Transform lt;
    public Transform lb;
    public Transform rb;
    public Transform rt1;

    private Vector3 startPos;
    RectTransform rtChild;

    bool isMoving = false;
    float timer = 0;
    void Update()
    {
        DragRangeLimit();
        isMoving = (newPos != oldPos);
        newPos = oldPos;
        timer += Time.deltaTime;
    }

    void Start()
    {
        rt = GetComponent<RectTransform>();
        rtChild = rt.transform.Find("mouse").GetComponent<RectTransform>();
        startPos = rt.position;
        pos = rt.position;

        minWidth = rt.rect.width / 2;
        maxWidth = transform.parent.GetComponent<RectTransform>().position.x+transform.parent.GetComponent<RectTransform>().sizeDelta.x- (rt.rect.width / 2);
        minHeight = rt.rect.height / 2;
        maxHeight = transform.parent.GetComponent<RectTransform>().position.y+transform.parent.GetComponent<RectTransform>().sizeDelta.y - (rt.rect.height / 2);
    }

    /// <summary>
    /// 拖拽范围限制
    /// </summary>
    void DragRangeLimit()
    {
        //限制水平/垂直拖拽范围在最小/最大值内
        //rangeX = Mathf.Clamp(rt.position.x, minWidth, maxWidth);
        //rangeY = Mathf.Clamp(rt.position.y, minHeight, maxHeight);

        rangeX = Mathf.Clamp(rt.position.x, lt.position.x+ rtChild.rect.width / 2, rt1.position.x- rtChild.rect.width / 2);
        rangeY = Mathf.Clamp(rt.position.y, lb.position.y+ rtChild.rect.height / 2, rt1.position.y- rtChild.rect.height / 2);
        //更新位置
        rt.position = new Vector3(rangeX, rangeY, 0);
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;

        //将屏幕坐标转换成世界坐标
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out globalMousePos))
        {
            //计算UI和指针之间的位置偏移量
            offset = rt.position - globalMousePos;
        }
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        oldPos = eventData.delta;
        if (isMoving&&timer>0.2f)
        {
            timer = 0;
            UDPControl.instance.uDPClient.Send("mouseMoveX&" + eventData.delta.x);
            UDPControl.instance.uDPClient.Send("mouseMoveY&" + eventData.delta.y);
        }
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        rt.position = startPos;
        UDPControl.instance.uDPClient.Send("mouseMoveX&" + 0);
        UDPControl.instance.uDPClient.Send("mouseMoveY&" + 0);
    }
    Vector2 oldPos;
    Vector2 newPos;
    Vector3 globalMousePos;
    /// <summary>
    /// 更新UI的位置
    /// </summary>
    private void SetDraggedPosition(PointerEventData eventData)
    {
        //Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out globalMousePos))
        {
            rt.position = offset + globalMousePos;
        }
    }
}