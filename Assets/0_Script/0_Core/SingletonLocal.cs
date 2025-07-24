using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonLocal<T> : MonoBehaviour where T:MonoBehaviour
{
    static T _inst;

    public static T instance
    {
        get
        {
            GameObject singleton_obj = FindFirstObjectByType<T>().gameObject;
            return _inst = (singleton_obj == null) ? singleton_obj.AddComponent<T>() : singleton_obj.GetComponent<T>();
        }
    }
}