<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="各類合約管理系統"
  title="合約主檔資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
 IF V(3)="Y" then
	functionOptName="產  品;補助項;帳  款;作  廢;作廢返轉;歷史異動"
	functionOptProgram="RTContractDetailK.ASP;RTContractGrantk.asp;RTContractarapk.asp;RTContractHdrop.asp;RTContractHdropcancel.asp;RTContractHlogK.ASP"
	functionOptPrompt="N;N;N;Y;Y;N"  
 ELSE
	functionOptName="產  品;補助項;帳  款;歷史異動"
	functionOptProgram="RTContractDetailK.ASP;RTContractGrantk.asp;RTContractarapk.asp;RTContractHlogK.ASP"
	functionOptPrompt="N;N;N;N"  
 END IF	
	
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="none;合約編號;對象代碼;對象;性質;大類別;小類別;合約起日;合約迄日;收付別;收付方式;收付起月;收付日;廢"
  sqlDelete="SELECT         HBCONTRACTH.CTNO, HBCONTRACTH.CONTRACTNO + '-' + convert(varchar(4),hbcontracth.volume) +'-'  + convert(varchar(3),hbcontracth.pagecnt), " _
         &"               HBCONTRACTH.CTOBJECT, HBCONTRACTH.CTOBJNAME, " _
         &"               HBContractTreeH.PROPERTYNM, HBContractTreeL1.CATEGORY1NM, " _
         &"               HBContractTreeL2.CATEGORY2NM, HBCONTRACTH.CTSTRDAT, " _
         &"               HBCONTRACTH.CTENDDAT, RTCode.CODENC, RTCode_1.CODENC AS Expr1, " _
         &"               HBCONTRACTH.STRBILLINGYM, HBCONTRACTH.MONTHLYDAT,case when HBCONTRACTH.DROPDAT IS NOT NULL then 'Y' else '' end " _
         &"FROM           HBCONTRACTH INNER JOIN " _
         &"               RTCode ON HBCONTRACTH.RCVORPAY = RTCode.CODE AND  " _
         &"               RTCode.KIND = 'F7' INNER JOIN " _
         &"               RTCode RTCode_1 ON HBCONTRACTH.ARAP = RTCode_1.CODE AND " _
         &"               RTCode_1.KIND = 'F5' INNER JOIN " _
         &"               HBContractTreeL1 ON " _
         &"               HBCONTRACTH.CTTree1 = HBContractTreeL1.CATEGORY1 AND " _
         &"               HBCONTRACTH.CTproperty = HBContractTreeL1.PROPERTYID LEFT OUTER JOIN " _
         &"               HBContractTreeL2 ON " _
         &"               HBCONTRACTH.CTproperty = HBContractTreeL2.PROPERTYID AND " _
         &"               HBCONTRACTH.CTTree1 = HBContractTreeL2.CATEGORY1 AND " _
         &"               HBCONTRACTH.CTTree2 = HBContractTreeL2.CATEGORY2 LEFT OUTER JOIN " _
         &"               HBContractTreeH ON " _
         &"               HBCONTRACTH.CTproperty = HBContractTreeH.PROPERTYID " _
         &"WHERE        (HBCONTRACTH.CTNO = 0) "
  dataTable="HBCONTRACTH"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="RTContractD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=500,scrollbars=yes"
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
  searchProg="RTContractS.asp"
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
     searchQry=" and HBCONTRACTH.CTNO <> 0 "
     searchShow="全部"
  End If
  sqlList="SELECT         HBCONTRACTH.CTNO,  HBCONTRACTH.CONTRACTNO + '-' + convert(varchar(4),hbcontracth.volume) +'-'  + convert(varchar(3),hbcontracth.pagecnt), " _
         &"               HBCONTRACTH.CTOBJECT, HBCONTRACTH.CTOBJNAME, " _
         &"               HBContractTreeH.PROPERTYNM, HBContractTreeL1.CATEGORY1NM, " _
         &"               HBContractTreeL2.CATEGORY2NM, HBCONTRACTH.CTSTRDAT, " _
         &"               HBCONTRACTH.CTENDDAT, RTCode.CODENC, RTCode_1.CODENC AS Expr1, " _
         &"               HBCONTRACTH.STRBILLINGYM, HBCONTRACTH.MONTHLYDAT,case when HBCONTRACTH.DROPDAT IS NOT NULL then 'Y' else '' end  " _
         &"FROM           HBCONTRACTH INNER JOIN " _
         &"               RTCode ON HBCONTRACTH.RCVORPAY = RTCode.CODE AND  " _
         &"               RTCode.KIND = 'F7' INNER JOIN " _
         &"               RTCode RTCode_1 ON HBCONTRACTH.ARAP = RTCode_1.CODE AND " _
         &"               RTCode_1.KIND = 'F5' INNER JOIN " _
         &"               HBContractTreeL1 ON " _
         &"               HBCONTRACTH.CTTree1 = HBContractTreeL1.CATEGORY1 AND " _
         &"               HBCONTRACTH.CTproperty = HBContractTreeL1.PROPERTYID LEFT OUTER JOIN " _
         &"               HBContractTreeL2 ON " _
         &"               HBCONTRACTH.CTproperty = HBContractTreeL2.PROPERTYID AND " _
         &"               HBCONTRACTH.CTTree1 = HBContractTreeL2.CATEGORY1 AND " _
         &"               HBCONTRACTH.CTTree2 = HBContractTreeL2.CATEGORY2 LEFT OUTER JOIN " _
         &"               HBContractTreeH ON " _
         &"               HBCONTRACTH.CTproperty = HBContractTreeH.PROPERTYID " _
         &"WHERE (HBCONTRACTH.CTNO <> 0) " &searchQry &" " _
         &"ORDER BY HBCONTRACTH.CONTRACTNO ,HBContracth.volume,hbcontracth.pagecnt "
'Response.Write "SQL=" &sqllist           
 session("FIRSTPROCESS")="Y"
 session("comq1xx")=""
 session("comnxx")=""
 session("comtypexx")=""
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>