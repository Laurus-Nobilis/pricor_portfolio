using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DDD
{
    [Serializable]
    [CreateAssetMenu(fileName = "CharacterViewTextMaster", menuName = "SO/TextMaster/CharacterView", order = 0)]
    public class CharacterViewTextMaster : ScriptableObject
    {
        public int n;
    }
}
