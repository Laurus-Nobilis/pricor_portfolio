using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DDD
{
    public class EquipmentManager : MonoBehaviour
    {
        EquipmentData _equipment;

        [Inject]
        public void Construct(EquipmentData equip)
        {
            _equipment = equip;
            Debug.Log($"E: {_equipment.TitleName}.....");
        }
    }
}
