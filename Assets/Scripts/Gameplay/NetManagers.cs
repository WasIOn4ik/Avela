using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class NetManagers : NetworkBehaviour
{
    public WorldCompositor compositor;
    public SemiNetworkManager semiNetwork;
    [Inject] public GameplayBase gameBase;

    public void Awake()
    {
        ProjectContext.Instance.Container.InjectGameObject(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //Создание контроллера для хоста
        if (IsHost)
        {
            PlayerController controller = gameBase.server.SpawnPlayerCOntroller(OwnerClientId);
        }
    }
}
