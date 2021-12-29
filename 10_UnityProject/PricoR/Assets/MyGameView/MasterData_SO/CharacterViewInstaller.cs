using UnityEngine;
using Zenject;

namespace DDD
{
    [CreateAssetMenu(fileName = "CharacterViewInstaller", menuName = "Installers/CharacterViewInstaller")]
    public class CharacterViewInstaller : ScriptableObjectInstaller<CharacterViewInstaller>
    {
        CharacterViewTextMaster _textMaster;
        EquipmentData _equipmentData;

        public override void InstallBindings()
        {
            //Container.Bind<>
            Container.BindInstance(_textMaster).AsCached();
            Container.BindInstance(_equipmentData).AsCached();

        }
    }
}