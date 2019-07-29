
public class BaseEventData
{

}

#region 由使用者创建
public class OnLoginEventData : BaseEventData
{
    public string nickName;
    public int gender;

    public OnLoginEventData(string nickName, int gender)
    {
        this.nickName = nickName;
        this.gender = gender;
    }
}

public class OnBattleEndEventData : BaseEventData
{
    public string uid;
    public string nickName;
    public int score;

    public OnBattleEndEventData(string uid, string nickName, int score)
    {
        this.uid = uid;
        this.nickName = nickName;
        this.score = score;
    }
}

#endregion