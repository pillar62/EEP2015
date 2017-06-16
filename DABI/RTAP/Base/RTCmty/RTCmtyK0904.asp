<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="營業員;業務員;管委會;客　戶"
  functionOptProgram="RTCmtyBusK.asp;RTCmtySaleK.asp;RTCmtySpK.asp;RTCustK.asp"
  functionOptPrompt="N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;社區<br>序號;社區名稱;縣市;規模<br>戶數;申裝戶;深耕戶; " _
            &"類別<br>未建檔;撤銷<BR>戶;現有戶<br>數合計;開發<br>比率%;未完<BR>工戶;已完<BR>工戶;申請日;T1開通日"
  sqlDelete="SELECT RTCmty.COMQ1 , RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
           &"RTCmty.APPLYCNT,RTcmty.T1PETITION,RTCmty.Schdat,RTcmty.T1Apply " _
           &"FROM RTCmty INNER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
           &"WHERE (((RTCmty.COMQ1)=0)) "
  dataTable="RTCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTCmtyD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
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
  searchProg="RTCmtyS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTCmty.ComQ1=0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="<>'*'"
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89020" or Ucase(emply)="T89018" OR  _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="P92010" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"
  if userlevel=2 then
    If searchShow="全部" Then
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
         &"Sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end),  " _
         &"Sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
         &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
         &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
         &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
         &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
         &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
         &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
         &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
         &"FROM RTEmployee INNER JOIN " _
         &"RTCmtySale ON RTEmployee.CUSID = RTCmtySale.CUSID INNER JOIN " _
         &"RTCounty INNER JOIN " _
         &"RTCust RIGHT OUTER JOIN " _               
         &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
         &"RTArea INNER JOIN " _
         &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
         &"RTCmty.CUTID = RTAreaCty.CUTID ON RTCmtySale.COMQ1 = RTCmty.COMQ1 " _
         &"WHERE RTArea.AREATYPE='1' AND " &searchQry &" and rtemployee.netid='" & Request.ServerVariables("LOGON_USER") & "' " _
         &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, " _
         &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
         &"RTCmty.T1APPLY " _
         &"ORDER BY RTCmty.COMN "
    Else
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
         &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end) ,  " _
         &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
         &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
         &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
         &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
         &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
         &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
         &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
         &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
         &"FROM RTCounty INNER JOIN " _
         &"RTCmtySale INNER JOIN " _
         &"RTCust RIGHT OUTER JOIN " _         
         &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCmtySale.COMQ1 = RTCmty.COMQ1 ON " _
         &"RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
         &"RTArea INNER JOIN " _
         &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
         &"RTCmty.CUTID = RTAreaCty.CUTID INNER JOIN " _
         &"RTEmployee ON RTCmtySale.CUSID = RTEmployee.CUSID " _
         &"WHERE RTArea.AREATYPE='1' and " &searchQry & " "  _
         &"and rtemployee.netid='" & Request.ServerVariables("LOGON_USER") & "' " _
         &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, " _
         &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
         &"RTCmty.T1APPLY " _                  
         &"ORDER BY RTCmty.COMN "
    End If
  else
    If searchShow="全部" Then
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
         &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end),  " _
         &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end), " _ 
         &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
         &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
         &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
         &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
         &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
         &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
         &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
         &"FROM RTArea RIGHT OUTER JOIN " _
         &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID  and rtarea.areaid" & DareaID & " RIGHT OUTER JOIN " _
         &"RTCmtySale RIGHT OUTER JOIN " _
         &"RTCust RIGHT OUTER JOIN " _
         &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCmtySale.COMQ1 = RTCmty.COMQ1 and rtcmtysale.exdat IS NULL ON  " _
         &"RTAreaCty.CUTID = RTCmty.CUTID LEFT OUTER JOIN " _
         &"RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
         &"WHERE RTArea.AREATYPE='1' AND " &searchQry &" " _
         &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, " _
         &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
         &"RTCmty.T1APPLY " _         
         &"ORDER BY RTCmty.COMN "
    Else
    sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
         &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end) ,  " _
         &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
         &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
         &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
         &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
         &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
         &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
         &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
         &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
         &"FROM RTArea RIGHT OUTER JOIN " _
         &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID  and rtarea.areaid" & DareaID & " RIGHT OUTER JOIN " _
         &"RTCmtySale RIGHT OUTER JOIN " _
         &"RTCust RIGHT OUTER JOIN " _         
         &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCmtySale.COMQ1 = RTCmty.COMQ1 and rtcmtysale.exdat IS NULL ON " _
         &"RTAreaCty.CUTID = RTCmty.CUTID LEFT OUTER JOIN " _
         &"RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _ 
         &"WHERE RTArea.AREATYPE='1' and " &searchQry &" " _
         &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, " _
         &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
         &"RTCmty.T1APPLY " _                  
         &"ORDER BY RTCmty.COMN "
    End If  
  end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE  FROM RTCmtyBus WHERE COMQ1 IN " &extDeleList(1) &" " 
     conn.Execute delSql
     delSql="DELETE  FROM RTCmtySale WHERE COMQ1 IN " &extDeleList(1) &" "
     conn.Execute delSql
     delSql="DELETE  FROM RTCmtySp WHERE COMQ1 IN " &extDeleList(1) &" "
     conn.Execute delSql
  End If
  conn.Close
End Sub
%>