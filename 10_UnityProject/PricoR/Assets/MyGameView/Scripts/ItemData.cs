using System;

[Serializable]
public class ItemData 
{
    public enum Type {
        Used,
        ExpUp,
        Costume,
        Ticket,
        HouseGoods,
    }
    public int id;
    public Type type = Type.Used;
    public string name;
    public string asset_id;
    public int value;// 適当に使える値。本当はType毎に必要なデータを別テーブルに用意するかもしれない。
    public int cost;
}

