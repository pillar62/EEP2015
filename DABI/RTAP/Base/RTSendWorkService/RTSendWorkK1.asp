<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區RT發包作業(客服部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客戶明細"
  functionOptProgram="RTSendWorkk.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;序號;社區名稱;T1開通日;筆數"
  sqlDelete="SELECT b.COMQ1,b.comq2, b.COMN, b.t1apply, COUNT(a.cusid) AS custcnt " _
           &"FROM RTCust a, RTCmty b " _
           &"WHERE a.COMQ1 = b.COMQ1 AND b.t1apply IS NOT NULL  AND " _
           &"a.sndinfodat IS NOT NULL and a.settype not in ('2','3') " _
           &"and RTCmty.COMQ1=0 " _          
           &"GROUP BY b.comq1,b.comq2, b.comn, b.t1apply " 
  
  dataTable=""
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="None"
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
  colSplit=2
  keyListPageSize=40
  searchProg="RTSENDWORKS1.ASP"
  searchFirst=False
  If searchQry="" Then
     searchQry=" and b.ComQ1<>0 AND A.SNDINFODAT IS NOT NULL AND A.REQDAT IS NULL and a.dropdat is null "
     searchShow="發包別：未發包  撤銷別：未撤銷"
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
         case "P"
            DAreaID="='A1'"                        
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID=""
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T90076" or _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"  
  
  If searchShow="全部" Then
  sqlList="SELECT b.COMQ1,b.comq2, b.COMN, b.t1apply, COUNT(a.cusid) AS custcnt " _
           &"FROM RTCust a, RTCmty b, RTAREACTY C, RTAREA D " _
           &"WHERE d.areaid " & DareaId & " and a.COMQ1 = b.COMQ1 AND b.t1apply IS NOT NULL AND a.sndinfodat IS NOT NULL " _
           &"AND a.settype Not IN ('2','3') AND b.ComQ1 <> 0 AND A.SNDINFODAT IS NOT NULL " _
           &"AND  b.cutid = c.cutid AND  " _
           &"c.areaid = d.areaid AND d.areatype = '1' " _
           &" " & searchqry  _           
           &" GROUP BY b.comq1,b.comq2, b.comn, b.t1apply "
  Else
  sqlList="SELECT b.COMQ1,b.comq2, b.COMN, b.t1apply, COUNT(a.cusid) AS custcnt " _
           &"FROM RTCust a, RTCmty b, RTAREACTY C, RTAREA D " _
           &"WHERE d.areaid " & Dareaid & " and a.COMQ1 = b.COMQ1 AND b.t1apply IS NOT NULL AND a.sndinfodat IS NOT NULL " _
           &"AND a.settype Not IN ('2','3') AND b.ComQ1 <> 0 AND A.SNDINFODAT IS NOT NULL " _
           &"AND  b.cutid = c.cutid AND  " _
           &"c.areaid = d.areaid AND d.areatype = '1' " _
           &" " & searchqry  _           
           &" GROUP BY b.comq1,b.comq2, b.comn, b.t1apply " 
  End if
  searchqry=replace(searchqry,"b.ComQ1","a.comq1")
 ' searchqry=replace(searchqry,"c.CutID","e.cutid")  
  searchqryDTL=replace(searchqry,"c.CutID","e.cutid")  
  Session("DSQL")=" a.sndinfodat IS NOT NULL and a.settype not in ('2','3')  " & searchqryDTL         
  'Response.Write "SQL=" & SQLlist & "<BR>"
 ' Response.Write "DSQL=" & SESSION("DSQL")& "<BR>"
End Sub
%>