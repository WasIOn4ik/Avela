using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public enum SceneTriggerType
{
    LoadTrigger,
    UnloadTrigger,
    SwitchTrigger,
    TileTrigger
}
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Collider))]
public class SceneSwitchController : MonoBehaviour
{
    [SerializeField] protected WorldCompositor compositor;

    delegate void triggerEnterDelegate(Collider other);

    triggerEnterDelegate triggerEnterFunc;
    triggerEnterDelegate triggerExitFunc;

    protected NetworkObject networkObject;

    public void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        triggerEnterFunc = EmptyFunction;
        triggerExitFunc = EmptyFunction;
    }

    public void InitAsServer()
    {
        triggerEnterFunc = HandleServerTriggerEnter;
        triggerExitFunc = HandleServerTriggerExit;
    }

    public void OnTriggerEnter(Collider other)
    {
        triggerEnterFunc(other);
    }

    public void OnTriggerExit(Collider other)
    {
        triggerExitFunc(other);
    }

    public void SetCompositor(WorldCompositor comp)
    {
        compositor = comp;
    }

    public void HandleServerTriggerEnter(Collider other)
    {
        var chunk = other.GetComponent<ChunkNode>();
        if (chunk)
        {
            Debug.Log("Interact");
            switch (chunk.triggerType)
            {
                case SceneTriggerType.LoadTrigger:
                    compositor.MarkNeeded(chunk.scenesList, networkObject.OwnerClientId, chunk.nodeName);
                    break;
                case SceneTriggerType.UnloadTrigger:
                    compositor.MarkUnneeded(chunk.scenesList, networkObject.OwnerClientId, chunk.nodeName);
                    break;
                case SceneTriggerType.SwitchTrigger:
                    break;
                case SceneTriggerType.TileTrigger:
                    compositor.MarkNeeded(chunk.scenesList, networkObject.OwnerClientId, chunk.nodeName);
                    break;
            }
        }
    }

    public void HandleServerTriggerExit(Collider other)
    {

        var chunk = other.GetComponent<ChunkNode>();
        if (chunk)
        {
            if (chunk.triggerType != SceneTriggerType.TileTrigger)
                return;

            compositor.MarkUnneeded(chunk.scenesList, networkObject.OwnerClientId, chunk.nodeName);
        }
    }

    public void EmptyFunction(Collider other)
    {

    }
}
