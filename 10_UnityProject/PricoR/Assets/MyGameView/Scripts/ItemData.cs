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
    public int value;// �K���Ɏg����l�B�{����Type���ɕK�v�ȃf�[�^��ʃe�[�u���ɗp�ӂ��邩������Ȃ��B
    public int cost;
}

