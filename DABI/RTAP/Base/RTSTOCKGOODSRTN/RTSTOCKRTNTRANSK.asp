<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="退貨單異動狀況查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="退貨單號;異動項次;退貨日;退貨廠商;驗退員;異動日期;異動別;異動人員"
  sqlDelete="SELECT RTSTOCKRTNTRANSLOG.RETURNNO, RTSTOCKRTNTRANSLOG.ENTRYNO, " _
           &"RTSTOCKRTNTRANSLOG.RETURNDAT, RTObj.SHORTNC, " _
           &"RTObj_1.SHORTNC AS Expr1, RTSTOCKRTNTRANSLOG.TRANSDAT, " _
           &"RTCode.CODENC, RTObj_2.SHORTNC AS Expr2 " _
           &"FROM RTObj RTObj_2 INNER JOIN " _
           &"RTEmployee RTEmployee_1 ON " _
           &"RTObj_2.CUSID = RTEmployee_1.CUSID INNER JOIN " _
           &"RTSTOCKRTNTRANSLOG INNER JOIN " _
           &"RTObj ON RTSTOCKRTNTRANSLOG.FACTORY = RTObj.CUSID INNER JOIN " _
           &"RTEmployee ON " _
           &"RTSTOCKRTNTRANSLOG.CHECKUSR = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID INNER JOIN " _
           &"RTCode ON RTSTOCKRTNTRANSLOG.TRANSCODE = RTCode.CODE AND " _
           &"RTCode.KIND = 'G2' ON " _
           &"RTEmployee_1.EMPLY = RTSTOCKRTNTRANSLOG.TRANSUSR " _
           &"WHERE RTSTOCKRTNTRANSLOG.RETURNNO='*' "       

  dataTable=""
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
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
     searchQry=" AND RTSTOCKRTNTRANSLOG.RETURNNO ='" & ARYPARMKEY(0) & "' "
     searchShow="全部"
  End If
  sqlList="SELECT RTSTOCKRTNTRANSLOG.RETURNNO, RTSTOCKRTNTRANSLOG.ENTRYNO, " _
           &"RTSTOCKRTNTRANSLOG.RETURNDAT, RTObj.SHORTNC, " _
           &"RTObj_1.SHORTNC AS Expr1, RTSTOCKRTNTRANSLOG.TRANSDAT, " _
           &"RTCode.CODENC, RTObj_2.SHORTNC AS Expr2 " _
           &"FROM RTObj RTObj_2 INNER JOIN " _
           &"RTEmployee RTEmployee_1 ON " _
           &"RTObj_2.CUSID = RTEmployee_1.CUSID INNER JOIN " _
           &"RTSTOCKRTNTRANSLOG INNER JOIN " _
           &"RTObj ON RTSTOCKRTNTRANSLOG.FACTORY = RTObj.CUSID INNER JOIN " _
           &"RTEmployee ON " _
           &"RTSTOCKRTNTRANSLOG.CHECKUSR = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID INNER JOIN " _
           &"RTCode ON RTSTOCKRTNTRANSLOG.TRANSCODE = RTCode.CODE AND " _
           &"RTCode.KIND = 'G2' ON " _
           &"RTEmployee_1.EMPLY = RTSTOCKRTNTRANSLOG.TRANSUSR " _
           &"WHERE RTSTOCKRTNTRANSLOG.RETURNNO ='" & ARYPARMKEY(0) & "' " &searchQry &" " 
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>