using UnityEngine;
using Zenject;

public class GameCoreInstaller : MonoInstaller
{
    public GameCore core;

    public override void InstallBindings()
    {
    }

    public void InstallBindingsPostStart()
    {
        Container.Bind<GameCore>().FromInstance(core).AsSingle().NonLazy();
    }
}