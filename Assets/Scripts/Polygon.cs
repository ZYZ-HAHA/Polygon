using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polygon
{
    public class UtilPolygon
    {
        public int id;
        private Vector3 pivot;
        public UtilLine[] lines = new UtilLine[6];
        Vector2[] verts = new Vector2[6];
        public UtilPolygon() {
            id = 0;
        }
        public UtilPolygon(int id){
            this.id = id;
            this.pivot = GetPiovt();
            for (int i = 0; i < lines.Length; i++)
            {
                UtilLine line = new UtilLine(this.id*10 + i);
                line.obj.localPosition = GetLinePos(i);
                line.obj.localEulerAngles = GetLineRotate(i);
                verts[i] = GetVertPos(i);
                lines[i] = line;
            }
        }
        //逆时针
        Vector2 GetVertPos(int index) {
            float line = Factory.Instance.Rect.sizeDelta.x;
            float angle = (60 * index + 30) * Mathf.PI / 180;
            return new  Vector2(line * Mathf.Cos(angle), line * Mathf.Sin(angle));
        }
        Vector3 GetPiovt()
        {
            int x = this.id / 100;
            int y = this.id % 100;
            float line = Factory.Instance.Rect.sizeDelta.x;
            float halfWidth = line * Mathf.Cos(30*Mathf.PI/180);
            if(y%2 == 0)
                return new Vector3((x * 2 + 1) * halfWidth, (y+1) * line + y*line*0.5f, 0);
            else
                return new Vector3(((x+1) * 2) * halfWidth, (y+1) * line + y*line*0.5f, 0);
        }
        /// <summary>
        /// 逆时针
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Vector3 GetLinePos(int index)
        {
            float line = Factory.Instance.Rect.sizeDelta.x;
            float radius = line * Mathf.Cos(30*Mathf.PI/180);
            Vector3 pos = new Vector3(radius * Mathf.Cos(60*index*Mathf.PI/180), radius * Mathf.Sin(60*index*Mathf.PI/180), 0);
            return this.pivot + pos;
        }
        Vector3 GetLineRotate(int index)
        {
            return new Vector3(0,0,index*60-90);
        }
        public bool ContainsPoint(Vector2 point)
        {
            point -= new Vector2(pivot.x,pivot.y);
            int j = verts.Length - 1;
            Vector3 normal = new Vector3(0, 0, 1);
            for (int i = 0; i < verts.Length; j = i++)
            {
                Vector3 normal0 = verts[i] - verts[j];
                Vector3 normal1 = point - verts[i];
                Vector3 normalp = Vector3.Normalize(Vector3.Cross(normal0, normal1));
                if (normalp != normal)
                    return false;
            }
            return true;
        }
    }
    public class UtilLine
    {
        public int id;
        public int index;
        public RectTransform rect;
        public Transform obj;
        public Image img;
        public UtilLine() {}
        public UtilLine(int id) { 
            this.id = id;
            this.index = id%10;
            this.obj = Factory.Instance.GetItem(id);
            this.rect = obj.GetComponent<RectTransform>();
            this.img = obj.GetComponent<Image>();
        }
    }
}
