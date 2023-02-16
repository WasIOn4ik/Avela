using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public struct ArtifactStatAvailability
{
    public ECharacterStatType stat;
    public float chance;
}

public class ArtifactSet
{
    public ArtifactSet(string tit, LocalizedString lTit, LocalizedString lDesc)
    {
        title = tit;
        localizedTitle = lTit;
        localizedDescription = lDesc;
    }

    public string title;
    public LocalizedString localizedTitle;
    public LocalizedString localizedDescription;

    public virtual void ApplyTwoPartsEffect()
    {
        throw new NotImplementedException();
    }

    public virtual void ApplyFourPartsEffect()
    {
        throw new NotImplementedException();
    }

    public virtual void ApplyMarkEffect()
    {
        throw new NotImplementedException();
    }

    public virtual void Initialize(PlayerCharacter character)
    {
        throw new NotImplementedException();
    }
}

[CreateAssetMenu(menuName = "Spes/ItemsLibrary", fileName = "itemsLibrary")]
public class ItemsLibrary : ScriptableObject
{
    public List<ItemWeapon> weaponsLibrary;

    public List<ItemBase> upgradeLibrary;

    public List<ItemArtifact> artifactsLibrary;

    public List<ItemBase> craftsLibrary;

    public List<ItemMission> missionItemsLibrary;

    [NonSerialized] List<ArtifactSet> sets;
}
