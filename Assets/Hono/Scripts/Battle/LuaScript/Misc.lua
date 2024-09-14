logger = {}
function logger.Info(str, ...)
    CS.UnityEngine.Debug.Log(string.format(str, ...))
end

function logger.IntToStr(int, str)
    if int == 1 then
        return str
    else return ""
    end
end

function PrintDamageLog(text, isSuccess)
    if isSuccess == 0 or nil then
        CS.UnityEngine.Debug.Log("<color=#00FF00>" .. tostring(text) .. "</color>")
    else
        CS.UnityEngine.Debug.Log("<color=#FF1515>" .. tostring(text) .. "</color>")
    end
end

PrintCompareString = {
    [1] = function()
        return ">"
    end,
    [2] = function()
        return ">="
    end,
    [3] = function()
        return "=="
    end,
    [4] = function()
        return "<"
    end,
    [5] = function()
        return "<="
    end,
}

logPart = ""
function combineLog(result, modifierName)
    if result == true then
        local log = "<color=#00FF00>" .. modifierName .. "</color>，"
        logPart = logPart .. log
    else
        local log = "<color=#FD6225>" .. modifierName .. "</color>，"
        logPart = logPart .. log
    end
end

CompareFunc = {
    [1] = function(left, right)
        --大于
        if left == nil or right == nil then
            return false
        end
        return left > right
    end,
    [2] = function(left, right)
        --大于等于
        if left == nil or right == nil then
            return false
        end
        return left >= right
    end,
    [3] = function(left, right)
        --等于
        if left == nil or right == nil then
            return false
        end
        return left == right
    end,
    [4] = function(left, right)
        --小于
        if left == nil or right == nil then
            return false
        end
        return left < right
    end,
    [5] = function(left, right)
        --小于等于
        if left == nil or right == nil then
            return false
        end
        return left <= right
    end,
}






-- PRD 没做完
DamageIndex = 0
Misc = {}
function Random(cirt, mode, ExCritPara)  -- 暴击值，方法0=真随机 1=RPD随机

    math.randomseed(os.time())
    local randomValue = math.random(0, 10000)
    damageInfo.IsCritical = 0
    if ExCritPara == -1 then
        return false
    elseif
        cirt > randomValue or ExCritPara == 1 then
    end
    return true
end


local function p_from_c(c)
    -- 模拟N次随机，计算是否会在N次随机之后必然发生
    local po, pb = 0, 0
    local sumN = 0
    local maxTries = math.ceil(1 / c)

    -- P = 1*c + 2*c(1-c) + 3*c(1-c)(1-2c)+....
    for n = 1, maxTries do
        po = math.min(1, c * n) * (1 - pb)
        pb = pb + po
        sumN = sumN + n * po
    end

    return (1 / sumN)
end

function c_from_p(p)
    local cu = p
    local cl = 0.0
    local cm
    local p1, p2 = 0, 1
    while true do
        cm = (cu + cl) / 2
        p1 = p_from_c(cm)
        if math.abs(p1 - p2) <= 0.000000001 then -- 如果发生的概率足够小，那么认为已经找到了对应的c
            break
        end

        if p1 > p then cu = cm else cl = cm end
        p2 = p1
    end

    return cm
end

function OnSpellStart()
    local nFails = 1
    if nFails == nil then nFails = 1 end
    local c = c_from_p(0.2)
    math.randomseed(os.time())
    local randomValue = math.random(0, 10000) / 10000
    local success = randomValue <= (c * 100 * nFails)
    if success then
        -- 执行具体的操作 
        PrintDamageLog("baoji")
        nFails = 1
    else
        nFails = nFails + 1
        PrintDamageLog("bubaoji")
    end
end

