using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Spes.UI;

public class PlayerController : NetworkBehaviour, IDamageable
{
    [Inject] public Camera mainCameraRef;
    [Inject] public WorldCompositor compositor;
    protected Vector3 moveDirection;
    [SerializeField] protected SceneSwitchController sceneSwither;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected Transform cameraSocket;
    [SerializeField] protected PlayerUpheadUI upheadUI;

    protected PlayerStats playerStats;

    NetworkVariable<string> playerName = new NetworkVariable<string>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void Awake()
    {
        ClientBase.instance.diContainer.InjectGameObject(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
            sceneSwither.InitAsServer();

        SetupComponents();
    }

    private void SetupComponents()
    {
        if (IsOwner)
        {
            mainCameraRef.transform.parent = cameraSocket;
            mainCameraRef.transform.localPosition = Vector3.zero;
            mainCameraRef.transform.localRotation = Quaternion.identity;
            upheadUI.gameObject.SetActive(false);
            return;
        }
        upheadUI.Init(this, 1.0f);
    }

    public void OnMovement(InputValue value)
    {
        var vec = value.Get<Vector2>();
        moveDirection = Vector3.zero;
        moveDirection.x += vec.x;
        moveDirection.z += vec.y;
    }

    public void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    #region IDamageable

    public DamageableInfo GetInfo()
    {
        return new DamageableInfo() { component = this, currentHP = playerStats.HP, maxHP = playerStats.MaxHP, title = playerName.Value, receiverType = EDamageSubject.Player };
    }

    public void ApplyDamage(Damage dmg)
    {
        playerStats.HP -= dmg.damage;
    }

    #endregion
}
