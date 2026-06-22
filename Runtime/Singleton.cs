using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Netcode;

public class Singleton<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
        OnSetInstance();
    }
    protected virtual void OnSetInstance()
    {

    }
    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
public class SingletonPersistent<T> : Singleton<T> where T : Component
{
    protected override void OnSetInstance()
    {
        DontDestroyOnLoad(Instance);
    }
}
public class SingletonSO<T> : ScriptableObject where T : SingletonSO<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");
                if(assets == null || assets.Length < 1)
                {
                    Debug.LogError("Failed to find SingletonSO");
                    return null;
                }
                else if(assets.Length > 1)
                {
                    Debug.LogWarning("Found too many instances of the SingletonSO. Returning first one...");
                }
                instance = assets[0];
            }

            return instance;
        }
    }

}
public class SingletonInstance<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null) instance = new();
            return instance;
        }
    }

}
//public class SingletonNetwork<T> : NetworkBehaviour where T : Component
//{
//    public static T Instance { get; private set; }

//    protected virtual void Awake()
//    {
//        if (Instance != null)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this as T;
//        OnSetInstance();
//    }
//    protected virtual void OnSetInstance()
//    {

//    }
//    public override void OnDestroy()
//    {
//        if (Instance == this)
//        {
//            Instance = null;
//        }
//        base.OnDestroy();
//    }
//}
//public class SingletonNetworkPersistent<T> : SingletonNetwork<T> where T : Component
//{
//    protected override void OnSetInstance()
//    {
//        DontDestroyOnLoad(Instance);
//    }
//}