using ModestTree;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Tester : INetworkPrefabInstanceHandler
{
    public void Destroy(NetworkObject networkObject)
    {

    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        throw new NotImplementedException();
    }
}

public class ServerBase : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] protected NetworkManager networkManagerPrefab;
    [SerializeField] private PlayerController playerControllerPrefab;
    [SerializeField] protected WorldCompositor worldCompositorPrefab;
    //S[Inject] protected WorldCompositor worldCompositor;
    [Inject] private DiContainer diContainer;

    public void Init()
    {
        networkManager = Instantiate(networkManagerPrefab);

        networkManager.ConnectionApprovalCallback = ApproveConnection;
        networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    public void StartHost()
    {

    }

    public void StartServer()
    {
        var netConf = networkManager.GetComponent<UnityTransport>();
        var data = netConf.ConnectionData;
        data.Address = "127.0.0.1";
        data.Port = 2545;
        networkManager.StartServer();
    }

    private void OnClientConnected(ulong clientID)
    {
        if (!networkManager.IsServer)
            return;

        var playerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        playerController.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (!networkManager.IsServer)
            return;

        response.CreatePlayerObject = false;
        response.Approved = true;
    }
}
