insert into RTLessorAVSCmtyLine
( COMQ1, LINEQ1, LINEGROUP, CONSIGNEE, AREAID, GROUPID, SALESID, DEVELOPERID, LINEIP, 
                            GATEWAY, SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, LINEISP, LINEIPTYPE, 
                            IPCNT, LINEDUEDAT, CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC, COD4, LANE, 
                            COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, COD9, ROOM, COD10, ADDROTHER, RZONE, RCVDAT, 
                            INSPECTDAT, AGREE, UNAGREEREASON, HINETNOTIFYDAT, HARDWAREDAT, ADSLAPPLYDAT, EUSR, EDAT, UUSR, 
                            UDAT, CANCELDAT, CANCELUSR, DROPDAT, SUPPLYRANGE, LOANNAME, LOANSOCIAL, MEMO, APPLYDAT, 
                            APPLYNAME, APPLYSOCIAL, APPLYCONTACTTEL, APPLYMOBILE, LOANCONTACTTEL, LOANMOBILE, AUTOIP, 
                            CONTAPPLYDAT, DROPKIND, SELECTCASE, FIBERID)

SELECT          B.COMQ1, LINEQ1, LINEGROUP, CONSIGNEE, AREAID, GROUPID, SALESID, DEVELOPERID, LINEIP, 
                            GATEWAY, SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, LINEISP, LINEIPTYPE, 
                            IPCNT, LINEDUEDAT, CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC, COD4, LANE, 
                            COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, COD9, ROOM, COD10, ADDROTHER, RZONE, RCVDAT, 
                            INSPECTDAT, AGREE, UNAGREEREASON, HINETNOTIFYDAT, HARDWAREDAT, ADSLAPPLYDAT, EUSR, EDAT, UUSR, 
                            UDAT, CANCELDAT, CANCELUSR, DROPDAT, SUPPLYRANGE, LOANNAME, LOANSOCIAL, MEMO, APPLYDAT, 
                            APPLYNAME, APPLYSOCIAL, APPLYCONTACTTEL, APPLYMOBILE, LOANCONTACTTEL, LOANMOBILE, AUTOIP, 
                            CONTAPPLYDAT, DROPKIND, SELECTCASE, FIBERID
FROM              RTLessorCmtyLine A
left join ( 
select A.COMQ1, b.COMQ1 as COMQ1O 
from RTLessorAVSCmtyH A
left join RTLessorCmtyH b on b.COMN=a.COMN
where A.comtype='8') b on b.COMQ1O=a.COMQ1