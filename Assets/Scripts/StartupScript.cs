using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StartupScript : MonoBehaviour
{
    [SerializeField] protected string sceneToLoad;
    public void Start()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
