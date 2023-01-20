using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[DisallowMultipleComponent]
public class WorldObjectHandler : NetworkBehaviour
{
    protected NetworkObject nobj;
    public List<ulong> hide;
    // Start is called before the first frame update
    void Awake()
    {
        nobj = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
            return;

        foreach (var c in hide)
        {
            nobj.NetworkHide(c);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
