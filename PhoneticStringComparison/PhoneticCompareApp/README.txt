README for the Phonetic String Comparison Library
Copyright Apache Software Foundation

Modified and Enhanced to suit Scandinavian Names by Per Ivar Nerseth


Using the Compare Console Application
-------------------------------------
The CompareApp is a Win32 Console Application.

Usage: PhoneticCompareApp.exe <input csv file> <output csv file>

Choose an input csv file and an output csv file and this utility
will perform a DoubleMetaphone encoding on the first column.

The resulting csv file will contain two columns:
One with the encoded name and one with the original name.

or specify the following to run an internal test:
PhoneticCompareApp.exe -test


PhoneticCompareApp.exe <input file path> <output file path>
e.g.
PhoneticCompareApp.exe "fornavn_redusert.csv" "fornavn_redusert_fixed.csv"


Using the Phonetic String Compare Library
-----------------------------------------
Import the PhoneticStringComparison.dll into your project

1. In Solution Explorer, double-click the My Project node for the project.
2. In the Project Designer, click the References tab.
3. Click the Add button to open the Add Reference dialog box.
4. In the Add Reference dialog box, select the tab indicating the type of component you want to reference.
5. Select the components you want to reference, then click OK.

C# 
using PhoneticStringComparison; // for DoubleMetaphone

private static DoubleMetaphone dm = new DoubleMetaphone();

string firstName = "THIS IS THE NAME";
string encodedFirstName = dm.Encode(firstName);


VB.NET
Imports PhoneticStringComparison

Dim dm as New DoubleMetaphone

Dim firstName As String = "THIS IS THE NAME"
Dim encodedFirstName As String = dm.Encode(firstName)
