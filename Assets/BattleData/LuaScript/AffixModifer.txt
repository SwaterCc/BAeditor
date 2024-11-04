


DamageSuffixApplyModifier = {

    -- 各种词缀效果在此方法列表中编写逻辑，注意返回值

    CrazyHeart = function(attacker, target, damageInfo, ValueParams)

        local taglist = {201, 202, 203};  --目标身上的tag

        local bukezudang = 221  --目标身上的tag
        local isboss = 222  --目标身上的tag
        local niangqiang = 223  --目标身上的tag

        local skilltag = 155
        local value = 0
        local buffCount = 0

        if (target:HasTag(taglist[0]) or  target:HasTag(taglist[1]) or  target:HasTag(taglist[2])) 
            and SkillHasTag(damageInfo.Tags, skilltag)
            then 
                for i = 0, #taglist do
                    if target:HasTag(taglist[i]) then
                        local valurR = ( math.random(ValueParams[0], ValueParams[1]) )
                        PrintDamageLog("伤害提高："..valurR/100 .."% ", 0) 
                        value = value + valurR

                        buffCount = buffCount + 1
                    end
                end
            PrintDamageLog("敌人身上每有一种不同的控制效果，你造成的持续性伤害就会提高[5-20]%[×]  "..  value/100 .."% " .. "一共有"..buffCount.."个控制效果")
        elseif target:HasTag(bukezudang)  or (target:HasTag(isboss) and  target:HasTag(niangqiang))
            and SkillHasTag(damageInfo.Tags, skilltag)
            then
            value = math.random(ValueParams[2], ValueParams[3])
            PrintDamageLog("不可阻挡的敌人和跟路的首领受到来自你的持续性伤害改为提高[10-40]%[x]"..value)
        end
        return  value / 10000
    end

}