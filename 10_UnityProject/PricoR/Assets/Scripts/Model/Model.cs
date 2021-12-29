using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace priconner //�����͍l�������B
//{

// Model �e��f�[�^�ɐӔC�����B
public abstract class Model<MainDataType> // <SubClass/* default �h���N���X ���Ă���default�w��ł��Ȃ����Ȃ��B*/>
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

