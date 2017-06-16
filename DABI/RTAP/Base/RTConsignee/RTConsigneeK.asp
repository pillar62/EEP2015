<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博(Sparq*)管理系統"
  title="經銷商基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="經銷ISP;責任轄區;經銷方案"
  functionOptProgram="RTConsigneeISPK.asp;RTConsigneeDutyK.asp;RTConsigneeCaseK.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="廠商統編;經銷代碼;簡稱;公司電話;聯絡人;聯絡電話;傳真電話;施工組別;標準施工費"
  sqlDelete="SELECT RTObj.CUSID, RTConsignee.cusno, RTObj.SHORTNC,RTConsignee.Tel,RTConsignee.CONT,RTConsignee.CONTTEL, " _
         &"RTConsignee.FAX, RTConsignee.TEAM,RTConsignee.StdFee " _
         &"FROM RTObj INNER JOIN " _
         &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID INNER JOIN " _
         &"RTObjKind ON RTObjLink.CUSTYID = RTObjKind.CUSTYID INNER JOIN " _
         &"RTConsignee ON RTObjLink.CUSID = RTConsignee.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON RTObj.CUTID1 = RTCounty.CUTID " _
         &"WHERE (RTObjKind.CUSTYID = '02') and (RTobj.CUSID='*') "
  dataTable="RTConsignee"
  userDefineDelete="Yes"  
  extTable="RTObj;RTObjLink"
  numberOfKey=1
  dataProg="RTConsigneeD.asp"
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
  keyListPageSize=30
  searchProg="RTConsigneeS.asp"
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
     searchQry=" RTObj.CUSID<>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT RTObj.CUSID, RTConsignee.cusno, RTObj.SHORTNC,RTConsignee.Tel,RTConsignee.CONT,RTConsignee.CONTTEL, " _
         &"RTConsignee.FAX, RTConsignee.TEAM,RTConsignee.StdFee  " _
         &"FROM RTObj INNER JOIN " _
         &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID INNER JOIN " _
         &"RTObjKind ON RTObjLink.CUSTYID = RTObjKind.CUSTYID INNER JOIN " _
         &"RTConsignee ON RTObjLink.CUSID = RTConsignee.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON RTObj.CUTID1 = RTCounty.CUTID " _
         &"WHERE (RTObjKind.CUSTYID = '02') AND " &searchQry &" " _
         &"ORDER BY RTObj.ShortNC "
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.recordset")  
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE  FROM RTConsigneeCty WHERE CUSID IN " &extDeleList(1) &" "
     conn.Execute delSql     
     delSql="DELETE  FROM RTConsigneeISP WHERE CUSID IN " &extDeleList(1) &" "
     conn.Execute delSql     
     delSql="DELETE  FROM RTObjLink WHERE CUSTYID='02' AND CUSID IN " &extDeleList(1) &" "
     conn.Execute delSql
     SelSql="Select * FROM RTObjLink WHERE  CUSID IN " &extDeleList(1) &" "
     rs.Open selsql,conn
     '當objlink已無該對象代碼其它關連時,才刪除對象主檔(以避免該對象有其它對象
     '類別時,卻將對象主檔刪除之錯誤
     if rs.EOF then                 
        delSql="DELETE  FROM RTObj WHERE CUSID IN " &extDeleList(1) &" " 
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