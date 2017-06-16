<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="Table 欄位中英對照表"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
  buttonEnable="N;N;Y;Y;Y;Y"
  functionOptName=" 欄位 ; 列印 "
  functionOptProgram="RTColumnK.asp;\rtap\RTTable\RTTableRPT.asp"
  functionOptPrompt="N;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="Table名稱(英);Table名稱(中);Owner;類別;備註"
  sqlDelete="SELECT tbOwner, tbType, tbName, tbNameC, tbDesc" _
         &"FROM ATableList "
  dataTable="ATableList"
  userDefineDelete="Yes"  
  'extTable="RTObj;RTObjLink"
  numberOfKey=1
  dataProg="RTTableD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=440,scrollbars=yes"
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
  searchProg="RTTableS.asp"
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
     searchQry=" tbName <>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT tbName, tbNameC, tbOwner, tbType, tbDesc " &_
          "FROM	  ATableList " &_  
          "WHERE " & searchQry &" " &_
          " ORDER BY tbType "
'Response.Write "SQL=" &sqllist           
End Sub

Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.recordset")  
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE FROM AColumnList WHERE tbName IN " &extDeleList(1) &" "
     conn.Execute delSql     
     'SelSql="Select * FROM AColumnList WHERE tbName IN " &extDeleList(1) &" "
     'rs.Open selsql,conn
     '當objlink已無該對象代碼其它關連時,才刪除對象主檔(以避免該對象有其它對象
     '類別時,卻將對象主檔刪除之錯誤
     'if rs.EOF then                 
     '   delSql="DELETE  FROM RTObj WHERE CUSID IN " &extDeleList(1) &" " 
     '   conn.Execute delSql
     'end if
     'rs.close
  End If
  conn.Close
  set rs=nothing
  set conn=nothing
  objectcontext.setcomplete  
End Sub
%>
