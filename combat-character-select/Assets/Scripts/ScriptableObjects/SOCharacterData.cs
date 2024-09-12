using System.Collections.Generic;
using UnityEngine;

namespace CombatEngine.Schemas
{
    [CreateAssetMenu(fileName = "SOCharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
    public class SOCharacterData : ScriptableObject
    {
        //Fixed dimension grid
        public readonly int rowCount = 2;
        public readonly int columnCount = 12;

        [System.Serializable]
        public class CharacterData
        {
            public string characterName;
            public Sprite profileIcon;
        }

        //List holding character data for Row0
        public List<CharacterData> row0;
        //List holding character data for Row1
        public List<CharacterData> row1;

        //Get the row object by index
        public List<CharacterData> GetRowByIndex(int index)
        {
            return index switch
            {
                0 => row0,
                1 => row1,
                _ => null,
            };
        }
    }
}
