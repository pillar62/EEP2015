<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="元訊用戶回娘家抽獎活動(第二重--購買PS2)明細資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="作廢;作廢回覆;列印出貨單"
  functionOptProgram="HB2002ACT222DROP.ASP;HB2002ACT222DROPBACK.ASP;HB2002ACT21P.ASP"
  functionOptPrompt="Y;Y;N"
  functionoptopen  ="1;1;1"    
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;產品代號;產品名稱;訂購數量;定價;售價;金額;訂購日;作廢日;出貨日;出貨單號"
 ' sqlDelete="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.shortnc, RTCust.CUSTYPE, " _
 '          &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME," _
 '          &"RTCust.OFFICE + ' ' + RTCust.EXTENSION  AS Office,RTCust.SNDINFODAT ,rtcust.reqdat " _
 '          &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
 '          &"WHERE RTCust.COMQ1=0 " _
 '          &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
   sqlDelete="SELECT HB2002ACT222.SERNO AS Expr1, HB2002ACT222.PRODUCTID AS Expr2,  " _
         &"RTProduct.PNAME,HB2002ACT222.QTY AS Expr3, HB2002ACT222.LISTPRICE AS Expr4, " _
         &"HB2002ACT222.SALEPRICE AS Expr5, HB2002ACT222.AMT AS Expr6, " _
         &"CONVERT(varchar(10), HB2002ACT222.EDAT, 111) AS EDAT, " _
         &"HB2002ACT222.DROPDAT AS Expr13, HB2002ACT222.HAULDAT, HB2002ACT222.HAULNO " _
         &"FROM HB2002ACT221 INNER JOIN " _
         &"RTCounty ON HB2002ACT221.CUTID1 = RTCounty.CUTID INNER JOIN " _
         &"HB2002ACT222 ON HB2002ACT221.SERNO = HB2002ACT222.SERNO INNER JOIN " _
         &"RTProduct ON HB2002ACT222.PRODUCTID = RTProduct.PID " _
         &"ORDER BY  HB2002ACT222.EDAT " 
  dataTable="HB2002ACT222"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=2
  dataProg="/webap/rtap/ACT2002/ACT22/RTCustd.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="rtcusts.asp"
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" HB2002ACT221.SERNO<>0 "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT HB2002ACT222.SERNO AS Expr1, HB2002ACT222.PRODUCTID AS Expr2,  " _
         &"RTProduct.PNAME,HB2002ACT222.QTY AS Expr3, HB2002ACT222.LISTPRICE AS Expr4, " _
         &"HB2002ACT222.SALEPRICE AS Expr5, HB2002ACT222.AMT AS Expr6, " _
         &"CONVERT(varchar(10), HB2002ACT222.EDAT, 111) AS EDAT, " _
         &"HB2002ACT222.DROPDAT AS Expr13, HB2002ACT222.HAULDAT,HB2002ACT222.HAULNO " _
         &"FROM HB2002ACT221 INNER JOIN " _
         &"RTCounty ON HB2002ACT221.CUTID1 = RTCounty.CUTID INNER JOIN " _
         &"HB2002ACT222 ON HB2002ACT221.SERNO = HB2002ACT222.SERNO INNER JOIN " _
         &"RTProduct ON HB2002ACT222.PRODUCTID = RTProduct.PID " _
         &"ORDER BY  HB2002ACT222.EDAT " 
 'Response.Write "sql=" & SQLLIST
End Sub
%>
