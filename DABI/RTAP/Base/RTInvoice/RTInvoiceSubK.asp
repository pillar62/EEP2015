<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="發票管理系統"
  title="發票明細維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";Y;Y;Y;Y;Y"  
  ButtonEnable=V(0) & ";"& V(1) &";"& "Y;Y;Y;Y"  
  'functionOptName="發票明細"
  'functionOptProgram="RTInvoiceSubK.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="發票號碼;項次;產品名稱;數量;單價;銷售額;稅額;合計"
  sqlDelete="SELECT INVNO, ENTRY, PRODNC, QTY, UNITAMT, SALEAMT, TAXAMT, SALEAMT+TAXAMT as Num " &_
		    "FROM RTInvoiceSub where invno ='' "
  dataTable="RTInvoiceSub"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTInvoiceSubD.asp"
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
  keyListPageSize=25
  'searchProg=""

  searchFirst=FALSE
  'If searchQry="" Then
     searchQry=" INVNO='" & aryparmkey(0) &"' "
  '   searchShow="最近發票"
  'End If

  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  'Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  sqlList="SELECT INVNO, ENTRY, PRODNC, QTY, UNITAMT, SALEAMT, TAXAMT, SALEAMT+TAXAMT as Num " &_
          "FROM RTInvoiceSub WHERE " & searchQry &" ORDER BY ENTRY "
  'Response.Write "SQL=" & SQLlist
End Sub

Sub SrRunUserDefineDelete()
    Dim conn, rs, strsql, sale, tax, total
    Set conn=Server.CreateObject("ADODB.Connection")
    SET RS=Server.CreateObject("ADODB.RECORDSET")  
    Conn.Open DSN
    
	strsql="select sum(saleamt) as sale, sum(taxamt) as tax, sum(saleamt) + sum(taxamt) as total " &_
		   "from RTInvoiceSub where invno IN " &extDeleList(1) &" "
    rs.Open strsql,conn
    If not rs.Eof Then	  
		sale = rs("sale")
		tax = rs("tax")
		total = rs("total")
	else
		sale = 0
		tax = 0
		total = 0
	end if
	rs.close
	
	strsql="update RTInvoice set salesum="& sale &", taxsum ="& tax &", totalsum="& total &_
		   ",amtc = '"& right("0000000" & cstr(total), 8) &"' where invno IN " &extDeleList(1) &" "
	conn.Execute strsql
	
	conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
%>