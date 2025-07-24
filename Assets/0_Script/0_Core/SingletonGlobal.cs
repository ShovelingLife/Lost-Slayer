using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGlobal<T> : MonoBehaviour where T : MonoBehaviour
{
    static T          _inst;
    static GameObject singletonObj;

    public static T instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindFirstObjectByType<T>();

                if (_inst == null)
                {
                    singletonObj = new GameObject();
                    singletonObj.name = "(Singleton) " + typeof(T).ToString();
                    _inst = singletonObj.AddComponent<T>();
                }
                else 
                    singletonObj = _inst.gameObject;

                DontDestroyOnLoad(singletonObj);
            }
            return _inst;
        }
    }
}