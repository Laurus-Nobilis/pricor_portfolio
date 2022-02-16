using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スプライトシートからスプライトを名前で参照するための処理行う。
/// </summary>
public class SpriteSheetManager : MonoBehaviour
{
    //スプライトシートに含まれるスプライトをキャッシュするディクショナリー
    private static Dictionary<string, Dictionary<string, Sprite>> _spriteSheets =
        new Dictionary<string, Dictionary<string, Sprite>>();

    /// <summary>
    ///スプライトシートに含まれるスプライトを読み込んでキャッシュするメソッド
    /// </summary>
    /// <param name="path">フォルダ名でフォルダ以下のSpriteを読む。ファイル名で該当ファイルのみ読む</param>
    public static void LoadAll(string path)
    {
        if (!_spriteSheets.ContainsKey(path))
        {
            _spriteSheets.Add(path, new Dictionary<string, Sprite>());
        }

        //スプライトを読み込んで、名前と紐づけてキャッシュする。
        //LoadAllしている事を明示しておきたいのでメソッド名もLoadAll()にしておこう。
        //（注：ResourcesではなくAdressableを使っていく。）
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        foreach (Sprite sprite in sprites)
        {
            if (!_spriteSheets[path].ContainsKey(sprite.name))
            {
                _spriteSheets[path].Add(sprite.name, sprite);
            }
        }
    }

    //開放処理: Profiler を見ながら確認する。
    public static void Remove(string path)
    {
        if (!_spriteSheets.ContainsKey(path))
        {
            foreach (var sprites in _spriteSheets[path])
            {
                Resources.UnloadAsset(sprites.Value.texture);
                Destroy(sprites.Value);//ここはDestroyか？
            }
            _spriteSheets.Remove(path);
        }
    }

    //スプライト名からスプライトシートに含まれるスプライトを返すMethod
    public static Sprite GetSpriteByName(string path, string name)
    {
        if (_spriteSheets.ContainsKey(path) && _spriteSheets[path].ContainsKey(name))
        {
            return _spriteSheets[path][name];
        }
        return null;
    }
}
