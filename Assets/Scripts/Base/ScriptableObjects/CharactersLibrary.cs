using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spes/CharactersLibrary")]
public class CharactersLibrary : ScriptableObject
{
    public List<CharacterDescriptor> characters;
}
