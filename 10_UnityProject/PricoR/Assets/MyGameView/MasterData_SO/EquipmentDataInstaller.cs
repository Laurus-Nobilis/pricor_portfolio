using UnityEngine;
using Zenject;

namespace DDD
{
    [CreateAssetMenu(fileName = "EquipmentDataInstaller", menuName = "Installers/EquipmentDataInstaller")]
    public class EquipmentDataInstaller : ScriptableObjectInstaller<EquipmentDataInstaller>
    {
        [SerializeField] EquipmentData _equipment;

        public override void InstallBindings()
        {
            Container.BindInstance(_equipment).AsCached();
        }
    }
}