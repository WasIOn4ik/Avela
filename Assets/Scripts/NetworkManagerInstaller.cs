using Unity.Netcode;
using UnityEngine;
using Zenject;

public class NetworkManagerInstaller : MonoInstaller
{
    public NetworkManager prefab;

    public override void InstallBindings()
    {
        Container.Bind<NetworkManager>().FromComponentsInNewPrefab(prefab).AsSingle().NonLazy();
    }
}