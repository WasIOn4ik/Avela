using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public delegate void AssetLoadDelegate();

public static class AssetLoadSerializationExtentions
{
    public static void ReadValueSafe(this FastBufferReader reader, out AssetReference asset)
    {
        reader.ReadValueSafe(out string readed);
        asset = new AssetReference(readed);
    }

    public static void WriteValueSafe(this FastBufferWriter writer, in AssetReference asset)
    {
        writer.WriteValueSafe(asset.AssetGUID);
    }

    public static void SerializeValue<T>(this BufferSerializer<T> reader, ref AssetReference assetReference) where T : IReaderWriter
    {
        if (reader.IsReader)
        {
            reader.GetFastBufferReader().ReadValueSafe(out string guid);
            assetReference = new AssetReference(guid);
        }
        else
        {
            reader.GetFastBufferWriter().WriteValueSafe(assetReference.AssetGUID);
        }
    }
}
public struct AssetLoadOperation
{
    public AssetLoadOperation(AssetReference asset, AssetLoadDelegate del)
    {
        assetRef = asset;
        funcToCall = del;
    }

    public AssetReference assetRef;
    public AssetLoadDelegate funcToCall;
}

public class AssetNetManager : NetworkBehaviour
{
    public Dictionary<ulong, List<AssetLoadOperation>> operations = new();

    public void Awake()
    {
        NetworkManager.OnClientDisconnectCallback += ClearClientOperations;
    }

    public void CallPrefabLoad(AssetReference asset, AssetLoadDelegate funcToCall, ulong clientID)
    {
        ClientRpcParams par = new ClientRpcParams();
        par.Send.TargetClientIds = new ulong[] { clientID };

        if (operations.TryGetValue(clientID, out var list))
        {
            list.Add(new AssetLoadOperation() { assetRef = asset, funcToCall = funcToCall });
        }
        else
        {
            operations.Add(clientID, new() { new AssetLoadOperation() { assetRef = asset, funcToCall = funcToCall } });
        }
        asset.LoadAssetAsync<GameObject>();
        RequestLoadPrefabClientRpc(asset, par);
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    protected void RequestLoadPrefabClientRpc(AssetReference address, ClientRpcParams param = default)
    {
        Debug.Log(address.AssetGUID);
        Debug.Log(AssetDatabase.GUIDToAssetPath(address.AssetGUID));
        address.LoadAssetAsync<GameObject>().Completed += x => Debug.Log(address.AssetGUID); SendPrefabLoadedServerRpc(address);
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    protected void SendPrefabServerRpc(AssetReference address)
    {

    }

    [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    protected void SendPrefabLoadedServerRpc(AssetReference address, ServerRpcParams param = default)
    {
        if (operations.TryGetValue(param.Receive.SenderClientId, out var list))
        {
            var el = list.FindIndex(x => x.assetRef == address);

            if (el != -1)
            {
                var s = list[el];
                list.RemoveAt(el);
                s.funcToCall();
            }
        }
        address.InstantiateAsync().Completed += x => x.Result.GetComponent<NetworkObject>().Spawn();
    }

    private void ClearClientOperations(ulong clientID)
    {
        if (operations.TryGetValue(clientID, out var list))
        {
            list.Clear();
            operations.Remove(clientID);
        }

    }
}
