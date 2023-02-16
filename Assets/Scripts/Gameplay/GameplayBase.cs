using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class GameplayBase : MonoBehaviour
{
    [SerializeField] public ClientBase client;
    [SerializeField] public ServerBase server;
    [SerializeField] protected Camera cameraRef;
    [SerializeField] protected GameCore gameInstance;

    public IMenuBase previousMenu;
    public List<IMenuBase> openedMenus;
    public List<IMenuBase> loadedMenus;

    public void Awake()
    {
        //NetworkManager.Singleton.PrefabHandler.AddHandler
        ClientBase.instance = client;
        foreach (var inst in ProjectContext.Instance.Installers)
        {
            if (inst is CameraInstaller camInst)
            {
                camInst.cameraInstance = cameraRef;
                camInst.InstallBindingsPostStart();
            }
            else if (inst is GameCoreInstaller coreinst)
            {
                coreinst.core = gameInstance;
                coreinst.InstallBindingsPostStart();
            }
        }
    }

    public void SetupAsClient()
    {
        server.CreateNetworkManager();
        client.Init();
        client.StartClient();
    }

    public void SetupAsServer()
    {
        server.CreateNetworkManager();
        server.Init();
        server.StartServer();
    }

    public void SetupAsHost()
    {
        server.CreateNetworkManager();
        server.Init();
        client.Init();
        server.StartHost();
    }
}
