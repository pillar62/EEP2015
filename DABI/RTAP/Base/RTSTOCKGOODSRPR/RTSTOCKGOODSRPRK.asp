<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="送修單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="產品明細;作   廢;作廢返轉;異動查詢"
  functionOptProgram="RTStockgoodsRPRdetailK.asp;RTSTOCKRPRDROP.ASP;RTSTOCKRPRDROPCANCEL.ASP;RTSTOCKRPRTRANSK.ASP"
  functionOptPrompt="N;N;Y;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="送修單號;送修日;送修廠商;送修員;作廢日;作廢員;產品筆數"
  sqlDelete="SELECT RTSTOCKREPAIRH.REPAIRNO, RTSTOCKREPAIRH.REPAIRDAT, " _
         &"RTObj_2.SHORTNC, RTObj_1.SHORTNC AS Expr1, " _
         &"RTSTOCKREPAIRH.DROPDAT, RTObj_3.SHORTNC AS Expr2, " _
         &"SUM(CASE WHEN RTSTOCKREPAIRD1.REPAIRno IS NULL THEN 0 ELSE 1 END)  " _ 
         &"FROM RTSTOCKREPAIRH INNER JOIN " _
         &"RTObj RTObj_2 ON " _
         &"RTSTOCKREPAIRH.FACTORY = RTObj_2.CUSID LEFT OUTER JOIN " _
         &"RTObj RTObj_3 INNER JOIN " _
         &"RTEmployee RTEmployee_1 ON RTObj_3.CUSID = RTEmployee_1.CUSID ON " _
         &"RTSTOCKREPAIRH.DROPUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
         &"RTSTOCKREPAIRD1 ON " _
         &"RTSTOCKREPAIRH.REPAIRNO = RTSTOCKREPAIRD1.REPAIRNO LEFT OUTER JOIN " _
         &"RTObj RTObj_1 INNER JOIN " _
         &"RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID ON " _
         &"RTSTOCKREPAIRH.CHECKUSR = RTEmployee.EMPLY " _
         &"WHERE RTSTOCKREPAIRH.REPAIRNO='*' " _           
         &"GROUP BY  RTSTOCKREPAIRH.REPAIRNO, RTSTOCKREPAIRH.REPAIRDAT, " _
         &"RTObj_2.SHORTNC, RTObj_1.SHORTNC, RTSTOCKREPAIRH.DROPDAT, " _
         &"RTObj_3.SHORTNC " _
         &"ORDER BY  RTSTOCKREPAIRH.REPAIRNO " 
  dataTable="RTSTOCKREPAIRH"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="RTSTOCKGOODSRPRD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTSTOCKGOODSRTNS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  If searchQry="" Then
     searchQry=" RTSTOCKREPAIRH.REPAIRNO <>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT RTSTOCKREPAIRH.REPAIRNO, RTSTOCKREPAIRH.REPAIRDAT, " _
         &"RTObj_2.SHORTNC, RTObj_1.SHORTNC AS Expr1, " _
         &"RTSTOCKREPAIRH.DROPDAT, RTObj_3.SHORTNC AS Expr2, " _
         &"SUM(CASE WHEN RTSTOCKREPAIRD1.REPAIRno IS NULL THEN 0 ELSE 1 END)  " _ 
         &"FROM RTSTOCKREPAIRH INNER JOIN " _
         &"RTObj RTObj_2 ON " _
         &"RTSTOCKREPAIRH.FACTORY = RTObj_2.CUSID LEFT OUTER JOIN " _
         &"RTObj RTObj_3 INNER JOIN " _
         &"RTEmployee RTEmployee_1 ON RTObj_3.CUSID = RTEmployee_1.CUSID ON " _
         &"RTSTOCKREPAIRH.DROPUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
         &"RTSTOCKREPAIRD1 ON " _
         &"RTSTOCKREPAIRH.REPAIRNO = RTSTOCKREPAIRD1.REPAIRNO LEFT OUTER JOIN " _
         &"RTObj RTObj_1 INNER JOIN " _
         &"RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID ON " _
         &"RTSTOCKREPAIRH.CHECKUSR = RTEmployee.EMPLY " _
         &"WHERE " &searchQry &" " _           
         &"GROUP BY  RTSTOCKREPAIRH.REPAIRNO, RTSTOCKREPAIRH.REPAIRDAT, " _
         &"RTObj_2.SHORTNC, RTObj_1.SHORTNC, RTSTOCKREPAIRH.DROPDAT, " _
         &"RTObj_3.SHORTNC " _
         &"ORDER BY  RTSTOCKREPAIRH.REPAIRNO " 
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>