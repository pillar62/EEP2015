<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/RTGetUserEmply.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="發票管理系統"
  title="發票資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";Y;Y;Y;Y;Y"
  ButtonEnable=V(0) & ";"& V(1) &";"& "Y;Y;Y;Y"  
  functionOptName="發票明細"
  functionOptProgram="RTInvoiceSubK.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="發票號碼;發票日期;發票抬頭;公司統編;聯數;稅別;銷售額;稅額;發票總額;列印批次;作廢日期"
  sqlDelete="SELECT a.INVNO, a.INVDAT, a.INVTITLE, a.UNINO, a.INVTYPE, b.codenc, a.SALESUM, "_
		   &"a.TAXSUM, a.TOTALSUM, a.BATCH, a.CANCELDAT "_
		   &"FROM RTInvoice a  left outer join RTCode b on a.TAXTYPE = b.CODE and b.KIND = 'P1' where invno ='' "
  dataTable="RTInvoice"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTInvoiceD.asp"
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
  keyListPageSize=25
  searchProg="RTInvoiceS.asp"

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" batch =(select max(batch) from RTInvoice) "
     searchShow="最近發票"
  End If

  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  'Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  sqlList="SELECT a.INVNO, a.INVDAT, a.INVTITLE, a.UNINO, a.INVTYPE, b.codenc, a.SALESUM, "_
		 &"a.TAXSUM, a.TOTALSUM, a.BATCH, a.CANCELDAT "_
		 &"FROM RTInvoice a  left outer join RTCode b on a.TAXTYPE = b.CODE and b.KIND = 'P1' where "_
		 & searchqry &" ORDER BY 2,1 "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()
	Dim conn
	Set conn=Server.CreateObject("ADODB.Connection")
	On Error Resume Next
	conn.Open DSN
	delSql="DELETE FROM RTInvoiceSub WHERE INVNO IN " &extDeleList(1) &" " 
	conn.Execute delSql
	conn.Close
	Set conn=Nothing
End Sub
%>