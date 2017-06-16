<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Hi-Building管理系統"
  title="銀行分行基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="none;分行代號;分行名稱"
  sqlDelete="SELECT RTBankbranch.HEADNO, RTBankbranch.branchno,RTBankbranch.branchnc " _
           &"FROM RTBankbranch " _
           &"WHERE (RTBankbranch.HEADNO = '*') " _
           &"order BY  RTBankbranch.HEADNO, RTBankbranch.branchno"
  dataTable="RTBankbranch"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=2
  dataProg="RTBankbranchD.asp"
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
  colSplit=3
  keyListPageSize=60
  searchProg="RTBankbranchS.asp"
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
     searchQry=" and RTBankbranch.HEADNO ='" & aryparmkey(0) & "' "
     searchShow="全部"
  End If
  sqlList="SELECT RTBankbranch.HEADNO, RTBankbranch.branchno,RTBankbranch.branchnc " _
           &"FROM RTBankbranch " _
           &"WHERE (RTBankbranch.HEADNO <> '*') and RTBankbranch.HEADNO ='" & aryparmkey(0) & "' " & searchqry & " " _
           &"order BY  RTBankbranch.HEADNO, RTBankbranch.branchno"
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>