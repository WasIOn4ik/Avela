using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Unique
}

[Serializable]
public class CharacterDescriptor
{
    public CharacterBaseStatsContainer characterStats;

    public List<CharacterStat> Levels0_30;

    public List<CharacterStat> Levels31_50;

    public List<CharacterStat> CharacterMission;

    public List<CharacterStat> Levels51_100;

    public uint CommonResourceID;
    public uint UncommonResourceID;
    public uint RareResourceID;
    public uint EpicResourceID;

    public uint CommonMaterialID;
    public uint UncommonMaterialID;
    public uint RareMaterialID;
    public uint EpicMaterialID;

    public AssetReference playerAssetReference;
}

public class CharacterState
{
    public delegate void LevelChangedDelegate(int level);
    public event LevelChangedDelegate OnLevelChanged;

    public int level;

    public int consts;


}
