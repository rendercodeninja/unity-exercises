using System.Collections.Generic;
using UnityEngine;

namespace CombatEngine.Schemas
{
    [CreateAssetMenu(fileName = "SOCharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
    public class SOCharacterData : ScriptableObject
    {

        [System.Serializable]
        public class CharacterData
        {
            public string characterName;
            public Sprite profileIcon;
        }

        public List<CharacterData> characterDataSet;
    }
}
