<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="RT收款明細表列印(客服部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & v(3)
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="列印確認"
  functionOptProgram="Verify.asp"
  functionOptPrompt="Y"
  'If V(1)="Y" then
  '   accessMode="U"
  'Else
     accessMode="I"
  'End IF
  DSN="DSN=RTLib"
  debug36=FALSE
  formatName="none;none;none;社區名稱;客戶;地址;收款金額;收款日;社區工程師;財務確認;列印批號"
  sqlDelete="SELECT a.comq1,a.cusid,a.entryno,b.COMN, c.CUSNC,Rtrim(IsNull(a.cutid1,'')) + rtrim(IsNull(a.TOWNSHIP1,''))+rtrim(IsNull(a.RADDR1,'')) as addr1, a.ACTRCVAMT, a.SCHDAT, e.CUSNC, a.finrdfmdat,a.RCVDTLNO " _
           & "FROM   RTCust a, RTCmty b, RTObj c, RTCmtySale d, RTObj e, RTObj f " _
           & "WHERE  a.COMQ1 = b.COMQ1 " _
           & "AND a.CUSID = c.CUSID " _
           & "AND a.COMQ1 =d.COMQ1  AND GetDate() Between d.TDAT AND IsNull(d.EXDAT, '9999/12/31') " _
           & "AND d.CUSID = e.CUSID " _
           & "AND a.PROFAC *= f.CUSID  and a.settype in ('1') " _
           & "and a.cusid='*' " 
  dataTable="RTOBJ"
  extTable=""
  numberOfKey=3
  dataProg="/webap/rtap/base/rtcmty/RTCustD.asp"
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
  searchProg="RTRevPrtS.asp"
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
     searchQry=" and (a.PROFAC <> '*') AND (a.rcvdtlno = '') and (a.actrcvamt > 0) " & ";;;<>'*'"
     searchShow="施工廠商：全部　收款狀況：已收款　列印狀況：未列印"
  End If
  X=split(searchqry,";")
  '---列印批號空白"
  '---轄區權限制
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
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T90076" or _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"
  sqlList="SELECT a.COMQ1, a.CUSID, a.ENTRYNO, b.COMN, c.CUSNC, RTRIM(ISNULL(a.CUTID1, '')) " _
         &"+ RTRIM(ISNULL(a.TOWNSHIP1, '')) + RTRIM(ISNULL(a.RADDR1, '')) AS addr1, " _
         &"a.ACTRCVAMT, a.SCHDAT, e.CUSNC AS Expr1, a.FINRDFMDAT, a.RCVDTLNO " _
         &"FROM RTObj f RIGHT OUTER JOIN " _
         &"RTObj c INNER JOIN " _
         &"RTCust a INNER JOIN " _
         &"RTCmty b ON a.COMQ1 = b.COMQ1 ON c.CUSID = a.CUSID ON " _
         &"f.CUSID = a.PROFAC LEFT OUTER JOIN " _
         &"RTObj e INNER JOIN " _
         &"RTEmployee h INNER JOIN " _
         &"RTCmtySale d ON h.CUSID = d.CUSID INNER JOIN " _
         &"RTAreaSales g ON h.EMPLY = g.CUSID AND g.AREAID " & DAREAID & " ON " _
         &"e.CUSID = h.CUSID ON a.COMQ1 = d.COMQ1 AND GETDATE() BETWEEN d.TDAT AND " _
         &"ISNULL(d.EXDAT, '9999/12/31') where a.comq1<>0 and a.settype in ('1') " &X(0) &" " _
         &"ORDER BY B.COMN "
  sqlstr="FROM RTObj f RIGHT OUTER JOIN " _
         &"RTObj c INNER JOIN " _
         &"RTCust a INNER JOIN " _
         &"RTCmty b ON a.COMQ1 = b.COMQ1 ON c.CUSID = a.CUSID ON " _
         &"f.CUSID = a.PROFAC LEFT OUTER JOIN " _
         &"RTObj e INNER JOIN " _
         &"RTEmployee h INNER JOIN " _
         &"RTCmtySale d ON h.CUSID = d.CUSID INNER JOIN " _
         &"RTAreaSales g ON h.EMPLY = g.CUSID AND g.AREAID " & DAREAID & " ON " _
         &"e.CUSID = h.CUSID ON a.COMQ1 = d.COMQ1 AND GETDATE() BETWEEN d.TDAT AND " _
         &"ISNULL(d.EXDAT, '9999/12/31') where a.comq1<>0 and a.settype in ('1') " &X(0)
 session("SQLSTRREVPRT")=replace(SQLstr,"'","""")
 session("ExistPrtNo")= X(2)
'Response.Write "SQL=" & sqllist
 '---X(1):收款表列印rcvdtlno(='':未列印;<>'':已列印;<>'*':全部)
 '---X(2):收款表批號rcvdtlno(<>'*':無批號)
 '---X(3):廠商profac(<>'*':全部)
 '---當搜尋條件:(1)列印狀態為"已列印且列印批號為空白"時不可列印(2)廠商為全部且列印批號為空白時,不可列印
 '---當搜尋條件:(3)廠商為全部,不可列印
 '90/03/20修改為廠商全部時仍可列印
 ' if X(3)="<>'*'" or (X(3)="<>'*'" and X(2)="<>'*'") then
  if X(2)="<>'*'" then
     ButtonEnable="N;N;Y;Y;Y;N"
  end if
End Sub
%>
