<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="各類合約管理系統"
  title="合約主檔歷史異動資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="none;合約編號;項次;對象代碼;對象;性質;大類別;小類別;合約起日;合約迄日;異動日;異動別;異動員"
  sqlDelete="SELECT       HBContractHlog.CTNO, " _
         &"               HBContractHlog.CONTRACTNO + '-' + CONVERT(varchar(4), " _
         &"               HBContractHlog.VOLUME) + '-' + CONVERT(varchar(3), " _
         &"               HBContractHlog.PAGECNT) AS Expr1,HBContractHlog.ENTRYNO, HBContractHlog.CTOBJECT, " _
         &"               HBContractHlog.CTOBJNAME, HBContractTreeH.PROPERTYNM, " _
         &"               HBContractTreeL1.CATEGORY1NM, HBContractTreeL2.CATEGORY2NM, " _
         &"               HBContractHlog.CTSTRDAT, HBContractHlog.CTENDDAT, " _
         &"               HBContractHlog.TRANSDAT, RTCode_1.CODENC, RTObj.CUSNC " _
         &"FROM           HBContractHlog INNER JOIN " _
         &"               RTCode ON HBContractHlog.RCVORPAY = RTCode.CODE AND " _
         &"               RTCode.KIND = 'F7' INNER JOIN " _
         &"               HBContractTreeL1 ON " _
         &"               HBContractHlog.CTTree1 = HBContractTreeL1.CATEGORY1 AND " _
         &"               HBContractHlog.CTproperty = HBContractTreeL1.PROPERTYID INNER JOIN " _
         &"               RTCode RTCode_1 ON HBContractHlog.TRANSCODE = RTCode_1.CODE AND " _
         &"               RTCode_1.KIND = 'G2' INNER JOIN " _
         &"               RTEmployee ON HBContractHlog.TRANSUSR = RTEmployee.EMPLY INNER JOIN " _
         &"               RTObj ON RTEmployee.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"               HBContractTreeL2 ON " _
         &"               HBContractHlog.CTproperty = HBContractTreeL2.PROPERTYID AND " _
         &"               HBContractHlog.CTTree1 = HBContractTreeL2.CATEGORY1 AND " _
         &"               HBContractHlog.CTTree2 = HBContractTreeL2.CATEGORY2 LEFT OUTER JOIN " _
         &"               HBContractTreeH ON " _
         &"               HBContractHlog.CTproperty = HBContractTreeH.PROPERTYID " _
         &"WHERE (HBCONTRACTHLOG.CTNO = 0) "  _
         &"ORDER BY HBCONTRACTHLOG.CONTRACTNO ,HBCONTRACTHLOG.volume,hbCONTRACTHLOG.pagecnt "
  dataTable="HBCONTRACTHLOG"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=500,scrollbars=yes"
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
     searchQry=" and HBCONTRACTHLOG.CTNO <> 0 "
     searchShow="全部"
  End If
  sqlList="SELECT         HBContractHlog.CTNO, " _
         &"               HBContractHlog.CONTRACTNO + '-' + CONVERT(varchar(4), " _
         &"               HBContractHlog.VOLUME) + '-' + CONVERT(varchar(3), " _
         &"               HBContractHlog.PAGECNT) AS Expr1,HBContractHlog.ENTRYNO, HBContractHlog.CTOBJECT, " _
         &"               HBContractHlog.CTOBJNAME, HBContractTreeH.PROPERTYNM, " _
         &"               HBContractTreeL1.CATEGORY1NM, HBContractTreeL2.CATEGORY2NM, " _
         &"               HBContractHlog.CTSTRDAT, HBContractHlog.CTENDDAT, " _
         &"               HBContractHlog.TRANSDAT, RTCode_1.CODENC, RTObj.CUSNC " _
         &"FROM           HBContractHlog INNER JOIN " _
         &"               RTCode ON HBContractHlog.RCVORPAY = RTCode.CODE AND " _
         &"               RTCode.KIND = 'F7' INNER JOIN " _
         &"               HBContractTreeL1 ON " _
         &"               HBContractHlog.CTTree1 = HBContractTreeL1.CATEGORY1 AND " _
         &"               HBContractHlog.CTproperty = HBContractTreeL1.PROPERTYID INNER JOIN " _
         &"               RTCode RTCode_1 ON HBContractHlog.TRANSCODE = RTCode_1.CODE AND " _
         &"               RTCode_1.KIND = 'G2' INNER JOIN " _
         &"               RTEmployee ON HBContractHlog.TRANSUSR = RTEmployee.EMPLY INNER JOIN " _
         &"               RTObj ON RTEmployee.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"               HBContractTreeL2 ON " _
         &"               HBContractHlog.CTproperty = HBContractTreeL2.PROPERTYID AND " _
         &"               HBContractHlog.CTTree1 = HBContractTreeL2.CATEGORY1 AND " _
         &"               HBContractHlog.CTTree2 = HBContractTreeL2.CATEGORY2 LEFT OUTER JOIN " _
         &"               HBContractTreeH ON " _
         &"               HBContractHlog.CTproperty = HBContractTreeH.PROPERTYID " _
         &"WHERE HBCONTRACTHLOG.CTNO =" & ARYPARMKEY(0) & " " _
         &"ORDER BY HBCONTRACTHLOG.CONTRACTNO ,HBCONTRACTHLOG.volume,hbCONTRACTHLOG.pagecnt "
'Response.Write "SQL=" &sqllist           
 session("FIRSTPROCESS")="Y"
 session("comq1xx")=""
 session("comnxx")=""
 session("comtypexx")=""
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>