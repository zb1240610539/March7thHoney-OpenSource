using March7thHoney.Data.Excel;
using March7thHoney.Enums.Mission;
using Newtonsoft.Json;
using March7thHoney.Util;

namespace March7thHoney.Data.Config;

public class MissionInfo
{
    public int MainMissionID { get; set; }
    public List<int> StartSubMissionList { get; set; } = [];
    public List<int> FinishSubMissionList { get; set; } = [];
    public List<SubMissionInfo> SubMissionList { get; set; } = [];
    public List<CustomValueInfo> MissionCustomValueList { get; set; } = [];
}

public class SubMissionInfo
{
    public int ID { get; set; }
    public int LevelPlaneID { get; set; }
    public int LevelFloorID { get; set; }
    public int MainMissionID { get; set; }
    public string MissionJsonPath { get; set; } = "";

    [JsonConverter(typeof(SafeStringEnumConverter<SubMissionTakeTypeEnum>))]
    public SubMissionTakeTypeEnum TakeType { get; set; }

    public List<int>? TakeParamIntList { get; set; } = []; 

    [JsonConverter(typeof(SafeStringEnumConverter<MissionFinishTypeEnum>))]
    public MissionFinishTypeEnum FinishType { get; set; }

    public int ParamInt1 { get; set; }
    public int ParamInt2 { get; set; }
    public int ParamInt3 { get; set; }
    public string ParamStr1 { get; set; } = "";
    public List<int>? ParamIntList { get; set; } = [];
    public List<MaterialItem>? ParamItemList { get; set; } = [];
    public List<FinishActionInfo>? FinishActionList { get; set; } = [];
    public int Progress { get; set; }
    public List<int>? GroupIDList { get; set; } = [];
    public int SubRewardID { get; set; }
    public string WayPointType { get; set; } = "";
    public int WayPointFloorID { get; set; }
    public int WayPointGroupID { get; set; }
    public int WayPointEntityID { get; set; }

    public IEnumerable<int> GetAutoLoadGroupIds()
    {
        foreach (var groupId in GroupIDList ?? [])
            if (groupId > 0)
                yield return groupId;

        if (FinishType == MissionFinishTypeEnum.KillMonster && ParamInt1 > 0)
            yield return ParamInt1;

        if (WayPointType == "Monster" && WayPointGroupID > 0)
            yield return WayPointGroupID;
    }

    public int GetAutoLoadMonsterInstId(int groupId)
    {
        if (FinishType == MissionFinishTypeEnum.KillMonster && ParamInt1 == groupId && ParamInt2 > 0)
            return ParamInt2;

        if (WayPointType == "Monster" && WayPointGroupID == groupId && WayPointEntityID > 0)
            return WayPointEntityID;

        return 0;
    }
}

public class CustomValueInfo
{
    public int Index { get; set; }
    public List<int> ValidValueParamList { get; set; } = [];
}

public class FinishActionInfo
{
    [JsonConverter(typeof(SafeStringEnumConverter<FinishActionTypeEnum>))]
    public FinishActionTypeEnum FinishActionType { get; set; }

    public List<int> FinishActionPara { get; set; } = [];
    public List<string> FinishActionParaString { get; set; } = [];
}
