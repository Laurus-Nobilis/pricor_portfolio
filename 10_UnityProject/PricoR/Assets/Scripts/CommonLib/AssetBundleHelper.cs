using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Assertions;

namespace CommonLib
{
    /// <summary>
    /// ��d���[�h�h�~�̂��߂ɁA��Ԃ������ƂɂȂ����B
    /// 
    /// ���ӁF
    ///     ���̃N���X���o�R������ AssetBundle�����[�h���ꂽ�ꍇ�A��d���[�h���N���肤��B�����琶�ŌĂяo����Ȃ��悤�ɂ���B
    /// </summary>
    public class AssetBundleHelper : Object
    {
        static Dictionary<string, AssetBundle> _cache = new Dictionary<string, AssetBundle>();

        public static bool IsLoaded(string key_path)
        {
            return _cache.ContainsKey(key_path);
        }
        public static void UnloadAssetBundle(string key_path, bool unloadAllLoadedObjects = false)
        {
            if (_cache.TryGetValue(key_path, out AssetBundle ab))
            {
                ab.Unload(unloadAllLoadedObjects);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset_bundle_path">�`���j"dir/asset_bundle_file"</param>
        /// <returns></returns>
        public static AssetBundle LoadInStreamingAssetPath(string asset_bundle_path)
        {
            if (_cache.TryGetValue(asset_bundle_path, out AssetBundle ab))
            {
                return ab;
            }
            var asset = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, asset_bundle_path));
            Assert.IsNotNull(asset);
            _cache[asset_bundle_path] = asset;
            return asset;
        }
        public static AssetBundle LoadDialogsInStreamingAsset()
        {
            const string path = "AssetBundles/dialogs";
            if (_cache.TryGetValue(path, out AssetBundle ab))
            {
                return ab;
            }
            var asset = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, path));
            Assert.IsNotNull(asset);
            _cache[path] = asset;
            return asset;
        }

        /// <summary>
        /// Dialog�n�A�Z�b�g�̃��[�h���܂Ƃ߂���������B
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameObject LoadDialog(string path)
        {
            var asset = LoadDialogsInStreamingAsset();
            return asset.LoadAsset<GameObject>(path);
        }
        /// <summary>
        /// �W�F�l���b�N�t���Ă�prefab��GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadDialog<T>(string path) where T : DialogBase
        {
            var asset = LoadDialogsInStreamingAsset();
            return asset.LoadAsset<T>(path);
        }
    }
}
