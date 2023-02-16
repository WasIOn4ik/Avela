using UnityEngine;
using Zenject;

public class WorldCompositorInstaller : MonoInstaller
{
    [SerializeField] private WorldCompositor compositorInstance;
    public override void InstallBindings()
    {
        Container.Bind<WorldCompositor>().FromInstance(compositorInstance).AsSingle().NonLazy();
        //Container.QueueForInject(compositorInstance);
    }
}