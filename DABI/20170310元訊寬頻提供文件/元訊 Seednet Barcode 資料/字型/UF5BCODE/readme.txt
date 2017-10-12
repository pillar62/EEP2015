==============================================================================
 Seagate Software Inc.                                    Phone: 604-669-8379
                                                          Fax  : 604-681-7163
							  Sales: 800-877-2340
 Email:   support@webacd.seagatesoftware.com       
 WebACD:  webacd.seagatesoftware.com
 Website: www.seagatesoftware.com                     
==============================================================================
 File Name: UF5BCODE.ZIP               Compatible with versions: 5,6 & 7
                                                                 16 & 32bit
 Contents:  README.TXT   - This file
            README16.TXT - 16bit instructions
            README32.TXT - 32bit instructions
            UFLBCODE.DLL - (16bit)    Version: 1.102
            U2LBCODE.DLL - (32bit)    Version: 1.105
            AZALEA39.EXE - Sample Code 39 Font supports digits 1-9 and
                           characters A,E,L and Z.  Also includes ordering 
                           information on how to contact Azalea Software, 
                           Inc. for additional fonts.
            CODE39.RPT   - Sample SCR 5/6 Report using the sample Code 39
                           True-Type font in conjunction with the new
                           UFL/U2LBCODE DLL files.

 Description:
 ------------
 These UFLs allow you to create barcodes on reports in conjunction with the
 Azalea Software, Inc. sample and full version font sets.  The UFL creates 
 the strings and special stop/start characters through formulas that must 
 then be be formatted with the appropiate installed barcode true-type font.

------------------------------------------------------------------------------
   
 Installation:
 -------------
 Copy the enclosed DLL file(s) to your \WINDOWS\CRYSTAL directory if you are 
 using it on your local (developer) machine.  If you are distributing these
 files for use with a compiled report or application, the DLL files needs to
 be copied to your client's \WINDOWS\SYSTEM directory.

 Directions for installing the sample Code 39 True-Type font are found in
 the AZALEA39.EXE file.


 Additional Functions Added to Formula Editor:
 ---------------------------------------------
 BarcodeC39 (x) 
 BarcodeI2of5(x)
 BarcodeC128A(x)
 BarcodeC128B(x)
 BarcodeC128C(x)
 BarcodeEan8(x)
 BarcodeEan13(x)
 BarcodeUPCA (x,y,z)
 BarcodeUPCE (x)
 BarcodeBookland (x,y)
 BarcodeISSN (x,y)
 BarcodePostnet (x)
 
 
 
 Formula Syntax:
 ---------------
 BarcodeC39 (Alphanumeric String) 
	Where "Alphanumeric String" is a string thatcan contain the numbers 0-9, 
	letters A-Z and a-z to a maximum of 254 characters.

 BarcodeI2of5 (Numeric String)
	Where "Numeric String" is a string that can contain the numbers 0-9. 
        The length of the string should be even, but if not, the function pads 
	the beginning with a zero to make even length.

 BarcodeC128A (AlphaNumeric String)
	Where "Numeric String" is a string that can contain the numbers 0-9, 
	letters A-Z and a-z  (lower case letters are mapped into control codes) 
	and control codes to a maximum of 254 characters.
 
 BarcodeC128B (AlphaNumeric String)
	Where "Alphanumeric String" is a string that can contain the numbers 0-9, 
	letters A-Z and a-z to a maximum of 254 characters.

 Barcode128C (Numeric String)
	Where "Numeric String" is a string that can contain the numbers 0-9. 
        The length of the string should be even, but if not, the function pads 
	the beginning with a zero to make even length.
 
 BarcodeEAN8 (Numeric String)
	Where "Numeric String" is a string that takes exactly 7 digits 
	from 0-9

 BarcodeEAN13 (Numeric String)
	Where "Numeric String" is a string that takes exactly 12 digits from
	0-9  

 BarcodeUPCA(Number System, Item, Manufacturer)
	Where "Number System" is a string that takes exactly 1 digit which
	represents the numbering system being used.
	"Item" is a string that takes exactly 5 digits which represent the item
	or product number.
	"Manufacturer" is a string that takes exactly 5 digits which represent
	the manufacturer.
 
 BarcodeUPCE(Compressed UPC String)
	Where "Compressed UPC String" is a string of 6 or 10 digits.

 BarcodeBookland(ISBN Number, 5 digit Supplimental) 
	Where "ISBN Number"  is a string that contains a 10 digit ISBN number 
	5 digit Supplimental is a string that contains a 5 digit number, the
	check digit could be an X.

 BarcodeISSN (ISSN Number,2 digit supplimental)
	Where "ISSN Number" is a string that contains a 10 digit ISSN Number:
	8 digit ISSN Number plus a 3 digit additional number (usually 000).
	"2 digit supplimental" is string that contains a 2 digit number.

 BarcodePostnet(Zipocode)
	Where "Zipcode" is a string that contains a valid zipcode of 5, 9 or 
	11 digits. This string also can contain separators (-) between the 
	digits.


 Examples:
 ---------
 BarcodeC39 ("ABC123ab")   Would return a Code 39 barcode that when scanned 
                           returns the value "ABC123abc"

 BarcodeI2of5("1234") -    Would return a Interleaved 2 of 5 barcode that when 
		       	   scanned returns the value "1234"

 BarcodeC128A("A.234") -  Would return a Code 128A barcode that when scanned
			  returns the value "A.234"

 BarcodeC128B("123ABCa")- Would return a Code 128C barcode that when scanned 
			  returns the value "123ABCa"

 BarcodeC128C("4567") -   Would return a Code 128C barcode that when 
		       	  scanned returns the value "4567"

 BarcodeEAN8 ("1234567") - Would return a  EAN8 barcode that when scanned 
			   returns the value "12345670"

 BarcodeEAN13 ("891023568974")- Would return a EAN13 barcode that when scanned
				returns the value "8910235689746"	       
	 
 BarcodeUPCA("0", "16143","13049" ) - Would return a UPCA barcode that when 
				      scanned returns the value "0061143130498"

 BarcodeUPCE("649953") - Would return a UPCE barcode that when scanned returns
			 the value "06499539"
 
 BarcodeBookland("0688123163","90000" ) -Would return a ISBN barcode that when 
					scanned returns the value "9780688123161"

 BarcodeISSN("00411086000","21") -Would return a ISSN barcode that when scanned returns 
			          the value "9770041108003"	
 
 BarcodePostnet("48358") - Would return a zipcode barcode that when scanned 
			    returns the value "483582"

  
==============================================================================
 Additional functions will only work if you have installed a purchased copy
 of one of the Azalea Software, Inc. barcode font packages.  

 For information on purchasing full font sets for any of these new functions,
 please contact Azalea Software, Inc. at:

 Toll Free: (800) 48-ASOFT          Phone: (206) 341-9500
 Website:   http://www.azalea.com   Email: info@azalea.com
==============================================================================