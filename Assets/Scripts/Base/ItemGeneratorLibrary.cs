using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StatProcs
{
    public ECharacterStatType stat;
    public List<float> procs;
    public float mainStatBase;
    public float mainStatIncrement;
}

[Serializable]
public class EquipmentPartAvailableStats
{
    public EArtifactPart part;
    public List<StatProcs> availableStats;
}

[Serializable]
public class SetPartsInfo
{
    public string setName;
    public List<EquipmentPartAvailableStats> statsInfo;
}

[CreateAssetMenu(menuName = "Spes/ItemGeneratorLibrary", fileName = "ItemGeneratorLibrary")]
public class ItemGeneratorLibrary : ScriptableObject
{
    public SetPartsInfo defaultSetInfo;

    public List<SetPartsInfo> partsStats;
}
