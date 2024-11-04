

openLog = false

logger = {}
function logger.Info(str, ...)
    if openLog then
        CS.UnityEngine.Debug.Log(string.format(str, ...)) 
    end
end

function logger.IntToStr(int, str)
    if openLog then
        if int == 1 then
            return str
        else
            return ""
        end
    end
end

function PrintDamageLog(text, isSuccess)
    if openLog then
        if isSuccess == 0 or isSuccess == nil then
            CS.UnityEngine.Debug.Log("<color=#00FF00>" .. tostring(text) .. "</color>")
        else
            CS.UnityEngine.Debug.Log("<color=#FF1515>" .. tostring(text) .. "</color>")
        end
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
    if openLog then
        if result == true then
            local log = "<color=#00FF00>" .. modifierName .. "</color>，"
            logPart = logPart .. log
        else
            local log = "<color=#FD6225>" .. modifierName .. "</color>，"
            logPart = logPart .. log
        end
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
