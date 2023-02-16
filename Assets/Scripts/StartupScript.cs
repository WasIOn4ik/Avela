using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

public class StartupScript : MonoBehaviour
{
    [SerializeField] protected string sceneToLoad;
    public void Start()
    {
        Addressables.InitializeAsync().WaitForCompletion();
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
