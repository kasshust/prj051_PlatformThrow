local choicetable        = conversationmanager.m_GiveMoneyList
table.insert(choicetable, "キャンセル")

say("いくら渡しますか")
-- 選択肢の生成
local choice = choose(choicetable)
if choice ~= #choicetable then
    -- お金が足りていない
    if not(conversationmanager.MoneyCheck(choicetable[choice])) then
        say("お金が足りてない")
        conversationmanager.GiveMoenyMenuCallback()
    else 
        conversationmanager.FireMoneyReaction(choicetable[choice])
    end
else
    -- キャンセル
    say("え～")
    conversationmanager.GiveMoenyMenuCallback()
end

