namespace ET;

public struct PlayerEnter
{
    public long UnitId;
    public PlayerInfoProto Info;
}

public struct PlayerUpdate
{
    public long UnitId;
    public PlayerInfoProto Info;
}

public struct PlayerLeave
{
    public long UnitId;
}