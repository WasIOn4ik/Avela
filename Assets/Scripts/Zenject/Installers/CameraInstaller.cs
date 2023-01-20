using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    [SerializeField] private Camera cameraInstance;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(cameraInstance).AsSingle().NonLazy();
    }
}