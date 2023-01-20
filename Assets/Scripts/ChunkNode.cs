using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ChunkNode : MonoBehaviour
{
    public string nodeName;
    public List<string> ensureLoaded;
    public static Dictionary<string, int> activeMaps = new Dictionary<string, int>();
    [SerializeField] protected TMP_Text chunkText;

    private List<Scene> loadedScenes = new List<Scene>();

    public void Awake()
    {
        chunkText.text = nodeName;
    }

    public void OnTriggerEnter(Collider other)
    {
        /*var no = other.gameObject.GetComponent<NetworkObject>();
        Debug.Log("Collision with + " + other.gameObject.name);
        if (no && no.gameObject.tag == "Player")
        {
            StartCoroutine(LoadScenes());
        }*/
    }

    public void OnTriggerExit(Collider other)
    {
        /*var no = other.gameObject.GetComponent<NetworkObject>();
        Debug.Log("Collision end with + " + other.gameObject.name);
        if (no && no.gameObject.tag == "Player")
        {
            StartCoroutine(UnloadScenes());
        }*/
    }

    private IEnumerator LoadScenes()
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneLoaded;
        int i = 0;

        while (i < ensureLoaded.Count)
        {
            while (true)
            {
                var state = NetworkManager.Singleton.SceneManager.LoadScene(ensureLoaded[i], LoadSceneMode.Additive);
                if (state == SceneEventProgressStatus.SceneEventInProgress)
                {
                    yield return null;
                }
                else if (state == SceneEventProgressStatus.Started)
                {
                    if (!activeMaps.TryAdd(ensureLoaded[i], 1))
                        activeMaps[ensureLoaded[i]]++;
                }
                i++;
                break;
            }
        }
    }

    private void OnSceneLoaded(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneEventType == SceneEventType.Load)
            loadedScenes.Add(sceneEvent.Scene);
        else if (sceneEvent.SceneEventType == SceneEventType.Unload)
            loadedScenes.Remove(sceneEvent.Scene);
    }

    private IEnumerator UnloadScenes()
    {
        int i = 0;

        foreach (var sc in loadedScenes)
        {
            if (activeMaps[sc.name] == 0)
            {
                while (true)
                {
                    var state = NetworkManager.Singleton.SceneManager.UnloadScene(sc);
                    if (state == SceneEventProgressStatus.SceneEventInProgress)
                    {
                        yield return null;
                    }
                    else if (state == SceneEventProgressStatus.Started)
                    {
                        activeMaps[sc.name]--;
                    }
                    break;
                }
            }

            NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneLoaded;
        }
    }

    public void Reset()
    {
        chunkText = GetComponentInChildren<TMP_Text>();
        ensureLoaded.Clear();
        string sceneName = SceneManager.GetActiveScene().name;
        nodeName = sceneName;
        chunkText.text = sceneName;
        int x = sceneName[0] - '0';
        int y = sceneName[2] - '0';
        int xt;
        int yt;

        ensureLoaded.Add(sceneName);

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                xt = x + i;
                yt = y + j;
                string addingName = xt + "_" + yt;

                if (xt >= 0 && yt >= 0 && addingName != sceneName)
                {
                    foreach (var sc in EditorBuildSettings.scenes)
                    {
                        var buildedName = sc.path.Split('/')[2].Split('.')[0];
                        if (buildedName == addingName)
                        {
                            ensureLoaded.Add(addingName);
                            break;
                        }
                    }
                }
            }
        }
    }
}

