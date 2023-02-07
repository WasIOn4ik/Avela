using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings
{
    public string playerName = "";
}

public class GameBase : MonoBehaviour
{
    [SerializeField] protected ClientBase client;
    [SerializeField] protected ServerBase server;
    public PlayerSettings playerSettings = new();

    public void SetupAsClient()
    {
        client.Init();
        client.StartClient();
    }

    public void SetupAsServer()
    {
        server.Init();
        server.StartServer();
    }

    public void SetupAsHost()
    {
        server.Init();
        client.Init();
        server.StartHost();
    }
}
