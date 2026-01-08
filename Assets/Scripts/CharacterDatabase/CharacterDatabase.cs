using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="CharacterDatabase",menuName ="Character/Database" )]
public class CharacterDatabase : ScriptableObject
{
    public CharacterOption[] characters;

    public CharacterOption GetCharacterByName(string genderName)
    {
        foreach (var c in characters)
        {
            if (c.genderName == genderName)
            {
                return c;
            }

        }
            return null;
    }
}
