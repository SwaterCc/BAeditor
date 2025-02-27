namespace Hono.Scripts.Battle.Core
{
    public enum ELevelGamePlayObjectType
    {
        //区域触发器（关心某个目标进出区域)
        TriggerBox,
        //刷怪器
        MonsterGen,
        //要塞（有阵营的一片区域，在这片区域内可以产生一些效果）逻辑固定
        Fortress,
        //据点（出兵口）逻辑固定
        Base,
        //可交互物(一个宝箱，一个大门)
        LevelObject,
        //游戏对象(未创建，存活，死亡)
        LevelActor,
        //游戏对象组
        LevelActorGroup,
        //玩家单位
        LevelPlayer,
        //玩家养成对象
        LevelCharacter,
        //玩家角色出生点
        LevelPlayerInitPoint,
        //玩家可布置点位
        LevelCharacterInitPoint,
    }
}