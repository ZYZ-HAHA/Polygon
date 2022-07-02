using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Polygon;

public class MainView : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerClickHandler
{
    Dictionary<int, UtilPolygon> m_List = new Dictionary<int, UtilPolygon>();
    Dictionary<int, UtilPolygon> area = new Dictionary<int, UtilPolygon>();
    Vector2[] directions = new Vector2[6];
    public RectTransform parent;
    public Transform line;
    int hor = 10;
    int vert = 20;
    Vector2 startPos = Vector2.zero;
    private void Awake()
    {
        Factory.Instance.Prefab = line.gameObject;
        Factory.Instance.Parent = parent.transform;
        directions[0] = new Vector2(1, 0);
        directions[1] = new Vector2(1, 1);
        directions[2] = new Vector2(-1, 1);
        directions[3] = new Vector2(-1, 0);
        directions[4] = new Vector2(-1, -1);
        directions[5] = new Vector2(1, -1);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetParentRect();
        for (int i = 0; i < vert; i++)
        {
            for(int j = 0; j < hor; j++)
            {
                int id = j * 100 + i;
                UtilPolygon polygon = new UtilPolygon(j*100+i);
                m_List.Add(id,polygon);
            }
        }
    }

    void SetParentRect()
    {
        float line = Factory.Instance.Rect.sizeDelta.x;
        float halfWidth = line * Mathf.Cos(30 * Mathf.PI / 180);
        float totalWidth = halfWidth * 2 * hor + halfWidth;
        float totalHeight = line * 2 * vert - 0.5f * (vert - 1) * line;
        parent.sizeDelta = new Vector2(totalWidth, totalHeight);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = new Vector2(eventData.position.x,eventData.position.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endPos = new Vector2(eventData.position.x, eventData.position.y);
        Vector2 normal = endPos - startPos;
        parent.anchoredPosition += normal;
        if (parent.anchoredPosition.x >= 2.5f)
            parent.anchoredPosition = new Vector3(2.5f, parent.anchoredPosition.y);
        else if(parent.anchoredPosition.x <= 1080 - parent.sizeDelta.x - 2.5f)
            parent.anchoredPosition = new Vector3(1080 - parent.sizeDelta.x - 2.5f, parent.anchoredPosition.y);


        if(parent.anchoredPosition.y >= 0)
            parent.anchoredPosition = new Vector3(parent.anchoredPosition.x, 0);
        else if (parent.anchoredPosition.y <= 1920 - parent.sizeDelta.y)
            parent.anchoredPosition = new Vector3(parent.anchoredPosition.x, 1920 - parent.sizeDelta.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 endPos = new Vector2(eventData.position.x, eventData.position.y);
        Vector2 normal = endPos - startPos;
        parent.anchoredPosition += normal*0.05f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 point = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, Camera.main, out point);
        float line = Factory.Instance.Rect.sizeDelta.x;
        float x = point.x / parent.sizeDelta.x * hor;
        float y = point.y/ parent.sizeDelta.y * vert;
        Vector2 pivot = new Vector2(x,y);
        int id = (int)x *100 + (int)y;
        UtilPolygon polygon;
        if(m_List.TryGetValue(id, out polygon))
        {
            if (polygon.ContainsPoint(point)){
                UtilPolygon single;
                if (area.TryGetValue(id, out single))
                {
                    for (int j = 0; j < single.lines.Length; j++)
                        single.lines[j].img.color = Color.white;
                    area.Remove(id);
                }
                else
                    area.Add(id, polygon);
            }
            else
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    pivot += directions[i];
                    id = (int)x * 100 + (int)y;
                    if (m_List.TryGetValue(id, out polygon))
                    {
                        if (polygon.ContainsPoint(point))
                        {
                            UtilPolygon single;
                            if (area.TryGetValue(id, out single))
                            {
                                for (int j = 0; j < single.lines.Length; j++)
                                    single.lines[j].img.color = Color.white;
                                area.Remove(id);
                            }
                            else
                                area.Add(id, polygon);
                            break;
                        }
                    }
                }
            }
        }
        foreach (UtilPolygon item in area.Values)
        {
            for (int i = 0; i < item.lines.Length; i++)
            {
                int closerId = Factory.Instance.GetCosed(item.lines[i].id);
                UtilPolygon ploygon;
                if (closerId < 0 || !area.TryGetValue(closerId / 10, out ploygon))
                    item.lines[i].img.color = Color.red;
                else
                    item.lines[i].img.color = Color.white;
            }
        }
    }
    // Update is called once per frame
}
