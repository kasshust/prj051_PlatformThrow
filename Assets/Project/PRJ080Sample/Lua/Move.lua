local currentfield = actmanager.GetField()
local fieldList    = actmanager.GetMoveFieldList()

local moveList           = {}
local choicetable        = {}

-- 移動可能な場所のみ抽出
for i = 1 , #fieldList do
    if fieldList[i] ~= currentfield then
        table.insert( moveList, fieldList[i])
    end
end

--　選択肢用のテーブルを作成
for i = 1 , #moveList do
    local name = actmanager.GetFieldName(moveList[i])
    table.insert(choicetable, name)
end
table.insert(choicetable, "キャンセル")

-- 選択肢の生成
local choice = choose(choicetable)

-- キャンセルの場合処理終了
if choice == #choicetable then
    actmanager.MoveMenuCallback()
    return 
else
-- 選択肢に応じてテキストの内容を変える
    local text = actmanager.GetFieldMoveText(moveList[choice])
    say(text)

--  指定のフィールドに移動
    actmanager.FireMoveAnimation(moveList[choice])
 
    -- actmanager.MoveMenuCallback()
end
