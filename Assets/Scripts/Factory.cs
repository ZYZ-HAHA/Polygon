using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
namespace Polygon
{
    public class Factory
    {
        private Dictionary<int, Transform> _factories = new Dictionary<int, Transform>();
        private static Factory instance;
        public static Factory Instance { get {
                if (instance == null)
                    instance = new Factory();
                return instance; 
            } }
        private GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }
        private RectTransform rect;
        public RectTransform Rect
        {
            get{
                if (rect == null)
                    rect = prefab.transform.GetComponent<RectTransform>();
                return rect;
            }
        }
        private Transform parent;
        public Transform Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public Transform GetItem(int id)
        {
            Transform item;
            int closerId = GetCosed(id);
            if (!_factories.TryGetValue(closerId, out item))
            {
                GameObject obj = GameObject.Instantiate(prefab);
                item = obj.transform;
                item.SetParent(parent);
                item.localScale = Vector3.one;
                item.name = id.ToString();
                obj.SetActive(true);
                _factories.Add(id, item);
            }
            return item;
        }
        public int GetCosed(int id)
        {
            int x = (id / 10)/100;
            int y = (id / 10)%100;
            int index = id % 10;
            if(index == 0)
                return ((x+1)*100 + y)*10 + index + 3;
            else if (index == 1)
                if(y%2==0)
                    return (x * 100 + y+1) * 10 + index + 3;
                else
                    return ((x+1) * 100 + y + 1) * 10 + index + 3;
            else if (index == 2)
                if (y % 2 == 0)
                    return ((x - 1) * 100 + y+1) * 10 + index + 3;
                else
                    return (x * 100 + y + 1) * 10 + index + 3;
            else if (index == 3)
                return ((x - 1) * 100 + y) * 10 + index - 3;
            else if (index == 4)
                if (y % 2 == 0)
                    return ((x-1) * 100 + y - 1) * 10 + index - 3;
                else
                    return (x * 100 + y - 1) * 10 + index - 3;
            else if (index == 5)
                if (y % 2 == 0)
                    return (x * 100 + y - 1) * 10 + index - 3;
                else
                    return ((x+1) * 100 + y - 1) * 10 + index - 3;
            else
                return 0;
        }
    }
}
