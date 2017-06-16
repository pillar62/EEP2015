<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="發包通知;撤銷通知;客訴處理;報竣異動;歷史異動;CALL-OUT;清除主線調整"
  functionOptProgram="RTSndInfo.asp;RTDropInfo.asp;RTFaqK.ASP;RTCUSTCHGOPT.ASP;RTcusthbchgk.asp;/WEBAP/RTAP/BASE/HBCALLOUTPROJECT/RTCUSTOPTK.ASP;RTCUSTLINEADJFLGCLR.ASP"
  functionOptPrompt ="Y;Y;N;Y;Y;N;Y"
  functionoptopen   ="1;1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  '====================== history until 90/12/28=================================
  'formatName="none;客戶代號;單次;名稱;開發種類;申請日;聯絡電話;公司電話;發包日;完工日;撤銷日期;安裝員類別"
  ' sqlDelete="SELECT RTCust.COMQ1, RTCust.CUSID, RTCust.ENTRYNO, RTObj.SHORTNC, " _
  '       &"RTCust.CUSTYPE, RTCust.RCVD, RTCust.HOME, " _
  '       &"RTCust.OFFICE + ' ' + RTCust.EXTENSION AS Office,  " _
  '       &"RTCust.REQDAT,rtcust.finishdat,RTCUST.DROPDAT, RTCode.CODENC " _
  '       &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
  '       &"RTCounty ON RTCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN RTCode ON RTCust.SETTYPE = RTCode.CODE " _
  '       &"WHERE RTCust.COMQ1=0 AND (RTCode.KIND = 'A7') " _
  '       &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO " 
  '===============================================================================
  formatName="none;none;none;客戶名稱;客戶IP;申請日;完工日;報竣日;欠退日;欠拆;聯絡電話;裝機地址;辦公室電話;同意書編號;none;FTTB-HNNO;FTTB送件日"
  sqlDelete= "SELECT RTCust.COMQ1, RTCust.CUSID, RTCust.ENTRYNO, RTObj.SHORTNC, " &_
             "		RTCust.IP, RTCust.RCVD, RTCust.FINISHDAT, RTCust.DOCKETDAT, " &_
             "		RTCust.DROPDAT, RTCust.OVERDUE, RTCust.HOME, " &_
             "		IsNull(RTCounty.CUTNC,'') + RTCust.TOWNSHIP1 + RTCust.RADDR1, " &_
             "		RTCust.OFFICE + Case When RTCust.OFFICE<>'' and RTCust.EXTENSION <>'' then '#' else ' ' end + RTCust.EXTENSION AS Office,rtcust.consentno " &_
			 "FROM	RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " &_
			 "	    LEFT OUTER JOIN RTCounty ON RTCust.CUTID1 = RTCounty.CUTID " &_
             "WHERE RTCust.COMQ1=0 " &_
             "ORDER BY RTObj.SHORTNC,RTCust.CUSID, RTCust.ENTRYNO, RTCust.DOCKETDAT " 
  dataTable="RTCust"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=3
  dataProg="RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=20
  searchProg="rtcustS.asp"
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTCust.CUSID<>'*' "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If  
 ' searchShow=FrGetCmtyDesc(aryParmKey(0))
  sqllist="SELECT RTCust.COMQ1, RTCust.CUSID, RTCust.ENTRYNO, RTObj.SHORTNC, " &_
          "		RTCust.IP, RTCust.RCVD, RTCust.FINISHDAT, RTCust.DOCKETDAT, " &_
          "		RTCust.DROPDAT, RTCust.OVERDUE, RTCust.HOME, " &_
          "		IsNull(RTCounty.CUTNC,'') + RTCust.TOWNSHIP1 + RTCust.RADDR1, " &_
          "		RTCust.OFFICE + Case When RTCust.OFFICE<>'' and RTCust.EXTENSION <>'' then '#' else ' ' end + RTCust.EXTENSION AS Office,rtcust.consentno,RTCODE.CODENC,  " &_
	      "    fttbcust.fttbcusno,fttbcust.snddat " & _
	      "FROM	RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " &_
		  "	    LEFT OUTER JOIN RTCounty ON RTCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN RTCODE ON RTCUST.CUSTLINEADJFLG=RTCODE.CODE AND RTCODE.KIND='L2' " &_
        "left outer join fttbcust on rtcust.comq1=fttbcust.comq1 and rtcust.cusid=fttbcust.cusid and rtcust.entryno=fttbcust.entryno " & _
          "WHERE "& searchqry &" and RTCust.COMQ1=" & aryParmKey(0) &" "&_
          "ORDER BY RTObj.SHORTNC,RTCust.CUSID, RTCust.ENTRYNO, RTCust.DOCKETDAT " 
  'Response.Write "sql=" & SQLLIST
  SESSION("COMQ1XX")=ARYPARMKEY(0)
  Dim conn,i,rsc,rs
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.RecordSet")  
  DSN="DSN=RTLIB"
  sql="SELECT COMQ1,COMTYPE FROM RTCMTY WHERE COMQ1=" & ARYPARMKEY(0)
  conn.Open DSN  
  RS.Open SQL,CONN
  IF RS("COMTYPE") >="01" AND RS("COMTYPE") <="05" THEN
     SESSION("COMTYPEXX")="1"
  ELSE
     SESSION("COMTYPEXX")="4"
  END IF
End Sub
Sub SrRunUserDefineDelete()
'(1)900413:為避免adsl客戶維護程式與hb客戶維護程式於刪除時(因對象皆為客戶'05')而造成objlink及obj無法match,因此obj及objlink改為不刪除
'========900413 modify start
'  Dim conn,i,rsc,rs
'  Set conn=Server.CreateObject("ADODB.Connection")
'  Set rs=Server.CreateObject("ADODB.RecordSet")  
'  Set rsc=Server.CreateObject("ADODB.RecordSet")    
'  On Error Resume Next  
'  conn.Open DSN
'  If Len(extDeleList(2)) > 0 Then
'     CUSIDXX=replace(extDeleList(2),"(","")
'     CUSIDXX=replace(CUSIDXX,")","")     
'     CUSIDARY=split(cusidxx,",")
'     for i=0 to Ubound(cusidary)
'         SelSql="select cusid from rtcust where cusid=" & cusidary(i) 
'         rsc.open selsql,conn
'         if rsc.eof then
'            delSql="DELETE  FROM RTObjLink WHERE CUSTYID='05' AND CUSID = " &cusidary(i) &" "
'            conn.Execute delSql  
'            SelSql="Select cusid FROM RTObjLink WHERE  CUSID = " &cusidary(i) &" "
'            rs.Open selsql,conn
            '當objlink已無該對象代碼其它關連時,才刪除對象主檔(以避免該對象有其它對象
            '類別時,卻將對象主檔刪除之錯誤
'            if rs.EOF then                    
'               delSql="DELETE  FROM RTObj WHERE CUSID = " &cusidary(i) &" " 
'               conn.Execute delSql
'            end if
'            rs.close
'          End If
'          rsc.close
'      next
'   end if
'   conn.close
'   set rs=nothing
'   set rsc=nothing
'   set conn=nothing
'========900413 modify end   
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->