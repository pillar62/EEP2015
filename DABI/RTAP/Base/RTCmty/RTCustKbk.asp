<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;N"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  accessMode="U"
  DSN="DSN=RTLib"
  formatName="none;客戶代號;單次;客戶名稱;開發種類;線路種類;申請日期;聯絡電話;公司電話"
  sqlDelete="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.CUSNC, RTCust.CUSTYPE, " _
           &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME, " _
           &"RTCust.OFFICE + ' ' + RTCust.EXTENSION  AS Office " _
           &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
           &"WHERE RTCust.COMQ1=0 " _
           &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
  dataTable="RTCust"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=3
  dataProg="RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=12
  searchProg="self"
  searchShow=FrGetCmtyDesc(aryParmKey(0))
  searchQry="RTCust.COMQ1=" &aryParmKey(0) &" "
  sqlList="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.CUSNC, RTCust.CUSTYPE, " _
           &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME, " _
           &"RTCust.OFFICE+' '+ RTCust.EXTENSION  AS Office " _
           &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
           &"WHERE " &searchQry &" " _
           &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(2)) > 0 Then
     delSql="DELETE  FROM RTObjLink WHERE CUSTYID='05' AND CUSID IN " &extDeleList(2) &" "
     conn.Execute delSql  
     SelSql="Select * FROM RTObjLink WHERE  CUSID IN " &extDeleList(2) &" "
     rs.Open selsql,conn
     '當objlink已無該對象代碼其它關連時,才刪除對象主檔(以避免該對象有其它對象
     '類別時,卻將對象主檔刪除之錯誤
     if rs.EOF then                    
        delSql="DELETE  FROM RTObj WHERE CUSID IN " &extDeleList(2) &" " 
        conn.Execute delSql
     end if
  End If
  conn.Close
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->