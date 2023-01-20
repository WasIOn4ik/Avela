using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESceneState
{
    Loaded,
    InProgress,
    Unloaded
}

public class WorldCompositor : MonoBehaviour
{
    protected Dictionary<string, List<GameObject>> neededScenes = new Dictionary<string, List<GameObject>>();
    protected Dictionary<string, ESceneState> loadedScenes = new Dictionary<string, ESceneState>();

    protected AsyncOperation currentOperation;

    public void MarkNeeded(string sceneName, GameObject go)
    {
        if (!neededScenes.ContainsKey(sceneName))
        {
            neededScenes.TryAdd(sceneName, new List<GameObject> { go });
            return;
        }

        neededScenes[sceneName].Add(go);
    }

    public void MarkUnneeded(string sceneName, GameObject go)
    {
        if (!neededScenes.ContainsKey(sceneName))
        {
            throw new ArgumentException($"Сцена с названием {sceneName} не была прежде отмечена как нужная");
        }

        if (!neededScenes[sceneName].Contains(go))
        {
            throw new ArgumentException($"Сцена с названием {sceneName} не является нужной объекту {go.name}");
        }

        neededScenes[sceneName].Remove(go);
    }

    public async void LoadScene(string sceneName)
    {
        //Debug.Log($"Закгрузка сцены {sceneName}");
        //loadedScenes.TryAdd(sceneName, ESceneState.Loaded);
        var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        scene.allowSceneActivation = true;
        currentOperation = scene;
    }

    public async void UnloadScene(string sceneName)
    {
        var state = SceneManager.GetSceneByName(sceneName).IsValid();
        //Debug.Log($"Отгрузка сцены {sceneName}: {state}");
        if (state)
        {
            //loadedScenes.TryAdd(sceneName, ESceneState.Unloaded);
            //Debug.Log($"Попытка отгрузки {sceneName}");
            var scene = SceneManager.UnloadSceneAsync(sceneName);
            scene.allowSceneActivation = true;
            currentOperation = scene;
        }
    }

    public void StopManagement()
    {
        StopCoroutine(HandleScenes());
    }

    public void StartManagement()
    {
        StartCoroutine(HandleScenes());
    }

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        StartManagement();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (!loadedScenes.ContainsKey(scene.name))
            throw new ArgumentException($"Сцена с именем {scene.name} Не загружена");

        Debug.Log($"Сцена {scene.name} отгружена");
        loadedScenes[scene.name] = ESceneState.Unloaded;
        foreach (var s in loadedScenes)
        {
            Debug.Log($"{s.Key} x {s.Value}");
        }
        Debug.Log("\n");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Сцена {scene.name} загружена из {gameObject.name}");
        if (!loadedScenes.ContainsKey(scene.name))
            loadedScenes.Add(scene.name, ESceneState.Loaded);
        else
            loadedScenes[scene.name] = ESceneState.Loaded;
    }

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
}
