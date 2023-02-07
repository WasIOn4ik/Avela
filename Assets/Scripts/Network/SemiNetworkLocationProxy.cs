using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SemiNetworkLocationProxy : MonoBehaviour
{
    public SemiNetworkManager semiNetworkManager;
    [SerializeField] public List<SemiNetworkObjectEntry> registry = new();
    [SerializeField] public int count;

    //public static List<SemiNetworkLocationProxy> proxies = new();

    public void Awake()
    {
        Debug.Log($"Прокси для сцены {gameObject.scene.name}  инциализировано");
        semiNetworkManager = SemiNetworkManager.instance;
        AddProxy(this);
    }

    public void OnDestroy()
    {
        RemoveProxy(this);
    }

    public static void AddProxy(SemiNetworkLocationProxy proxy)
    {
        SemiNetworkedConfigSO.Instance.AddProxy(proxy);
    }
    public static void RemoveProxy(SemiNetworkLocationProxy proxy)
    {
        SemiNetworkedConfigSO.Instance.RemoveProxy(proxy);
    }

    public void RegisterInstance(SemiBehaviour instance)
    {
        int index = registry.FindIndex(x => x.className == instance.className);
        if (index == -1)
            registry.Add(new SemiNetworkObjectEntry() { className = instance.className, instances = new List<SemiBehaviour> { instance } });
        else
            registry[index].instances.Add(instance);

        count = registry.Count;

        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
        EditorUtility.SetDirty(this);
    }
    public void UnregisterInstance(SemiBehaviour instance)
    {
        int index = registry.FindIndex(x => x.className == instance.className);
        if (index == -1)
            return;
        else
            registry[index].instances.Remove(instance);

        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
        EditorUtility.SetDirty(this);
    }

    public bool TryRegisterInstance(string className, SemiBehaviour instance)
    {
        if (!String.IsNullOrEmpty(className))
        {
            if (instance)
            {
                foreach (var el in registry)
                {
                    if (el.className == className)
                    {
                        if (!el.instances.Contains(instance))
                            el.instances.Add(instance);
                    }
                }
            }
            else
                Debug.LogError("Попытка зарегистрировать нулевой инстанс");
        }
        else
        {
            if (instance)
                Debug.LogError("Попытка зарегистрировать инстанс для нулевого префаба");
            else
                Debug.LogError("Попытка зарегистрировать нулевой инстанс для нулевого префаба");
        }
        return false;
    }
}
