<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="合約管理系統"
  title="合約屬性樹(大類別)資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="小類別;頁數異動"
  functionOptProgram="RTContractTreeL2K.asp;RTContractTreeL1DCHGOPT.ASP"
  functionOptPrompt="N;Y"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="none;類別代號;中文說明;每冊頁次;異動碼;最近異動日"
  sqlDelete="SELECT propertyid, category1,category1nm,VOLUMEPAGE,CHGFLAG,LASTCHGDAT from HBcontractTreeL1 WHERE propertyid ='*' "
  dataTable="RTContractTreeL1"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=2
  dataProg="RTContractTreeL1D.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=800,height=220,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=40
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
     searchQry=" propertyid ='" & aryparmkey(0) & "' "
     searchShow="全部"
  End If
  sqlList="SELECT propertyid, category1,category1nm,VOLUMEPAGE,CHGFLAG,LASTCHGDAT  from HBcontractTreeL1 WHERE propertyid ='" & aryparmkey(0) & "' and " &searchQry &" order by propertyid,CATEGORY1 "
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>