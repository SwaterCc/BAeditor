---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by shirui.
--- DateTime: 2024/9/5 下午4:43
---

Faction = Faction or {}

local EFactionType = {
    Neutrality = 0,
    Friendly = 1,
    Enemy = 2,
}

--可扩充，默认1是玩家阵营
Faction.FactionIDs = { 1, 2, 3, 4, 5, 6, 7, 8, 9 }

Faction.InfoTable = {
    --找不到默认中立
    [1] = { [EFactionType.Friendly] = { 1, 2 }, [EFactionType.Enemy] = { 3, 4, 5, 6, 7, 8, 9 } }
}

Faction.GetFaction = function(id1, id2)
    if Faction.InfoTable[id1] == nil then
        return
    end
    
    for res, factionIds in pairs(Faction.InfoTable[id1]) do
        for _, faction in ipairs(factionIds) do
            if faction == id2 then
                return res
            end
        end
    end
    
    PrintDamageLog(EFactionType.Neutrality)
    return EFactionType.Neutrality
end