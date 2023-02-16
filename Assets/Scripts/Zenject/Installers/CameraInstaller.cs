using System;
using UnityEngine;
using Zenject;

[Serializable]
public class CameraInstaller : MonoInstaller
{
    public Camera cameraInstance;

    public override void InstallBindings()
    {
    }

    public void InstallBindingsPostStart()
    {
        Container.Bind<Camera>().FromInstance(cameraInstance).AsSingle().NonLazy();
    }
}