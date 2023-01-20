using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneUtility : MonoBehaviour
{
    [MenuItem("Spes/Recalculate Scenes")]
    public static void RecalculateScenes()
    {
        var activeScene = EditorSceneManager.GetActiveScene();
        var sc = AssetDatabase.FindAssets("t: SceneAsset");
        Debug.Log(sc.Length);
        var sp = new List<string>();
        foreach (var s in sc)
        {
            var path = AssetDatabase.GUIDToAssetPath(s);
            //Debug.Log(p);
            if (path.StartsWith("Assets/Scenes/"))
            {
                //var loadedScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(p);
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                var obj = FindObjectOfType<ChunkNode>();
                var assetName = path.Split('/');
                var sceneName = assetName[assetName.GetUpperBound(0)].Split('.')[0];
                sp.Add(sceneName);
                if (obj)
                {
                    obj.Reset();
                    EditorUtility.SetDirty(obj);
                    Debug.Log($"Объект {obj.name} в сцене {sceneName} сброшен");
                }
                Debug.Log("Результат: " + (EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), path) ? "Успешно" : "С ошибкой"));
                //EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        }
        //EditorSceneManager.OpenScene(activeScene.path);

    }
}
