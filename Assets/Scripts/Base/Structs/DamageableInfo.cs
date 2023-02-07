using System;
using UnityEngine;

public enum EDamageSubject
{
    Player,
    Trap,
    AIControlled,
    World
}


[Serializable]
public struct DamageableInfo
{
    public string title;
    public MonoBehaviour component;
    public float maxHP;
    public float currentHP;
    public EDamageSubject receiverType;
}
