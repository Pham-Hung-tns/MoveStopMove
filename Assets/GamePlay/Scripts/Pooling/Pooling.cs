using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    public static Pooling instance;

    private Dictionary<string, Queue<GameObject>> EverythingGO = new Dictionary<string, Queue<GameObject>>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void _Push(string key, GameObject gob)
    {
        gob.SetActive(false);
        if (!EverythingGO.ContainsKey(key))
        {
            EverythingGO.Add(key, new Queue<GameObject>());
        }
        EverythingGO[key].Enqueue(gob);
    }

    // this _pull instatiate gameObject from Resources
    public GameObject _PullResources(string key, string path)
    {
        if (EverythingGO.ContainsKey(key))
        {
            if (EverythingGO[key].Count > 0)
            {
                GameObject gobCopy = EverythingGO[key].Dequeue();
                gobCopy.SetActive(true);
                return gobCopy;
            }
            else
            {
                GameObject gobCopy = Instantiate(Resources.Load<GameObject>(path));
                gobCopy.SetActive(true);
                return gobCopy;
            }
        }
        else
        {
            GameObject gobCopy = Instantiate(Resources.Load<GameObject>(path));
            gobCopy.SetActive(true);
            return gobCopy;
        }
    }

    // this _pull instatiate gameObject from scriptable object and set position.
    public GameObject _Pull(string key, GameObject obj, Transform newTransform)
    {
        if (EverythingGO.ContainsKey(key))
        {
            if (EverythingGO[key].Count > 0)
            {
                GameObject gobCopy = EverythingGO[key].Dequeue();
                gobCopy.SetActive(true);
                return gobCopy;
            }
            else
            {
                GameObject gobCopy = Instantiate(obj, newTransform);
                gobCopy.SetActive(true);
                return gobCopy;
            }
        }
        else
        {
            GameObject gobCopy = Instantiate(obj, newTransform);
            gobCopy.SetActive(true);
            return gobCopy;
        }
    }
}