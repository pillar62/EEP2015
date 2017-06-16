<%
'--------------------------------------------------
'錯誤說明︰
'CheckID(Request("ID")) = "True"  通過檢查
'CheckID(Request("ID")) = "False" 不合法的統一編號
Function ValidBID(bid)
  dim n(8)

  If (Len(bid) <> 8) Then
     ValidBid = False
     Exit Function
  End If
  for i = 1 to 8
    ch = Mid(bid,i,1)
    If (ch<"0" or ch>"9") Then
       ValidBID = False
       Exit Function
    End If
  next
  for i = 1 to 8
    n(i) = ASC(Mid(bid,i,1))-48   '將字串轉為數字
  next
 '權數為1的部分直接相加
  wt = n(1) + n(3) + n(5) + n(8)
 '計算權數
  n(2) = n(2) * 2
  n(2) = Int(n(2)/10) + n(2) Mod 10
  n(4) = n(4) * 2
  n(4) = Int(n(4)/10) + n(4) Mod 10
  n(6) = n(6) * 2
  n(6) = Int(n(6)/10) + n(6) Mod 10
  n(7) = n(7) * 4
  n(7) = Int(n(7)/10) + n(7) Mod 10
  wt = wt + n(2) + n(4) + n(6) + n(7)
 '能被10整除代表正確
  if wt Mod 10 = 0 Then
     ValidBID = True
  Else 
    '假如營利事業統一編號第7位為"7"
     If Mid(bid,7,1)="7" Then
        wt = wt - n(7) + Int(n(7)/10)  '重新計算加權數
        if wt Mod 10 = 0 Then          '若能被10整除，代表正確
           ValidBID = True
        Else
           ValidBID = False            '否則錯誤
        End If
     Else
        ValidBID = False
     End If
  End If 
End Function
%> 
