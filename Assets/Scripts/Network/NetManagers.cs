using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetManagers : NetworkBehaviour
{
    public WorldCompositor compositor;
    public SemiNetworkManager semiNetwork;
    //public GameBase

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //if(IsClient)
            //GameBase
    }
}
