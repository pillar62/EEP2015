<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTEBTCUSTCHG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=" & key(3) 
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   CHGCOD1=RSXX("CHGCOD1")
   CHGCOD2=RSXX("CHGCOD2")
   CHGCOD3=RSXX("CHGCOD3")
   CHGCOD4=RSXX("CHGCOD4")
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("FINISHDAT"))) <> 0 then
      endpgm="4"
   elseif RSXX("CHGCOD2") <> 0 then
      endpgm="5"      
   ELSE
      sqlyy="select max(SEQ) as SEQ FROM RTEBTCUSTCHGlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=" & key(3) 
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("SEQ"))) > 0 then
         SEQ=rsyy("SEQ") + 1
      else
         SEQ=1
      end if
      rsyy.close
    
     '產生RTEBTCUST異動記錄資料
      sqlyy="insert into RTEBTCUSTCHGlog " _
           &"SELECT " _
           &"COMQ1, LINEQ1, CUSID, ENTRYNO, " & SEQ & ", GETDATE(), 'F','" & V(0) & "',  CHGCOD1, CHGCOD2, CHGCOD3, CHGCOD4, CHGCOD4OPT, APPLYDAT, " _
           &"NCUSNC, NSOCIALID, NCUTID2, NTOWNSHIP2, NVILLAGE2, NCOD21,  NNEIGHBOR2, NCOD22, NSTREET2, NCOD23, NSEC2, NCOD24, NLANE2, " _
           &"NCOD25, NTOWN2, NALLEYWAY2, NCOD26, NNUM2, NCOD27, NFLOOR2,  NCOD28, NROOM2, NCOD29, NBIRTHDAY, NCUTID3, NTOWNSHIP3, " _
           &"NVILLAGE3, NCOD31, NNEIGHBOR3, NCOD32, NSTREET3, NCOD33, NSEC3,NCOD34, NLANE3, NCOD35, NTOWN3, NALLEYWAY3, NCOD36, NNUM3, " _
           &"NCOD37, NFLOOR3, NCOD38, NROOM3, NCOD39, NEMAIL, NCONTACT, NCONTACTTEL, NMOBILE, DROPDAT, DROPUSR, TRANSCHKDAT, TRANSDAT, " _
           &"TRANSNO, EBTREPLYDAT, EBTREPLYCOD, EUSR, EDAT, UUSR, UDAT, FINISHDAT, FINISHCHKDAT, FINISHTNSDAT, FINISHTNSNO, NCOMQ1, " _
           &"NLINEQ1, NCUTID1, NTOWNSHIP1, NVILLAGE1, NCOD11, NNEIGHBOR1, NCOD12, NSTREET1, NCOD13, NSEC1, NCOD14, NLANE1, NCOD15, NTOWN1, " _
           &"NALLEYWAY1, NCOD16, NNUM1, NCOD17, NFLOOR1, NCOD18, NROOM1, NCOD19, NRZONE1, NRZONE2, NRZONE3, NTELNO " _
           &"FROM RTEBTCUSTCHG where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=" & key(3) 
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      '-----------------

      '----------------------      
   '   set rsyy=nothing
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCUSTCHG set FINISHdat=getdate() where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=" & key(3) 
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCUSTCHGlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=" & key(3) & " AND SEQ=" & SEQ
            CONNXX.Execute sqlyy 
         else
            '檢核正確--可執行異動完工更新作業
            '更新前先產生異動記錄
            IF CHGCOD1=1 AND CHGCOD2=1 AND CHGCOD3=1 THEN 
               CHGCOD="AL"
            ELSEIF CHGCOD1=1 AND CHGCOD2=1 AND CHGCOD3<>1 THEN 
               CHGCOD="DM"
            ELSEIF CHGCOD1=1 AND CHGCOD2<>1 AND CHGCOD3=1 THEN 
               CHGCOD="DT"
            ELSEIF CHGCOD1<>1 AND CHGCOD2=1 AND CHGCOD3=1 THEN 
               CHGCOD="MT"
            ELSEIF CHGCOD1=1 AND CHGCOD2<>1 AND CHGCOD3<>1 THEN 
               CHGCOD="CD"
            ELSEIF CHGCOD1<>1 AND CHGCOD2=1 AND CHGCOD3<>1 THEN 
               CHGCOD="CM"
            ELSEIF CHGCOD1=1 AND CHGCOD2<>1 AND CHGCOD3=1 THEN 
               CHGCOD="CT"                                             
            END IF
            sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCUSTlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
            rsyy.Open sqlyy,connxx
      
            if len(trim(rsyy("ENTRYNO"))) > 0 then
               ENTRYNO=rsyy("ENTRYNO") + 1
            else
               ENTRYNO=1
            end if
            rsyy.close
            sqlyy="insert into RTEBTCUSTlog " _
                 &"SELECT " _
                 &"COMQ1, LINEQ1, CUSID, " & ENTRYNO & ", GETDATE(),'" & CHGCOD & "','" & V(0) & "'," _  
                 &"CUSNC, SOCIALID, CUTID1, TOWNSHIP1, VILLAGE1, " _
                 &"COD11, NEIGHBOR1, COD12, STREET1, COD13, SEC1, COD14, LANE1, COD15, " _
                 &"TOWN1, ALLEYWAY1, COD16, NUM1, COD17, FLOOR1, COD18, ROOM1, " _
                 &"COD19, CUTID2, TOWNSHIP2, VILLAGE2, COD21, NEIGHBOR2, COD22, " _
                 &"STREET2, COD23, SEC2, COD24, LANE2, COD25, TOWN2, ALLEYWAY2, " _
                 &"COD26, NUM2, COD27, FLOOR2, COD28, ROOM2, COD29, CUTID3, " _
                 &"TOWNSHIP3, VILLAGE3, COD31, NEIGHBOR3, COD32, STREET3, COD33, SEC3, " _
                 &"COD34, LANE3, COD35, TOWN3, ALLEYWAY3, COD36, NUM3, COD37, FLOOR3, " _
                 &"COD38, ROOM3, COD39, BIRTHDAY, CONTACT, MOBILE, EMAIL, CONTACTTEL, " _
                 &"PAYTYPE, AVSPMCODE, DIALERPMCODE, DIALERPAYTYPE, AGENTNAME, " _
                 &"AGENTTEL, AGENTSOCIAL, CUTID4, TOWNSHIP4, VILLAGE4, NEIGHBOR4, " _
                 &"STREET4, SEC4, LANE4, TOWN4, ALLEYWAY4, NUM4, FLOOR4, ROOM4, " _
                 &"RCVD, APPLYDAT, APPLYTNSDAT, APPLYAGREE, PROGRESSID, FINISHDAT, " _
                 &"DOCKETDAT, TRANSDAT, STRBILLINGDAT, RZONE1, RZONE2, RZONE3, " _
                 &"RZONE4, AREAID, GROUPID, SALESID, COD41, COD42, COD43, COD44, COD45, " _
                 &"COD46, COD47, COD48, COD49, DROPDAT, FREECODE, OVERDUE, " _
                 &"CUSTAPPLYDAT, OLDSERVICECUTDAT, AVSNO, EUSR, EDAT, UUSR, UDAT, " _
                 &"TRANSNOAPPLY, TRANSNODOCKET, CASETYPE, MEMO, CANCELDAT, " _
                 &"CANCELUSR, TNSCUSTCASE, MOVETOCOMQ1, MOVETOLINEQ1, " _
                 &"MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVETODAT, MOVEFROMDAT, " _ 
                 &"ENDBILLINGDAT " _
                 &"FROM RTEBTCUST where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "  
            connxx.Execute SQLYY        
            '(變更用戶資料及換號)
            IF CHGCOD1 = 1 AND CHGCOD3=1 THEN
               '更新用戶名稱
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CUSNC=RTEBTCUSTCHG.NCUSNC " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCUSNC <> '' " 
               connxx.Execute SQLXX
               '更新出生日期
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.BIRTHDAY=RTEBTCUSTCHG.NBIRTHDAY " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NBIRTHDAY <> '' "                     
               connxx.Execute SQLXX
               '更新email
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.EMAIL=RTEBTCUSTCHG.NEMAIL " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NEMAIL <> '' "                     
               connxx.Execute SQLXX
               '更新聯絡人姓名
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CONTACT=RTEBTCUSTCHG.NCONTACT " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCONTACT <> '' "                     
               connxx.Execute SQLXX
               '更新聯絡人電話
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CONTACTTEL=RTEBTCUSTCHG.NCONTACTTEL " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCONTACTTEL <> '' "                     
               connxx.Execute SQLXX
               '更新行動電話
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.MOBILE=RTEBTCUSTCHG.NMOBILE " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NMOBILE <> '' "                     
               connxx.Execute SQLXX
                 '更新身份證號
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.SOCIALID=RTEBTCUSTCHG.NSOCIALID " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3)  & " AND RTEBTCUSTCHG.NSOCIALID <> '' " 
               connxx.Execute SQLXX  
                 '更新戶籍地址
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CUTID2=RTEBTCUSTCHG.NCUTID2," _
                    &"RTEBTCUST.TOWNSHIP2=RTEBTCUSTCHG.NTOWNSHIP2,RTEBTCUST.VILLAGE2=RTEBTCUSTCHG.NVILLAGE2, " _
                    &"RTEBTCUST.COD21=RTEBTCUSTCHG.NCOD21, RTEBTCUST.NEIGHBOR2= RTEBTCUSTCHG.NNEIGHBOR2,RTEBTCUST.COD22=RTEBTCUSTCHG.NCOD22," _
                    &"RTEBTCUST.STREET2=RTEBTCUSTCHG.NSTREET2,RTEBTCUST.COD23=RTEBTCUSTCHG.NCOD23,RTEBTCUST.SEC2=RTEBTCUSTCHG.NSEC2," _
                    &"RTEBTCUST.COD24=RTEBTCUSTCHG.NCOD24,RTEBTCUST.LANE2=RTEBTCUSTCHG.NLANE2, RTEBTCUST.COD25= RTEBTCUSTCHG.NCOD25," _
                    &"RTEBTCUST.TOWN2=RTEBTCUSTCHG.NTOWN2, RTEBTCUST.ALLEYWAY2= RTEBTCUSTCHG.NALLEYWAY2,RTEBTCUST.COD26=RTEBTCUSTCHG.NCOD26," _
                    &"RTEBTCUST.NUM2=RTEBTCUSTCHG.NNUM2,RTEBTCUST.COD27=RTEBTCUSTCHG.NCOD27,RTEBTCUST.FLOOR2=RTEBTCUSTCHG.NFLOOR2," _
                    &"RTEBTCUST.COD28=RTEBTCUSTCHG.NCOD28,RTEBTCUST.ROOM2=RTEBTCUSTCHG.NROOM2,RTEBTCUST.COD29=RTEBTCUSTCHG.NCOD29 " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCUTID2 <> '' "             
               connxx.Execute SQLXX  
                 '更新帳寄地址
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CUTID3=RTEBTCUSTCHG.NCUTID3," _
                    &"RTEBTCUST.TOWNSHIP3=RTEBTCUSTCHG.NTOWNSHIP3,RTEBTCUST.VILLAGE3=RTEBTCUSTCHG.NVILLAGE3, " _
                    &"RTEBTCUST.COD31=RTEBTCUSTCHG.NCOD31, RTEBTCUST.NEIGHBOR3= RTEBTCUSTCHG.NNEIGHBOR3,RTEBTCUST.COD32=RTEBTCUSTCHG.NCOD32," _
                    &"RTEBTCUST.STREET3=RTEBTCUSTCHG.NSTREET3,RTEBTCUST.COD33=RTEBTCUSTCHG.NCOD33,RTEBTCUST.SEC3=RTEBTCUSTCHG.NSEC3," _
                    &"RTEBTCUST.COD34=RTEBTCUSTCHG.NCOD34,RTEBTCUST.LANE3=RTEBTCUSTCHG.NLANE3, RTEBTCUST.COD35= RTEBTCUSTCHG.NCOD35," _
                    &"RTEBTCUST.TOWN3=RTEBTCUSTCHG.NTOWN3, RTEBTCUST.ALLEYWAY3= RTEBTCUSTCHG.NALLEYWAY3,RTEBTCUST.COD36=RTEBTCUSTCHG.NCOD36," _
                    &"RTEBTCUST.NUM3=RTEBTCUSTCHG.NNUM3,RTEBTCUST.COD37=RTEBTCUSTCHG.NCOD37,RTEBTCUST.FLOOR3=RTEBTCUSTCHG.NFLOOR3," _
                    &"RTEBTCUST.COD38=RTEBTCUSTCHG.NCOD38,RTEBTCUST.ROOM3=RTEBTCUSTCHG.NROOM3,RTEBTCUST.COD39=RTEBTCUSTCHG.NCOD39 " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCUTID3 <> '' "             
                connxx.Execute SQLXX
                 '更新附掛電話
                 '更新附掛前先產生異動記錄檔
                sqlyy="select max(SEQ) as SEQ FROM RTEBTCUSTEXTlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=1 "
                rsyy.Open sqlyy,connxx
                if len(trim(rsyy("SEQ"))) > 0 then
                   SEQ=rsyy("SEQ") + 1
                else
                   SEQ=1
                end if
                rsyy.close      
                '產生RTEBTCUSTEXT異動記錄資料
                sqlyy="insert into RTEBTCUSTEXTlog " _
                     &"SELECT " _
                     &"COMQ1, LINEQ1, CUSID, ENTRYNO, " & SEQ & ", GETDATE(), 'F','" & V(0) & "', TELNO, DIALERPAYTYPE, SRVTYPE, SDATE, DROPDAT, CHKDAT, TRANSDAT, EUSR, EDAT, UUSR, UDAT, CBCCONTRACTNO, EMCONTRACTNO " _
                     &"FROM RTEBTCUSTEXT where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=1 "  
               ' Response.Write sqlyy
                CONNXX.Execute sqlyy     
                '更新                 
                SQLXX=" update RTEBTCUSTEXT set   " _
                    &"RTEBTCUSTEXT.TELNO=RTEBTCUSTCHG.NTELNO " _
                    &"FROM " _
                    &"RTEBTCUSTEXT INNER JOIN RTEBTCUSTCHG ON RTEBTCUSTEXT.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUSTEXT.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUSTEXT.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3)  & " AND RTEBTCUSTCHG.NTELNO <> '' " 
                connxx.Execute SQLXX   
                
            '(換號)                    
            ELSEIF CHGCOD1 <> 1 AND CHGCOD3 = 1 THEN
                 '更新附掛電話
                 '更新附掛前先產生異動記錄檔
                sqlyy="select max(SEQ) as SEQ FROM RTEBTCUSTEXTlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=1 "
                rsyy.Open sqlyy,connxx
                if len(trim(rsyy("SEQ"))) > 0 then
                   SEQ=rsyy("SEQ") + 1
                else
                   SEQ=1
                end if
                rsyy.close      
                '產生RTEBTCUSTEXT異動記錄資料
                sqlyy="insert into RTEBTCUSTEXTlog " _
                     &"SELECT " _
                     &"COMQ1, LINEQ1, CUSID, ENTRYNO, " & SEQ & ", GETDATE(), 'F','" & V(0) & "', TELNO, DIALERPAYTYPE, SRVTYPE, SDATE, DROPDAT, CHKDAT, TRANSDAT, EUSR, EDAT, UUSR, UDAT, CBCCONTRACTNO, EMCONTRACTNO " _
                     &"FROM RTEBTCUSTEXT where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=1 "  
               ' Response.Write sqlyy
                CONNXX.Execute sqlyy     
                '更新附掛電話                 
                SQLXX=" update RTEBTCUSTEXT set   " _
                    &"RTEBTCUSTEXT.TELNO=RTEBTCUSTCHG.NTELNO " _
                    &"FROM " _
                    &"RTEBTCUSTEXT INNER JOIN RTEBTCUSTCHG ON RTEBTCUSTEXT.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUSTEXT.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUSTEXT.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3)  & " AND RTEBTCUSTCHG.NTELNO <> '' " 
                connxx.Execute SQLXX    
            '(變更用戶資料)                   
            ELSEIF CHGCOD1 = 1 AND CHGCOD3 <> 1 THEN
               '更新用戶名稱
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CUSNC=RTEBTCUSTCHG.NCUSNC " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCUSNC <> '' " 
               connxx.Execute SQLXX
               '更新出生日期
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.BIRTHDAY=RTEBTCUSTCHG.NBIRTHDAY " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NBIRTHDAY <> '' "                     
               connxx.Execute SQLXX
               '更新email
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.EMAIL=RTEBTCUSTCHG.NEMAIL " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NEMAIL <> '' "                     
               connxx.Execute SQLXX
               '更新聯絡人姓名
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CONTACT=RTEBTCUSTCHG.NCONTACT " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCONTACT <> '' "                     
               connxx.Execute SQLXX
               '更新聯絡人電話
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CONTACTTEL=RTEBTCUSTCHG.NCONTACTTEL " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCONTACTTEL <> '' "                     
               connxx.Execute SQLXX
               '更新行動電話
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.MOBILE=RTEBTCUSTCHG.NMOBILE " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NMOBILE <> '' "                     
               connxx.Execute SQLXX
                 '更新身份證號
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.SOCIALID=RTEBTCUSTCHG.NSOCIALID " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3)  & " AND RTEBTCUSTCHG.NSOCIALID <> '' " 
               connxx.Execute SQLXX  
                 '更新戶籍地址
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CUTID2=RTEBTCUSTCHG.NCUTID2," _
                    &"RTEBTCUST.TOWNSHIP2=RTEBTCUSTCHG.NTOWNSHIP2,RTEBTCUST.VILLAGE2=RTEBTCUSTCHG.NVILLAGE2, " _
                    &"RTEBTCUST.COD21=RTEBTCUSTCHG.NCOD21, RTEBTCUST.NEIGHBOR2= RTEBTCUSTCHG.NNEIGHBOR2,RTEBTCUST.COD22=RTEBTCUSTCHG.NCOD22," _
                    &"RTEBTCUST.STREET2=RTEBTCUSTCHG.NSTREET2,RTEBTCUST.COD23=RTEBTCUSTCHG.NCOD23,RTEBTCUST.SEC2=RTEBTCUSTCHG.NSEC2," _
                    &"RTEBTCUST.COD24=RTEBTCUSTCHG.NCOD24,RTEBTCUST.LANE2=RTEBTCUSTCHG.NLANE2, RTEBTCUST.COD25= RTEBTCUSTCHG.NCOD25," _
                    &"RTEBTCUST.TOWN2=RTEBTCUSTCHG.NTOWN2, RTEBTCUST.ALLEYWAY2= RTEBTCUSTCHG.NALLEYWAY2,RTEBTCUST.COD26=RTEBTCUSTCHG.NCOD26," _
                    &"RTEBTCUST.NUM2=RTEBTCUSTCHG.NNUM2,RTEBTCUST.COD27=RTEBTCUSTCHG.NCOD27,RTEBTCUST.FLOOR2=RTEBTCUSTCHG.NFLOOR2," _
                    &"RTEBTCUST.COD28=RTEBTCUSTCHG.NCOD28,RTEBTCUST.ROOM2=RTEBTCUSTCHG.NROOM2,RTEBTCUST.COD29=RTEBTCUSTCHG.NCOD29 " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCUTID2 <> '' "             
               connxx.Execute SQLXX  
                 '更新帳寄地址
               SQLXX=" update RTEBTCUST set   " _
                    &"RTEBTCUST.CUTID3=RTEBTCUSTCHG.NCUTID3," _
                    &"RTEBTCUST.TOWNSHIP3=RTEBTCUSTCHG.NTOWNSHIP3,RTEBTCUST.VILLAGE3=RTEBTCUSTCHG.NVILLAGE3, " _
                    &"RTEBTCUST.COD31=RTEBTCUSTCHG.NCOD31, RTEBTCUST.NEIGHBOR3= RTEBTCUSTCHG.NNEIGHBOR3,RTEBTCUST.COD32=RTEBTCUSTCHG.NCOD32," _
                    &"RTEBTCUST.STREET3=RTEBTCUSTCHG.NSTREET3,RTEBTCUST.COD33=RTEBTCUSTCHG.NCOD33,RTEBTCUST.SEC3=RTEBTCUSTCHG.NSEC3," _
                    &"RTEBTCUST.COD34=RTEBTCUSTCHG.NCOD34,RTEBTCUST.LANE3=RTEBTCUSTCHG.NLANE3, RTEBTCUST.COD35= RTEBTCUSTCHG.NCOD35," _
                    &"RTEBTCUST.TOWN3=RTEBTCUSTCHG.NTOWN3, RTEBTCUST.ALLEYWAY3= RTEBTCUSTCHG.NALLEYWAY3,RTEBTCUST.COD36=RTEBTCUSTCHG.NCOD36," _
                    &"RTEBTCUST.NUM3=RTEBTCUSTCHG.NNUM3,RTEBTCUST.COD37=RTEBTCUSTCHG.NCOD37,RTEBTCUST.FLOOR3=RTEBTCUSTCHG.NFLOOR3," _
                    &"RTEBTCUST.COD38=RTEBTCUSTCHG.NCOD38,RTEBTCUST.ROOM3=RTEBTCUSTCHG.NROOM3,RTEBTCUST.COD39=RTEBTCUSTCHG.NCOD39 " _
                    &"FROM " _
                    &"RTEBTCUST INNER JOIN RTEBTCUSTCHG ON RTEBTCUST.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
                    &"RTEBTCUST.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUST.CUSID = RTEBTCUSTCHG.CUSID " _
                    &"where RTEBTCUSTCHG.comq1=" & KEY(0) & " and RTEBTCUSTCHG.lineq1=" & key(1) & " AND RTEBTCUSTCHG.CUSID='" & KEY(2) & "' AND RTEBTCUSTCHG.ENTRYNO=" & KEY(3) & " AND RTEBTCUSTCHG.NCUTID3 <> '' "             
                connxx.Execute SQLXX
            END IF
         '   connxx.Execute SQLXX
            If Err.number > 0 then
               endpgm="2"
               '發生錯誤時，刪除異動檔所新增的異動資料
               errmsg=cstr(Err.number) & "=" & Err.description
               sqlyy="delete * FROM RTEBTCUSTCHGlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and ENTRYNO=" & key(3) & " AND SEQ=" & SEQ
               CONNXX.Execute sqlyy 
            ELSE
               endpgm="1"
               errmsg=""
            END IF
         end if      
      end if
   END IF
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS用戶異動完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此異動已完工結案，不可重複執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此異動包含移機項目，不可直接結案，請建立異動派工單後由派工單結案之" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    else
       msgbox "無法執行用戶異動完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylinesndworkdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>