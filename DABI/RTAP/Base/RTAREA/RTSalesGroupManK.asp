<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="業務組別與業務員關係資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & "N;Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="單筆刪除"
  functionOptProgram="RTSalesGroupManDrop.asp"
  functionOptPrompt="Y"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="none;none;工號;none;姓名;版次;生效日期;截止日期"
  sqlDelete="SELECT RTSalesGroupRef.AREAID,RTSalesGroupref.GROUPID , RTSalesGroupref.EMPLY,RTSalesGroupref.version, RTobj.cusnc, " _
           &"RTSalesGroupref.version,RTSalesGroupREF.SDATE,RTSalesGroupREF.EDATE " _
           &"FROM RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTSalesGroupREF ON RTEmployee.EMPLY = RTSalesGroupREF.EMPLY " _
           &"WHERE RTSalesGroupref.AREAID='*'"
  dataTable="RTSalesGroupREF"
  userDefineDelete=""  
  extTable=""
  numberOfKey=4
  dataProg="RTSalesGroupManD.asp"
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
  searchshow1=""
  searchshow2=""
  searchQry=" RTSalesGroupRef.AREAID='" & aryparmkey(0) & "' AND RTSalesGroupRef.GROUPID='" & aryparmkey(1) & "'"
  searchShow1=FrGetAreaDesc(aryParmKey(0))  
  searchShow2=FrGetSalesGroupDesc(aryParmKey(0),aryparmkey(1))    
  searchshow=searchshow1 & searchshow2
  searchFirst=false
  sqlList="SELECT RTSalesGroupRef.AREAID,RTSalesGroupref.GROUPID , RTSalesGroupref.EMPLY,RTSalesGroupref.version, RTobj.cusnc, " _
         &"RTSalesGroupref.version,RTSalesGroupREF.SDATE,RTSalesGroupREF.EDATE " _
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
         &"RTSalesGroupREF ON RTEmployee.EMPLY = RTSalesGroupREF.EMPLY " _
         &"WHERE " & searchqry 
'Response.Write "SQL=" &sqllist           
End Sub

%>
<!-- #include file="RTGetAreaDesc.inc" -->
<!-- #include file="RTSalesGroupDesc.inc" -->