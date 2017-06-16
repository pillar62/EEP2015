<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="通知技術;撤銷通知;客訴處理"
  functionOptProgram="RTSndInfo.asp;RTDropInfo.asp;RTFaqK.ASP"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="客戶代號;none;客戶名稱;申請單號;社區名稱;申請日;回覆日期;天數;裝機地址;聯絡電話"
   sqlDelete="SELECT rtsparqadslcust.CUSID, rtsparqadslcust.ENTRYNO, RTObj.SHORTNC,rtsparqadslcust.orderno,rtsparqadslcust.HOUSENAME, " _
         &"RTCustADSL.RCVD,RTCUSTADSL.REPLYDATE,CASE WHEN RTCUSTADSL.RCVD IS NOT NULL AND RTCUSTADSL.REPLYDATE IS NOT NULL " _
         &"THEN ltrim(str(Datediff(mi, rtsparqadslcust.RCVD, rtsparqadslcust.REPLYDATE)/1440)) " _
         &"WHEN rtsparqadslcust.RCVD IS NOT NULL AND rtsparqadslcust.REPLYDATE IS NULL " _
         &"THEN ltrim(str(Datediff(mi, rtsparqadslcust.RCVD, GETDATE())/1440)) ELSE 0 END, " _
         &"RTCOUNTY.CUTNC + rtsparqadslcust.TOWNSHIP2 + rtsparqadslcust.RADDR2, " _
         &"rtsparqadslcust.HOME " _
         &"FROM rtsparqadslcust INNER JOIN " _
         &"RTObj ON rtsparqadslcust.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON rtsparqadslcust.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON rtsparqadslcust.ISP = RTCode1.CODE AND  " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode ON rtsparqadslcust.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _
         &"WHERE rtsparqadslcust.cusid='*' " _
         &"ORDER BY RTCOUNTY.CUTNC, rtsparqadslcust.TOWNSHIP2, rtsparqadslcust.RADDR2,rtobj.shortnc "
  dataTable="rtsparqadslcust"
  userDefineDelete=""
  extTable=""
  numberOfKey=2
  dataProg="RTCustD.asp"
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
  searchProg="self"
  searchFirst=false
  searchqry="rtsparqadslcust.cutid2='" & aryparmkey(0) & "' and rtsparqadslcust.township2='" & aryparmkey(1) & "' and rtsparqadslcust.housename='" & aryparmkey(2) & "' "
  If searchQry="" Then
     searchShow="全部"
     searchQry="RTCUSTADSL.CUSID<>'*' "
  ELSE
     SEARCHFIRST=FALSE
  End If  
  sqllist="SELECT rtsparqadslcust.CUSID, rtsparqadslcust.ENTRYNO, RTObj.SHORTNC,rtsparqadslcust.orderno,rtsparqadslcust.HOUSENAME, " _
         &"rtsparqadslcust.RCVD,rtsparqadslcust.REPLYDATE,CASE WHEN rtsparqadslcust.RCVD IS NOT NULL AND rtsparqadslcust.REPLYDATE IS NOT NULL " _
         &"THEN ltrim(str(Datediff(mi, rtsparqadslcust.RCVD, rtsparqadslcust.REPLYDATE)/1440)) " _
         &"WHEN rtsparqadslcust.RCVD IS NOT NULL AND rtsparqadslcust.REPLYDATE IS NULL " _
         &"THEN ltrim(str(Datediff(mi, rtsparqadslcust.RCVD, GETDATE())/1440)) ELSE 0 END, " _
         &"RTCOUNTY.CUTNC + rtsparqadslcust.TOWNSHIP2 + rtsparqadslcust.RADDR2, " _
         &"rtsparqadslcust.HOME " _
         &"FROM rtsparqadslcust INNER JOIN " _
         &"RTObj ON rtsparqadslcust.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON rtsparqadslcust.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON rtsparqadslcust.ISP = RTCode1.CODE AND  " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode ON rtsparqadslcust.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _
         &"WHERE " & searchqry & " " _
         &"ORDER BY RTCOUNTY.CUTNC, rtsparqadslcust.TOWNSHIP2, rtsparqadslcust.RADDR2,rtobj.shortnc "
 ' Response.Write "sql=" & SQLLIST
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
'         SelSql="select cusid from RTCUSTADSL where cusid=" & cusidary(i) 
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
