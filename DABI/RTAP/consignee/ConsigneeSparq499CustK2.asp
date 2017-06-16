<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->

<%
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if

Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博499用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
 ' V=split(SrAccessPermit,";")
  'AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
 ' ButtonEnable=v(0) & ";N;Y;Y;Y;Y"  
  buttonEnable="N;N;Y;Y;Y;Y"
  functionOptName="異動紀錄"
  functionOptProgram="ConsigneeSparq499CustLogk.asp"
  functionOptPrompt="N"
  functionoptopen="1"
 ' If V(1)="Y" then
 '    accessMode="U"
 ' Else
     accessMode="I"
 ' End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;主線;主線名稱;用戶;none;用戶IP;裝機地址;連絡電話;繳款方式;申請日;完工日;報竣日;轉檔日;退租日;作廢日;移入;移出"
  sqlDelete="SELECT RTSparq499Cust.COMQ1, RTSparq499Cust.LINEQ1,RTSparq499Cust.CUSID,rtrim(ltrim(convert(char(6),RTSparq499Cust.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTSparq499Cust.lineQ1))) as comqline,RTSparq499Cmtyh.comn,LEFT(RTSparq499Cust.CUSnc,4),RTCODE_3.CODENC,  " _
           &"RTSparq499Cust.CUSTIP1+'.'+RTSparq499Cust.CUSTIP2+'.'+RTSparq499Cust.CUSTIP3+'.'+RTSparq499Cust.CUSTIP4,(RTCounty.CUTNC + RTSparq499Cust.TOWNSHIP2 + RTSparq499Cust.RADDR2 ) AS raddr, " _
           &"RTSparq499Cust.CONTACTTEL+','+RTSparq499Cust.MOBILE, RTCode_2.CODENC, " _
           &"RTSparq499Cust.APPLYDAT, " _
           &"RTSparq499Cust.FINISHDAT, RTSparq499Cust.DOCKETDAT, RTSparq499Cust.TRANSDAT, RTSparq499Cust.DROPDAT, RTSparq499Cust.CANCELDAT, " _
           &"case when RTSparq499Cust.MOVEFROMCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVEFROMCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVEFROMlineQ1))) else '' end, " _
           &"case when RTSparq499Cust.MOVETOCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVETOCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVETOlineQ1))) else '' end " _
           &"FROM RTSparq499Cust LEFT OUTER JOIN RTCode RTCode_2 ON RTSparq499Cust.PAYTYPE = RTCode_2.CODE " _
           &"AND RTCode_2.KIND = 'M1' LEFT OUTER JOIN RTCounty ON RTSparq499Cust.CUTID2 = RTCounty.CUTID  inner join RTSparq499Cmtyh on " _
           &"RTSparq499Cust.comq1=RTSparq499Cmtyh.comq1 inner join RTSparq499Cmtyline on RTSparq499Cust.comq1=RTSparq499Cmtyline.comq1 and " _
           &"RTSparq499Cust.lineq1=RTSparq499Cmtyline.lineq1 INNER JOIN RTAREATOWNSHIP ON RTSparq499CmtyLINE.CUTID=RTAREATOWNSHIP.CUTID AND " _
           &"RTSparq499CmtyLINE.TOWNSHIP=RTAREATOWNSHiP.TOWNSHIP " _ 
           &"LEFT OUTER JOIN RTCODE RTCODE_3 ON RTSparq499Cust.CASETYPE = RTCODE_3.CODE AND RTCODE_3.KIND='L9' " _
           &"where " & searchqry & " AND RTAREATOWNSHIP.AREAID " & DAREAID & " " _
           &"GROUP BY  RTSparq499Cust.COMQ1, RTSparq499Cust.LINEQ1,RTSparq499Cust.CUSID,rtrim(ltrim(convert(char(6),RTSparq499Cust.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTSparq499Cust.lineQ1))),RTSparq499Cmtyh.comn,LEFT(RTSparq499Cust.CUSnc,4),RTCODE_3.CODENC,  " _
           &"RTSparq499Cust.CUSTIP1+'.'+RTSparq499Cust.CUSTIP2+'.'+RTSparq499Cust.CUSTIP3+'.'+RTSparq499Cust.CUSTIP4,RTCounty.CUTNC + RTSparq499Cust.TOWNSHIP2 + RTSparq499Cust.RADDR2 , " _
           &"RTSparq499Cust.CONTACTTEL+','+RTSparq499Cust.MOBILE, RTCode_2.CODENC, " _
           &"RTSparq499Cust.APPLYDAT, " _
           &"RTSparq499Cust.FINISHDAT, RTSparq499Cust.DOCKETDAT, RTSparq499Cust.TRANSDAT, RTSparq499Cust.DROPDAT, RTSparq499Cust.CANCELDAT, " _
           &"case when RTSparq499Cust.MOVEFROMCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVEFROMCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6),RTSparq499Cust.MOVEFROMlineQ1))) else '' end, " _
           &"case when RTSparq499Cust.MOVETOCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVETOCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6),RTSparq499Cust.MOVETOlineQ1))) else '' end " _
           &"ORDER BY RTSparq499Cust.COMQ1, RTSparq499Cust.LINEQ1,LEFT(RTSparq499Cust.CUSnc,4) " 

  dataTable="RTSparq499Cust"
  userDefineDelete=""
  numberOfKey=3
  dataProg="ConsigneeSparq499CustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="ConsigneeSparq499CustS2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" and RTSparq499Cust.COMQ1 <> 0 and RTSparq499CmtyLINE.consignee='" & Session("UserID")& "' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
          sqlList="SELECT RTSparq499Cust.COMQ1, RTSparq499Cust.LINEQ1,RTSparq499Cust.CUSID,rtrim(ltrim(convert(char(6),RTSparq499Cust.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTSparq499Cust.lineQ1))) as comqline,RTSparq499Cmtyh.comn,LEFT(RTSparq499Cust.CUSnc,4),RTCODE_3.CODENC,  " _
           &"RTSparq499Cust.CUSTIP1+'.'+RTSparq499Cust.CUSTIP2+'.'+RTSparq499Cust.CUSTIP3+'.'+RTSparq499Cust.CUSTIP4,(RTCounty.CUTNC + RTSparq499Cust.TOWNSHIP2 + RTSparq499Cust.RADDR2 ) AS raddr, " _
           &"RTSparq499Cust.CONTACTTEL+','+RTSparq499Cust.MOBILE, RTCode_2.CODENC, " _
           &"RTSparq499Cust.APPLYDAT, " _
           &"RTSparq499Cust.FINISHDAT, RTSparq499Cust.DOCKETDAT, RTSparq499Cust.TRANSDAT, RTSparq499Cust.DROPDAT, RTSparq499Cust.CANCELDAT, " _
           &"case when RTSparq499Cust.MOVEFROMCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVEFROMCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVEFROMlineQ1))) else '' end, " _
           &"case when RTSparq499Cust.MOVETOCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVETOCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVETOlineQ1))) else '' end " _
           &"FROM RTSparq499Cust  LEFT OUTER JOIN RTCode RTCode_2 ON RTSparq499Cust.PAYTYPE = RTCode_2.CODE " _
           &"AND RTCode_2.KIND = 'M1' LEFT OUTER JOIN RTCounty ON RTSparq499Cust.CUTID2 = RTCounty.CUTID  inner join RTSparq499Cmtyh on " _
           &"RTSparq499Cust.comq1=RTSparq499Cmtyh.comq1 inner join RTSparq499Cmtyline on RTSparq499Cust.comq1=RTSparq499Cmtyline.comq1 and " _
           &"RTSparq499Cust.lineq1=RTSparq499Cmtyline.lineq1 INNER JOIN RTAREATOWNSHIP ON RTSparq499CmtyLINE.CUTID=RTAREATOWNSHIP.CUTID AND " _
           &"RTSparq499CmtyLINE.TOWNSHIP=RTAREATOWNSHiP.TOWNSHIP " _ 
           &"LEFT OUTER JOIN RTCODE RTCODE_3 ON RTSparq499Cust.CASETYPE = RTCODE_3.CODE AND RTCODE_3.KIND='L9' " _
           &"where RTSparq499CmtyLINE.consignee='" & Session("UserID")& "' " & searchqry  & " " _
           &"GROUP BY  RTSparq499Cust.COMQ1, RTSparq499Cust.LINEQ1,RTSparq499Cust.CUSID,rtrim(ltrim(convert(char(6),RTSparq499Cust.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTSparq499Cust.lineQ1))),RTSparq499Cmtyh.comn,LEFT(RTSparq499Cust.CUSnc,4),RTCODE_3.CODENC,  " _
           &"RTSparq499Cust.CUSTIP1+'.'+RTSparq499Cust.CUSTIP2+'.'+RTSparq499Cust.CUSTIP3+'.'+RTSparq499Cust.CUSTIP4,RTCounty.CUTNC + RTSparq499Cust.TOWNSHIP2 + RTSparq499Cust.RADDR2 , " _
           &"RTSparq499Cust.CONTACTTEL+','+RTSparq499Cust.MOBILE, RTCode_2.CODENC, " _
           &"RTSparq499Cust.APPLYDAT, " _
           &"RTSparq499Cust.FINISHDAT, RTSparq499Cust.DOCKETDAT, RTSparq499Cust.TRANSDAT, RTSparq499Cust.DROPDAT, RTSparq499Cust.CANCELDAT, " _
           &"case when RTSparq499Cust.MOVEFROMCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVEFROMCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6),RTSparq499Cust.MOVEFROMlineQ1))) else '' end, " _
           &"case when RTSparq499Cust.MOVETOCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTSparq499Cust.MOVETOCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6),RTSparq499Cust.MOVETOlineQ1))) else '' end " _
           &"ORDER BY RTSparq499Cust.COMQ1, RTSparq499Cust.LINEQ1,LEFT(RTSparq499Cust.CUSnc,4) " 
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>