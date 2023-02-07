using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameBaseInstaller : MonoInstaller
{
    [SerializeField] private GameBase gameBasePrefab;


    public override void InstallBindings()
    {
        Debug.Log("1111");
        Container.Bind<GameBase>().FromComponentsInNewPrefab(gameBasePrefab).AsSingle().NonLazy();
        //Container.InstantiatePrefabForComponent<GameBase>(gameBasePrefab, Vector3.zero, Quaternion.identity, null);//
    }
}
