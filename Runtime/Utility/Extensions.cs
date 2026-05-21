using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Extensions
{
    public static void DestroyChildren(this Transform transform)
    {
        for (int c = transform.childCount - 1; c >= 0; --c)
        {
            var go = transform.GetChild(c).gameObject;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    Object.DestroyImmediate(go);
                };
                continue;
            }
#endif
            Object.Destroy(go);

        }
    }

    public static List<Transform> GetChildren(this Transform transform)
    {
        var children = new List<Transform>();
        for(int i = 0; i < transform.childCount; ++i)
        {
            children.Add(transform.GetChild(i));
        }
        return children;
    }

    public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go;
#if UNITY_EDITOR
        go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.SetParent(parent, true);
#else
        go = Object.Instantiate(prefab, position, rotation, parent);
#endif
        return go;
    }
    public static void SetParentFlushly(this RectTransform child, RectTransform parent)
    {
        child.SetParent(parent, false); 
        child.anchorMin = Vector2.zero;
        child.anchorMax = Vector2.one;
        child.offsetMin = Vector2.zero;
        child.offsetMax = Vector2.zero;
    }
    public static Collider[] OverlapCapsule(this CapsuleCollider collider)
    {
        Vector3 center = collider.transform.position + collider.center;
        Vector3 axis = Vector3.zero;
        axis[collider.direction] = (collider.height - 2f * collider.radius) * 0.5f;
        return Physics.OverlapCapsule(center + axis, center - axis, collider.radius);
    }
    public static void Invoke(this MonoBehaviour mb, System.Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
    public static T GetRandom<T>(this IEnumerable<T> collection)
    {
        var c = collection.ToArray();
        if (c.Length <= 0) return default(T);
        return c[Random.Range(0, c.Length)];
    }
    public static ICollection<T> GetNRandom<T>(this IEnumerable<T> collection, int count)
    {
        var bag = collection.ToList().OrderBy(i => Random.Range(0f, 1f)).ToList();
        var result = new List<T>();
        while (count > 0)
        {
            if (bag.Count() <= 0)
            {
                //Debug.LogWarning("There weren't enough elements to grab randomly");
                break;
            }
            int r = Random.Range(0, bag.Count()); // random index from the bag
            result.Add(bag[r]);
            bag.RemoveAt(r);
            count--;
        }
        return result;
    }
    public static T MinElement<T>(this IEnumerable<T> collection, System.Func<T, float> selector)
    {
        return collection.Aggregate((min, x) => (selector(x) < selector(min) ? x : min));
    }
    public static T MaxElement<T>(this IEnumerable<T> collection, System.Func<T, float> selector)
    {
        return collection.Aggregate((min, x) => (selector(x) > selector(min) ? x : min));
    }
    /// <summary>
    /// Modify the float array so that the sum of elements is 1
    /// </summary>
    public static void Normalize(this float[] array)
    {
        float sum = array.Sum();
        if (Mathf.Approximately(sum, 0f)) return;
        for (int i = 0; i < array.Length; ++i)
        {
            array[i] /= sum;
        }
    }
    /// <summary>
    /// Returns whether the parameter was already the value
    /// </summary>
    public static bool TrySet(this ref bool b, bool newVal)
    {
        if (b == newVal) return true;
        b = newVal;
        return false;
    }
    public static int EvaluateRoundedInt(this ParticleSystem.MinMaxCurve curve, float time)
    {
        return Mathf.RoundToInt(curve.Evaluate(time, Random.Range(0f,1f)));
    }

    public static int Coinflip =>Random.Range(0f, 1f) < 0.5f ? -1 : 1;
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        var type = original.GetType();
        var copy = destination.AddComponent(type);
        var fields = type.GetFields();
        foreach (var field in fields) field.SetValue(copy, field.GetValue(original));
        return copy as T;
    }
    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public static void ForEachBackward<T>(this IEnumerable<T> e, System.Action<T> a)
    {
        for(int i = e.Count()-1; i >= 0; --i)
        {
            a(e.ElementAt(i));
        }
    }

}
