using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.IO;

public class TitleView : MonoBehaviour
{
    [SerializeField] Button _debugBtn;
    [SerializeField] Button _forceStartBtn;
    [SerializeField] Canvas _canvas;
    private GameObject _nameDialog;
    private string _nameBuff = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        _debugBtn?.onClick.AddListener(() =>
        {
            //ユーザーデータ削除
            Debug.Log("clicked ユーザーデータ初期化");
            UserModel.SaveUserAccount(0, string.Empty);
        });

        _forceStartBtn?.onClick.AddListener(() =>
        {
            //サーバー無しで開始
            Models.Instance.SetDebugUser();
            LoadSceneAsync();
        });

        //var assetBundle = AssetBundle.LoadFromFile(Path.Combine( Application.streamingAssetsPath,"AssetBundles/dialogs"));
        _nameDialog = CommonLib.AssetBundleHelper.LoadDialog("Assets/Prefab/InputNameDialog.prefab");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnTap()
    {
        Debug.Log("OnTap title panle");

        if (!UserModel.ExistUser() && string.IsNullOrEmpty(_nameBuff))
        {
            ShowInputNameDialog();
            return;
        }

        var name = (
            UserModel.ReadUserName() != string.Empty
            ? UserModel.ReadUserName()
            : _nameBuff
            );

        Director.Instance.TouchGuard.SetEnable(true);

        LoginTask task = new LoginTask(() =>
        {
            LoadSceneAsync();   
        }
        , () =>
        {
            Director.Instance.TouchGuard.SetEnable(false);
            Debug.LogError("Login API失敗");
            Director.Instance.Alert.Show("Login エラー。\nネットワークをチェックすべし。");
        }
        , UserModel.ReadUserId()
        , name
        );

        Director.Instance.NetworkManager.Request(task);
    }

    async void LoadSceneAsync()
    {
        Director.Instance.TouchGuard.SetEnable(false);
        var asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        await asyncLoad.ToUniTask(Progress.Create<float>(f => Debug.Log($"progress : {f}")));
    }

    /// <summary>
    /// ユーザーネーム入力ダイアログを表示。
    /// </summary>
    private void ShowInputNameDialog()
    {
        var dlg = GameObject.Instantiate(_nameDialog, _canvas.transform);

        // Instantiate 前だと設定できないかもしれない？？？
        dlg.GetComponentInChildren<InputField>().onEndEdit.AddListener(str =>
        {
            _nameBuff = str;
        });
        var btns = dlg.GetComponentsInChildren<Button>();
        foreach (var b in btns)
        {
            b.onClick.AddListener(() =>
            {
                _nameDialog.gameObject.SetActive(false);
                if (_nameBuff != string.Empty)
                {
                    OnTap();
                }
            });
        }
    }
}
