using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(menuName = "Spes/LocalizationLibrary", fileName = "LocalizationLibrary")]
public class LocalizationLibrary : ScriptableObject
{
    [Serializable]
    internal struct localizedStat
    {
        public ECharacterStatType stat;
        public LocalizedString localizedString;
    }

    [SerializeField] internal List<localizedStat> localizedStats;

    public string LocalizedStatTitle(ECharacterStatType stat)
    {
        return localizedStats.Find(x => x.stat == stat).localizedString.GetLocalizedString();
    }
    public AsyncOperationHandle<string> LocalizedStatTitleAsync(ECharacterStatType stat)
    {
        return localizedStats.Find(x => x.stat == stat).localizedString.GetLocalizedStringAsync();
    }
}
