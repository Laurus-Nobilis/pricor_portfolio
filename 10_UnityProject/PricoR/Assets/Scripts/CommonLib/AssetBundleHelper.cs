using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Assertions;
namespace CommonLib
{
    /// <summary>
    /// TODO:　二重ロード防止のために、状態を持つことになったので staticメンバー をやめておきたい。かといってシングルトンにはしない。さてどうする？=>まぁ結局メモリは取るので、事前に開けとくかそういうメモリどう使うか的アプローチでよかろう。つまるところ参照型はstaticでもいいよって事。キャッシュでのヒープをどうするかは別途。
    /// AssetBundleManager的な物を、
    /// 注意：
    ///     このクラスを経由せずに AssetBundleがロードされた場合、二重ロードが起こりうる。もし生で使われていた場合回避策か制約を設けたい所だが。
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
        /// <param name="asset_bundle_path">形式）"dir/asset_bundle_file"</param>
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
        /// Dialog系アセットのロードをまとめただけだよ。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameObject LoadDialog(string path)
        {
            var asset = LoadDialogsInStreamingAsset();
            return asset.LoadAsset<GameObject>(path);
        }
        /// <summary>
        /// ジェネリック付けてもprefabはGameObject
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
