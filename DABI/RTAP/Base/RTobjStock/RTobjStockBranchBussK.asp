<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="證券公司營業員基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  functionOptName=""
  functionOptProgram=""
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;N"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;營業員代號;姓名;出生日期;性別;聯絡電話;行動電話;電子郵件信箱"
  sqlDelete="SELECT RTBussMan.STOCKID, RTBussMan.BRANCH, RTBussMan.CUSID, RTObj.CUSNC," _ 
           &"RTBussMan.BIRTHDAY, case RTBussMan.SEX when 'M' then '男' when 'F' then '女' end as SEXNC, RTBussMan.CONTACT, " _
           &"RTBussMan.MOBIL, RTBussMan.EMAIL " _
           &"FROM RTBussMan INNER JOIN " _
           &"RTObj ON RTBussMan.CUSID = RTObj.CUSID " _ 
           &"WHERE (stockid='*') "
  dataTable="RTBussMan"
  numberOfKey=3
  userDefineDelete="Yes"    
  dataProg="RTobjStockBranchBussD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  searchShow=FrGetStockBussDesc(aryParmKey(0),aryparmkey(1))
  searchQry="rtbussman.stockid='" &aryParmKey(0) &"' and rtbussman.branch='" & aryparmKey(1) & "'"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT RTBussMan.STOCKID, RTBussMan.BRANCH, RTBussMan.CUSID, RTObj.CUSNC," _ 
           &"RTBussMan.BIRTHDAY, case RTBussMan.SEX when 'M' then '男' when 'F' then '女' end as SEXNC, RTBussMan.CONTACT, " _
           &"RTBussMan.MOBIL, RTBussMan.EMAIL " _
           &"FROM RTBussMan INNER JOIN " _
           &"RTObj ON RTBussMan.CUSID = RTObj.CUSID " _
           &"WHERE " & searchQry & " " _
           &"ORDER BY stockid "
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.recordset")  
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE  FROM RTObjLink WHERE CUSTYID='09' AND CUSID IN " &extDeleList(3) &" "
     conn.Execute delSql
     SelSql="Select * FROM RTObjLink WHERE  CUSID IN " &extDeleList(3) &" "
     rs.Open selsql,conn
     '當objlink已無該對象代碼其它關連時,才刪除對象主檔(以避免該對象有其它對象
     '類別時,卻將對象主檔刪除之錯誤
     if rs.EOF then     
        delSql="DELETE  FROM RTObj WHERE CUSID IN " &extDeleList(3) &" " 
        conn.Execute delSql
     end if
     rs.close
  End If
  conn.Close
  set rs=nothing
  set conn=nothing
  objectcontext.setcomplete  
End Sub
%>
<!-- #include file="RTGetStockBussDesc.inc" -->