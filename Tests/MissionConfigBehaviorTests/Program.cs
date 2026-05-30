using March7thHoney.Data.Config;
using March7thHoney.Enums.Mission;

var killMonsterMission = new SubMissionInfo
{
    FinishType = MissionFinishTypeEnum.KillMonster,
    ParamInt1 = 16
};

AssertSequence(killMonsterMission.GetAutoLoadGroupIds(), [16],
    "KillMonster missions should auto-load the monster group from ParamInt1.");

var explicitMission = new SubMissionInfo
{
    FinishType = MissionFinishTypeEnum.Talk,
    ParamInt1 = 16,
    GroupIDList = [5, 6]
};

AssertSequence(explicitMission.GetAutoLoadGroupIds(), [5, 6],
    "Non-monster missions should keep using explicit GroupIDList only.");

var waypointMonsterMission = new SubMissionInfo
{
    FinishType = MissionFinishTypeEnum.Talk,
    WayPointType = "Monster",
    WayPointGroupID = 16,
    WayPointEntityID = 200001
};

AssertSequence(waypointMonsterMission.GetAutoLoadGroupIds(), [16],
    "Monster waypoint missions should auto-load the waypoint group.");

static void AssertSequence(IEnumerable<int> actual, int[] expected, string message)
{
    var actualArray = actual.ToArray();
    if (!actualArray.SequenceEqual(expected))
        throw new Exception($"{message} Expected [{string.Join(", ", expected)}], got [{string.Join(", ", actualArray)}].");
}
