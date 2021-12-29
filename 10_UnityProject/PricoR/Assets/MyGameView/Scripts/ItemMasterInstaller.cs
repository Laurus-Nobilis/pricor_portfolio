using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ItemMasterInstaller", menuName = "Installers/ItemMasterInstaller")]
public class ItemMasterInstaller : ScriptableObjectInstaller<ItemMasterInstaller>
{
    [SerializeField] ItemMaster _itemMaster;

    public override void InstallBindings()
    {
        Container.BindInstance(_itemMaster).AsCached();
    }
}