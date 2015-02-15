Imports System
Imports System.Text

'
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' * 
' *      http://www.apache.org/licenses/LICENSE-2.0
' * 
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' 
Namespace PhoneticStringComparison

'    *
'	 * Encodes a string into a double metaphone value.
'	 * This Implementation is based on the algorithm by <CITE>Lawrence Philips</CITE>.
'	 * <ul>
'	 * <li>Original Article: <a
'	 * href="http://www.cuj.com/documents/s=8038/cuj0006philips/">
'	 * http://www.cuj.com/documents/s=8038/cuj0006philips/</a></li>
'	 * <li>Original Source Code: <a href="ftp://ftp.cuj.com/pub/2000/1806/philips.zip">
'	 * ftp://ftp.cuj.com/pub/2000/1806/philips.zip</a></li>
'	 * </ul>
'	 * 
'	 * @author Apache Software Foundation
'	 * modified by Per Ivar Nerseth to better suit the scandinavian languages
'	 
	Public Class DoubleMetaphone
'        *
'		 * "Vowels" to test for
'		 
		' extended to support scandinavian characters
		' http://www.findwise.com/blog/bryan-brian-briane-bryne-or-what-was-his-name-again/
		' https://code.google.com/p/google-refine/source/browse/trunk/main/src/com/google/refine/clustering/binning/Metaphone3.java?r=2102
		Private Const VOWELS As String = "AEIOUY¿¡¬√ƒ≈∆»… ÀÃÕŒœ“”‘’÷ÿŸ⁄€‹›"

'        *
'		 * 
'		 * Prefixes when present which are not pronounced
'		 
		Private Shared ReadOnly SILENT_START() As String = { "GN", "KN", "PN", "WR", "PS" }
		Private Shared ReadOnly L_R_N_M_B_H_F_V_W_SPACE() As String = { "L", "R", "N", "M", "B", "H", "F", "V", "W", " " }
		Private Shared ReadOnly ES_EP_EB_EL_EY_IB_IL_IN_IE_EI_ER() As String = { "ES", "EP", "EB", "EL", "EY", "IB", "IL", "IN", "IE", "EI", "ER" }
		Private Shared ReadOnly L_T_K_S_N_M_B_Z() As String = { "L", "T", "K", "S", "N", "M", "B", "Z" }

'        *
'		 * Maximum length of an encoding, default is 4
'		 
		Protected maxCodeLen As Integer = 4

		Public Sub New()
		End Sub

'        *
'		 * Encode a value with Double Metaphone
'		 *
'		 * @param value string to encode
'		 * @return an encoded string
'		 
		Public Function Encode(ByVal value As String) As String
			Return Encode(value, False)
		End Function

'        *
'		 * Encode a value with Double Metaphone, optionally using the alternate
'		 * encoding.
'		 *
'		 * @param value string to encode
'		 * @param alternate use alternate encode
'		 * @return an encoded string
'		 
		Public Function Encode(ByVal value As String, ByVal alternate As Boolean) As String
			value = CleanInput(value)
			If value Is Nothing Then
				Return Nothing
			End If

			Dim slavoGermanic As Boolean = IsSlavoGermanic(value)
			Dim index As Integer = If(IsSilentStart(value), 1, 0)

			Dim result As New DoubleMetaphoneResult(Me.GetMaxCodeLen(), Me)

			Do While (Not result.IsComplete()) AndAlso index <= value.Length - 1
				Select Case value.Chars(index)
					Case "A"c, "E"c, "I"c, "O"c, "U"c, "Y"c, "¿"c, "¡"c, "¬"c, "√"c, "ƒ"c, "≈"c, "∆"c, "»"c, "…"c, " "c, "À"c, "Ã"c, "Õ"c, "Œ"c, "œ"c, "“"c, "”"c, "‘"c, "’"c, "÷"c, "ÿ"c, "Ÿ"c, "⁄"c, "€"c, "‹"c, "›"c
						index = HandleAEIOUY(value, result, index)
					Case "B"c
						result.Append("P"c)
						index = If(CharAt(value, index + 1) = "B"c, index + 2, index + 1)
					Case ChrW(&H00C7)
						' A C with a Cedilla
						result.Append("S"c)
						index += 1
					Case "C"c
						index = HandleC(value, result, index)
					Case "D"c
						index = HandleD(value, result, index)
					Case "F"c
						index = HandleF(value, result, index)
					Case "G"c
						index = HandleG(value, result, index, slavoGermanic)
					Case "H"c
						index = HandleH(value, result, index)
					Case "J"c
						index = HandleJ(value, result, index, slavoGermanic)
					Case "K"c
						result.Append("K"c)
						index = If(CharAt(value, index + 1) = "K"c, index + 2, index + 1)
					Case "L"c
						index = HandleL(value, result, index)
					Case "M"c
						result.Append("M"c)
						index = If(ConditionM0(value, index), index + 2, index + 1)
					Case "N"c
						result.Append("N"c)
						index = If(CharAt(value, index + 1) = "N"c, index + 2, index + 1)
					Case ChrW(&H00D1)
						' N with a tilde (spanish ene)
						result.Append("N"c)
						index += 1
					Case "P"c
						index = HandleP(value, result, index)
					Case "Q"c
						result.Append("K"c)
						index = If(CharAt(value, index + 1) = "Q"c, index + 2, index + 1)
					Case "R"c
						index = HandleR(value, result, index, slavoGermanic)
					Case "S"c
						index = [HandleS](value, result, index, slavoGermanic)
					Case "T"c
						index = HandleT(value, result, index)
					Case "V"c
						index = HandleV(value, result, index)
					Case "W"c
						index = HandleW(value, result, index)
					Case "X"c
						index = HandleX(value, result, index)
					Case "Z"c
						index = HandleZ(value, result, index, slavoGermanic)
					Case Else
						index += 1
				End Select
			Loop

			Return If(alternate, result.GetAlternate(), result.GetPrimary())
		End Function

'        *
'		 * Check if the Double Metaphone values of two <code>string</code> values
'		 * are equal.
'		 * 
'		 * @param value1 The left-hand side of the encoded {@link string#equals(Object)}.
'		 * @param value2 The right-hand side of the encoded {@link string#equals(Object)}.
'		 * @return <code>true</code> if the encoded <code>string</code>s are equal;
'		 *          <code>false</code> otherwise.
'		 * @see #isDoubleMetaphoneEqual(string,string,bool)
'		 
		Friend Function IsDoubleMetaphoneEqual(ByVal value1 As String, ByVal value2 As String) As Boolean
			Return IsDoubleMetaphoneEqual(value1, value2, False)
		End Function

'        *
'		 * Check if the Double Metaphone values of two <code>string</code> values
'		 * are equal, optionally using the alternate value.
'		 * 
'		 * @param value1 The left-hand side of the encoded {@link string#equals(Object)}.
'		 * @param value2 The right-hand side of the encoded {@link string#equals(Object)}.
'		 * @param alternate use the alternate value if <code>true</code>.
'		 * @return <code>true</code> if the encoded <code>string</code>s are equal;
'		 *          <code>false</code> otherwise.
'		 
		Friend Function IsDoubleMetaphoneEqual(ByVal value1 As String, ByVal value2 As String, ByVal alternate As Boolean) As Boolean
			Return Encode(value1, alternate) = Encode(value2, alternate)
		End Function

'        *
'		 * Returns the maxCodeLen.
'		 * @return int
'		 
		Friend Function GetMaxCodeLen() As Integer
			Return Me.maxCodeLen
		End Function

'        *
'		 * Sets the maxCodeLen.
'		 * @param maxCodeLen The maxCodeLen to set
'		 
		Friend Sub SetMaxCodeLen(ByVal maxCodeLen As Integer)
			Me.maxCodeLen = maxCodeLen
		End Sub

		' BEGIN HANDLERS

'        *
'		 * Handles 'A', 'E', 'I', 'O', 'U', and 'Y' cases
'		 
		Private Function HandleAEIOUY(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If index = 0 Then
				result.Append("A"c)
			End If
			Return index + 1
		End Function

'        *
'		 * Handles 'C' cases
'		 
		Private Function HandleC(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If ConditionC0(value, index) Then ' very confusing, moved out
				result.Append("K"c)
				index += 2
			ElseIf index = 0 AndAlso Contains(value, index, 6, "CAESAR") Then
				result.Append("S"c)
				index += 2
			ElseIf Contains(value, index, 2, "CH") Then
				index = HandleCH(value, result, index)
			ElseIf Contains(value, index, 2, "CZ") AndAlso (Not Contains(value, index - 2, 4, "WICZ")) Then
				' "Czerny"
				result.Append("S"c, "X"c)
				index += 2
			ElseIf Contains(value, index + 1, 3, "CIA") Then
				' "focaccia"
				result.Append("X"c)
				index += 3
			ElseIf Contains(value, index, 2, "CC") AndAlso Not(index = 1 AndAlso CharAt(value, 0) = "M"c) Then
				' double "cc" but not "McClelland"
				Return HandleCC(value, result, index)
			ElseIf Contains(value, index, 2, "CK", "CG", "CQ") Then
				result.Append("K"c)
				index += 2
			ElseIf Contains(value, index, 2, "CI", "CE", "CY") Then
				' Italian vs. English
				If Contains(value, index, 3, "CIO", "CIE", "CIA") Then
					result.Append("S"c, "X"c)
				Else
					result.Append("S"c)
				End If
				index += 2
			Else
				result.Append("K"c)
				If Contains(value, index + 1, 2, " C", " Q", " G") Then
					' Mac Caffrey, Mac Gregor
					index += 3
				ElseIf Contains(value, index + 1, 1, "C", "K", "Q") AndAlso (Not Contains(value, index + 1, 2, "CE", "CI")) Then
					index += 2
				Else
					index += 1
				End If
			End If

			Return index
		End Function

'        *
'		 * Handles 'CC' cases
'		 
		Private Function HandleCC(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If Contains(value, index + 2, 1, "I", "E", "H") AndAlso (Not Contains(value, index + 2, 2, "HU")) Then
				' "bellocchio" but not "bacchus"
				If (index = 1 AndAlso CharAt(value, index - 1) = "A"c) OrElse Contains(value, index - 1, 5, "UCCEE", "UCCES") Then
					' "accident", "accede", "succeed"
					result.Append("KS")
				Else
					' "bacci", "bertucci", other Italian
					result.Append("X"c)
				End If
				index += 3
			Else
				result.Append("K"c)
				index += 2
			End If

			Return index
		End Function

'        *
'		 * Handles 'CH' cases
'		 
		Private Function HandleCH(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If index > 0 AndAlso Contains(value, index, 4, "CHAE") Then ' Michael
				result.Append("K"c, "X"c)
				Return index + 2
			ElseIf index = 0 AndAlso Contains(value, index, 3, "CHA") Then ' PIN: "Chasper"
				result.Append("K"c, "X"c)
				Return index + 2
			ElseIf index = 0 AndAlso Contains(value, index, 4, "CHIR") Then ' PIN: "Chirstine"
				result.Append("K"c, "X"c)
				Return index + 2
			' PIN: if the name ends with ch like "Didrich"
			ElseIf index = value.Length - 2 Then
				result.Append("K"c, "X"c)
				Return index + 2
			ElseIf ConditionCH0(value, index) Then ' Greek roots ("chemistry", "chorus", etc.)
				result.Append("K"c)
				Return index + 2
			ElseIf ConditionCH1(value, index) Then
				' Germanic, Greek, or otherwise 'ch' for 'kh' sound
				result.Append("K"c)
				Return index + 2
			Else
				If index > 0 Then
					If Contains(value, 0, 2, "MC") Then
						result.Append("K"c)
					Else
						result.Append("X"c, "K"c)
					End If
				Else
					result.Append("X"c)
				End If
				Return index + 2
			End If
		End Function

'        *
'		 * Handles 'D' cases
'		 
		Private Function HandleD(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If Contains(value, index, 2, "DG") Then
				' "Edge"
				If Contains(value, index + 2, 1, "I", "E", "Y") Then
					result.Append("J"c)
					index += 3
					' "Edgar"
				Else
					result.Append("TK")
					index += 2
				End If
			ElseIf Contains(value, index, 2, "DT", "DD") Then
				result.Append("T"c)
				index += 2

			' PIN: If the last letter is D and it ends with 'IND' or 'UND' like 'Eyvind', 'ÿyvind', '≈smund', 'Edmund', 'Amund', 'Omund'
			ElseIf (index = value.Length - 1) AndAlso (value.ToUpper().EndsWith("IND") OrElse value.ToUpper().EndsWith("UND")) Then
				' do nothing
				index += 1
			Else
				result.Append("T"c)
				index += 1
			End If
			Return index
		End Function

'        *
'		 * Handles 'F' cases
'		 
		Private Function HandleF(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer

			' PIN: Pafvel is the same as Povel, Efven the same as Even, Halfvor the same as Halvor etc.
			If Contains(value, index, 2, "FV") Then
				result.Append("F"c)
				index += 2
			Else
				result.Append("F"c)
				index = If(CharAt(value, index + 1) = "F"c, index + 2, index + 1)
			End If
			Return index
		End Function

'        *
'		 * Handles 'G' cases
'		 
		Private Function HandleG(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer, ByVal slavoGermanic As Boolean) As Integer
			If CharAt(value, index + 1) = "H"c Then
				index = HandleGH(value, result, index)
			ElseIf CharAt(value, index + 1) = "N"c Then
				If index = 1 AndAlso IsVowel(CharAt(value, 0)) AndAlso (Not slavoGermanic) Then
					result.Append("KN", "N")
				ElseIf (Not Contains(value, index + 2, 2, "EY")) AndAlso CharAt(value, index + 1) <> "Y"c AndAlso (Not slavoGermanic) Then
					result.Append("N", "KN")
				Else
					result.Append("KN")
				End If
				index = index + 2
			ElseIf Contains(value, index + 1, 2, "LI") AndAlso (Not slavoGermanic) Then
				result.Append("KL", "L")
				index += 2
			ElseIf index = 0 AndAlso (CharAt(value, index + 1) = "Y"c OrElse Contains(value, index + 1, 2, ES_EP_EB_EL_EY_IB_IL_IN_IE_EI_ER)) Then
				' -ges-, -gep-, -gel-, -gie- at beginning
				result.Append("K"c, "J"c)
				index += 2
			ElseIf (Contains(value, index + 1, 2, "ER") OrElse CharAt(value, index + 1) = "Y"c) AndAlso (Not Contains(value, 0, 6, "DANGER", "RANGER", "MANGER")) AndAlso (Not Contains(value, index - 1, 1, "E", "I")) AndAlso (Not Contains(value, index - 1, 3, "RGY", "OGY")) Then
				' -ger-, -gy-
				result.Append("K"c, "J"c)
				index += 2
			ElseIf Contains(value, index + 1, 1, "E", "I", "Y") OrElse Contains(value, index - 1, 4, "AGGI", "OGGI") Then
				' Italian "biaggi"
				If (Contains(value, 0, 4, "VAN ", "VON ") OrElse Contains(value, 0, 3, "SCH")) OrElse Contains(value, index + 1, 2, "ET") Then
					' obvious germanic
					result.Append("K"c)
				ElseIf Contains(value, index + 1, 4, "IER") Then
					result.Append("J"c)
				Else
					result.Append("J"c, "K"c)
				End If
				index += 2
			ElseIf CharAt(value, index + 1) = "G"c Then
				index += 2
				result.Append("K"c)
			Else
				index += 1
				result.Append("K"c)
			End If
			Return index
		End Function

'        *
'		 * Handles 'GH' cases
'		 
		Private Function HandleGH(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If index > 0 AndAlso (Not IsVowel(CharAt(value, index - 1))) Then
				result.Append("K"c)
				index += 2
			ElseIf index = 0 Then
				If CharAt(value, index + 2) = "I"c Then
					result.Append("J"c)
				Else
					result.Append("K"c)
				End If
				index += 2
			ElseIf (index > 1 AndAlso Contains(value, index - 2, 1, "B", "H", "D")) OrElse (index > 2 AndAlso Contains(value, index - 3, 1, "B", "H", "D")) OrElse (index > 3 AndAlso Contains(value, index - 4, 1, "B", "H")) Then
				' Parker's rule (with some further refinements) - "hugh"
				index += 2
			Else
				If index > 2 AndAlso CharAt(value, index - 1) = "U"c AndAlso Contains(value, index - 3, 1, "C", "G", "L", "R", "T") Then
					' "laugh", "McLaughlin", "cough", "gough", "rough", "tough"
					result.Append("F"c)
				ElseIf index > 0 AndAlso CharAt(value, index - 1) <> "I"c Then
					result.Append("K"c)
				End If
				index += 2
			End If
			Return index
		End Function

'        *
'		 * Handles 'H' cases
'		 
		Private Function HandleH(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			' only keep if first & before vowel or between 2 vowels
			If (index = 0 OrElse IsVowel(CharAt(value, index - 1))) AndAlso IsVowel(CharAt(value, index + 1)) Then
				result.Append("H"c)
				index += 2
				' also takes car of "HH"
			Else
				' do nothing
				index += 1
			End If
			Return index
		End Function

'        *
'		 * Handles 'J' cases
'		 
		Private Function HandleJ(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer, ByVal slavoGermanic As Boolean) As Integer
			If Contains(value, index, 4, "JOSE") OrElse Contains(value, 0, 4, "SAN ") Then
				' obvious Spanish, "Jose", "San Jacinto"
				If (index = 0 AndAlso (CharAt(value, index + 4) = " "c) OrElse value.Length = 4) OrElse Contains(value, 0, 4, "SAN ") Then
					result.Append("H"c)
				Else
					result.Append("J"c, "H"c)
				End If
				index += 1
			Else
				If index = 0 AndAlso (Not Contains(value, index, 4, "JOSE")) Then
					result.Append("J"c, "A"c) ' Yankelovich/Jankelowicz
				ElseIf IsVowel(CharAt(value, index - 1)) AndAlso (Not slavoGermanic) AndAlso (CharAt(value, index + 1) = "A"c OrElse CharAt(value, index + 1) = "O"c) Then
					' spanish pron. of .e.g. 'bajador'
					result.Append("J"c, "H"c)
				ElseIf index = value.Length - 1 Then
					result.Append("J"c, " "c)
						 ' PIN: add "B" to handle cases like "Anbj¯r", "Anbi¯r"
				ElseIf (Not Contains(value, index + 1, 1, L_T_K_S_N_M_B_Z)) AndAlso (Not Contains(value, index - 1, 1, "S", "K", "L", "B")) Then
					result.Append("J"c)
				End If

				If CharAt(value, index + 1) = "J"c Then
					index += 2
				Else
					index += 1
				End If
			End If
			Return index
		End Function

'        *
'		 * Handles 'L' cases
'		 
		Private Function HandleL(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			' PIN: do not add another L if the last added character was also a L
			If (Not result.GetPrimary().ToUpper().EndsWith("L")) Then
				result.Append("L"c)
			End If
			' PIN: Todo: handle spanish e.g. 'cabrillo', 'gallegos'
			' one example can be found here: http://swoodbridge.com/DoubleMetaPhone/double_metaphone_func_1-01.txt
			If CharAt(value, index + 1) = "L"c Then
				If ConditionL0(value, index) Then
					result.AppendAlternate(" "c)
				End If
				index += 2
			Else
				index += 1
			End If
			Return index
		End Function

'        *
'		 * Handles 'P' cases
'		 
		Private Function HandleP(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If CharAt(value, index + 1) = "H"c Then
				result.Append("F"c)
				index += 2
			Else
				' also account for "campbell" and "raspberry"
				result.Append("P"c)
				index = If(Contains(value, index + 1, 1, "P", "B"), index + 2, index + 1)
			End If
			Return index
		End Function

'        *
'		 * Handles 'R' cases
'		 
		Private Function HandleR(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer, ByVal slavoGermanic As Boolean) As Integer
			'french e.g. 'rogier', but exclude 'hochmeier'
			If index = value.Length - 1 AndAlso (Not slavoGermanic) AndAlso Contains(value, index - 2, 2, "IE") AndAlso (Not Contains(value, index - 4, 2, "ME", "MA")) Then
				result.AppendAlternate("R"c)
			Else
				result.Append("R"c)
			End If
			Return If(CharAt(value, index + 1) = "R"c, index + 2, index + 1)
		End Function

'        *
'		 * Handles 'S' cases
'		 
		Private Function [HandleS](ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer, ByVal slavoGermanic As Boolean) As Integer
			If Contains(value, index - 1, 3, "ISL", "YSL") Then
				' special cases "island", "isle", "carlisle", "carlysle"
				index += 1
			ElseIf index = 0 AndAlso Contains(value, index, 5, "SUGAR") Then
				' special case "sugar-"
				result.Append("X"c, "S"c)
				index += 1
			ElseIf Contains(value, index, 2, "SH") Then
				If Contains(value, index + 1, 4, "HEIM", "HOEK", "HOLM", "HOLZ") Then
					' germanic
					result.Append("S"c)
				Else
					result.Append("X"c)
				End If
				index += 2
			ElseIf Contains(value, index, 3, "SIO", "SIA") OrElse Contains(value, index, 4, "SIAN") Then
				' Italian and Armenian
				If slavoGermanic Then
					result.Append("S"c)
				Else
					result.Append("S"c, "X"c)
				End If
				index += 3
			ElseIf (index = 0 AndAlso Contains(value, index + 1, 1, "M", "N", "L", "W")) OrElse Contains(value, index + 1, 1, "Z") Then
				' german & anglicisations, e.g. "smith" match "schmidt" //
				' "snider" match "schneider"
				' also, -sz- in slavic language altho in hungarian it //
				'   is pronounced "s"
				result.Append("S"c, "X"c)
				index = If(Contains(value, index + 1, 1, "Z"), index + 2, index + 1)
			ElseIf Contains(value, index, 2, "SC") Then
				index = HandleSC(value, result, index)
			Else
				If index = value.Length - 1 AndAlso Contains(value, index - 2, 2, "AI", "OI") Then
					' french e.g. "resnais", "artois"
					result.AppendAlternate("S"c)
				Else
					result.Append("S"c)
				End If
				index = If(Contains(value, index + 1, 1, "S", "Z"), index + 2, index + 1)
			End If
			Return index
		End Function

'        *
'		 * Handles 'SC' cases
'		 
		Private Function HandleSC(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If CharAt(value, index + 2) = "H"c Then
				' Schlesinger's rule
				If Contains(value, index + 3, 2, "OO", "ER", "EN", "UY", "ED", "EM") Then
					' Dutch origin, e.g. "school", "schooner"
					If Contains(value, index + 3, 2, "ER", "EN") Then
						' "schermerhorn", "schenker"
						result.Append("X", "SK")
					Else
						result.Append("SK")
					End If
				Else
					If index = 0 AndAlso (Not IsVowel(CharAt(value, 3))) AndAlso CharAt(value, 3) <> "W"c Then
						result.Append("X"c, "S"c)
					Else
						result.Append("X"c)
					End If
				End If
			ElseIf Contains(value, index + 2, 1, "I", "E", "Y") Then
				result.Append("S"c)
			Else
				result.Append("SK")
			End If
			Return index + 3
		End Function

'        *
'		 * Handles 'T' cases
'		 
		Private Function HandleT(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			If Contains(value, index, 4, "TION") Then
				result.Append("X"c)
				index += 3
			ElseIf Contains(value, index, 3, "TCH") Then ' PIN: removed TIA to make Krisjan and Kristian handled in a similar way
				result.Append("X"c)
				index += 3

			' special case "thomas", "thames"
			' Contains(value, index + 2, 2, "OM", "AM") ||
			' PIN: IN scandinavian languages the TH is almost always a T
			' PIN: THORE, THEA, THURRE, THEO, THONETTE, THINE, THOLOFF, THRINE, THOMAS, THROND, THERESE
			ElseIf Contains(value, index, 2, "TH") Then
				result.Append("T"c)
				index += 2
			ElseIf Contains(value, index, 3, "TTH") Then
					' special case germanic
					' special case "ottho", "otthe"
				If Contains(value, 0, 4, "VAN ", "VON ") OrElse Contains(value, 0, 3, "SCH") OrElse Contains(value, index + 3, 1, "O", "E") Then
					result.Append("T"c)
				Else
					result.Append("0"c, "T"c)
				End If
				index += 2
			Else
				result.Append("T"c)
				index = If(Contains(value, index + 1, 1, "T", "D"), index + 2, index + 1)
			End If
			Return index
		End Function

'        *
'		 * Handles 'V' cases
'		 
		Private Function HandleV(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer

			' PIN: If the previous character was F, this can be ignored - like Effvind
			If index > 0 Then
				Dim target As String = value.Substring(index - 1, 1).ToUpper()
				If target.Equals("F") Then
					' ignore
				Else
					result.Append("F"c)
				End If
				index += 1
			Else
				result.Append("F"c)
				index = If(CharAt(value, index + 1) = "V"c, index + 2, index + 1)
			End If
			Return index
		End Function

'        *
'		 * Handles 'W' cases
'		 
		Private Function HandleW(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			' PIN: In scandinavian languages the W is not a Vowel but another V - which is handled as V
			Return HandleV(value, result, index)

			' PIN: THE BELOW IS NOT USED

			If Contains(value, index, 2, "WR") Then
				' can also be in middle of word
				result.Append("R"c)
				index += 2
			Else
				If index = 0 AndAlso (IsVowel(CharAt(value, index + 1)) OrElse Contains(value, index, 2, "WH")) Then
					If IsVowel(CharAt(value, index + 1)) Then
						' Wasserman should match Vasserman
						result.Append("A"c, "F"c)
					Else
						' need Uomo to match Womo
						result.Append("A"c)
					End If
					index += 1
				ElseIf (index = value.Length - 1 AndAlso IsVowel(CharAt(value, index - 1))) OrElse Contains(value, index - 1, 5, "EWSKI", "EWSKY", "OWSKI", "OWSKY") OrElse Contains(value, 0, 3, "SCH") Then
					' Arnow should match Arnoff
					result.AppendAlternate("F"c)
					index += 1
				ElseIf Contains(value, index, 4, "WICZ", "WITZ") Then
					' Polish e.g. "filipowicz"
					result.Append("TS", "FX")
					index += 4
				Else
					index += 1
				End If
			End If
			Return index
		End Function

'        *
'		 * Handles 'X' cases
'		 
		Private Function HandleX(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer) As Integer
			' PIN: Initial 'X' is pronounced 'Z' e.g. 'Xavier'
			' except when using Xs as in Xstian (Kristian)
			If index = 0 AndAlso Contains(value, index, 2, "XS", "XS") Then
				''XS' maps to 'KRS'
				result.Append("KRS")
				index += 2
			ElseIf index = 0 Then
				''Z' maps to 'S'
				result.Append("S"c)
				index += 1
			Else
				If Not((index = value.Length - 1) AndAlso (Contains(value, index - 3, 3, "IAU", "EAU") OrElse Contains(value, index - 2, 2, "AU", "OU"))) Then
					' French e.g. breaux
					result.Append("KS")
				End If
				index = If(Contains(value, index + 1, 1, "C", "X"), index + 2, index + 1)
			End If
			Return index
		End Function

'        *
'		 * Handles 'Z' cases
'		 
		Private Function HandleZ(ByVal value As String, ByVal result As DoubleMetaphoneResult, ByVal index As Integer, ByVal slavoGermanic As Boolean) As Integer
			If CharAt(value, index + 1) = "H"c Then
				' Chinese pinyin e.g. "zhao" or Angelina "Zhang"
				result.Append("J"c)
				index += 2
			Else
				If Contains(value, index + 1, 2, "ZO", "ZI", "ZA") OrElse (slavoGermanic AndAlso (index > 0 AndAlso CharAt(value, index - 1) <> "T"c)) Then
					result.Append("S", "TS")
				Else
					result.Append("S"c)
				End If
				index = If(CharAt(value, index + 1) = "Z"c, index + 2, index + 1)
			End If
			Return index
		End Function

		' BEGIN CONDITIONS

'        *
'		 * Complex condition 0 for 'C'
'		 
		Private Function ConditionC0(ByVal value As String, ByVal index As Integer) As Boolean
			If Contains(value, index, 4, "CHIA") Then
				Return True
			ElseIf index <= 1 Then
				Return False
			ElseIf IsVowel(CharAt(value, index - 2)) Then
				Return False
			ElseIf (Not Contains(value, index - 1, 3, "ACH")) Then
				Return False
			Else
				Dim c As Char = CharAt(value, index + 2)
				Return (c <> "I"c AndAlso c <> "E"c) OrElse Contains(value, index - 2, 6, "BACHER", "MACHER")
			End If
		End Function

'        *
'		 * Complex condition 0 for 'CH'
'		 
		Private Function ConditionCH0(ByVal value As String, ByVal index As Integer) As Boolean
			If index <> 0 Then
				Return False
			ElseIf (Not Contains(value, index + 1, 5, "HARAC", "HARIS")) AndAlso (Not Contains(value, index + 1, 3, "HOR", "HYM", "HIA", "HEM")) Then
				Return False
			ElseIf Contains(value, 0, 5, "CHORE") Then
				Return False
			Else
				Return True
			End If
		End Function

'        *
'		 * Complex condition 1 for 'CH'
'		 
		Private Function ConditionCH1(ByVal value As String, ByVal index As Integer) As Boolean
					' 'architect' but not 'arch', orchestra', 'orchid'
					 ' e.g. 'wachtler', 'weschsler', but not 'tichner'
			Return ((Contains(value, 0, 4, "VAN ", "VON ") OrElse Contains(value, 0, 3, "SCH")) OrElse Contains(value, index - 2, 6, "ORCHES", "ARCHIT", "ORCHID") OrElse Contains(value, index + 2, 1, "T", "S") OrElse ((Contains(value, index - 1, 1, "A", "O", "U", "E") OrElse index = 0) AndAlso (Contains(value, index + 2, 1, L_R_N_M_B_H_F_V_W_SPACE) OrElse index + 1 = value.Length - 1)))
		End Function

'        *
'		 * Complex condition 0 for 'L'
'		 
		Private Function ConditionL0(ByVal value As String, ByVal index As Integer) As Boolean
			If index = value.Length - 3 AndAlso Contains(value, index - 1, 4, "ILLO", "ILLA", "ALLE") Then
				Return True
			ElseIf (Contains(value, index - 1, 2, "AS", "OS") OrElse Contains(value, value.Length - 1, 1, "A", "O")) AndAlso Contains(value, index - 1, 4, "ALLE") Then
				Return True
			Else
				Return False
			End If
		End Function

'        *
'		 * Complex condition 0 for 'M'
'		 
		Private Function ConditionM0(ByVal value As String, ByVal index As Integer) As Boolean
			If CharAt(value, index + 1) = "M"c Then
				Return True
			End If
			Return Contains(value, index - 1, 3, "UMB") AndAlso ((index + 1) = value.Length - 1 OrElse Contains(value, index + 2, 2, "ER"))
		End Function

		' BEGIN HELPER FUNCTIONS

'        *
'		 * Determines whether or not a value is of slavo-germanic orgin. A value is
'		 * of slavo-germanic origin if it contians any of 'W', 'K', 'CZ', or 'WITZ'.
'		 
		Private Function IsSlavoGermanic(ByVal value As String) As Boolean
			Return value.IndexOf("W"c) > -1 OrElse value.IndexOf("K"c) > -1 OrElse value.IndexOf("CZ") > -1 OrElse value.IndexOf("WITZ") > -1
		End Function

'        *
'		 * Determines whether or not a character is a vowel or not
'		 
		Private Function IsVowel(ByVal ch As Char) As Boolean
			Return VOWELS.IndexOf(ch) <> -1
		End Function

'        *
'		 * Determines whether or not the value starts with a silent letter.  It will
'		 * return <code>true</code> if the value starts with any of 'GN', 'KN',
'		 * 'PN', 'WR' or 'PS'.
'		 
		Private Function IsSilentStart(ByVal value As String) As Boolean
			Dim result As Boolean = False
			For i As Integer = 0 To SILENT_START.Length - 1
				If value.StartsWith(SILENT_START(i)) Then
					result = True
					Exit For
				End If
			Next i
			Return result
		End Function

'        *
'		 * Cleans the input
'		 
		Private Function CleanInput(ByVal input As String) As String
			If input Is Nothing Then
				Return Nothing
			End If
			input = input.Trim()
			If input.Length = 0 Then
				Return Nothing
			End If
			Return input.ToUpper()
		End Function

'        *
'		 * Gets the character at index <code>index</code> if available, otherwise
'		 * it returns <code>Character.MIN_VALUE</code> so that there is some sort
'		 * of a default
'		 
		Protected Function CharAt(ByVal value As String, ByVal index As Integer) As Char
			If index < 0 OrElse index >= value.Length Then
				Return Char.MinValue
			End If
			Return value.Chars(index)
		End Function

'        *
'		 * Shortcut method with 1 criteria
'		 
		Private Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria As String) As Boolean
			Return Contains(value, start, length, New String() { criteria })
		End Function

'        *
'		 * Shortcut method with 2 criteria
'		 
		Private Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria1 As String, ByVal criteria2 As String) As Boolean
			Return Contains(value, start, length, New String() { criteria1, criteria2 })
		End Function

'        *
'		 * Shortcut method with 3 criteria
'		 
		Private Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria1 As String, ByVal criteria2 As String, ByVal criteria3 As String) As Boolean
			Return Contains(value, start, length, New String() { criteria1, criteria2, criteria3 })
		End Function

'        *
'		 * Shortcut method with 4 criteria
'		 
		Private Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria1 As String, ByVal criteria2 As String, ByVal criteria3 As String, ByVal criteria4 As String) As Boolean
			Return Contains(value, start, length, New String() { criteria1, criteria2, criteria3, criteria4 })
		End Function

'        *
'		 * Shortcut method with 5 criteria
'		 
		Private Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria1 As String, ByVal criteria2 As String, ByVal criteria3 As String, ByVal criteria4 As String, ByVal criteria5 As String) As Boolean
			Return Contains(value, start, length, New String() { criteria1, criteria2, criteria3, criteria4, criteria5 })
		End Function

'        *
'		 * Shortcut method with 6 criteria
'		 
		Private Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria1 As String, ByVal criteria2 As String, ByVal criteria3 As String, ByVal criteria4 As String, ByVal criteria5 As String, ByVal criteria6 As String) As Boolean
			Return Contains(value, start, length, New String() { criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 })
		End Function

'        *
'		 * Determines whether <code>value</code> contains any of the criteria starting
'		 * at index <code>start</code> and matching up to length <code>length</code>
'		 
		Protected Shared Function Contains(ByVal value As String, ByVal start As Integer, ByVal length As Integer, ByVal criteria() As String) As Boolean
			Dim result As Boolean = False
			If start >= 0 AndAlso start + length <= value.Length Then
				Dim target As String = value.Substring(start, length)

				For i As Integer = 0 To criteria.Length - 1
					If target = criteria(i) Then
						result = True
						Exit For
					End If
				Next i
			End If
			Return result
		End Function
	End Class

'    *
'	 * Inner class for storing results, since there is the optional alternate
'	 * encoding.
'	 
	Friend Class DoubleMetaphoneResult
		Private primary As StringBuilder = Nothing
		Private alternate As StringBuilder = Nothing
		Private maxLength As Integer
		Private owner As DoubleMetaphone

		Friend Sub New(ByVal maxLength As Integer, ByVal owner As DoubleMetaphone)
			Me.maxLength = maxLength
			Me.owner = owner

			primary = New StringBuilder(owner.GetMaxCodeLen())
			alternate = New StringBuilder(owner.GetMaxCodeLen())
		End Sub

		Friend Sub Append(ByVal value As Char)
			AppendPrimary(value)
			AppendAlternate(value)
		End Sub

		Friend Sub Append(ByVal primary As Char, ByVal alternate As Char)
			AppendPrimary(primary)
			AppendAlternate(alternate)
		End Sub

		Friend Sub AppendPrimary(ByVal value As Char)
			If Me.primary.Length < Me.maxLength Then
				Me.primary.Append(value)
			End If
		End Sub

		Friend Sub AppendAlternate(ByVal value As Char)
			If Me.alternate.Length < Me.maxLength Then
				Me.alternate.Append(value)
			End If
		End Sub

		Friend Sub Append(ByVal value As String)
			AppendPrimary(value)
			AppendAlternate(value)
		End Sub

		Friend Sub Append(ByVal primary As String, ByVal alternate As String)
			AppendPrimary(primary)
			AppendAlternate(alternate)
		End Sub

		Friend Sub AppendPrimary(ByVal value As String)
			Dim addChars As Integer = Me.maxLength - Me.primary.Length
			If value.Length <= addChars Then
				Me.primary.Append(value)
			Else
				Me.primary.Append(value.Substring(0, addChars))
			End If
		End Sub

		Friend Sub AppendAlternate(ByVal value As String)
			Dim addChars As Integer = Me.maxLength - Me.alternate.Length
			If value.Length <= addChars Then
				Me.alternate.Append(value)
			Else
				Me.alternate.Append(value.Substring(0, addChars))
			End If
		End Sub

		Friend Function GetPrimary() As String
			Return Me.primary.ToString()
		End Function

		Friend Function GetAlternate() As String
			Return Me.alternate.ToString()
		End Function

		Friend Function IsComplete() As Boolean
			Return Me.primary.Length >= Me.maxLength AndAlso Me.alternate.Length >= Me.maxLength
		End Function
	End Class
End Namespace