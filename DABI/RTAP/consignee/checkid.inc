<%
'========================================================================================================================
'ID Check v2.0
'改版說明：
'這個版本和之前版本除了更換字母對照表和一些小部份的 debug 外，其餘的完全沒有改變。
'這次的字母對照表包含到 I O W Z 的驗證，若有什麼錯誤的地方還請告知。
'程式說明：
'這是支 ASP 的身份證字號驗證函式，在此與大家分享，
'因為看過大部份身份證驗證的函式都是用陣列的方式來寫，
'而我總覺得用陣列速度會變慢，且因為真的不太喜歡用陣列(雖然有時真的很好用)，
'所以在寫這個函式時完全沒有使用到陣列，
'另外，不用懷疑， A123456789 真的是合法的身份證字號，
'可是這一組實在是太好猜了，所以若你不希望有人用 A123456789 來突破驗證的話，
'可以自行加寫一個判斷式將 A123456789 排除審核，
'若有發現程式的 Bug 或是覺得哪裡寫不好的，
'歡迎任意修改本程式，不過前提是要 Copy 一份給我哦！
'本程式可以任意修改傳播與使用，唯獨作者資訊希望大家請不要刪除與修改！
'-------------------- 作者資訊 --------------------
'作者：小白
'ICQ：4999381
'E-mail：samchung@tpts4.seed.net.tw
'--------------------------------------------------
'錯誤說明︰
'CheckID(Request("ID")) = "True"  通過檢查
'CheckID(Request("ID")) = "False" 不合法的身份證字號
'CheckID(Request("ID")) = "ERR-1" 不可留空白或輸入位數錯誤
'CheckID(Request("ID")) = "ERR-2" 身份證字號的第一碼必需是合法的英文字母
'CheckID(Request("ID")) = "ERR-3" 身份證字號的第二碼必需是數字 1 或 2
'CheckID(Request("ID")) = "ERR-4" 身份證字號的後九碼必需是數字
'========================================================================================================================
Function CheckID(InputID)
CheckID = "False"
InputID = UCase(InputID)
IF InputID <> "" AND Len(InputID) = 10 THEN
	IF ASC(Left(InputID,1)) < 64 OR ASC(Left(InputID,1)) > 91 THEN
		CheckID = "ERR-2"
		'身份證字號的第一碼必需是合法的英文字母
	ELSEIF MID(InputID,2,1) <> "1" AND MID(InputID,2,1) <> "2" THEN
		CheckID = "ERR-3"
		'身份證字號的第二碼必需是數字 1 或 2
	ELSE
		FOR i = 2 to Len(InputID)
			IF ASC(Mid(InputID,i,1)) < 47 OR ASC(Mid(InputID,i,1)) > 58 THEN CheckID = "ERR-4"
			'身份證字號的後九碼必需是數字
		NEXT
	END IF
	IF Mid(CheckID,1,3) <> "ERR" THEN
		ID_ABC_Data = "A10B11C12D13E14F15G16H17I34J18K19L20M21N22O35P23Q24R25S26T27U28V29W32X30Y31Z33"
		InputID = Mid(ID_ABC_Data,InStr(ID_ABC_Data,Left(InputID,1)) + 1,2) & Mid(InputID,2)
		GetNo = 2
		SUM = Left(InputID,1)
		FOR i = 9 TO 1 STEP -1
			SUM = SUM + Mid(InputID,GetNo,1) * i
			GetNo = GetNo + 1
		NEXT
		IF ASC(Right(InputID,1)) = ASC(Right(10 - Right(SUM,1),1)) THEN CheckID = "True"
		'通過檢查
	END IF
ELSE
	CheckID = "ERR-1"
	'不可留空白或輸入位數錯誤
END IF
End Function
%>
