<%
  Dim fieldRole,fieldPa,fieldPb,fieldpc,fieldpd,fieldpe
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
%>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="ADSL客戶基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  'sqlFormatDB="SELECT * FROM RTCust WHERE Comq1=0 "
  sqlFormatDB="SELECT CUSID, ENTRYNO,STOCKID,BRANCH,BUSSMAN,BUSSID,SEX,BIRTHDAY, " _
             &"cutid1,township1,raddr1,rzone1,cutid2,township2,raddr2,rzone2, " _
             &"cutid3,township3,raddr3,rzone3,SPEED,LINETYPE,CASETYPE,USEKIND, " _
             &"RCVD,HOUSETYPE,HOUSENAME,HOUSEQTY,exttel,HOME,FAX,CONTACT,OFFICE, EXTENSION, MOBILE, EMAIL, " _
             &"VOUCHER, EUSR,EDAT,UUSR,UDAT,PROFAC,SNDINFODAT, REQDAT, INSPRTNO, INSPRTDAT, INSPRTUSR,  " _
             &"FINISHDAT, DOCKETDAT, INCOMEDAT, AR, ACTRCVAMT, DROPDAT, RCVDTLNO,  " _
             &"RCVDTLPRT, SCHDAT, FINRDFMDAT, FINCFMUSR, BONUSCAL, DROPDESC, " _
             &"UNFINISHDESC, PAYDTLPRTNO, PAYDTLDAT, PAYDTLUSR, ACCCFMDAT, " _
             &"ACCCFMUSR, ENDCOD, NOTE,OPERENVID, SETTYPE, " _
             &"SETSALES, PRESETDATE, PRESETHOUR, PRESETMIN, SETFEE, SETFEEDIFF, " _
             &"SETFEEDESC,orderno,ss365,REPLYDATE,Lookdat,formaldat,deliverdat,socialid,agree,haveroom,homestat,lookdesc,hn,docno " _
             &"FROM singlecustadsl where cusid='*'"
           
  sqllist    ="SELECT CUSID, ENTRYNO,STOCKID,BRANCH,BUSSMAN,BUSSID,SEX,BIRTHDAY, " _
             &"cutid1,township1,raddr1,rzone1,cutid2,township2,raddr2,rzone2, " _
             &"cutid3,township3,raddr3,rzone3,SPEED,LINETYPE,CASETYPE,USEKIND, " _
             &"RCVD,HOUSETYPE,HOUSENAME,HOUSEQTY,exttel,HOME,FAX,CONTACT,OFFICE, EXTENSION, MOBILE, EMAIL, " _
             &"VOUCHER, EUSR,EDAT,UUSR,UDAT,PROFAC,SNDINFODAT, REQDAT, INSPRTNO, INSPRTDAT, INSPRTUSR,  " _
             &"FINISHDAT, DOCKETDAT, INCOMEDAT, AR, ACTRCVAMT, DROPDAT, RCVDTLNO,  " _
             &"RCVDTLPRT, SCHDAT, FINRDFMDAT, FINCFMUSR, BONUSCAL, DROPDESC, " _
             &"UNFINISHDESC, PAYDTLPRTNO, PAYDTLDAT, PAYDTLUSR, ACCCFMDAT, " _
             &"ACCCFMUSR, ENDCOD, NOTE,OPERENVID, SETTYPE, " _
             &"SETSALES, PRESETDATE, PRESETHOUR, PRESETMIN, SETFEE, SETFEEDIFF, " _
             &"SETFEEDESC,orderno,ss365,REPLYDATE,Lookdat,formaldat,deliverdat,socialid,agree,haveroom,homestat,lookdesc,hn,docno  " _
             &"FROM singlecustadsl where "
 ' Response.write "SQL=" & SQLlist
 ' Response.end            
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userDefineRead="Yes"
  userDefineSave="Yes"
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'-------單次------------------------------
    If Not IsNumeric(dspKey(1)) Then dspKey(1)=0
'-------戶數------------------------------
    If Not IsNumeric(dspKey(27)) or len(trim(dspkey(27))) = 0 Then dspKey(27)=0    
'--------------- -------------------------
    If Not IsNumeric(dspKey(50)) Then dspKey(50)=0   '應收金額
    If Not IsNumeric(dspKey(51)) Then dspKey(51)=0   '實收金額 
    If Not IsNumeric(dspKey(69)) Then dspKey(69)=3   '安裝類別
    If Not IsNumeric(dspKey(74)) Then dspKey(74)=0   '標準施工費
    If Not IsNumeric(dspKey(75)) Then dspKey(75)=0   '施工補助費   
    If Not IsNumeric(dspKey(72)) Then dspKey(72)=0   '裝機(時)
    If Not IsNumeric(dspKey(73)) Then dspKey(73)=0   '裝機(分)     
'-------裝機樓層 -------------------------
'    if len(trim(dspkey(14))) = 0 then dspkey(14)=0
'    if len(trim(dspkey(15))) = 0 then dspkey(15)=0
'    if len(trim(dspkey(16))) = 0 then dspkey(16)=""
    If len(trim(dspkey(0))) < 1 then
       message="請入客戶代碼"
       formValid=False
'    elseIf Not (IsNumeric(dspKey(14)) Or IsNumeric(dspKey(15))) Then
'       message="請入裝機樓層資料"
'       formValid=False       
'-------撤銷原因 -------------------------
'    elseIf IsDate(dspKey(28)) and Len(dspKey(35)) < 1 Then
'          message="請輸入撤銷原因"
'          formValid=False
'-------預定裝機時間----------------------
'    elseIf (dspKey(49) = "1" Or dspKey(49) = "2") and _
'           Not (IsDate(dspKey(51)) And IsNumeric(dspKey(52)) _
'           And IsNumeric(dspKey(53))) Then
'              message="請輸入預定裝機時間"
'              formValid=False
    elseIf dspKey(72) > 24 Or dspKey(73) > 59 Then
       message="請輸入正確預定裝機時間"
       formValid=False
    elseif len(trim(extdb(0))) < 1 then
       message="請輸入客戶名稱"
       formValid=False    
'    elseif len(trim(dspkey(58))) < 1 then
'       message="請輸入客戶性別"
'       formValid=False           
    elseif not Isdate(dspkey(7)) and len(dspkey(7)) > 0 then
       message="出生日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(24)) and len(dspkey(24))  > 0 then
       message="申請日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(42)) and len(dspkey(42))  > 0 then
       message="通知發包日期錯誤"
       formValid=False     
    elseif not Isdate(dspkey(43)) and len(dspkey(43))  > 0 then
       message="發包日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(47)) and len(dspkey(47))  > 0 then
       message="完工日期錯誤"
       formValid=False     
    elseif not Isdate(dspkey(48)) and len(dspkey(48))  > 0 then
       message="報竣日期錯誤"
       formValid=False             
    elseif not IsNumeric(dspkey(50)) and len(dspkey(50))  > 0 then
       message="應收金額錯誤"
       formValid=False           
    elseif not IsNumeric(dspkey(51)) and len(dspkey(51))  > 0 then
       message="實收金額錯誤"
       formValid=False             
    elseif not Isdate(dspkey(52)) and len(dspkey(52))  > 0 then
       message="撤銷日期錯誤"
       formValid=False             
    elseif not Isdate(dspkey(55)) and len(dspkey(55))  > 0 then
       message="收款日期錯誤"
       formValid=False          
    elseif not Isdate(dspkey(71)) and len(dspkey(71))  > 0 then
       message="預定裝機日期錯誤"
       formValid=False          
    elseif not IsNumeric(dspkey(72)) and len(dspkey(72))  > 0 then
       message="預定裝機時間錯誤"
       formValid=False          
    elseif not IsNumeric(dspkey(73)) and len(dspkey(73))  > 0 then
       message="預定裝機時間錯誤"
       formValid=False              
    elseif not IsNumeric(dspkey(75)) and len(dspkey(75))  > 0 then
       message="施工補助金額錯誤"
       formValid=False                     
    elseif (dspkey(69)="1" or dspkey(69)="2" ) and dspkey(41) <> "" then
       message="安裝人員為(業務)或(技術部)時,施工廠商必須空白"
       formvalid=false
    elseif (dspkey(69)="3" ) and dspkey(41) = "" then
       message="安裝人員為(廠商)時,施工廠商不得空白"
       formvalid=false       
    elseif (dspkey(69)="1" ) and dspkey(50) = "" then
       message="安裝人員為(業務)時,預定安裝人員不得空白"
       formvalid=false              
    End If
'-------入帳日期=報竣日期--------------
'    dspkey(25)=dspkey(24)
    if dspkey(6) <> "F" and dspkey(6) <>"M" then dspkey(6)=""
'-------Not allowed Nulls fields-------------
'    Dim aryNull
'    aryNull=Array(0,3,4,5,6,7,8,10,11,12,13,16,17,20,22,29,30,33,35,36,37,39,41,42,43,44,45,47,50,56)
'    For i = 0  To Ubound(aryNull)
'        If Len(dspKey(aryNull(i)))<1 Then
'           formValid=False
'           message="Null string Not allowed"
'           Exit For
'        End If
'    Next
'    aryNull=Array(9,18,19,21,23,24,25,28,31,32,34,38,40,46,48,51)
'    For i = 0  To Ubound(aryNull)
'        If Len(dspKey(aryNull(i)))=0 Then
'        ElseIf IsDate(dspKey(aryNull(i))) Then
'        Else           
'           formValid=False
'           message="Date field Invalid"
'           Exit For
'        End If
'    Next
'廠商標準施工費(施工廠商不為空白，且無付款列印批號時，始可變更）
    if len(trim(dspkey(41))) > 0 and len(trim(dspkey(61))) = 0 then
       Dim Connsupp,Rssupp,sqlsupp,dsn
       Set connsupp=server.CreateObject("ADODB.Connection")
       Set rssupp=Server.CreateObject("ADODB.RecordSet")
       DSN="DSN=RTLIB"
       Sqlsupp="select * from RtSupp where cusid='" & dspkey(41) & "'"
       connsupp.open DSN
       rssupp.open sqlsupp,connsupp,1,1
       if rssupp.eof then
          dspkey(74) = 0
       else
          dspkey(74) = rssupp("STDFee")
       end if
    end if
 '收據名稱為空白時，
   IF len(trim(dspkey(36))) = 0 then
      dspkey(36)=extdb(0)
   end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(39)=V(0)
    end if   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   End Sub 
   Sub SrSelonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
       prog="RTFaQFinishUsr.asp"
       'showopt="Y;Y;Y;Y"表示對話方塊中要顯示的項目(業務工程師;客服人員;技術部;廠商)
       if clickkey="KEY5" then
          showopt="Y;N;N;N"
       else
          showopt="N;N;N;N"
       end if
       prog=prog & "?showopt=" & showopt
       FUsr=Window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;")  
      'Fusrid(0)=維修人員工號或廠商代號  fusrid(1)=只為於上一畫面中秀出中文名稱(無其它作用) fusrid(2)="1"為業務"2"為技術"3"為廠商"4"為客服(作為資料存放於何欄位之依據)
       if Fusr <> "N" then
         '先清除資料
         document.all("key5").value=""
         FUsrID=Split(Fusr,";")   
         '廠商取8位,其餘取6位   
         if Fusrid(2)="3"  then 
            document.all(clickkey).value =  left(Fusrid(0),8)
         else
            document.all(clickkey).value =  left(Fusrid(0),6)
         end if 
       End if
    '   Set winP=window.Opener
    '   Set docP=winP.document
    '   docP.all("keyform").Submit
    '   winP.focus()             
    '   window.close
   End Sub    
   Sub SrBUSonclick()
       prog="RTOBJSTOCKBRANCHBUSSD.asp"
       prog=prog & "?KEY=" & document.all("KEY2").VALUE & ";" & document.all("KEY3").VALUE
       FUsr=Window.open(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       Window.form.Submit
   End Sub    
   Sub Srcounty9onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY8").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key9").value =  trim(Fusrid(0))
          document.all("key11").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub    
   Sub Srcounty13onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY12").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key13").value =  trim(Fusrid(0))
          document.all("key15").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub   
   Sub Srcounty17onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY16").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key17").value =  trim(Fusrid(0))
          document.all("key19").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub                
   
   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
   End Sub    
   
   Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
   End Sub
   
   Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
   End Sub      
   
   Sub SrCmtysel()
       Dim ClickID,prog
       prog="RTCmtySelK.asp"
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid
       CuTID2=document.all("key12").value
       township2=document.all("key13").value
       prog=prog & "?PARM=" & CutID2 & ";" & township2
       Fcmty=window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;scroll:Yes")  
       document.all("key26").value=Fcmty
   End Sub    
   </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
    <td width="20%" class=dataListHead>客戶代號</td>
    <td width="20%" bgcolor="silver">
        <input type="text" name="key0" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;" maxlength="10" size="14"
               value="<%=dspKey(0)%>" class=dataListEntry></td>
    <td width="20%" class=dataListHead>客戶單次</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key1" readonly
               style="text-align:left;" maxlength="6" size="10"
               value="<%=dspKey(1)%>" class=dataListdata></td>
    <td width="20%" bgcolor="orange" style="font-size:16px">收件編號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key77" readonly
               style="text-align:left;" maxlength="6" size="10"
               value="<%=dspKey(77)%>" class=dataListdata style="color:red"></td>
    </tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(37))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(37)=V(0)
      '          extdb(46)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(37))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(38)=datevalue(now())
    else
        if len(trim(dspkey(39))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(39)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(39))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(37))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(40)=datevalue(now())
    end if  
' -------------------------------------------------------------------------------------------- 
    IF len(trim(dspkey(36))) = 0 then
      dspkey(36)=extdb(0)
    end if
    Dim conn,rs,s,sx,sql,t
    '結案碼
    If dspKey(66)="Y" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldPc=""
       fieldpd=""       
       fieldpe=""      
       fieldpf=""              
    Else
       fieldPa=""
       fieldPc=" Onclick=""Srbtnonclick"" "
       fieldpd=" onclick=""SrSelOnClick"" "       
       fieldpe=" onclick=""SrClear"" "     
       fieldpf=" onclick=""Srcmtysel"" "           
    End If
    '收款表已列印或安裝員類別為發包(或空白)時，不可按安裝員工鈕，不可更改安裝人員資料（即安裝員工鈕disable)
    If Len(Trim(dspKey(53))) > 0  Then
       fieldPb=" class=""dataListData"" readonly "
    Else
       fieldPb=""
    End If
    if dspkey(66)="Y" or Len(Trim(dspKey(53))) > 0  then
       fieldPbx=""       
    else
       fieldPbx="SrAddusr()"
    end if        
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>                                      
  <div class=dataListTagOn> 
<table width="100%" ><tr><td width="100%">&nbsp;</td></tr>                                                      
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag1" height="354">  
    <tr>
        <td width="15%" class="dataListHead" height="32">身份證字號</td>
        <td width="30%" height="32" bgcolor="silver"> 
        <input type="password" name="key83" size="10" maxlength="10" value="<%=dspkey(83)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td> 
        <td width="15%" class="dataListHead" height="32">HN號碼</td>
        <td width="30%"  height="32" bgcolor="silver"> 
        <input type="text" name="key88" size="10" maxlength="10" value="<%=dspkey(88)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>         
        <td width="15%" class="dataListHead" height="32">聯單號碼</td>
        <td width="30%"  height="32" bgcolor="silver"> 
        <input type="text" name="key89" size="12" maxlength="12" value="<%=dspkey(89)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>         
    </tr>
    <tr>                   
        <td width="15%" class="dataListHead" height="32">證券公司</td>                                      
        <td width="30%" height="32" bgcolor="silver">
<%  Call SrGetBRANCHBUSS(accessMode,sw,Len(Trim(FIELDROLE(1) &dataProtect)),dspkey(2),dspkey(3),dspkey(4),s,t,U)%>        
           <select size="1" name="key2" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> onChange="SrRenew()" class="dataListEntry">                                            
              <%=s%>
           </select>
           <select size="1" name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> onChange="SrRenew()" class="dataListEntry">                                            
              <%=T%>
           </select>           
        </td>                              
        <td width="8%" class="dataListHead" height="32">營業員</td>
        <td width="16%" height="32" bgcolor="silver">
           <select size="1" name="key4" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry">                                            
              <%=U%>
           </select> 
         <input type="button" id="B4"  name="B4"   width="100%" style="Z-INDEX: 1"  value="...." onclick="SrBusonclick()"  >           
        </td>     
<% 
   %>               
        <td width="8%" class="dataListHead" height="32">業務員</td>                              
        <td width="16%" height="32" bgcolor="silver">
      <input type="text" name="key5" size="6" maxlength="50" readonly value="<%=dspkey(5)%>" <%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" >
     <input type="button" id="B5"  name="B5"   width="100%" style="Z-INDEX: 1"  value="...." <%=fieldpd%>  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
        </TD>
        </TR>
      <tr>                                      
        <td width="15%" class="dataListHead" height="32">客戶名稱</td>                                      
        <td width="30%" height="32" bgcolor="silver">
          <input type="text" name="ext0" size="28" maxlength="50" value="<%=extdb(0)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                              
        <td width="8%" class="dataListHead" height="32">性別</td>
<%  dim sexd1, sexd2
    If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
       sexd1=""
       sexd2=""
    Else
       sexd1=" disabled "
       sexd2=" disabled "
    End If
    If dspKey(6)="M" Then sexd1=" checked "    
    If dspKey(6)="F" Then sexd2=" checked " %>                          
        <td width="16%" height="32" bgcolor="silver">
        <input type="radio" value="M" <%=sexd1%> name="key6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtec%>>男
        <input type="radio" name="key6" value="F" <%=sexd2%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>>女</td>                              
        <td width="8%" class="dataListHead" height="32">出生日期</td>                              
        <td width="16%" height="32" bgcolor="silver">
          <input type="text" name="key7" size="10" value="<%=dspkey(7)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class=dataListEntry>
          <input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>></td>                              
      </tr>                              
      <tr>                              
        <td width="15%" class="dataListHead" height="32">帳單(通訊)地址</td>                              
        <td width="60%" colspan="3" height="32" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(8))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX09=" onclick=""Srcounty9onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(8) & "' " 
       SXX09=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(8) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
         <select size="1" name="key8"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
         <input type="text" name="key9" size="8" value="<%=dspkey(9)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B9"  name="B9"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX09%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C9"  name="C9"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >       
        <input type="text" name="key10" size="41" value="<%=dspkey(10)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="32">郵遞區號</td>                                 
        <td width="16%" height="32" bgcolor="silver"><input type="text" name="key11" size="10" value="<%=dspkey(11)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="32">裝機地址</td>                                 
        <td width="60%" colspan="3" height="32" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(12))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX13=" onclick=""Srcounty13onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(12) & "' " 
       SXX13=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(12) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
         <select name="key12" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1"  style="text-align:left;" maxlength="8" class="dataListEntry">
        <%=s%></select> 
        <input type="text" name="key13" size="8" value="<%=dspkey(13)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B13"  name="B13"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX13%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C13"  name="C13"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >            
<input type="text" name="key14" size="41" value="<%=dspkey(14)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="32">郵遞區號</td>                                 
        <td width="16%" height="32" bgcolor="silver"><input type="text" name="key15" size="10" value="<%=dspkey(15)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>
      <tr>                                 
        <td width="15%" class="dataListHead" height="32">戶籍地址</td>                                 
        <td width="60%" colspan="3" height="32" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if    
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"        
       sxx17=" onclick=""Srcounty17onclick()"" "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(16) & "' " 
       sxx17=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(16) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <select name="key16" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>
        <input type="text" name="key17" size="8" value="<%=dspkey(17)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B17"  name="B17"   width="100%" style="Z-INDEX: 1"  value="..." <%=sxx17%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C17"  name="C17"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >    
<input type="text" name="key18" size="41" value="<%=dspkey(18)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="32">郵遞區號</td>                                 
        <td width="16%" height="32" bgcolor="silver"><input type="text" name="key19" size="10" value="<%=dspkey(19)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>                                
      <tr>          
<script language="vbscript">
Sub SrAddrEqual()
  Dim i,objOpt
  For i = 0 To document.All("key12").Options.Length-1
      document.All("key12").Remove 0
  Next
  For i = 0 To document.All("key8").Options.Length-1
      Set objOpt=Document.CreateElement("OPTION")
      objOpt.Text=document.All("key8").Options(i).Text
      objOpt.Value=document.All("key8").Options(i).Value
      document.All("key12").Add objOpt
  Next
  For i = 0 To document.All("key13").Options.Length-1
      document.All("key13").Remove 0
  Next
  For i = 0 To document.All("key9").Options.Length-1
      Set objOpt=Document.CreateElement("OPTION")
      objOpt.Text=document.All("key9").Options(i).Text
      objOpt.Value=document.All("key9").Options(i).Value
      document.All("key13").Add objOpt
  Next
  document.All("key12").value=document.All("key8").value
  document.All("key13").value=document.All("key9").value
  document.All("key14").value=document.All("key10").value
  document.All("key15").value=document.All("key11").value
End Sub 
Sub SrAddrEqual2()
  Dim i,objOpt
  For i = 0 To document.All("key16").Options.Length-1
      document.All("key16").Remove 0
  Next
  For i = 0 To document.All("key8").Options.Length-1
      Set objOpt=Document.CreateElement("OPTION")
      objOpt.Text=document.All("key8").Options(i).Text
      objOpt.Value=document.All("key8").Options(i).Value
      document.All("key16").Add objOpt
  Next
  For i = 0 To document.All("key17").Options.Length-1
      document.All("key17").Remove 0
  Next
  For i = 0 To document.All("key9").Options.Length-1
      Set objOpt=Document.CreateElement("OPTION")
      objOpt.Text=document.All("key9").Options(i).Text
      objOpt.Value=document.All("key9").Options(i).Value
      document.All("key17").Add objOpt
  Next
  document.All("key16").value=document.All("key8").value
  document.All("key17").value=document.All("key9").value
  document.All("key18").value=document.All("key10").value
  document.All("key19").value=document.All("key11").value
End Sub 
Sub SrAddUsr()
  ExistUsr=document.all("key70").value
  InsType=cstr(document.all("key69").value)
  UsrStr=Window.showModalDialog("RTCustAddUsr.asp?parm=" & existusr & "@" & instype   ,"Dialog","dialogWidth:410px;dialogHeight:400px;")
  if UsrStr<>False then
     UsrStrAry=split(UsrStr,"@")
     document.all("key70").value=UsrStrAry(0)
     document.all("REF01").value=UsrStrAry(1)     
  end if
End Sub

Sub Srpay()
  if document.all("key69").value = "1" then
     document.all("key74").value = 200
  else
     document.all("key74").value = 0
  end if
end sub
</script>                       
        <td width="35%" class="dataListHead" colspan="2" height="34" bgcolor="silver">
<%  dim seld1
    If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
       seld1=""
    Else
       seld1=" disabled "
    End If
    %>
            <input type="radio" name="rdo1" value="1"<%=seld1%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> 
                   onClick="SrAddrEqual()">裝機地址同帳單地址
            <input type="radio" name="rdo2" value="1"<%=seld1%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> 
                   onClick="SrAddrEqual2()">戶籍地址同帳單地址</td>                                 
        <td width="8%" class="dataListHead" height="23">申請速度</td>
<% aryOption=Array("512/64Kbps")
   s=""
   If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(20)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(20) &""">" &dspKey(20) &"</option>"
   End If%>                                      
        <td width="16%" height="23" bgcolor="silver"><select size="1" name="key20" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                             
        <%=s%></select></td>      
        <td width="8%" class="dataListHead" height="32">線路種類</td>
<% aryOption=Array("ADSL")
   s=""
   If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then   
      For i = 0 To Ubound(aryOption)
          If dspKey(21)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(21) &""">" &dspKey(21) &"</option>"
   End If%>                                  
        <td width="16%" height="32" bgcolor="silver"><select size="1" name="key21" style="font-family: 新細明體; font-size: 10pt"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                                  
        <%=s%></select></td>                                     
      </tr>                                 
      <tr>                            
        <td width="15%" class="dataListHead" height="32">申請方案</td>
 <td width="30%" height="32" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='B9' " 
       If len(trim(dspkey(22))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='B9' AND CODE='" & dspkey(22) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(22) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>                  
   <select size="1" name="key22" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select>
<% aryOption=Array("經濟型","單機型","商業型")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(23)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(23) &""">" &dspKey(23) &"</option>"
   End If%>               
   <select size="1" name="key23" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select>
   ＋
<% aryOption=Array("","先看先贏")
   s=""
   sx=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(78)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(78) &""">" &dspKey(78) &"</option>"
   End If%>                   
   <select size="1" name="key78" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select>   
   </td>                                     
        <td width="8%" class="dataListHead">意願表日期</td>                     
        <td width="16%"  bgcolor="silver">
          <input type="text" name="key24" size="10" value="<%=dspKey(24)%>"  <%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B24"  name="B24" height="100%" width="100%" style="Z-INDEX: 1" value="...."  <%=fieldpc%>></td> 
        <td width="8%"  class="dataListHead" height="34">回覆日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver"><input type="text" name="key79" size="10" value="<%=dspKey(79)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
                  <input type="button" id="B79"  name="B79" height="100%" width="100%" style="Z-INDEX: 1" value="...."  <%=fieldpc%>></td>       
      </tr>                     
      <tr>                            
        <td width="15%" class="dataListHead" height="32">堪查日期</td>
         <td width="30%" height="32" bgcolor="silver">       
          <input type="text" name="key80" size="10" value="<%=dspKey(80)%>"  <%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B80"  name="B80" height="100%" width="100%" style="Z-INDEX: 1" value="...."  <%=fieldpc%>>
          <%  dim rdo1, rdo2
              If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then
                 rdo1=""
                 rdo2=""
              Else
                 rdo1=" disabled "
                 rdo2=" disabled "
              End If
             ' If Trim(dspKey(84))="" Then dspKey()="Y"
              If trim(dspKey(84))="Y" Then rdo1=" checked "    
              If trim(dspKey(84))="N" Then rdo2=" checked " 
             %>
        <input type="radio" value="Y" <%=rdo1%> name="key84" <%=fieldRole(1)%><%=dataProtect%>>可建置
        <input type="radio" value="N" <%=rdo2%>  name="key84" <%=fieldRole(1)%><%=dataProtect%>>無法建置
          </td> 
        <td width="8%" class="dataListHead" height="32">堪查結果</td>
         <td width="16%"  height="32" bgcolor="silver">       
         <% aryOption=Array("","有電信室","無電信室","無電信箱")
            s=""
            If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then 
               For i = 0 To Ubound(aryOption)
                   If dspKey(85)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
               Next
            Else
                   s="<option value=""" &dspKey(85) &""">" &dspKey(85) &"</option>"
            End If%>               
         <select size="1" name="key85" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
           <%=s%>
         </select>
         <% aryOption=Array("","跨棟","獨棟","雙拼")
            s=""
            If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then 
               For i = 0 To Ubound(aryOption)
                   If dspKey(86)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
               Next
            Else
                   s="<option value=""" &dspKey(86) &""">" &dspKey(86) &"</option>"
            End If%>               
         <select size="1" name="key86" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
           <%=s%>
         </select>         
         </td>
          <td width="8%" class="dataListHead">正式申請日</td>                     
          <td width="16%"  bgcolor="silver">
          <input type="text" name="key81" size="10" value="<%=dspKey(81)%>"  <%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B81"  name="B81" height="100%" width="100%" style="Z-INDEX: 1" value="...."  <%=fieldpc%>></td> 
      </tr>
      <tr>
        <td width="15%"  class="dataListHead" height="34">堪查補充說明</td>  
        <td width="30%"  colspan="3" height="21" bgcolor="silver">
        <input type="text" name="key87" style="text-align:left;" maxlength="300" size="60"
               value="<%=dspKey(87)%>" class=dataListentry style="color:red">
        </td>
        <td width="8%"  class="dataListHead" height="34">送件日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key82" size="10" value="<%=dspKey(82)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B82"  name="B82" height="100%" width="100%" style="Z-INDEX: 1" value="...."  <%=fieldpc%>></td>       
      </tr>                                      
      <tr>                                    
        <td width="15%" class="dataListHead" height="21">住宅種類</td>                                    
        <td width="30%"  colspan="3" height="21" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C2' " 
       If len(trim(dspkey(25))) < 1 Then
          sx=" selected " 
        '  s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
        '  s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C2' AND CODE='" & dspkey(25) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(25) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key25" style="font-family: 新細明體; font-size: 10pt"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                                  
        <%=s%>
   </select>
   &nbsp;社區名稱<input type="text" name="key26" size="15" MAXLENGTH="30" value="<%=dspKey(26)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        <input type="button" id="B26"  name="B26"   width="100%" style="Z-INDEX: 1"  value="...." <%=fieldpf%>  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C26"  name="C26"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
   共<input type="text" name="key27" size="4" value="<%=dspKey(27)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> maxlength="4" class="dataListEntry">戶</td>                                 
              <td width="8%"  class="dataListHead" height="34">附掛電話</td>                                 
      <td width="16%"  height="34" bgcolor="silver"><input type="text" name="key28" size="15" value="<%=dspKey(28)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>    
      </tr>                                 
      <tr>                                    
        <td width="15%" class="dataListHead" height="23">聯絡電話</td>                                 
        <td width="30%" height="23"><input type="text" name="key29" size="15" value="<%=dspkey(29)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">傳真電話</td>                                 
        <td width="16%" height="23" bgcolor="silver"><input type="text" name="key30" size="15" value="<%=dspkey(30)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">聯絡人</td>                                 
        <td width="16%" height="23" bgcolor="silver"><input type="text" name="key31" size="10" value="<%=dspkey(31)%>" maxlength="20" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" bgcolor="silver">公司電話</td>                                 
        <td width="30%" height="23"><input type="text" name="key32" size="15" value="<%=dspkey(32)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">分機<input type="text" name="key33" size="5" value="<%=dspkey(33)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">行動電話</td>                                 
        <td width="16%"  height="23" bgcolor="silver"><input type="text" name="key34" size="15" value="<%=dspkey(34)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" height="23" bgcolor="orange" style="font-size:16px">計費起日</td>                     
        <% if len(trim(dspkey(82))) > 0 then
              k=Dateadd("m",3,dspkey(82))
           end if
        %>            
        <td width="16%" height="23" bgcolor="silver"><input type="text"  size="10" value="<%=k%>" readonly maxlength="20" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata"></td>                    
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="32">電子郵件信箱</td>                                 
        <td width="30%" height="32" bgcolor="silver"><input type="text" name="key35" size="30" value="<%=dspkey(35)%>" maxlength="30" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">收據名稱</td>                                 
        <td width="16%" colspan="3" height="23" bgcolor="silver"><input type="text" name="key36" size="15" value="<%=dspkey(36)%>" maxlength="50" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        <font size=2>(空白時預設為客戶名稱)</font></td>                   
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" style="display:none">輸入人員</td>                                 
        <td width="30%" height="23" bgcolor="silver" style="display:none"><input type="text" name="key37" size="10" class="dataListData" value="<%=dspKey(37)%>" readonly style="display:none"><%=EusrNc%></td>                                 
        <td width="8%" class="dataListHead" height="23" style="display:none">輸入日期</td>                                 
        <td width="40%" colspan="3" height="23" bgcolor="silver" style="display:none"><input type="text" name="key38" size="15" class="dataListData" value="<%=dspKey(38)%>" readonly style="display:none"></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" style="display:none">異動人員</td>                                 
        <td width="30%" height="23" bgcolor="silver" style="display:none"><input type="text" name="key39" size="10" class="dataListData" value="<%=dspKey(39)%>" readonly style="display:none"><%=UUsrNc%></td>                                 
        <td width="8%" class="dataListHead" height="23" style="display:none">異動日期</td>                                 
        <td width="40%" colspan="3" height="23" bgcolor="silver" style="display:none"><input type="text" name="key40" size="15" class="dataListData" value="<%=dspKey(40)%>" readonly style="display:none"></td>                                 
      </tr>
      <tr>
      <td bgcolor="orange" style="font-size:16px">
      每月費用
      </td>
      <%if len(trim(dspkey(78))) > 0 then
           K=599
        else
           k=399
        end if
      %>
      <td class="dataListData" style="font-size:16px;color:red"><%=K%></td>
      </tr>
    </table>                            
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none">                           
      <tr>                         
        <td width="20%" class="dataListHead">施工廠商</td>                     
        <td width="18%" bgcolor="silver">
<%
    If (sw="E" Or (accessMode="A" And sw="")) And Len(Trim(fieldPa &fieldPb &fieldRole(1) &dataProtect))<1 Then 
       sql="SELECT RTSuppCty.CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTSuppCty ON RTObj.CUSID = RTSuppCty.CUSID INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID RIGHT OUTER JOIN " _
          &"singlecustadsl ON RTSuppCty.CUTID = singlecustadsl.CUTID2 " _
          &"WHERE (RTObjLink.CUSTYID = '04') and singlecustadsl.cusid='" & dspkey(0) & "'"
    Else
       sql="SELECT RTObj.CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN RTSupp ON RTObj.CUSID = RTSupp.CUSID " _
          &"WHERE RTSupp.CUSID='" &dspKey(41) &"' "
    End If
  '  Response.Write "SQL=" & SQL & "<BR>"
    rs.Open sql,conn
    s=""
    If rs.Eof Then 
       s="<option value="""" selected>&nbsp;</option>"
    else
       sx=""
       s="<option value="""">&nbsp;</option>" & vbcrlf      
       Do While Not rs.Eof
          If rs("CusID")=dspKey(41) Then sx=" selected "
          s=s &"<option value=""" &rs("CusID") &"""" &sx &">" &rs("SHORTNC") &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
       Loop
    end if
    rs.Close
%>
        <select name="key41" <%=fieldRole(1)%><%=dataProtect%><%=fieldPa%><%=fieldPb%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select></td> 
        <td width="17%" class="dataListHead">通知發包日期</td>                     
        <td width="17%" colspan="1" bgcolor="silver">
          <input type="text" name="key42" size="10" value="<%=dspKey(42)%>" readonly <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%> class="dataListdata" maxlength="10"></td>                                               
        <td width="14%" class="dataListHead">發包日期</td>                     
        <td width="18%" colspan="1" bgcolor="silver">
          <input type="text" name="key43" size="10" value="<%=dspKey(43)%>" <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10">
          <input type="button" id="B43"  name="B43" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>>          </td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">安裝表批號</td>                    
        <td width="18%" bgcolor="silver"><input type="text" name="key44" size="10" class="dataListData" value="<%=dspKey(44)%>" readonly></td>                     
        <td width="17%" class="dataListHead">安裝表列印日</td>                     
        <td width="17%" bgcolor="silver"><input type="text" name="key45" size="10" class="dataListData" value="<%=dspKey(45)%>" readonly></td>                     
        <td width="14%" class="dataListHead">列印人員</td>                     
        <td width="18%" bgcolor="silver"><input type="text" name="key46" size="10" class="dataListData" value="<%=dspKey(46)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">完工日期</td>                    
        <td width="18%" bgcolor="silver">
          <input type="text" name="key47" size="10" value="<%=dspKey(47)%>" <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="17%" class="dataListHead">報竣日期</td>   
        <td width="18%" bgcolor="silver">                  
         <input type="text" name="key48" size="10" value="<%=dspKey(48)%>" <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10">
          <input type="button" id="B48"  name="B48" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>></td>                     
        <td width="14%" class="dataListHead">入帳日期</td>                     
        <td width="18%">
          <input type="text" name="key49" size="10" value="<%=dspKey(49)%>"   class="dataListdata" readonly maxlength="10">
          <input type="button" id="B49"  name="B49" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">應收金額</td>                    
        <td width="18%" bgcolor="silver">
          <input type="text" name="key50" size="10" value="<%=dspKey(50)%>" <%=fieldPa%><%=fieldPb%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="17%" class="dataListHead">實收金額</td>                     
        <td width="17%" bgcolor="silver">
        <input type="text" name="key51" size="10" value="<%=dspKey(51)%>" <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="14%" class="dataListHead">撤銷日期</td>                     
        <td width="18%" bgcolor="silver">
          <input type="text" name="key52" size="10" value="<%=dspKey(52)%>" <%=fieldPa%><%=fieldPb%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B52"  name="B52" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">收款表批號</td>                    
        <td width="18%" bgcolor="silver"><input type="text" name="key53" size="10" class="dataListData" value="<%=dspKey(53)%>" readonly></td>                     
        <td width="17%" class="dataListHead">列印人員</td>                     
        <td width="17%" bgcolor="silver"><input type="text" name="key54" size="10" class="dataListData" value="<%=dspKey(54)%>" readonly></td>                     
        <td width="14%" class="dataListHead">收款日期</td>                     
        <td width="18%" bgcolor="silver">
         <input type="text" name="key55" size="10" value="<%=dspKey(55)%>" <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" maxlength="10">
          <input type="button" id="B55"  name="B55" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">財務收款確認日</td>                    
        <td width="18%" bgcolor="silver"><input type="text" name="key56" size="10" class="dataListData" value="<%=dspKey(56)%>" readonly></td>                     
        <td width="17%" class="dataListHead">財務確認人員</td>                     
        <td width="17%" bgcolor="silver"><input type="text" name="key57" size="10" class="dataListData" value="<%=dspKey(57)%>" readonly></td>                     
        <td width="14%" class="dataListHead">獎金計算日期</td>                     
        <td width="18%" bgcolor="silver">
          <input type="text" name="key58" size="10" value="<%=dspKey(58)%>" readonly  class="dataListdata" maxlength="10"></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">撤銷原因說明</td>                    
        <td width="83%" colspan="5" bgcolor="silver">
          <input type="text" name="key59" size="70" value="<%=dspKey(59)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">未完工原因</td>                    
        <td width="83%" colspan="5" bgcolor="silver">
          <input type="text" name="key60" size="70" value="<%=dspKey(60)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">付款表批號</td>                    
        <td width="18%" bgcolor="silver"><input type="text" name="key61" size="10" class="dataListData" value="<%=dspKey(61)%>" readonly></td>                     
        <td width="17%" class="dataListHead">付款表日期</td>                     
        <td width="17%" bgcolor="silver"><input type="text" name="key62" size="10" class="dataListData" value="<%=dspKey(62)%>" readonly></td>                     
        <td width="14%" class="dataListHead">列印人員</td>                     
        <td width="18%" bgcolor="silver"><input type="text" name="key63" size="10" class="dataListData" value="<%=dspKey(63)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">付款會計審核確認日</td>                    
        <td width="18%" bgcolor="silver"><input type="text" name="key64" size="10" class="dataListData" value="<%=dspKey(64)%>" readonly></td>                     
        <td width="17%" class="dataListHead">會計審核人員</td>                     
        <td width="17%" bgcolor="silver"><input type="text" name="key65" size="10" class="dataListData" value="<%=dspKey(65)%>" readonly></td>                     
        <td width="14%" class="dataListHead">結案碼</td>                     
        <td width="18%" bgcolor="silver"><input type="text" name="key66" size="10" class="dataListData" value="<%=dspKey(66)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">施工備註說明</td>                    
        <td width="83%" colspan="5" bgcolor="silver">
          <input type="text" name="key67" size="72" value="<%=dspKey(67)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">施工環境代碼</td>                    
        <td width="18%" bgcolor="silver">
        <%
    If (sw="E" Or (accessMode="A" And sw="")) And Len(Trim(fieldPa  &FIELDROLE(1) &dataProtect))<1 Then 
       sql="SELECT code, codenc " _
          &"FROM RTcode where kind='C4' " 
    Else
       sql="SELECT code, codenc " _
          &"FROM RTcode where kind='C4' and code='" &dspKey(68) &"' "
    End If
    rs.Open sql,conn
    s=""
    If rs.Eof Then s="<option value="""" selected>&nbsp;</option>"
    sx=""
    Do While Not rs.Eof
       If rs("code")=dspKey(68) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
        <select name="key68" <%=FIELDROLE(1)%><%=dataProtect%><%=fieldPa%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>

        <td width="17%" class="dataListHead">安裝員類別</td>
<%' if userlevel=1 then
  '    aryOption=Array("","業務自行安裝","技術部安裝")      
  '    aryOptionV=Array("0","1","2")
  ' elseif userlevel=4 then
  '    aryOption=Array("","技術部安裝","發包")
  '    aryOptionV=Array("0","2","3")
  ' elseif userlevel=31 then
      aryOption=Array("","業務自行安裝","技術部安裝","發包")
      aryOptionV=Array("0","1","2","3")   
  ' end if
   s=""
   If Len(Trim(fieldPa &fieldRole(1) &dataProtect)) > 0 Then
      s="<option value=""" &dspKey(69) &""">" &aryOption(dspKey(69)) &"</option>"
   Else
      For i = 0 To Ubound(aryOption)
          If dspKey(69)=aryOptionV(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOptionV(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   End If%>                    
        <td width="17%" bgcolor="silver"><select size="1" onChange="Srpay()" name="key69" <%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <%=s%></select></td>                     
        <td width="14%" class="dataListHead">
        <input type="button" name="EMPLOY" <%=fieldPa%><%=fieldPb%> class=keyListButton onclick="SrAddUsr()" value="裝機員工"></td>                     
        <td width="18%" bgcolor="silver">
  <% 
    Usrary=split(dspkey(70),";")
    qrystrng=""
    s1=""
    existusr=""
    if Ubound(Usrary) >= 0 then
       existUsr="("
       for i=0 to Ubound(usrary)
           existUsr=existUsr & "'" & usrary(i) & "',"
       next
       existUsr=mid(existUsr,1,len(existUsr)-1)
       existUsr=existUsr & ")"
       qrystring=" and rtemployee.emply in " & existusr
    end if
    if len(trim(qrystring)) < 1 then
       qrystring=" and rtemployee.emply='*' "
    end if
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08'" _
          & qrystring
    rs.Open sql,conn
    Do While Not rs.Eof
       s1=s1 & rs("cusnc") & ";"
       rs.MoveNext
    Loop
    if trim(len(s1)) > 0 then 
       s1=mid(s1,1,len(s1)-1)
    else
       dspkey(70)=""
       s1=""
    end if 
    rs.Close
    conn.Close   
    set rs=Nothing   
    set conn=Nothing
   %>       
          <input type="text" name="key70" size="14" value="<%=dspKey(70)%>"  class="dataListData"  readonly maxlength="50" style="display:none">
          <input type="text" name="ref01" size="10" value="<%=S1%>"  class="dataListData"  readonly maxlength="50">
          </td>                   
      </tr>                                     
      <tr>            
        <td width="20%" class="dataListHead">預定裝機日期</td>                    
        <td width="18%" bgcolor="silver">
          <input type="text" name="key71" size="10" value="<%=dspKey(71)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10">
          <input type="button" id="B71"  name="B71" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpc%>></td>                     
        <td width="17%" class="dataListHead">預定裝機時間(時)</td>                     
        <td width="17%" bgcolor="silver">
          <input type="text" name="key72" size="10" value="<%=dspKey(72)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="2"></td>                     
        <td width="14%" class="dataListHead">預定裝機時間(分)</td>                     
        <td width="18%" bgcolor="silver">
          <input type="text" name="key73" size="10" value="<%=dspKey(73)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="2"></td>                   
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">標準施工費</td>                    
        <td width="18%" bgcolor="silver">
        <input type="text" name="key74" size="10" class="dataListData" value="<%=dspKey(74)%>" readonly ></td>                     
        <td width="17%" class="dataListHead">施工補助費</td>                     
        <td width="17%" bgcolor="silver">
        <input type="text" name="key75" size="10" value="<%=dspKey(75)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="15"></td>                     
        <td width="32%" colspan="2">　</td>                     
      </tr>                                     
      <tr>                       
        <td width="20%" class="dataListHead">施工補助費說明</td>                    
        <td width="83%" colspan="5" bgcolor="silver">
          <input type="text" name="key76" size="70" value="<%=dspKey(76)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="25"></td>                     
      </tr>                                     
    </table>
<table width="100%"><tr><td width="100%">&nbsp;</td></tr>                                                                                                   
  </div>                               
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    rs.Open "SELECT * FROM RTObj WHERE CusID='" &dspKey(0) &"' ",conn
    extDB(0)=rs("CusNC")
   ' extDB(1)=rs("CutID1")
   ' extDB(2)=rs("TownShip1")
   ' extDB(3)=rs("RAddr1")
   ' extDB(4)=rs("RZone1")
   ' extDB(5)=rs("CutID2")
   ' extDB(6)=rs("TownShip2")
   ' extDB(7)=rs("RAddr2")
   ' extDB(8)=rs("RZone2")
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
'------ RTObj ---------------------------------------------------
    rs.Open "SELECT * FROM RTObj WHERE CusID='" &dspKey(0) &"' ",conn,3,3
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("CusID")=dspKey(0)
       End If
    End If

    rs("CusNC")=extDB(0)
    rs("ShortNC")=Left(extDB(0),5)
   ' rs("CutID1")=extDB(1)
   ' rs("TownShip1")=extDB(2)
   ' rs("RAddr1")=extDB(3)
   ' rs("RZone1")=extDB(4)
   ' rs("CutID2")=extDB(5)
   ' rs("TownShip2")=extDB(6)
   ' rs("RAddr2")=extDB(7)
   ' rs("RZone2")=extDB(8)
    rs("Eusr")=""
    rs("Edat")=now()
    rs("Uusr")=""
    rs("Udat")=now()
    rs.Update
    rs.Close
'------ RTObjLink -----------------------------------------------
    rs.Open "SELECT * FROM RTObjLink WHERE CustYID='05' AND CusID='" &dspKey(0) &"' ",conn,3,3
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("CusID")=dspKey(0)
          rs("CustYID")="05"
       End If
    End If
    rs("Eusr")=""
    rs("Edat")=now()
    rs("Uusr")=""
    rs("Udat")=now()
    rs.Update
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include file="rtgetBRANCHBUSS.inc" -->
<!-- #include file="RTGetCountyTownShip.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->