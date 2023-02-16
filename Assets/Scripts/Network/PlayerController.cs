using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Spes.UI;
using UnityEngine.AddressableAssets;
using Unity.Collections;

public static class PlayerControllerSerializationHelper
{
    public static void ReadValueSafe(this FastBufferReader reader, out string asset)
    {
        reader.ReadValueSafe(out string readed);
        asset = readed;
    }

    public static void WriteValueSafe(this FastBufferWriter writer, in string asset)
    {
        writer.WriteValueSafe(asset);
    }

    public static void SerializeValue<T>(this BufferSerializer<T> reader, ref string assetReference) where T : IReaderWriter
    {
        if (reader.IsReader)
        {
            reader.GetFastBufferReader().ReadValueSafe(out string guid);
            assetReference = guid;
        }
        else
        {
            reader.GetFastBufferWriter().WriteValueSafe(assetReference);
        }
    }
}

public class PlayerController : NetworkBehaviour, IDamageable
{
    #region Variables

    [Inject] public Camera mainCameraRef;
    [Inject] protected GameCore gameInstance;

    [Header("Components")]
    [SerializeField] protected PlayerStorage storage;
    [SerializeField] protected SceneSwitchController sceneSwither;
    [SerializeField] protected Transform cameraSocket;
    [SerializeField] protected PlayerUpheadUI upheadUI;

    [Header("Properties")]
    [SerializeField] protected float speed = 2f;

    [Header("Runtime references")]
    [ReadOnly] public ServerBase serverRef;
    [ReadOnly] public WorldCompositor compositor;

    protected Vector3 moveDirection;

    public static PlayerController local;

    NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    #endregion

    #region UnityCallbacks

    public void Awake()
    {
        ProjectContext.Instance.Container.InjectGameObject(gameObject);
    }

    #endregion

    #region NetcodeCallbacks

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            sceneSwither.InitAsServer();
        }
        if (IsClient)
        {
            if (IsOwner)
                local = this;
            name = IsOwner ? "LocalPlayerController_" + OwnerClientId : "PlayerController_" + OwnerClientId;
        }

        SetupComponents();
        if (IsServer)
            S_CreatePlayerStorage();
    }

    #endregion

    #region Functions

    public void SetCompositor(WorldCompositor comp)
    {
        compositor = comp;
        sceneSwither.SetCompositor(comp);
    }

    protected void S_SpawnPlayerCharacter()
    {
        var asset = gameInstance.Characters.characters[0].playerAssetReference;
        serverRef.assetManager.CallPrefabLoad(asset, () =>
        {
            asset.InstantiateAsync().Completed += y =>
            {
                y.Result.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            };
        }, OwnerClientId);
    }

    protected void S_CreatePlayerStorage()
    {
        //Создание и исинхронизация состояния игрока
        S_SpawnPlayerCharacter();
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

    #endregion

    #region IDamageable

    public DamageableInfo GetInfo()
    {
        return new DamageableInfo();//{ component = this, currentHP = playerStats.HP, maxHP = playerStats.MaxHP, title = playerName.Value.Value, receiverType = EDamageSubject.Player };
    }

    public void ApplyDamage(Damage dmg)
    {
        //playerStats.HP -= dmg.damage;
    }

    #endregion
}
