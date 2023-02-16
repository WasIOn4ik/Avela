using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameBaseInstaller : MonoInstaller
{
    [SerializeField] private GameplayBase gameBasePrefab;


    public override void InstallBindings()
    {
        Container.Bind<GameplayBase>().FromComponentsInNewPrefab(gameBasePrefab).AsSingle().NonLazy();
        //Container.InstantiatePrefabForComponent<GameBase>(gameBasePrefab, Vector3.zero, Quaternion.identity, null);//
    }
}
