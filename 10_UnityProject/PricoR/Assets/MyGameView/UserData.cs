using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;


public partial class UserData : MonoBehaviour , ISerializationCallbackReceiver
{
    static private UserData _instance = null;//シングルトン。

    public partial class OwnedItems { };

    [SerializeField, HideInInspector]
    int _savelevel, _savetmpexp, _savestaminamax;
    [SerializeField, HideInInspector]
    long _saveplayerid;
    [SerializeField, HideInInspector]
    string _savename;

    public long PlayerID { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Exp { get; set; }
    public int StaminaMax { get; private set; }
    //int _curStamina;      //サーバーとのズレが n秒程度発生する。ズレ幅は0.1秒以内になれば理想的
    //int _prevResponseStamina; //前回通信時に受け取ったのスタミナ。サーバー時間と一緒に受け取る。
    
    [SerializeField,HideInInspector]
    public OwnedItems Items { get; private set; }
    public List<CharacterData> Characters { get; private set; }

    public static UserData Instance { 
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance nothing.");
                return null;
            }
            return _instance;
        }
    }


    //QuestProgress _questProgress;
    //StoryProgress _storyProgress;
    //クラン null=所属していない


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
    }

    public static void SetData(UserData t)
    {
        var ins = Instance;
        ins.PlayerID = t.PlayerID;
        ins.Name = t.Name;
        ins.Level = t.Level;
        ins.Exp = t.Exp;
        ins.StaminaMax = t.StaminaMax;
        ins.Items = t.Items;
    }

#if UNITY_EDITOR
    public void TestLoad()
    {
        PlayerID = 11111;
        Name = "ぽんぽこ";
        Level = 151;
        Exp = 435;
        StaminaMax = 862;
        Items = new OwnedItems();
        Items.SetData(Master.Instance.Item.items);
    }
#endif

    public void Load(string json)
    {

    }

    public void OnBeforeSerialize()
    {
        _savelevel = Level;
        _savename = Name;
        _saveplayerid = PlayerID;
        _savestaminamax = StaminaMax;
        _savetmpexp = Exp;

        Debug.LogWarning("Json Before ");
    }

    public void OnAfterDeserialize()
    {
        Level = _savelevel;
        Name = _savename;
        PlayerID = _saveplayerid;
        StaminaMax = _savestaminamax;
        Exp = _savetmpexp;

        Debug.LogWarning("Json After");
    }
}


public partial class UserData
{
    public partial class OwnedItems //このクラスを追加するのは、上手くいってない証左か？
    {
        // アイテムIDリスト。
        [SerializeField, HideInInspector]
        List<ItemData> _items = new List<ItemData>();
        public List<ItemData> ItemList { get => _items; }


        public void Dump()
        {
            foreach (var item in ItemList)
            {
                Debug.LogWarning(item.name);
            }
        }


        public void SetData(List<ItemData> list_)
        {
            _items.Clear();
            _items = list_;


            Dump();
        }
        public void SetData(ItemData[] arr)
        {
            _items = new List<ItemData>(arr);
        }
        public void AddData(ItemData item)
        {
            _items.Add(item);
        }
        public ItemData GetItem(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                Debug.Assert(false, "存在しないインデックス値");
                return null;
            }
            return _items[index];
        }
    }
}

