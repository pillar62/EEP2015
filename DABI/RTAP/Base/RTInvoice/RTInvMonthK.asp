<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="發票管理系統"
  title="每月發票號碼字軌維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=v(0)&";Y;Y;Y;Y;Y"  
  buttonEnable="Y;Y;Y;Y;Y;N"
  'functionOptName="所屬社區線路"
  'functionOptProgram="RTPowerBillCmtyK.asp"
  'functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="年份;月份;發票字軌;二聯發票起始號碼;二聯發票結束號碼;三聯發票起始號碼;三聯發票結束號碼;"
  sqlDelete="SELECT	INVYEAR, INVMONTH, INVTRACK, INVNOS, INVNOE, INVNOS3, INVNOE3 "_
		   &"FROM	RTInvMonth "_
		   &"where	INVTRACK ='*' "
  dataTable="RTInvMonth"
  extTable=""
  numberOfKey=2
  dataProg="RTInvMonthD.asp"
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
  keyListPageSize=25
  
  'searchProg="RTInvMonthS.asp"
  searchFirst=FALSE
  If searchQry="" then
     searchQry=" INVTRACK<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If  
  sqlList="SELECT	INVYEAR, INVMONTH, INVTRACK, INVNOS, INVNOE, INVNOS3, INVNOE3 " &_
		  "FROM	RTInvMonth " &_
		  "where	" & searchQry &_
		  " order by invyear desc, invmonth desc "
'Response.Write "sql=" & SQLLIST         
End Sub
%>
