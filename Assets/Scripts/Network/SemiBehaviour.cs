using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
[DefaultExecutionOrder(10)]
public class SemiBehaviour : MonoBehaviour
{
    [MenuItem("Toolkit/Duplicate %d")]
    static void DuplicateGameObject()
    {
        if (Selection.activeGameObject.GetComponent<SemiBehaviour>())
        {
            Object prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(Selection.activeGameObject);
            GameObject duplicate = null;

            if (prefabRoot != null)
            {
                duplicate = PrefabUtility.InstantiatePrefab(prefabRoot, Selection.activeGameObject.scene) as GameObject;
            }
            else
            {
                //Duplicate object
                duplicate = Instantiate(Selection.activeGameObject,
                Selection.activeGameObject.transform.position,
                Selection.activeGameObject.transform.rotation,
                Selection.activeGameObject.transform.parent);
            }

            //Move directly underneath original
            duplicate.transform.SetSiblingIndex(
            Selection.activeGameObject.transform.GetSiblingIndex() + 1);

            //Rename and increment
            duplicate.name = IncrementName(Selection.activeGameObject.name);

            //Select new object
            Selection.activeGameObject = duplicate;

            duplicate.gameObject.SendMessage("ForceReset");
            //Register Undo
            Undo.RegisterCreatedObjectUndo(duplicate, "Duplicated GameObject");
        }
        else
        {
            //Don't break default behaviour elsewhere
            EditorApplication.ExecuteMenuItem("Edit/Duplicate");
        }
    }

    private static string IncrementName(string input)
    {
        Stack<char> stack = new Stack<char>();
        int length = input.Length;
        bool isNum;

        for (var i = length - 1; i >= 0; i--)
        {
            isNum = char.IsNumber(input[i]);

            if (!isNum && i != length - 1)
            {
                break;
            }

            if (isNum) stack.Push(input[i]);
        }

        string result = new string(stack.ToArray());

        if (result.Length <= 0)
        {
            return input + " 02";
        }

        int num = int.Parse(result);

        result = input.Replace(num.ToString(), (num + 1).ToString());

        return result;
    }

    protected SemiNetworkManager semiNetworkManager;

    [SerializeField, ReadOnly] public ulong semiID;

    [SerializeField] [HideInInspector] private bool bInit = false;

    [SerializeField] public string className;

    protected void Awake()
    {
        semiNetworkManager = SemiNetworkManager.instance;
        CustomReset();
        Debug.Log("Awake");
    }

    protected void CustomReset()
    {
        if (Application.isEditor)
        {
            if (!bInit)
            {
                ForceReset();
            }
            bInit = true;
        }
    }

    protected void ForceReset()
    {
        semiID = SemiNetworkedConfigSO.Instance.AssignNextID();
        SemiNetworkedConfigSO.Instance.RegisterInstanceToSceneProxy(this);
    }

    public void Spawn()
    {
        if (!semiNetworkManager.NetworkManager.IsServer)
        {
            Debug.LogError("Попытка спавна полусетевого объекта на клиенте");
        }
    }

    protected void OnDestroy()
    {
        if (Application.isEditor)
        {
            if (gameObject.scene.isLoaded)
            {
                SemiNetworkedConfigSO.Instance.FreeID(semiID);
                SemiNetworkedConfigSO.Instance.UnregisterInstanceFromSceneProxy(this);
            }
        }
    }
}
