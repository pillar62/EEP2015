<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Hi-Building管理系統"
  title="銀行基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="分行"
  functionOptProgram="RTbankbranchk.asp"
  functionOptPrompt="N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="銀行代號;銀行名稱;銀行類別;分行數"
  sqlDelete="SELECT RTBank.HEADNO AS Expr1, RTBank.HEADNC AS Expr2, " _
           &"RTCode.CODENC AS Expr3, COUNT(*) " _
           &"AS Expr5 " _
           &"FROM RTBank INNER JOIN RTCode ON RTBank.BANKTYPE = RTCode.CODE LEFT OUTER JOIN " _
           &"RTBankBranch ON RTBank.HEADNO = RTBankBranch.HEADNO " _
           &"WHERE         (RTCode.KIND = 'G1') and rtbank.headno='*' " _
           &"GROUP BY  RTBank.HEADNO, RTBank.HEADNC, RTCode.CODENC, RTBank.SHORTNC " 
  dataTable="RTBank"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="RTBankD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=40
  searchProg="RTBankS.asp"
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
     searchQry=" and RTBank.HEADNO<>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT RTBank.HEADNO AS Expr1, RTBank.HEADNC AS Expr2, " _
           &"RTCode.CODENC AS Expr3, COUNT(*) " _
           &"AS Expr5 " _
           &"FROM RTBank INNER JOIN RTCode ON RTBank.BANKTYPE = RTCode.CODE LEFT OUTER JOIN " _
           &"RTBankBranch ON RTBank.HEADNO = RTBankBranch.HEADNO " _
           &"WHERE         (RTCode.KIND = 'G1') and rtbank.headno<>'*' " & searchQry & " " _
           &"GROUP BY  RTBank.HEADNO, RTBank.HEADNC, RTCode.CODENC " 
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>