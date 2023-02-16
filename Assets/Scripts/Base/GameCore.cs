using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings
{
    public string playerName = "";
}

public class GameCore : MonoBehaviour
{
    [SerializeField] protected PlayerSettings playerSettings = new();
    [SerializeField] protected CharactersLibrary charactersLib;
    [SerializeField] protected LocalizationLibrary localizationLib;

    public PlayerSettings Settings { get { return playerSettings; } }

    public CharactersLibrary Characters { get { return charactersLib; } }

    public LocalizationLibrary Localization { get { return localizationLib; } }

}
