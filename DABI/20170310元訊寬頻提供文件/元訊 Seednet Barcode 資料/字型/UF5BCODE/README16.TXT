The Azalea Bar Code UFL for Crystal Reports

This user function library (UFL) allows you to create bar codes from 
within your Crystal Reports files.  The Azalea Bar Code UFL works with 
our TrueType bar code fonts.  Our fonts are used for both the screen 
display and the printed output.  The UFL creates the strings that must
be formatted in the bar code fonts.  It handles all of the check digit
calculations and character mapping.

Installing the UFL

With Crystal Reports closed, place the U2LBCODE.DLL (For 16-Bit users, 
use the uflbcode.dll file instead) file in your \WINDOWS\SYSTEM directory.
It will then appear as one of the available forumulas.

Using the UFL

1. Create a field in Crystal Reports.

2. Click on Menu/Insert/Formula field.

3. Click the New button to create a new formula.

4. Name your formula.

5. Several BarcodeXXX functions will appear in the formula edit box.

6. Double click the desired function name.

7. Type a database field name in the Formula text column to pass live data to
   the UFL.  For Example:
     BarcodeC128A({barcode})
   Because "barcode" is a field name, you must put the brackets around it.

   Pass a constant string to the UFL to get static bar code symbols that are
   all the same. - For Example:
     BarcodeC39("AZALEA")

8. After you have entered the correct formula text, click the Accept button.

9. Locate the formula name you just created in the Insert Fields dialog box.
   Click on the Insert button to put the bar code into your report.

10. Format the text in one of Azalea Software's bar code fonts at the correct
    size. The point size you use will determine the height of the bars.
