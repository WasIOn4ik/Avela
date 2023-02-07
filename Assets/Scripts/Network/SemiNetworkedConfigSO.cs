using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spes/SemiNetworkConfig")]
public class SemiNetworkedConfigSO : ScriptableObject
{
    private static SemiNetworkedConfigSO _instance = null;

    public static SemiNetworkedConfigSO Instance
    {
        get
        {
            if (_instance == null)
            {
                var res = Resources.FindObjectsOfTypeAll<SemiNetworkedConfigSO>();

                if (res.Length == 0)
                {
                    Debug.LogError("SemiNetworkedCinfigSO was not created");
                    return null;
                }
                else if (res.Length > 1)
                {
                    string r = "";
                    foreach (var rr in res)
                    {
                        r += rr.name + " ";
                    }
                    Debug.LogError($"SemiNetworkedCinfigSO has multiple copies: {r}");
                    return null;
                }
                _instance = res[0];
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            return _instance;
        }
    }
    [ReadOnly, SerializeField] protected ulong lastID = 0;

    [ReadOnly, SerializeField] protected List<ulong> recycledIDs = new();

    [ReadOnly, SerializeField] public List<SemiNetworkLocationProxy> proxies;

    public ulong AssignNextID()
    {
        ulong res;
        if (recycledIDs.Count > 0)
        {
            res = recycledIDs[0];
            recycledIDs.RemoveAt(0);
            return res;
        }
        else
        {
            res = ++lastID;
        }

        return res;
    }

    public ulong GetNextID()
    {
        return lastID + 1;
    }

    public void FreeID(ulong id)
    {
        recycledIDs.Add(id);
    }

    public bool ValidateID(ulong id)
    {
        if (lastID >= id)
            return false;

        return recycledIDs.Contains(id);
    }

    public void AddProxy(SemiNetworkLocationProxy proxy)
    {
        if (!proxies.Contains(proxy))
            proxies.Add(proxy);
    }

    public void RemoveProxy(SemiNetworkLocationProxy proxy)
    {
        proxies.Remove(proxy);
    }

    public void ValidateProxies()
    {
        for (int i = 0; i < proxies.Count; i++)
        {
            if (!proxies[i])
            {
                proxies.RemoveAt(i);
                i--;
            }
        }
    }

    public void RegisterInstanceToSceneProxy(SemiBehaviour beh)
    {
        foreach (var el in proxies)
        {
            if (el.gameObject.scene.buildIndex == beh.gameObject.scene.buildIndex)
            {
                Debug.Log($"Регистрация {beh.semiID} в {el.gameObject.name}");
                el.RegisterInstance(beh);
                return;
            }
        }
    }

    public void UnregisterInstanceFromSceneProxy(SemiBehaviour beh)
    {
        foreach (var el in proxies)
        {
            if (el.gameObject.scene.buildIndex == beh.gameObject.scene.buildIndex)
            {
                Debug.Log($"Удаление регистрации {beh.semiID} в {el.gameObject.name}");
                el.UnregisterInstance(beh);
                return;
            }
        }
    }
}
