
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.Compilation;
using Unity.Collections;
using System.Text;

public class WorldCompositor : NetworkBehaviour
{
    #region ServerVariables
    protected Dictionary<ulong, List<SceneDescriptor>> loadedScenes = new();
    protected Dictionary<ulong, ulong[]> cachedReceivers = new();
    public List<ServerSceneDescriptor> serverScenes = new();
    public string baseSceneToLoad;
    #endregion

    public delegate void SceneDelegateHandler(ulong clientID, string[] scenes);
    public delegate void SceneEventDelegateHandler(ulong clientID, string[] scenes, ESceneEvent eventType);
    public delegate void SceneEventErrorDelegateHandler(ulong clientID, ESceneError sceneError);
    public delegate void SceneEventCompletedDelegateHandler(ulong[] clientIDs, string[] scenes);
    public delegate void SceneEventCompletedUntypedDelegateHandler(ulong[] clientIDs, string[] scenes, ESceneEvent eventType);

    public event SceneDelegateHandler onSceneLoadStarted;
    public event SceneDelegateHandler onSceneLoadCompleted;

    public event SceneDelegateHandler onSceneUnloadStarted;
    public event SceneDelegateHandler onSceneUnloadCompleted;

    public event SceneDelegateHandler onSceneSwitchStarted;
    public event SceneDelegateHandler onSceneSwitchCompleted;
    public event SceneEventCompletedDelegateHandler onSceneSwitchEventCompleted;

    public event SceneDelegateHandler onSceneSyncStarted;
    public event SceneDelegateHandler onSceneSyncCompleted;

    public event SceneEventErrorDelegateHandler onSceneErrorOccured;

    public event SceneEventCompletedUntypedDelegateHandler onSceneEventCompleted;

    protected AsyncOperation currentOperation;

    public void Awake()
    {
        SceneManager.LoadSceneAsync(baseSceneToLoad, LoadSceneMode.Additive);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.OnClientConnectedCallback += OnPlayerConnected;
        NetworkManager.OnClientDisconnectCallback += OnPlayerDisconneced;

    }

    public void OnPlayerConnected(ulong clientID)
    {
        cachedReceivers.Add(clientID, new ulong[] { clientID });
        loadedScenes.Add(clientID, new List<SceneDescriptor>());
    }

    public void OnPlayerDisconneced(ulong clientID)
    {
        cachedReceivers.Remove(clientID);
        loadedScenes.Remove(clientID);
    }

    /// <summary>
    /// отмечает сцену как "Нужную" для загрузки игроком 
    /// </summary>
    /// <param name="scenesList">Название сцен</param>
    /// <param name="clientID">id сетевого игрока</param>
    public void MarkNeeded(List<string> scenesList, ulong clientID, string callerScene)
    {
        bool excludeCallerScene = !String.IsNullOrEmpty(callerScene);

        /*List<SceneDescriptor> handledScenes = new();
        //Для каждой сцены из переданных
        foreach (var sc in scenesList)
        {
            ESceneState currentState = ESceneState.Unloaded;
            //Пытаемся получить старое состояние и если его нет, то сцена считается не загруженной
            var value = loadedScenes[clientID]; //Получаем данные о сценах игрока
            //var currentScd = value.Find(x => x.sceneName == sc);//Ищем данные о предыдущем состоянии

            int index = value.FindIndex(x => x.sceneName == sc);
            var currentScd = index > -1 ? value[index] : new SceneDescriptor() {bNeeded = true,  }
            if (currentScd.chunkLockers == null)
                currentScd.chunkLockers = new();

            currentScd.chunkLockers.Add(callerScene);

            if (excludeCallerScene && callerScene == sc)
            {
                currentState = ESceneState.Loaded;
                Debug.Log($"Сцена {sc} должна быть исключена");
            }
            else if (currentScd.sceneName == sc)//Если старые данные найдены обновляем состояние
            {
                currentState = currentScd.state;
            }

            handledScenes.Add(new SceneDescriptor() { sceneName = sc, bNeeded = true, state = currentState, chunkLockers = currentScd.chunkLockers });
        }
        loadedScenes[clientID] = handledScenes;
        */

        var sceneDescriptors = loadedScenes[clientID];
        foreach (var sc in scenesList)
        {
            // Обработка сервера
            int si = serverScenes.FindIndex(x => x.sceneName == sc);
            if (si == -1)
            {
                serverScenes.Add(new ServerSceneDescriptor() { bNeeded = true, sceneName = sc, state = ESceneState.Unloaded, clientLockers = new() { clientID } });
            }
            else
            {
                var scd = serverScenes[si];
                scd.clientLockers.Add(clientID);
                scd.bNeeded = true;
                serverScenes[si] = scd;
            }

            //Конец обработки сервера
            int i = sceneDescriptors.FindIndex(x => x.sceneName == sc);
            if (i == -1)
            {
                sceneDescriptors.Add(new SceneDescriptor() { bNeeded = true, sceneName = sc, state = ESceneState.Unloaded, chunkLockers = new() { callerScene } });
            }
            else
            {
                var scd = sceneDescriptors[i];
                scd.chunkLockers.Add(callerScene);
                scd.bNeeded = true;
                sceneDescriptors[i] = scd;
            }
        }

        //Обработка серверной сцены, вызывающей подзагрузку для того, чтобы она не загружала себя
        var csi = serverScenes.FindIndex(x => x.sceneName == callerScene);
        var currentScdServ = serverScenes[csi];
        if (currentScdServ.state == ESceneState.Unloaded)
        {
            currentScdServ.state = ESceneState.Loaded;
            serverScenes[csi] = currentScdServ;
        }

        //Обработка сцены, вызывающей подзагрузку для того, чтобы она не загружала себя
        var ci = sceneDescriptors.FindIndex(x => x.sceneName == callerScene);
        var currentScd = sceneDescriptors[ci];
        if (currentScd.state == ESceneState.Unloaded)
        {
            currentScd.state = ESceneState.Loaded;
            sceneDescriptors[ci] = currentScd;
        }

        StartCoroutine(HandleScenesForClient(clientID));
        StartCoroutine(HandleScenesForServer());
    }

    /// <summary>
    /// Отмечает сцену как "ненужную" для игрока
    /// </summary>
    /// <param name="sceneName">Название сцены</param>
    /// <param name="clientID">id сетевого игрока</param>
    public void MarkUnneeded(List<string> scenesList, ulong clientID, string callerScene)
    {
        /*List<SceneDescriptor> handledScenes = new();
        //Для каждой сцены из переданных
        foreach (var sc in scenesList)
        {
            var value = loadedScenes[clientID]; //Получаем данные о сценах игрока
            var currentScd = value.Find(x => x.sceneName == sc);//Ищем данные о предыдущем состоянии

            if (currentScd.sceneName != sc)//Старые данные всегда должны быть найдены
                throw new ArgumentException($"сцена {sc} не была обработана прежде и не может стать ненужной игроку {clientID}");

            currentScd.chunkLockers.Remove(callerScene);

            var currentState = currentScd.state;
            bool bIsNeed = currentScd.chunkLockers.Count > 0;

            handledScenes.Add(new SceneDescriptor() { sceneName = sc, bNeeded = bIsNeed, state = currentState, chunkLockers = currentScd.chunkLockers });
        }
        loadedScenes[clientID] = handledScenes;*/

        var sceneDescriptors = loadedScenes[clientID];
        foreach (var sc in scenesList)
        {
            //Обработка сервера
            int si = serverScenes.FindIndex(x => x.sceneName == sc);
            var sscd = serverScenes[si];
            sscd.clientLockers.Remove(clientID);

            if (sscd.clientLockers.Count < 1)
            {
                sscd.bNeeded = false;
                serverScenes[si] = sscd;
            }
            //Конец обработки сервера

            var i = sceneDescriptors.FindIndex(x => x.sceneName == sc);
            var scd = sceneDescriptors[i];
            scd.chunkLockers.Remove(callerScene);

            if (scd.chunkLockers.Count < 1)
            {
                scd.bNeeded = false;
                sceneDescriptors[i] = scd;
            }

        }

        StartCoroutine(HandleScenesForClient(clientID));
        StartCoroutine(HandleScenesForServer());
    }

    /// <summary>
    /// Инициирует загрузку сцены
    /// </summary>
    /// <param name="sceneName"></param>
    protected void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        scene.allowSceneActivation = true;
        currentOperation = scene;
    }

    /// <summary>
    /// Инициирует отгрузку сцены
    /// </summary>
    /// <param name="sceneName"></param>
    protected void UnloadScene(string sceneName)
    {
        var state = SceneManager.GetSceneByName(sceneName).IsValid();
        if (state)
        {
            var scene = SceneManager.UnloadSceneAsync(sceneName);
            scene.allowSceneActivation = true;
            currentOperation = scene;
        }
    }
    /*
        public void StopManagement()
        {
            StopCoroutine(HandleScenes());
        }

        public void StartManagement()
        {
            StartCoroutine(HandleScenes());
        }
    */

    private IEnumerator HandleScenesForClient(ulong clientID)
    {
        List<string> scenesToLoad = new();
        List<string> scenesToUnload = new();

        var targetClientScenes = loadedScenes[clientID];


        for (int i = 0; i < targetClientScenes.Count; i++)
        {
            SceneDescriptor neededScene = targetClientScenes[i];
            if (neededScene.bNeeded)
            {
                if (neededScene.state != ESceneState.Loaded && neededScene.state != ESceneState.InLoad)
                {
                    Debug.Log(neededScene);
                    scenesToLoad.Add(neededScene.sceneName);
                    neededScene.state = ESceneState.InLoad;
                }
            }
            else
            {
                if (neededScene.state != ESceneState.Unloaded && neededScene.state != ESceneState.InUnload)
                {
                    scenesToUnload.Add(neededScene.sceneName);
                    neededScene.state = ESceneState.InUnload;
                }
            }
            targetClientScenes[i] = neededScene;
            yield return null;
        }
        var scenesToLoadArr = scenesToLoad.ToArray();
        var scenesToUnloadArr = scenesToUnload.ToArray();

        Debug.Log("На загрузку:");
        foreach (var s in scenesToLoadArr)
        {
            Debug.Log($"Обработка сцены {s.ToString()}");
        }

        Debug.Log("На отгрузку");
        foreach (var s in scenesToUnloadArr)
        {
            Debug.Log($"Обработка сцены {s.ToString()}");
        }

        if (scenesToLoad.Count > 0 && onSceneLoadStarted != null)
            onSceneLoadStarted.Invoke(clientID, scenesToLoadArr);

        if (scenesToUnload.Count > 0 && onSceneUnloadStarted != null)
            onSceneUnloadStarted.Invoke(clientID, scenesToUnloadArr);

        if (clientID != OwnerClientId)
            CallUpdateScenesClientRpc(scenesToLoadArr, scenesToUnloadArr, new ClientRpcParams() { Send = new() { TargetClientIds = cachedReceivers[clientID] } });
    }

    private IEnumerator HandleScenesForServer()
    {
        List<string> scenesToLoad = new();
        List<string> scenesToUnload = new();

        for (int i = 0; i < serverScenes.Count; i++)
        {
            ServerSceneDescriptor neededScene = serverScenes[i];
            if (neededScene.bNeeded)
            {
                if (neededScene.state != ESceneState.Loaded && neededScene.state != ESceneState.InLoad)
                {
                    Debug.Log(neededScene);
                    scenesToLoad.Add(neededScene.sceneName);
                    neededScene.state = ESceneState.InLoad;
                }
            }
            else
            {
                if (neededScene.state != ESceneState.Unloaded && neededScene.state != ESceneState.InUnload)
                {
                    scenesToUnload.Add(neededScene.sceneName);
                    neededScene.state = ESceneState.InUnload;
                }
            }
            serverScenes[i] = neededScene;
            yield return null;
        }
        var scenesToLoadArr = scenesToLoad.ToArray();
        var scenesToUnloadArr = scenesToUnload.ToArray();

        StartCoroutine(HandleSelectedScenes(scenesToUnloadArr, scenesToLoadArr));
    }
    /*
    private IEnumerator HandleScenes()
    {
        while (true)
        {
            foreach (var sc in neededScenes)
            {
                if (currentOperation != null)
                    if (!currentOperation.isDone)
                        yield return null;
                Debug.LogError($"{sc.Key}: {sc.Value.Count}");
                if (sc.Value.Count > 0)
                {
                    if (!loadedScenes.ContainsKey(sc.Key) || loadedScenes[sc.Key] == ESceneState.Unloaded)
                        LoadScene(sc.Key);
                }
                else
                {
                    Debug.LogWarning($"{sc.Key}: {loadedScenes[sc.Key].ToString()}");
                    if (loadedScenes.ContainsKey(sc.Key) && loadedScenes[sc.Key] == ESceneState.Loaded)
                    {
                        UnloadScene(sc.Key);
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
*/
    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    private void CallUpdateScenesClientRpc(string[] scenesToLoad, string[] scenesToUnload, ClientRpcParams param)
    {
        StartCoroutine(HandleSelectedScenes(scenesToUnload, scenesToLoad));
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    private void InformSceneEventServerRpc(string[] scenes, ESceneEvent eventType, ulong clientID)
    {
        var list = loadedScenes[clientID];
        switch (eventType)
        {
            case ESceneEvent.OnLoadComplete:

                foreach (var s in scenes)
                {
                    int index = list.FindIndex(x => x.sceneName == s);
                    var el = list[index];
                    el.state = ESceneState.Loaded;
                    list[index] = el;

                }

                if (onSceneLoadCompleted != null)
                    onSceneLoadCompleted.Invoke(clientID, scenes);

                break;

            case ESceneEvent.OnSwitchComplete:

                for (int i = 0; i < list.Count; i++)
                {
                    var s = list[i];
                    if (Array.IndexOf(scenes, s.sceneName) > -1)
                    {
                        s.state = ESceneState.Loaded;
                    }
                    else
                    {
                        s.state = ESceneState.Unloaded;
                    }
                    list[i] = s;
                }

                if (onSceneSwitchCompleted != null)
                    onSceneSwitchCompleted.Invoke(clientID, scenes);

                break;

            case ESceneEvent.OnUnloadComplete:

                foreach (var s in scenes)
                {
                    int index = list.FindIndex(x => x.sceneName == s);
                    var el = list[index];
                    el.state = ESceneState.Unloaded;
                    list[index] = el;
                }

                if (onSceneUnloadCompleted != null)
                    onSceneUnloadCompleted.Invoke(clientID, scenes);

                break;
        }
    }

    protected IEnumerator HandleSelectedScenes(string[] scenesToUnload, string[] scenesToLoad)
    {
        bool bLoad = false;
        bool bUnload = false;

        //Подгружаем нужные сцены
        foreach (var sc in scenesToLoad)
        {
            if (currentOperation != null)
                if (!currentOperation.isDone)
                    yield return null;

            bLoad = true;
            LoadScene(sc);
        }
        if (bLoad)
        {
            while (!currentOperation.isDone)
            {
                yield return null;
            }

            if (onSceneLoadCompleted != null)
                onSceneLoadCompleted.Invoke(NetworkManager.LocalClientId, scenesToLoad);

            InformSceneEventServerRpc(scenesToLoad, ESceneEvent.OnLoadComplete, NetworkManager.LocalClientId);
        }

        //Сначала отгружаем ненужные сцены
        foreach (var sc in scenesToUnload)
        {
            if (currentOperation != null)
                if (!currentOperation.isDone)
                    yield return null;

            bUnload = true;
            UnloadScene(sc);
        }

        if (bUnload)
        {
            while (!currentOperation.isDone)
            {
                yield return null;
            }

            if (onSceneUnloadCompleted != null)
                onSceneUnloadCompleted.Invoke(NetworkManager.LocalClientId, scenesToUnload);

            InformSceneEventServerRpc(scenesToUnload, ESceneEvent.OnUnloadComplete, NetworkManager.LocalClientId);
        }


    }
}

/*[InitializeOnLoad]
public class BuildPP
{
    static BuildPP()
    {
        Debug.Log("BuildPP() received control");
        CompilationPipeline.assemblyCompilationFinished += CodeInjector.Injector;
    }
}
public static class CodeInjector
{
    static public void Injector(string filename, CompilerMessage[] CompilerMessages)
    {
        // I see control passed here every time the scripts are rebuilt,
        // even when I switch to Unity from VS...
    }
}*/

