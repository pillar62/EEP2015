DROP VIEW V_RT3071
GO

CREATE VIEW V_RT3071 AS 
SELECT     PRTNO,      CODENC, belongnc, amtnc, qty, amt, unino, invtitle, COMN, CUSNC, raddr, CONTACTTEL, DOCKETDAT, rcvmoneydat, 
                            STRBILLINGDAT, duedat, memo, gtserial, CREDITCARDNO
FROM              (SELECT     PRTNO, CODENC, belongnc, rcvnc AS amtnc, qty, rcvmoney AS amt, unino, invtitle, COMN, CUSNC, raddr, 
                                                        CONTACTTEL, DOCKETDAT, rcvmoneydat, STRBILLINGDAT, duedat, memo, gtserial, CREDITCARDNO
                            FROM               dbo.V_RT307
                            WHERE           (rcvmoney > 0)
                            UNION
                            SELECT     PRTNO,      CODENC, belongnc, setnc AS amtnc, qty, setmoney AS amt, unino, invtitle, COMN, CUSNC, raddr, 
                                                        CONTACTTEL, DOCKETDAT, rcvmoneydat, STRBILLINGDAT, duedat, memo, gtserial, CREDITCARDNO
                            FROM              dbo.V_RT307
                            WHERE          (setmoney > 0)
                            UNION
                            SELECT      PRTNO,     CODENC, belongnc, gtnc AS amtnc, qty, GTMONEY AS amt, unino, invtitle, COMN, CUSNC, raddr, 
                                                        CONTACTTEL, DOCKETDAT, rcvmoneydat, STRBILLINGDAT, duedat, memo, gtserial, CREDITCARDNO
                            FROM              dbo.V_RT307
                            WHERE          (GTMONEY > 0)
                            UNION
                            SELECT       PRTNO,    CODENC, belongnc, returnnc AS amtnc, qty, returnmoney AS amt, unino, invtitle, COMN, CUSNC, raddr, 
                                                        CONTACTTEL, DOCKETDAT, rcvmoneydat, STRBILLINGDAT, duedat, memo, gtserial, CREDITCARDNO
                            FROM              dbo.V_RT307
                            WHERE          (returnmoney > 0)) AS A