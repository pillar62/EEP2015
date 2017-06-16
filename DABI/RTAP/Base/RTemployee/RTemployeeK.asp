<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="員工基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="眷屬健保"
  functionOptProgram="RTEmpFamInsK.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="none;工號;姓名;職稱;部門名稱;分機號碼;行動電話;電子郵件;離職;健保費;勞保費合計"
  sqlDelete="SELECT a.CUSID, a.EMPLY, c.CUSNC, b.CODEnc, f.DEPTN4," _
           &"a.EXTENSION, a.MOBILE," _
           &"a.EMAIL, a.TRAN2, Healthins, laborins " _
           &"FROM RTemployee a, rtcode b, rtobj c, rtobjlink d," _
           &"rtcode e, rtdept f " _
           &"WHERE a.cusid = c.cusid AND c.cusid = d.cusid AND  " _
           &"a.titleid *= b.code AND b.kind = 'A3' AND " _
           &"A.TYPE *= E.CODE AND E.KIND = 'A4' AND " _
           &"a.dept *= f.dept and d.custyid = '08' and a.CUSID='*' "
  dataTable="RTEmployee"
  extTable="RTObj;RTObjLink"
  userDefineDelete="Yes"    
  numberOfKey=1
  dataProg="RTemployeeD.asp"
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
  searchProg="RTemployeeS.asp"
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
     searchQry=" a.TRAN2 <>'10' "
     searchShow="全部"
  End If
  sqlList="SELECT a.cusid, a.emply, c.cusnc, b.codenc, f.deptn4, a.extension, a.mobile, a.email, " &_
		"case a.tran2 when '10' then 'Y' else '' end, laborins, " &_
		"sum(isnull(g.healthins,0)) " &_
		"FROM RTemployee a " &_
		"inner join RTObj c on c.cusid = a.cusid " &_
		"left outer join RTCode b on b.code = a.titleid and b.kind ='A3' " &_
		"left outer join rtdept f on f.dept = a.dept " &_
		"left outer join RTEmpFamIns g on g.cusid = a.cusid " &_
		"where " &searchQry &_
		" group by a.cusid, a.emply, c.cusnc, b.codenc, f.deptn4, a.extension, a.mobile, a.email, " &_
		"case a.tran2 when '10' then 'Y' else '' end, laborins " &_
        "ORDER BY a.emply "
'Response.Write "SQL=" &SQLlist
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.Recordset")  
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE  FROM RTObjLink WHERE CUSTYID='08' AND CUSID IN " &extDeleList(1) &" "
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
