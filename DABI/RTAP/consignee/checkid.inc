<%
'========================================================================================================================
'ID Check v2.0
'�睊�����G
'�o�Ӫ����M���e�������F�󴫦r����Ӫ��M�@�Ǥp������ debug �~�A��l�������S�����ܡC
'�o�����r����Ӫ��]�t�� I O W Z �����ҡA�Y��������~���a���ٽЧi���C
'�{�������G
'�o�O�� ASP �������Ҧr�����Ҩ禡�A�b���P�j�a���ɡA
'�]���ݹL�j�������������Ҫ��禡���O�ΰ}�C���覡�Ӽg�A
'�ӧ��`ı�o�ΰ}�C�t�׷|�ܺC�A�B�]���u�����ӳ��w�ΰ}�C(���M���ɯu���ܦn��)�A
'�ҥH�b�g�o�Ө禡�ɧ����S���ϥΨ�}�C�A
'�t�~�A�����h�áA A123456789 �u���O�X�k�������Ҧr���A
'�i�O�o�@�չ�b�O�Ӧn�q�F�A�ҥH�Y�A���Ʊ榳�H�� A123456789 �Ӭ�}���Ҫ��ܡA
'�i�H�ۦ�[�g�@�ӧP�_���N A123456789 �ư��f�֡A
'�Y���o�{�{���� Bug �άOı�o���̼g���n���A
'�w����N�ק糧�{���A���L�e���O�n Copy �@�����ڮ@�I
'���{���i�H���N�ק�Ǽ��P�ϥΡA�߿W�@�̸�T�Ʊ�j�a�Ф��n�R���P�ק�I
'-------------------- �@�̸�T --------------------
'�@�̡G�p��
'ICQ�G4999381
'E-mail�Gsamchung@tpts4.seed.net.tw
'--------------------------------------------------
'���~�����J
'CheckID(Request("ID")) = "True"  �q�L�ˬd
'CheckID(Request("ID")) = "False" ���X�k�������Ҧr��
'CheckID(Request("ID")) = "ERR-1" ���i�d�ťթο�J��ƿ��~
'CheckID(Request("ID")) = "ERR-2" �����Ҧr�����Ĥ@�X���ݬO�X�k���^��r��
'CheckID(Request("ID")) = "ERR-3" �����Ҧr�����ĤG�X���ݬO�Ʀr 1 �� 2
'CheckID(Request("ID")) = "ERR-4" �����Ҧr������E�X���ݬO�Ʀr
'========================================================================================================================
Function CheckID(InputID)
CheckID = "False"
InputID = UCase(InputID)
IF InputID <> "" AND Len(InputID) = 10 THEN
	IF ASC(Left(InputID,1)) < 64 OR ASC(Left(InputID,1)) > 91 THEN
		CheckID = "ERR-2"
		'�����Ҧr�����Ĥ@�X���ݬO�X�k���^��r��
	ELSEIF MID(InputID,2,1) <> "1" AND MID(InputID,2,1) <> "2" THEN
		CheckID = "ERR-3"
		'�����Ҧr�����ĤG�X���ݬO�Ʀr 1 �� 2
	ELSE
		FOR i = 2 to Len(InputID)
			IF ASC(Mid(InputID,i,1)) < 47 OR ASC(Mid(InputID,i,1)) > 58 THEN CheckID = "ERR-4"
			'�����Ҧr������E�X���ݬO�Ʀr
		NEXT
	END IF
	IF Mid(CheckID,1,3) <> "ERR" THEN
		ID_ABC_Data = "A10B11C12D13E14F15G16H17I34J18K19L20M21N22O35P23Q24R25S26T27U28V29W32X30Y31Z33"
		InputID = Mid(ID_ABC_Data,InStr(ID_ABC_Data,Left(InputID,1)) + 1,2) & Mid(InputID,2)
		GetNo = 2
		SUM = Left(InputID,1)
		FOR i = 9 TO 1 STEP -1
			SUM = SUM + Mid(InputID,GetNo,1) * i
			GetNo = GetNo + 1
		NEXT
		IF ASC(Right(InputID,1)) = ASC(Right(10 - Right(SUM,1),1)) THEN CheckID = "True"
		'�q�L�ˬd
	END IF
ELSE
	CheckID = "ERR-1"
	'���i�d�ťթο�J��ƿ��~
END IF
End Function
%>