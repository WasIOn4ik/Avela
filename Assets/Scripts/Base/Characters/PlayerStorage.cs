using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Localization;

public class CharacterStorage
{
    public string name;
    public bool bUnlocked;
    public int level;
    public int duplicates;
    public int upgrade;
    public int missionsCompleted;
}

public enum ItemCategory
{
    Weapon,
    Upgrade,
    Artifact,
    Craft,
    Mission
}

public class Inventory
{
    public const int WeaponArraySize = 1000;
    public const int UpgradeArraySize = 500;
    public const int ArtifactArraySize = 2000;
    public const int CraftArraySize = 500;
    public const int MissionArraySize = 1000;
    ItemWeapon[] weapons = new ItemWeapon[WeaponArraySize];
    ItemBase[] upgrade = new ItemBase[UpgradeArraySize];
    ItemArtifact[] artifacts = new ItemArtifact[CraftArraySize];
    ItemBase[] crafts = new ItemBase[CraftArraySize];
    ItemMission[] missionItems = new ItemMission[MissionArraySize];
}

[Serializable]
public class ItemBase
{
    public uint itemID;
    public Texture2D icon;
    public LocalizedString title;
    public LocalizedString description;
    [NonSerialized] public uint ingameID;
    [NonSerialized] public Inventory inventoryRef;
    [NonSerialized] public int count;

    public virtual void OnClicked()
    {

    }
}

[Serializable]
public struct SerializedItem
{
    public uint itemID;
    public uint ingameID;
    public uint count;
}

[Serializable]
public struct SerializedWeapon
{
    public uint itemID;
    public uint ingameID;
    public byte level;
    public byte consts;
}

[Serializable]
public struct ArtifactStat
{
    public ECharacterStatType stat;
    public float value;
}

[Serializable]
public struct SerializedArtifact
{
    public uint itemID;
    public uint ingameID;
    public byte level;

    public ArtifactStat[] stats;
}

[Serializable]
public class ItemWeapon : ItemBase
{

}

[Serializable]
public class ItemArtifact : ItemBase
{

}

[Serializable]
public class ItemMission : ItemBase
{

}

public class PlayerStorage : NetworkBehaviour
{
    #region Variables

    public delegate void OnLoadedDelegate(ulong clientID);
    public event OnLoadedDelegate OnLoaded;

    public List<CharacterStorage> characters;

    #endregion

    #region UnityCallbacks

    #endregion

    #region RPCs
    /*
        /// <summary>
        /// Синхронизация upgrades, crafts, missions
        /// </summary>
        /// <param name="cat">Категория</param>
        /// <param name="items">Предметы</param>
        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        public void SendInventoryClientRpc(ItemCategory cat, SerializedItem[] items)
        {

        }

        [ClientRpc(Delivery = RpcDelivery.Reliable)]
        public void SendWeaponsClientRpc(SerializedWeapon[] weapons)
        {

        }
    */
    #endregion

    #region Functions

    protected bool LoadData()
    {
        return true;
    }

    protected bool SaveData()
    {
        return true;
    }
    #endregion
}
