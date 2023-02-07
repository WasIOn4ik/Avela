using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct SemiNetworkObjectEntry
{
    public string className;

    [SerializeField] public List<SemiBehaviour> instances;
}

[DefaultExecutionOrder(-10)]
public class SemiNetworkManager : NetworkBehaviour
{
    public bool bIseWorldCompositor = true;
    public static SemiNetworkManager instance;

    protected void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
