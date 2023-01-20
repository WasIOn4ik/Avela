using UnityEngine;
using Zenject;

public class WorldCompositorInstaller : MonoInstaller
{
    [SerializeField] private WorldCompositor worldCompositor;
    public override void InstallBindings()
    {
        Container.Bind<WorldCompositor>().FromInstance(worldCompositor).AsSingle();
        Container.QueueForInject(worldCompositor);
    }
}