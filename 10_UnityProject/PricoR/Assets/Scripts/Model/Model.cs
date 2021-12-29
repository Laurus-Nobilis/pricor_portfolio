using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace priconner //所属は考え直す。
//{

// Model 各種データに責任を持つ。
public abstract class Model<MainDataType> // <SubClass/* default 派生クラス っていうdefault指定できないかなぁ。*/>
{
    public MainDataType ParseFromJson(string json)
    {
        return JsonUtility.FromJson<MainDataType>(json);
    }

    public string ParseToJson(MainDataType t)
    {
        return JsonUtility.ToJson(t);
    }

}

//}

