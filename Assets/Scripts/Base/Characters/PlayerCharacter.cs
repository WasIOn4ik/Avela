using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

using CST = ECharacterStatType;

public enum ECharacterStatType
{
    /// <summary>
    /// Здоровье персонажа  результат
    /// </summary>
    HP,
    /// <summary>
    /// Здоровье персонажа  0.01%
    /// </summary>
    HPPercent,
    /// <summary>
    /// Здоровье персонажа  
    /// </summary>
    HPFlat,

    /// <summary>
    ///  Мана персонажа (используется для навыков)  результат
    /// </summary>
    MP,
    /// <summary>
    ///  Мана персонажа (используется для навыков)  0.01%
    /// </summary>
    MPPercent,
    /// <summary>
    ///  Мана персонажа (используется для навыков)  
    /// </summary>
    MPFlat,

    /// <summary>
    /// Сила атаки персонажа  результат
    /// </summary>
    ATK,
    /// <summary>
    /// Сила атаки персонажа  0.01%
    /// </summary>
    ATKPercent,
    /// <summary>
    /// Сила атаки персонажа 
    /// </summary>
    ATKFlat,

    /// <summary>
    /// Защита персонажа  результат
    /// </summary>
    Protection,
    /// <summary>
    /// Защита персонажа  0.01%
    /// </summary>
    ProtectionPercent,
    /// <summary>
    /// Защита персонажа 
    /// </summary>
    ProtectionFlat,

    /// <summary>
    /// Эффективность персонажа, повышает общий урон, коэффициенты пробития и статусов 
    /// </summary>
    Efficiency,
    /// <summary>
    /// Количество выносливости персонажа, используется для ударов и блоков 
    /// </summary>
    Energy,

    /// <summary>
    /// Восстановление энергии 
    /// </summary>
    EnergyRecovery,
    /// <summary>
    /// Восстановление маны 
    /// </summary>
    MPRecovery,
    /// <summary>
    /// Восстановление здоровья 
    /// </summary>
    HPRecovery,
    /// <summary>
    /// Восстановление способностей 
    /// </summary>
    AbilityRecovery,
    /// <summary>
    /// Сопротивление магии, статусам, ядам 
    /// </summary>

    Resistance,
    /// <summary>
    /// Ловкость, повышает скорость действий в открытом мире, а также движения и атаки 
    /// </summary>
    Dexterity,
    /// <summary>
    /// Множитель критического урона 
    /// </summary>
    CriticalDamage,
    /// <summary>
    /// Множитель шанса крита 
    /// </summary>
    CriticalChance,
    /// <summary>
    /// Прочность щита 
    /// </summary>
    ShieldDurability
}

public enum EArtifactPart
{
    Ring,
    EarRing,
    Brooch,
    Mark,
    Necklace
}

public struct SeparatedStats
{
    public float baseHP;
    public float addictionHP;
    public float HP;

    public float baseMP;
    public float addictionMP;
    public float MP;

    public float baseATK;
    public float addictionATK;
    public float ATK;

    public float baseProtection;
    public float addictionProtection;
    public float Protection;

    public float Efficiency;

    public float Energy;

    public float EnergyRecovery;

    public float HPRecovery;

    public float MPRecovery;

    public float AbilityRecovery;

    public float Resistance;

    public float Dexterity;

    public float CritDamage;

    public float CritChance;

    public float shieldDurability;

    public void AddStatValue(CST type, float value)
    {
        switch (type)
        {
            case CST.HPFlat:
                baseHP += value;
                break;
            case CST.MPFlat:
                baseMP += value;
                break;
            case CST.ATKFlat:
                baseATK += value;
                break;
            case CST.ProtectionFlat:
                baseProtection += value;
                break;
            case CST.Efficiency:
                Efficiency += value;
                break;
            case CST.Energy:
                Energy += value;
                break;
            case CST.EnergyRecovery:
                Energy += value;
                break;
            case CST.MPRecovery:
                MPRecovery += value;
                break;
            case CST.HPRecovery:
                HPRecovery += value;
                break;
            case CST.AbilityRecovery:
                AbilityRecovery += value;
                break;
            case CST.Resistance:
                Resistance += value;
                break;
            case CST.Dexterity:
                Dexterity += value;
                break;
            case CST.CriticalDamage:
                CritDamage += value;
                break;
            case CST.CriticalChance:
                CritChance += value;
                break;
            case CST.ShieldDurability:
                shieldDurability += value;
                break;
        }
    }
}

public struct DetailedSeparatedStats
{
    public float baseHP;
    public float equipmentHP;
    public float addictionHP;
    public float resultHP;

    public float baseMP;
    public float equipmentMP;
    public float addictionMP;
    public float resultMP;

    public float baseATK;
    public float equipmentATK;
    public float addictionATK;
    public float resultATK;

    public float baseProtection;
    public float equipmentProtection;
    public float addictionProtection;
    public float resultProtection;

    public float baseEfficiency;
    public float equipmentEfficiency;
    public float addictionEfficiency;
    public float resultEfficiency;

    public float baseEnergy;
    public float equipmentEnergy;
    public float addictionEnergy;
    public float resultEnergy;

    public float baseEnergyRecovery;
    public float equipmentEnergyRecovery;
    public float addictionEnergyRecovery;
    public float resultEnergyRecovery;

    public float baseHPRecovery;
    public float equipmentHPRecovery;
    public float addictionHPRecovery;
    public float resultHPRecovery;

    public float baseMPRecovery;
    public float equipmentMPRecovery;
    public float addictionMPRecovery;
    public float resultMPRecovery;

    public float baseAbilityRecovery;
    public float equipmentAbilityRecovery;
    public float addictionAbiliyRecovery;
    public float resultAbilityRecovery;

    public float baseResistance;
    public float equipmentResistance;
    public float addictionResistance;
    public float resultResistance;

    public float baseDexterity;
    public float equipmentDexterity;
    public float addictionDexterity;
    public float resultDexterity;

    public float baseCritDamage;
    public float equipmentCritDamage;
    public float addictionCritDamage;
    public float resultCritDamage;

    public float baseCritChance;
    public float equipmentCritChance;
    public float addictionCritChance;
    public float resultCritChance;

    public float baseShieldDurability;
    public float equipmentShieldDurability;
    public float addictionShieldDurability;
    public float resultShieldDurability;

    public void SetSCharacterValue(CST type, float value)
    {
        switch (type)
        {
            case CST.HPFlat:
                baseHP = value;
                break;
            case CST.MPFlat:
                baseMP = value;
                break;
            case CST.ATKFlat:
                baseATK = value;
                break;
            case CST.ProtectionFlat:
                baseProtection = value;
                break;
            case CST.Efficiency:
                baseEfficiency = value;
                break;
            case CST.Energy:
                baseEnergy = value;
                break;
            case CST.EnergyRecovery:
                baseEnergyRecovery = value;
                break;
            case CST.MPRecovery:
                baseMPRecovery = value;
                break;
            case CST.HPRecovery:
                baseHPRecovery = value;
                break;
            case CST.AbilityRecovery:
                baseAbilityRecovery = value;
                break;
            case CST.Resistance:
                baseResistance = value;
                break;
            case CST.Dexterity:
                baseDexterity = value;
                break;
            case CST.CriticalDamage:
                baseCritDamage = value;
                break;
            case CST.CriticalChance:
                baseCritChance = value;
                break;
            case CST.ShieldDurability:
                baseShieldDurability = value;
                break;
        }
    }
    public void AddEquipmentValue(CST type, float value)
    {
        switch (type)
        {
            case CST.HPFlat:
                equipmentHP += value;
                break;
            case CST.MPFlat:
                equipmentMP += value;
                break;
            case CST.ATKFlat:
                equipmentATK += value;
                break;
            case CST.ProtectionFlat:
                equipmentProtection += value;
                break;
            case CST.Efficiency:
                equipmentEfficiency += value;
                break;
            case CST.Energy:
                equipmentEnergy += value;
                break;
            case CST.EnergyRecovery:
                equipmentEnergyRecovery = value;
                break;
            case CST.MPRecovery:
                equipmentMPRecovery = value;
                break;
            case CST.HPRecovery:
                equipmentHPRecovery = value;
                break;
            case CST.AbilityRecovery:
                equipmentAbilityRecovery = value;
                break;
            case CST.Resistance:
                equipmentResistance = value;
                break;
            case CST.Dexterity:
                equipmentDexterity = value;
                break;
            case CST.CriticalDamage:
                equipmentCritDamage = value;
                break;
            case CST.CriticalChance:
                equipmentCritChance = value;
                break;
            case CST.ShieldDurability:
                equipmentShieldDurability = value;
                break;
        }
    }

    public void CalculateResults()
    {
        resultEfficiency = baseEfficiency + equipmentEfficiency + addictionEfficiency;
        resultEnergy = baseEnergy + equipmentEnergy + addictionEnergy;
        resultEnergyRecovery = baseEnergyRecovery + equipmentEnergyRecovery + addictionEnergyRecovery;
        resultHPRecovery = baseHPRecovery + equipmentEnergyRecovery + addictionEnergyRecovery;
        resultMPRecovery = baseMPRecovery + equipmentMPRecovery + addictionMPRecovery;
        resultHPRecovery = baseHPRecovery + equipmentHPRecovery + addictionHPRecovery;
        resultAbilityRecovery = baseAbilityRecovery + equipmentAbilityRecovery + addictionAbiliyRecovery;
        resultResistance = baseResistance + equipmentResistance + addictionResistance;
        resultDexterity = baseDexterity + equipmentDexterity + addictionDexterity;
        resultCritDamage = baseCritDamage + equipmentCritDamage + addictionCritDamage;
        resultCritChance = baseCritChance + equipmentCritChance + addictionCritChance;
        resultShieldDurability = baseShieldDurability + equipmentShieldDurability + addictionShieldDurability;
    }
}

[Serializable]
public class CharacterStat
{
    public CharacterStat(ECharacterStatType type, float val)
    {
        stat = type;
        value = val;
    }
    public ECharacterStatType stat;
    public float value;

    public static CharacterStat operator *(CharacterStat stat, int val)
    {
        CharacterStat res = new CharacterStat(stat.stat, stat.value);
        res.value *= val;

        return res;
    }
}

[Serializable]
public abstract class StatsContainer
{
    [SerializeField] protected List<CharacterStat> stats;

    public abstract void ApplyToManager(CharacterStatsComponent manager);

    public abstract void DenyFromManager(CharacterStatsComponent manager);

    public IEnumerable<CharacterStat> GetStats()
    {
        return stats;
    }

    public StatsContainer AddStat(CharacterStat stat)
    {
        stats.Add(stat);
        return this;
    }

    public StatsContainer AddStat(CST newStat, float newValue)
    {
        stats.Add(new CharacterStat(newStat, newValue));
        return this;
    }

    public float GetValue(CST type)
    {
        var res = stats.Find(x => { return x.stat == type; });
        return res == null ? 0 : res.value;
    }
}

[Serializable]
public class CharacterBaseStatsContainer : StatsContainer
{
    public CharacterBaseStatsContainer() : base()
    {
        stats = new()
        {
            new CharacterStat(CST.HPFlat, 0f),
            new CharacterStat(CST.MPFlat, 0f),
            new CharacterStat(CST.ATKFlat, 0f),
            new CharacterStat(CST.ProtectionFlat, 0f),
            new CharacterStat(CST.Efficiency, 0f),
            new CharacterStat(CST.Energy, 0f),
            new CharacterStat(CST.EnergyRecovery, 0f),
            new CharacterStat(CST.HPRecovery, 0f),
            new CharacterStat(CST.MPRecovery, 0f),
            new CharacterStat(CST.AbilityRecovery, 0f),
            new CharacterStat(CST.Resistance, 0f),
            new CharacterStat(CST.Dexterity, 0f),
            new CharacterStat(CST.CriticalDamage, 0f),
            new CharacterStat(CST.CriticalChance, 0f),
            new CharacterStat(CST.ShieldDurability, 0f)
        };
    }

    public override void ApplyToManager(CharacterStatsComponent manager)
    {
        manager.BaseStats = this;
    }

    public override void DenyFromManager(CharacterStatsComponent manager)
    {
        throw new System.FieldAccessException("CharacterBaseStatsContainer не может быть убран из контроллера статов");
    }
}

public class EffectStatsContainer : StatsContainer
{
    public string effectName;

    public override void ApplyToManager(CharacterStatsComponent manager)
    {
        manager.ApplyEffect(this);
    }

    public override void DenyFromManager(CharacterStatsComponent manager)
    {
        manager.DenyEffect(this);
    }
}

[Serializable]
public struct CharacterUpgradeStats
{
    List<CharacterStat> stats;
}

public class CharacterStatsComponent
{
    public delegate void UpdateStatsDelegate();
    public event UpdateStatsDelegate onStatsChanged;

    protected Dictionary<CST, float> resultStats = new()
    {
        { CST.HP, 0f },
        { CST.HPFlat, 0f },
        { CST.HPPercent, 0f },

        { CST.MP, 0f },
        { CST.MPFlat, 0f },
        { CST.MPPercent, 0f },

        { CST.ATK, 0f },
        { CST.ATKFlat, 0f },
        { CST.ATKPercent, 0f },

        { CST.Protection, 0f },
        { CST.ProtectionFlat, 0f },
        { CST.ProtectionPercent, 0f },

        { CST.Efficiency, 0f },
        { CST.Energy, 0f },
        { CST.EnergyRecovery, 0f },
        { CST.HPRecovery, 0f },
        { CST.MPRecovery, 0f },
        { CST.AbilityRecovery, 0f },
        { CST.Resistance, 0f },
        { CST.Dexterity, 0f },
        { CST.CriticalDamage, 0f },
        { CST.CriticalChance, 0f },
        { CST.ShieldDurability, 0f }
    };

    protected StatsContainer baseCharacterStats;

    protected StatsContainer weaponStats;

    protected StatsContainer petStats;

    protected Dictionary<string, StatsContainer> effects;

    public StatsContainer BaseStats
    {
        get { return baseCharacterStats; }
        set
        {
            baseCharacterStats = value;
            RecalculateResultStats();
        }
    }

    public StatsContainer WeaponStats
    {
        get { return weaponStats; }
        set
        {
            weaponStats = value;
            RecalculateResultStats();
        }
    }

    public StatsContainer PetStats
    {
        get { return petStats; }
        set
        {
            petStats = value;
            RecalculateResultStats();
        }
    }

    public void ApplyEffect(StatsContainer effect, bool recalculateStats = true)
    {
        if (recalculateStats)
            RecalculateResultStats();
    }

    public void DenyEffect(StatsContainer effect, bool recalculateStats = true)
    {
        if (recalculateStats)
            RecalculateResultStats();
    }

    protected void RecalculateResultStats()
    {
        foreach (var stat in baseCharacterStats.GetStats())
        {
            resultStats[stat.stat] = stat.value;
        }

        foreach (var stat in weaponStats.GetStats())
        {
            resultStats[stat.stat] += stat.value;
        }

        if (petStats != null)
        {
            foreach (var stat in petStats.GetStats())
            {
                resultStats[stat.stat] += stat.value;
            }
        }

        foreach (var effect in effects.Values)
        {
            foreach (var stat in effect.GetStats())
            {
                resultStats[stat.stat] += stat.value;
            }
        }

        resultStats[CST.HP] = resultStats[CST.HPFlat] * resultStats[CST.HPPercent];
        resultStats[CST.MP] = resultStats[CST.MPFlat] * resultStats[CST.MPPercent];
        resultStats[CST.ATK] = resultStats[CST.ATKFlat] * resultStats[CST.ATKPercent];
        resultStats[CST.Protection] = resultStats[CST.ProtectionFlat] * resultStats[CST.ProtectionPercent];

        if (onStatsChanged != null)
            onStatsChanged();
    }

    public SeparatedStats GetSeparatedStats()
    {
        SeparatedStats res = new();
        foreach (var stat in resultStats)
        {
            res.AddStatValue(stat.Key, stat.Value);
        }

        res.addictionHP = resultStats[CST.HP] - resultStats[CST.HPFlat];
        res.addictionATK = resultStats[CST.ATK] - resultStats[CST.ATKFlat];
        res.addictionMP = resultStats[CST.MP] - resultStats[CST.MPFlat];
        res.addictionProtection = resultStats[CST.Protection] - resultStats[CST.ProtectionFlat];

        return res;
    }

    public DetailedSeparatedStats GetDetailed()
    {
        DetailedSeparatedStats res = new();
        foreach (var stat in baseCharacterStats.GetStats())
        {
            res.SetSCharacterValue(stat.stat, stat.value);
        }

        foreach (var stat in weaponStats.GetStats())
        {
            res.AddEquipmentValue(stat.stat, stat.value);
        }

        if (petStats != null)
            foreach (var stat in petStats.GetStats())
            {
                res.AddEquipmentValue(stat.stat, stat.value);
            }

        foreach (var effect in effects.Values)
        {
            foreach (var stat in effect.GetStats())
            {
                res.AddEquipmentValue(stat.stat, stat.value);
            }
        }

        res.addictionHP = resultStats[CST.HP] - weaponStats.GetValue(CST.HPFlat) - (petStats == null ? 0 : petStats.GetValue(CST.HPFlat));
        res.addictionHP = resultStats[CST.MP] - weaponStats.GetValue(CST.MPFlat) - (petStats == null ? 0 : petStats.GetValue(CST.MPFlat));
        res.addictionHP = resultStats[CST.ATK] - weaponStats.GetValue(CST.ATKFlat) - (petStats == null ? 0 : petStats.GetValue(CST.ATKFlat));
        res.addictionHP = resultStats[CST.Protection] - weaponStats.GetValue(CST.ProtectionFlat) - (petStats == null ? 0 : petStats.GetValue(CST.ProtectionFlat));

        res.CalculateResults();

        return res;
    }
}

public class PlayerCharacter : NetworkBehaviour
{
    #region Variables
    public static float speedCoef = 10f;

    public Vector3 moveDirection;
    public float speed = 1f;

    CharacterStatsComponent stats;

    #endregion

    #region UnityCallbacks

    public void Update()
    {
        transform.position += moveDirection * speed * speedCoef * Time.deltaTime;
    }

    #endregion

    #region Functions

    #endregion

    #region InputCallbacks

    public void OnMovement(InputValue value)
    {
        var vec = value.Get<Vector2>();
        moveDirection = Vector3.zero;
        moveDirection.x += vec.x;
        moveDirection.z += vec.y;
    }

    #endregion
}
