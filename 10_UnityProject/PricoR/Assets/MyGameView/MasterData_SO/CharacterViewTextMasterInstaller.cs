using UnityEngine;
using Zenject;

namespace DDD
{
    [CreateAssetMenu(fileName = "CharacterViewTextMasterInstaller", menuName = "Installers/CharacterViewTextMasterInstaller")]
    public class CharacterViewTextMasterInstaller : ScriptableObjectInstaller<CharacterViewTextMasterInstaller>
    {
        [SerializeField]CharacterViewTextMaster _master;

        public override void InstallBindings()
        {
            Debug.Log("CVTextMaster Installer");

            Container.BindInstance(_master).AsCached();
        }
    }
}