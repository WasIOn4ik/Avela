using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ClientBase : MonoBehaviour
{
    public Camera mainCamera;
    [Inject] public DiContainer diContainer;

    public static ClientBase instance;

    public void Init()
    {

    }

    public void StartClient()
    {

    }
}
