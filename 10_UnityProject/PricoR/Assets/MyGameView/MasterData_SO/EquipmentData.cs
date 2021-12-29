using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace DDD
{
    [Serializable]
    [CreateAssetMenu(menuName = "SO/EquipmentData", fileName = "EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        [SerializeField] private string titleName;
        public string TitleName { get => titleName; set => titleName = value; }
    }
}
