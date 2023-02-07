using UnityEngine;
using Zenject;

public class WorldCompositorInstaller : MonoInstaller
{
    [SerializeField] private WorldCompositor worldCompositorPrefab;
    public override void InstallBindings()
    {
        Container.Bind<WorldCompositor>().FromComponentsInNewPrefab(worldCompositorPrefab).AsSingle().NonLazy();
        Container.QueueForInject(worldCompositorPrefab);
    }
}