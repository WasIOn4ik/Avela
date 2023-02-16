using ModestTree;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ServerBase : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] protected NetworkManager networkManagerPrefab;
    [SerializeField] private PlayerController playerControllerPrefab;
    [SerializeField] protected WorldCompositor worldCompositorPrefab;
    [NonSerialized] public WorldCompositor compositor;
    public AssetNetManager assetManager;

    public void CreateNetworkManager()
    {
        networkManager = Instantiate(networkManagerPrefab);
        networkManager.NetworkConfig.SpawnTimeout = 1f;
        compositor = Instantiate(worldCompositorPrefab);
        assetManager = compositor.GetComponent<AssetNetManager>();
        DontDestroyOnLoad(compositor);
    }

    public void Init()
    {
        networkManager.ConnectionApprovalCallback = ApproveConnection;
        networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    public void StartHost()
    {
        var netConf = networkManager.GetComponent<UnityTransport>();
        var data = netConf.ConnectionData;
        data.Address = "127.0.0.1";
        data.Port = 2545;
        SceneManager.activeSceneChanged += _startHost;
        SceneManager.LoadSceneAsync("RootScene");
    }

    private void _startHost(Scene scene, Scene scene2)
    {
        SceneManager.activeSceneChanged -= _startHost;
        networkManager.StartHost();
    }

    public void StartServer()
    {
        var netConf = networkManager.GetComponent<UnityTransport>();
        var data = netConf.ConnectionData;
        data.Address = "127.0.0.1";
        data.Port = 2545;
        SceneManager.activeSceneChanged += _startServer;
        SceneManager.LoadSceneAsync("RootScene");
    }

    private void _startServer(Scene scene, Scene scene2)
    {
        SceneManager.activeSceneChanged -= _startServer;
        networkManager.StartServer();
    }

    private void OnClientConnected(ulong clientID)
    {
        if (!networkManager.IsServer)
            return;

        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PlayerController controller = SpawnPlayerCOntroller(clientID);
        }
    }

    public PlayerController SpawnPlayerCOntroller(ulong clientID)
    {
        var playerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        playerController.serverRef = this;
        Debug.LogError(compositor.name);
        playerController.SetCompositor(compositor);
        playerController.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);

        return playerController;
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (!networkManager.IsServer)
            return;

        response.CreatePlayerObject = false;
        response.Approved = true;
    }
}
