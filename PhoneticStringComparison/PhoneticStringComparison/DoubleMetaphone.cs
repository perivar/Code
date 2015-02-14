﻿using System;
using System.Text;

/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace PhoneticStringComparison
{
	
	/**
	 * Encodes a string into a double metaphone value.
	 * This Implementation is based on the algorithm by <CITE>Lawrence Philips</CITE>.
	 * <ul>
	 * <li>Original Article: <a
	 * href="http://www.cuj.com/documents/s=8038/cuj0006philips/">
	 * http://www.cuj.com/documents/s=8038/cuj0006philips/</a></li>
	 * <li>Original Source Code: <a href="ftp://ftp.cuj.com/pub/2000/1806/philips.zip">
	 * ftp://ftp.cuj.com/pub/2000/1806/philips.zip</a></li>
	 * </ul>
	 * 
	 * @author Apache Software Foundation
	 * modified by Per Ivar Nerseth to better suit the scandinavian languages
	 */
	public class DoubleMetaphone
	{
		/**
		 * "Vowels" to test for
		 */
		// extended to support scandinavian characters
		// http://www.findwise.com/blog/bryan-brian-briane-bryne-or-what-was-his-name-again/
		// https://code.google.com/p/google-refine/source/browse/trunk/main/src/com/google/refine/clustering/binning/Metaphone3.java?r=2102
		private const string VOWELS = "AEIOUYÀÁÂÃÄÅÆÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝ";

		/**
		 * 
		 * Prefixes when present which are not pronounced
		 */
		private static readonly string[] SILENT_START = new string[] { "GN", "KN", "PN", "WR", "PS" };
		private static readonly string[] L_R_N_M_B_H_F_V_W_SPACE = new string[] { "L", "R", "N", "M", "B", "H", "F", "V", "W", " " };
		private static readonly string[] ES_EP_EB_EL_EY_IB_IL_IN_IE_EI_ER = new string[] { "ES", "EP", "EB", "EL", "EY", "IB", "IL", "IN", "IE", "EI", "ER" };
		private static readonly string[] L_T_K_S_N_M_B_Z = new string[] { "L", "T", "K", "S", "N", "M", "B", "Z" };

		/**
		 * Maximum length of an encoding, default is 4
		 */
		protected int maxCodeLen = 4;

		public DoubleMetaphone()
		{
		}

		/**
		 * Encode a value with Double Metaphone
		 *
		 * @param value string to encode
		 * @return an encoded string
		 */
		public string Encode(string value)
		{
			return Encode(value, false);
		}

		/**
		 * Encode a value with Double Metaphone, optionally using the alternate
		 * encoding.
		 *
		 * @param value string to encode
		 * @param alternate use alternate encode
		 * @return an encoded string
		 */
		public string Encode(string value, bool alternate)
		{
			value = CleanInput(value);
			if (value == null)
			{
				return null;
			}

			bool slavoGermanic = IsSlavoGermanic(value);
			int index = IsSilentStart(value) ? 1 : 0;

			DoubleMetaphoneResult result = new DoubleMetaphoneResult(this.GetMaxCodeLen(), this);

			while (!result.IsComplete() && index <= value.Length - 1)
			{
				switch (value[index])
				{
					case 'A':
					case 'E':
					case 'I':
					case 'O':
					case 'U':
					case 'Y':
					case 'À':
					case 'Á':
					case 'Â':
					case 'Ã':
					case 'Ä':
					case 'Å':
					case 'Æ':
					case 'È':
					case 'É':
					case 'Ê':
					case 'Ë':
					case 'Ì':
					case 'Í':
					case 'Î':
					case 'Ï':
					case 'Ò':
					case 'Ó':
					case 'Ô':
					case 'Õ':
					case 'Ö':
					case 'Ø':
					case 'Ù':
					case 'Ú':
					case 'Û':
					case 'Ü':
					case 'Ý':
						index = HandleAEIOUY(value, result, index);
						break;
					case 'B':
						result.Append('P');
						index = CharAt(value, index + 1) == 'B' ? index + 2 : index + 1;
						break;
					case '\u00C7':
						// A C with a Cedilla
						result.Append('S');
						index++;
						break;
					case 'C':
						index = HandleC(value, result, index);
						break;
					case 'D':
						index = HandleD(value, result, index);
						break;
					case 'F':
						index = HandleF(value, result, index);
						break;
					case 'G':
						index = HandleG(value, result, index, slavoGermanic);
						break;
					case 'H':
						index = HandleH(value, result, index);
						break;
					case 'J':
						index = HandleJ(value, result, index, slavoGermanic);
						break;
					case 'K':
						result.Append('K');
						index = CharAt(value, index + 1) == 'K' ? index + 2 : index + 1;
						break;
					case 'L':
						index = HandleL(value, result, index);
						break;
					case 'M':
						result.Append('M');
						index = ConditionM0(value, index) ? index + 2 : index + 1;
						break;
					case 'N':
						result.Append('N');
						index = CharAt(value, index + 1) == 'N' ? index + 2 : index + 1;
						break;
					case '\u00D1':
						// N with a tilde (spanish ene)
						result.Append('N');
						index++;
						break;
					case 'P':
						index = HandleP(value, result, index);
						break;
					case 'Q':
						result.Append('K');
						index = CharAt(value, index + 1) == 'Q' ? index + 2 : index + 1;
						break;
					case 'R':
						index = HandleR(value, result, index, slavoGermanic);
						break;
					case 'S':
						index = HandleS(value, result, index, slavoGermanic);
						break;
					case 'T':
						index = HandleT(value, result, index);
						break;
					case 'V':
						index = HandleV(value, result, index);
						break;
					case 'W':
						index = HandleW(value, result, index);
						break;
					case 'X':
						index = HandleX(value, result, index);
						break;
					case 'Z':
						index = HandleZ(value, result, index, slavoGermanic);
						break;
					default:
						index++;
						break;
				}
			}

			return alternate ? result.GetAlternate() : result.GetPrimary();
		}

		/**
		 * Check if the Double Metaphone values of two <code>string</code> values
		 * are equal.
		 * 
		 * @param value1 The left-hand side of the encoded {@link string#equals(Object)}.
		 * @param value2 The right-hand side of the encoded {@link string#equals(Object)}.
		 * @return <code>true</code> if the encoded <code>string</code>s are equal;
		 *          <code>false</code> otherwise.
		 * @see #isDoubleMetaphoneEqual(string,string,bool)
		 */
		internal bool IsDoubleMetaphoneEqual(string value1, string value2)
		{
			return IsDoubleMetaphoneEqual(value1, value2, false);
		}

		/**
		 * Check if the Double Metaphone values of two <code>string</code> values
		 * are equal, optionally using the alternate value.
		 * 
		 * @param value1 The left-hand side of the encoded {@link string#equals(Object)}.
		 * @param value2 The right-hand side of the encoded {@link string#equals(Object)}.
		 * @param alternate use the alternate value if <code>true</code>.
		 * @return <code>true</code> if the encoded <code>string</code>s are equal;
		 *          <code>false</code> otherwise.
		 */
		internal bool IsDoubleMetaphoneEqual(string value1, string value2, bool alternate)
		{
			return Encode(value1, alternate) == Encode(value2, alternate);
		}

		/**
		 * Returns the maxCodeLen.
		 * @return int
		 */
		internal int GetMaxCodeLen()
		{
			return this.maxCodeLen;
		}

		/**
		 * Sets the maxCodeLen.
		 * @param maxCodeLen The maxCodeLen to set
		 */
		internal void SetMaxCodeLen(int maxCodeLen)
		{
			this.maxCodeLen = maxCodeLen;
		}

		// BEGIN HANDLERS

		/**
		 * Handles 'A', 'E', 'I', 'O', 'U', and 'Y' cases
		 */
		private int HandleAEIOUY(string value, DoubleMetaphoneResult result, int index)
		{
			if (index == 0)
			{
				result.Append('A');
			}
			return index + 1;
		}

		/**
		 * Handles 'C' cases
		 */
		private int HandleC(string value, DoubleMetaphoneResult result, int index)
		{
			if (ConditionC0(value, index)) // very confusing, moved out
			{
				result.Append('K');
				index += 2;
			}
			else if (index == 0 && Contains(value, index, 6, "CAESAR"))
			{
				result.Append('S');
				index += 2;
			}
			else if (Contains(value, index, 2, "CH"))
			{
				index = HandleCH(value, result, index);
			}
			else if (Contains(value, index, 2, "CZ") && !Contains(value, index - 2, 4, "WICZ"))
			{
				// "Czerny"
				result.Append('S', 'X');
				index += 2;
			}
			else if (Contains(value, index + 1, 3, "CIA"))
			{
				// "focaccia"
				result.Append('X');
				index += 3;
			}
			else if (Contains(value, index, 2, "CC") && !(index == 1 && CharAt(value, 0) == 'M'))
			{
				// double "cc" but not "McClelland"
				return HandleCC(value, result, index);
			}
			else if (Contains(value, index, 2, "CK", "CG", "CQ"))
			{
				result.Append('K');
				index += 2;
			}
			else if (Contains(value, index, 2, "CI", "CE", "CY"))
			{
				// Italian vs. English
				if (Contains(value, index, 3, "CIO", "CIE", "CIA"))
				{
					result.Append('S', 'X');
				}
				else
				{
					result.Append('S');
				}
				index += 2;
			}
			else
			{
				result.Append('K');
				if (Contains(value, index + 1, 2, " C", " Q", " G"))
				{
					// Mac Caffrey, Mac Gregor
					index += 3;
				}
				else if (Contains(value, index + 1, 1, "C", "K", "Q") &&
				         !Contains(value, index + 1, 2, "CE", "CI"))
				{
					index += 2;
				}
				else
				{
					index++;
				}
			}

			return index;
		}

		/**
		 * Handles 'CC' cases
		 */
		private int HandleCC(string value, DoubleMetaphoneResult result, int index)
		{
			if (Contains(value, index + 2, 1, "I", "E", "H") &&
			    !Contains(value, index + 2, 2, "HU"))
			{
				// "bellocchio" but not "bacchus"
				if ((index == 1 && CharAt(value, index - 1) == 'A') ||
				    Contains(value, index - 1, 5, "UCCEE", "UCCES"))
				{
					// "accident", "accede", "succeed"
					result.Append("KS");
				}
				else
				{
					// "bacci", "bertucci", other Italian
					result.Append('X');
				}
				index += 3;
			}
			else
			{ // Pierce's rule
				result.Append('K');
				index += 2;
			}

			return index;
		}

		/**
		 * Handles 'CH' cases
		 */
		private int HandleCH(string value, DoubleMetaphoneResult result, int index)
		{
			if (index > 0 && Contains(value, index, 4, "CHAE")) // Michael
			{
				result.Append('K', 'X');
				return index + 2;
			}
			else if (index == 0 && Contains(value, index, 3, "CHA")) // PIN: "Chasper"
			{
				result.Append('K', 'X');
				return index + 2;
			}
			else if (index == 0 && Contains(value, index, 4, "CHIR")) // PIN: "Chirstine"
			{
				result.Append('K', 'X');
				return index + 2;
			}
			// PIN: if the name ends with ch like "Didrich"
			else if ( index == value.Length - 2)
			{
				result.Append('K', 'X');
				return index + 2;
			}
			else if (ConditionCH0(value, index)) // Greek roots ("chemistry", "chorus", etc.)
			{
				result.Append('K');
				return index + 2;
			}
			else if (ConditionCH1(value, index))
			{
				// Germanic, Greek, or otherwise 'ch' for 'kh' sound
				result.Append('K');
				return index + 2;
			}
			else
			{
				if (index > 0)
				{
					if (Contains(value, 0, 2, "MC"))
					{
						result.Append('K');
					}
					else
					{
						result.Append('X', 'K');
					}
				}
				else
				{
					result.Append('X');
				}
				return index + 2;
			}
		}

		/**
		 * Handles 'D' cases
		 */
		private int HandleD(string value, DoubleMetaphoneResult result, int index)
		{
			if (Contains(value, index, 2, "DG"))
			{
				// "Edge"
				if (Contains(value, index + 2, 1, "I", "E", "Y"))
				{
					result.Append('J');
					index += 3;
					// "Edgar"
				}
				else
				{
					result.Append("TK");
					index += 2;
				}
			}
			else if (Contains(value, index, 2, "DT", "DD"))
			{
				result.Append('T');
				index += 2;
			}
			
			// PIN: If the last letter is D and it ends with 'IND' or 'UND' like 'Eyvind', 'Øyvind', 'Åsmund', 'Edmund', 'Amund', 'Omund'
			else if ( (index == value.Length - 1) && (value.ToUpper().EndsWith("IND") || value.ToUpper().EndsWith("UND")))
			{
				// do nothing
				index++;
			}
			else
			{
				result.Append('T');
				index++;
			}
			return index;
		}

		/**
		 * Handles 'F' cases
		 */
		private int HandleF(string value, DoubleMetaphoneResult result, int index) {
			
			// PIN: Pafvel is the same as Povel, Efven the same as Even, Halfvor the same as Halvor etc.
			if (Contains(value, index, 2, "FV"))
			{
				result.Append('F');
				index += 2;
			} else {
				result.Append('F');
				index = CharAt(value, index + 1) == 'F' ? index + 2 : index + 1;
			}
			return index;
		}
		
		/**
		 * Handles 'G' cases
		 */
		private int HandleG(string value, DoubleMetaphoneResult result, int index, bool slavoGermanic)
		{
			if (CharAt(value, index + 1) == 'H')
			{
				index = HandleGH(value, result, index);
			}
			else if (CharAt(value, index + 1) == 'N')
			{
				if (index == 1 && IsVowel(CharAt(value, 0)) && !slavoGermanic)
				{
					result.Append("KN", "N");
				}
				else if (!Contains(value, index + 2, 2, "EY") &&
				         CharAt(value, index + 1) != 'Y' && !slavoGermanic)
				{
					result.Append("N", "KN");
				}
				else
				{
					result.Append("KN");
				}
				index = index + 2;
			}
			else if (Contains(value, index + 1, 2, "LI") && !slavoGermanic)
			{
				result.Append("KL", "L");
				index += 2;
			}
			else if (index == 0 && (CharAt(value, index + 1) == 'Y' || Contains(value, index + 1, 2, ES_EP_EB_EL_EY_IB_IL_IN_IE_EI_ER)))
			{
				// -ges-, -gep-, -gel-, -gie- at beginning
				result.Append('K', 'J');
				index += 2;
			}
			else if ((Contains(value, index + 1, 2, "ER") ||
			          CharAt(value, index + 1) == 'Y') &&
			         !Contains(value, 0, 6, "DANGER", "RANGER", "MANGER") &&
			         !Contains(value, index - 1, 1, "E", "I") &&
			         !Contains(value, index - 1, 3, "RGY", "OGY"))
			{
				// -ger-, -gy-
				result.Append('K', 'J');
				index += 2;
			}
			else if (Contains(value, index + 1, 1, "E", "I", "Y") ||
			         Contains(value, index - 1, 4, "AGGI", "OGGI"))
			{
				// Italian "biaggi"
				if ((Contains(value, 0, 4, "VAN ", "VON ") || Contains(value, 0, 3, "SCH")) || Contains(value, index + 1, 2, "ET"))
				{
					// obvious germanic
					result.Append('K');
				}
				else if (Contains(value, index + 1, 4, "IER"))
				{
					result.Append('J');
				}
				else
				{
					result.Append('J', 'K');
				}
				index += 2;
			}
			else if (CharAt(value, index + 1) == 'G')
			{
				index += 2;
				result.Append('K');
			}
			else
			{
				index++;
				result.Append('K');
			}
			return index;
		}

		/**
		 * Handles 'GH' cases
		 */
		private int HandleGH(string value, DoubleMetaphoneResult result, int index)
		{
			if (index > 0 && !IsVowel(CharAt(value, index - 1)))
			{
				result.Append('K');
				index += 2;
			}
			else if (index == 0)
			{
				if (CharAt(value, index + 2) == 'I')
				{
					result.Append('J');
				}
				else
				{
					result.Append('K');
				}
				index += 2;
			}
			else if ((index > 1 && Contains(value, index - 2, 1, "B", "H", "D")) ||
			         (index > 2 && Contains(value, index - 3, 1, "B", "H", "D")) ||
			         (index > 3 && Contains(value, index - 4, 1, "B", "H")))
			{
				// Parker's rule (with some further refinements) - "hugh"
				index += 2;
			}
			else
			{
				if (index > 2 && CharAt(value, index - 1) == 'U' &&
				    Contains(value, index - 3, 1, "C", "G", "L", "R", "T"))
				{
					// "laugh", "McLaughlin", "cough", "gough", "rough", "tough"
					result.Append('F');
				}
				else if (index > 0 && CharAt(value, index - 1) != 'I')
				{
					result.Append('K');
				}
				index += 2;
			}
			return index;
		}

		/**
		 * Handles 'H' cases
		 */
		private int HandleH(string value, DoubleMetaphoneResult result, int index)
		{
			// only keep if first & before vowel or between 2 vowels
			if ((index == 0 || IsVowel(CharAt(value, index - 1))) &&
			    IsVowel(CharAt(value, index + 1)))
			{
				result.Append('H');
				index += 2;
				// also takes car of "HH"
			}
			else
			{
				// do nothing
				index++;
			}
			return index;
		}

		/**
		 * Handles 'J' cases
		 */
		private int HandleJ(string value, DoubleMetaphoneResult result, int index, bool slavoGermanic)
		{
			if (Contains(value, index, 4, "JOSE") || Contains(value, 0, 4, "SAN "))
			{
				// obvious Spanish, "Jose", "San Jacinto"
				if ((index == 0 && (CharAt(value, index + 4) == ' ') ||
				     value.Length == 4) || Contains(value, 0, 4, "SAN "))
				{
					result.Append('H');
				}
				else
				{
					result.Append('J', 'H');
				}
				index++;
			}
			else
			{
				if (index == 0 && !Contains(value, index, 4, "JOSE"))
				{
					result.Append('J', 'A'); // Yankelovich/Jankelowicz
				}
				else if (IsVowel(CharAt(value, index - 1)) && !slavoGermanic &&
				         (CharAt(value, index + 1) == 'A' || CharAt(value, index + 1) == 'O'))
				{
					// spanish pron. of .e.g. 'bajador'
					result.Append('J', 'H');
				}
				else if (index == value.Length - 1)
				{
					result.Append('J', ' ');
				}
				else if (!Contains(value, index + 1, 1, L_T_K_S_N_M_B_Z)
				         // PIN: add "B" to handle cases like "Anbjør", "Anbiør"
				         && !Contains(value, index - 1, 1, "S", "K", "L", "B"))
				{
					result.Append('J');
				}

				if (CharAt(value, index + 1) == 'J')
				{
					index += 2;
				}
				else
				{
					index++;
				}
			}
			return index;
		}

		/**
		 * Handles 'L' cases
		 */
		private int HandleL(string value, DoubleMetaphoneResult result, int index)
		{
			// PIN: do not add another L if the last added character was also a L
			if (!result.GetPrimary().ToUpper().EndsWith("L")) {
				result.Append('L');
			}
			// PIN: Todo: handle spanish e.g. 'cabrillo', 'gallegos'
			// one example can be found here: http://swoodbridge.com/DoubleMetaPhone/double_metaphone_func_1-01.txt
			if (CharAt(value, index + 1) == 'L')
			{
				if (ConditionL0(value, index))
				{
					result.AppendAlternate(' ');
				}
				index += 2;
			}
			else
			{
				index++;
			}
			return index;
		}

		/**
		 * Handles 'P' cases
		 */
		private int HandleP(string value,
		                    DoubleMetaphoneResult result,
		                    int index)
		{
			if (CharAt(value, index + 1) == 'H')
			{
				result.Append('F');
				index += 2;
			}
			else
			{
				// also account for "campbell" and "raspberry"
				result.Append('P');
				index = Contains(value, index + 1, 1, "P", "B") ? index + 2 : index + 1;
			}
			return index;
		}

		/**
		 * Handles 'R' cases
		 */
		private int HandleR(string value,
		                    DoubleMetaphoneResult result,
		                    int index,
		                    bool slavoGermanic)
		{
			//french e.g. 'rogier', but exclude 'hochmeier'
			if (index == value.Length - 1 && !slavoGermanic &&
			    Contains(value, index - 2, 2, "IE") &&
			    !Contains(value, index - 4, 2, "ME", "MA"))
			{
				result.AppendAlternate('R');
			}
			else
			{
				result.Append('R');
			}
			return CharAt(value, index + 1) == 'R' ? index + 2 : index + 1;
		}

		/**
		 * Handles 'S' cases
		 */
		private int HandleS(string value,
		                    DoubleMetaphoneResult result,
		                    int index,
		                    bool slavoGermanic)
		{
			if (Contains(value, index - 1, 3, "ISL", "YSL"))
			{
				// special cases "island", "isle", "carlisle", "carlysle"
				index++;
			}
			else if (index == 0 && Contains(value, index, 5, "SUGAR"))
			{
				// special case "sugar-"
				result.Append('X', 'S');
				index++;
			}
			else if (Contains(value, index, 2, "SH"))
			{
				if (Contains(value, index + 1, 4,
				             "HEIM", "HOEK", "HOLM", "HOLZ"))
				{
					// germanic
					result.Append('S');
				}
				else
				{
					result.Append('X');
				}
				index += 2;
			}
			else if (Contains(value, index, 3, "SIO", "SIA") || Contains(value, index, 4, "SIAN"))
			{
				// Italian and Armenian
				if (slavoGermanic)
				{
					result.Append('S');
				}
				else
				{
					result.Append('S', 'X');
				}
				index += 3;
			}
			else if ((index == 0 && Contains(value, index + 1, 1, "M", "N", "L", "W")) || Contains(value, index + 1, 1, "Z"))
			{
				// german & anglicisations, e.g. "smith" match "schmidt" //
				// "snider" match "schneider"
				// also, -sz- in slavic language altho in hungarian it //
				//   is pronounced "s"
				result.Append('S', 'X');
				index = Contains(value, index + 1, 1, "Z") ? index + 2 : index + 1;
			}
			else if (Contains(value, index, 2, "SC"))
			{
				index = HandleSC(value, result, index);
			}
			else
			{
				if (index == value.Length - 1 && Contains(value, index - 2,
				                                          2, "AI", "OI"))
				{
					// french e.g. "resnais", "artois"
					result.AppendAlternate('S');
				}
				else
				{
					result.Append('S');
				}
				index = Contains(value, index + 1, 1, "S", "Z") ? index + 2 : index + 1;
			}
			return index;
		}

		/**
		 * Handles 'SC' cases
		 */
		private int HandleSC(string value,
		                     DoubleMetaphoneResult result,
		                     int index)
		{
			if (CharAt(value, index + 2) == 'H')
			{
				// Schlesinger's rule
				if (Contains(value, index + 3,
				             2, "OO", "ER", "EN", "UY", "ED", "EM"))
				{
					// Dutch origin, e.g. "school", "schooner"
					if (Contains(value, index + 3, 2, "ER", "EN"))
					{
						// "schermerhorn", "schenker"
						result.Append("X", "SK");
					}
					else
					{
						result.Append("SK");
					}
				}
				else
				{
					if (index == 0 && !IsVowel(CharAt(value, 3)) && CharAt(value, 3) != 'W')
					{
						result.Append('X', 'S');
					}
					else
					{
						result.Append('X');
					}
				}
			}
			else if (Contains(value, index + 2, 1, "I", "E", "Y"))
			{
				result.Append('S');
			}
			else
			{
				result.Append("SK");
			}
			return index + 3;
		}

		/**
		 * Handles 'T' cases
		 */
		private int HandleT(string value,
		                    DoubleMetaphoneResult result,
		                    int index)
		{
			if (Contains(value, index, 4, "TION"))
			{
				result.Append('X');
				index += 3;
			}
			else if (Contains(value, index, 3, "TCH")) // PIN: removed TIA to make Krisjan and Kristian handled in a similar way
			{
				result.Append('X');
				index += 3;
			}
			
			// special case "thomas", "thames"
			// Contains(value, index + 2, 2, "OM", "AM") ||
			// PIN: IN scandinavian languages the TH is almost always a T
			// PIN: THORE, THEA, THURRE, THEO, THONETTE, THINE, THOLOFF, THRINE, THOMAS, THROND, THERESE
			else if (Contains(value, index, 2, "TH"))
			{
				result.Append('T');
				index += 2;
			}
			else if (Contains(value, index, 3, "TTH"))
			{
				if (
					// special case germanic
					Contains(value, 0, 4, "VAN ", "VON ") ||
					Contains(value, 0, 3, "SCH")
					
					// special case "ottho", "otthe"
					|| Contains(value, index + 3, 1, "O", "E"))
				{
					result.Append('T');
				}
				else
				{
					result.Append('0', 'T');
				}
				index += 2;
			}
			else
			{
				result.Append('T');
				index = Contains(value, index + 1, 1, "T", "D") ? index + 2 : index + 1;
			}
			return index;
		}
		
		/**
		 * Handles 'V' cases
		 */
		private int HandleV(string value,
		                    DoubleMetaphoneResult result,
		                    int index) {
			
			// PIN: If the previous character was F, this can be ignored - like Effvind
			if (index > 0) {
				string target = value.Substring(index - 1, 1).ToUpper();
				if (target.Equals("F")) {
					// ignore
				} else {
					result.Append('F');
				}
				index++;
			} else {
				result.Append('F');
				index = CharAt(value, index + 1) == 'V' ? index + 2 : index + 1;
			}
			return index;
		}
		
		/**
		 * Handles 'W' cases
		 */
		private int HandleW(string value,
		                    DoubleMetaphoneResult result,
		                    int index)
		{
			// PIN: In scandinavian languages the W is not a Vowel but another V - which is handled as V
			return HandleV(value, result, index);
			
			// PIN: THE BELOW IS NOT USED
			
			if (Contains(value, index, 2, "WR"))
			{
				// can also be in middle of word
				result.Append('R');
				index += 2;
			}
			else
			{
				if (index == 0 && (IsVowel(CharAt(value, index + 1)) ||
				                   Contains(value, index, 2, "WH")))
				{
					if (IsVowel(CharAt(value, index + 1)))
					{
						// Wasserman should match Vasserman
						result.Append('A', 'F');
					}
					else
					{
						// need Uomo to match Womo
						result.Append('A');
					}
					index++;
				}
				else if ((index == value.Length - 1 && IsVowel(CharAt(value, index - 1))) ||
				         Contains(value, index - 1,
				                  5, "EWSKI", "EWSKY", "OWSKI", "OWSKY") ||
				         Contains(value, 0, 3, "SCH"))
				{
					// Arnow should match Arnoff
					result.AppendAlternate('F');
					index++;
				}
				else if (Contains(value, index, 4, "WICZ", "WITZ"))
				{
					// Polish e.g. "filipowicz"
					result.Append("TS", "FX");
					index += 4;
				}
				else
				{
					index++;
				}
			}
			return index;
		}

		/**
		 * Handles 'X' cases
		 */
		private int HandleX(string value,
		                    DoubleMetaphoneResult result,
		                    int index)
		{
			// PIN: Initial 'X' is pronounced 'Z' e.g. 'Xavier'
			// except when using Xs as in Xstian (Kristian)
			if (index == 0 && Contains(value, index, 2, "XS", "XS"))
			{
				//'XS' maps to 'KRS'
				result.Append("KRS");
				index += 2;
			}
			else if (index == 0)
			{
				//'Z' maps to 'S'
				result.Append('S');
				index++;
			}
			else
			{
				if (!((index == value.Length - 1) &&
				      (Contains(value, index - 3, 3, "IAU", "EAU") ||
				       Contains(value, index - 2, 2, "AU", "OU"))))
				{
					// French e.g. breaux
					result.Append("KS");
				}
				index = Contains(value, index + 1, 1, "C", "X") ? index + 2 : index + 1;
			}
			return index;
		}

		/**
		 * Handles 'Z' cases
		 */
		private int HandleZ(string value, DoubleMetaphoneResult result, int index,
		                    bool slavoGermanic)
		{
			if (CharAt(value, index + 1) == 'H')
			{
				// Chinese pinyin e.g. "zhao" or Angelina "Zhang"
				result.Append('J');
				index += 2;
			}
			else
			{
				if (Contains(value, index + 1, 2, "ZO", "ZI", "ZA") || (slavoGermanic && (index > 0 && CharAt(value, index - 1) != 'T')))
				{
					result.Append("S", "TS");
				}
				else
				{
					result.Append('S');
				}
				index = CharAt(value, index + 1) == 'Z' ? index + 2 : index + 1;
			}
			return index;
		}

		// BEGIN CONDITIONS

		/**
		 * Complex condition 0 for 'C'
		 */
		private bool ConditionC0(string value, int index)
		{
			if (Contains(value, index, 4, "CHIA"))
			{
				return true;
			}
			else if (index <= 1)
			{
				return false;
			}
			else if (IsVowel(CharAt(value, index - 2)))
			{
				return false;
			}
			else if (!Contains(value, index - 1, 3, "ACH"))
			{
				return false;
			}
			else
			{
				char c = CharAt(value, index + 2);
				return (c != 'I' && c != 'E')
					|| Contains(value, index - 2, 6, "BACHER", "MACHER");
			}
		}

		/**
		 * Complex condition 0 for 'CH'
		 */
		private bool ConditionCH0(string value, int index)
		{
			if (index != 0)
			{
				return false;
			}
			else if (!Contains(value, index + 1, 5, "HARAC", "HARIS") &&
			         !Contains(value, index + 1, 3, "HOR", "HYM", "HIA", "HEM"))
			{
				return false;
			}
			else if (Contains(value, 0, 5, "CHORE"))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/**
		 * Complex condition 1 for 'CH'
		 */
		private bool ConditionCH1(string value, int index)
		{
			return ((Contains(value, 0, 4, "VAN ", "VON ") || Contains(value, 0,
			                                                           3, "SCH")) ||
			        // 'architect' but not 'arch', orchestra', 'orchid'
			        Contains(value, index - 2, 6, "ORCHES", "ARCHIT", "ORCHID") ||
			        Contains(value, index + 2, 1, "T", "S") ||
			        ((Contains(value, index - 1, 1, "A", "O", "U", "E") || index == 0) &&
			         // e.g. 'wachtler', 'weschsler', but not 'tichner'
			         (Contains(value, index + 2, 1, L_R_N_M_B_H_F_V_W_SPACE) || index + 1 == value.Length - 1)));
		}

		/**
		 * Complex condition 0 for 'L'
		 */
		private bool ConditionL0(string value, int index)
		{
			if (index == value.Length - 3 &&
			    Contains(value, index - 1, 4, "ILLO", "ILLA", "ALLE"))
			{
				return true;
			}
			else if ((Contains(value, index - 1, 2, "AS", "OS") ||
			          Contains(value, value.Length - 1, 1, "A", "O")) &&
			         Contains(value, index - 1, 4, "ALLE"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/**
		 * Complex condition 0 for 'M'
		 */
		private bool ConditionM0(string value, int index)
		{
			if (CharAt(value, index + 1) == 'M')
			{
				return true;
			}
			return Contains(value, index - 1, 3, "UMB")
				&& ((index + 1) == value.Length - 1 || Contains(value,
				                                                index + 2, 2, "ER"));
		}

		// BEGIN HELPER FUNCTIONS

		/**
		 * Determines whether or not a value is of slavo-germanic orgin. A value is
		 * of slavo-germanic origin if it contians any of 'W', 'K', 'CZ', or 'WITZ'.
		 */
		private bool IsSlavoGermanic(string value)
		{
			return value.IndexOf('W') > -1 || value.IndexOf('K') > -1 ||
				value.IndexOf("CZ") > -1 || value.IndexOf("WITZ") > -1;
		}

		/**
		 * Determines whether or not a character is a vowel or not
		 */
		private bool IsVowel(char ch)
		{
			return VOWELS.IndexOf(ch) != -1;
		}

		/**
		 * Determines whether or not the value starts with a silent letter.  It will
		 * return <code>true</code> if the value starts with any of 'GN', 'KN',
		 * 'PN', 'WR' or 'PS'.
		 */
		private bool IsSilentStart(string value)
		{
			bool result = false;
			for (int i = 0; i < SILENT_START.Length; i++)
			{
				if (value.StartsWith(SILENT_START[i]))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		/**
		 * Cleans the input
		 */
		private string CleanInput(string input)
		{
			if (input == null)
			{
				return null;
			}
			input = input.Trim();
			if (input.Length == 0)
			{
				return null;
			}
			return input.ToUpper();
		}

		/**
		 * Gets the character at index <code>index</code> if available, otherwise
		 * it returns <code>Character.MIN_VALUE</code> so that there is some sort
		 * of a default
		 */
		protected char CharAt(string value, int index)
		{
			if (index < 0 || index >= value.Length)
			{
				return Char.MinValue;
			}
			return value[index];
		}

		/**
		 * Shortcut method with 1 criteria
		 */
		private static bool Contains(string value, int start, int length,
		                             string criteria)
		{
			return Contains(value, start, length,
			                new string[] { criteria });
		}

		/**
		 * Shortcut method with 2 criteria
		 */
		private static bool Contains(string value, int start, int length,
		                             string criteria1, string criteria2)
		{
			return Contains(value, start, length,
			                new string[] { criteria1, criteria2 });
		}

		/**
		 * Shortcut method with 3 criteria
		 */
		private static bool Contains(string value, int start, int length,
		                             string criteria1, string criteria2,
		                             string criteria3)
		{
			return Contains(value, start, length,
			                new string[] { criteria1, criteria2, criteria3 });
		}

		/**
		 * Shortcut method with 4 criteria
		 */
		private static bool Contains(string value, int start, int length,
		                             string criteria1, string criteria2,
		                             string criteria3, string criteria4)
		{
			return Contains(value, start, length,
			                new string[]
			                {
			                	criteria1, criteria2, criteria3,
			                	criteria4
			                });
		}
		
		/**
		 * Shortcut method with 5 criteria
		 */
		private static bool Contains(string value, int start, int length,
		                             string criteria1, string criteria2,
		                             string criteria3, string criteria4,
		                             string criteria5)
		{
			return Contains(value, start, length,
			                new string[]
			                {
			                	criteria1, criteria2, criteria3,
			                	criteria4, criteria5
			                });
		}

		/**
		 * Shortcut method with 6 criteria
		 */
		private static bool Contains(string value, int start, int length,
		                             string criteria1, string criteria2,
		                             string criteria3, string criteria4,
		                             string criteria5, string criteria6)
		{
			return Contains(value, start, length,
			                new string[]
			                {
			                	criteria1, criteria2, criteria3,
			                	criteria4, criteria5, criteria6
			                });
		}

		/**
		 * Determines whether <code>value</code> contains any of the criteria starting
		 * at index <code>start</code> and matching up to length <code>length</code>
		 */
		protected static bool Contains(string value, int start, int length, string[] criteria)
		{
			bool result = false;
			if (start >= 0 && start + length <= value.Length)
			{
				string target = value.Substring(start, length);

				for (int i = 0; i < criteria.Length; i++)
				{
					if (target == criteria[i])
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	/**
	 * Inner class for storing results, since there is the optional alternate
	 * encoding.
	 */
	internal class DoubleMetaphoneResult
	{
		private StringBuilder primary = null;
		private StringBuilder alternate = null;
		private int maxLength;
		private DoubleMetaphone owner;

		internal DoubleMetaphoneResult(int maxLength, DoubleMetaphone owner)
		{
			this.maxLength = maxLength;
			this.owner = owner;

			primary = new StringBuilder(owner.GetMaxCodeLen());
			alternate = new StringBuilder(owner.GetMaxCodeLen());
		}

		internal void Append(char value)
		{
			AppendPrimary(value);
			AppendAlternate(value);
		}

		internal void Append(char primary, char alternate)
		{
			AppendPrimary(primary);
			AppendAlternate(alternate);
		}

		internal void AppendPrimary(char value)
		{
			if (this.primary.Length < this.maxLength)
			{
				this.primary.Append(value);
			}
		}

		internal void AppendAlternate(char value)
		{
			if (this.alternate.Length < this.maxLength)
			{
				this.alternate.Append(value);
			}
		}

		internal void Append(string value)
		{
			AppendPrimary(value);
			AppendAlternate(value);
		}

		internal void Append(string primary, string alternate)
		{
			AppendPrimary(primary);
			AppendAlternate(alternate);
		}

		internal void AppendPrimary(string value)
		{
			int addChars = this.maxLength - this.primary.Length;
			if (value.Length <= addChars)
			{
				this.primary.Append(value);
			}
			else
			{
				this.primary.Append(value.Substring(0, addChars));
			}
		}

		internal void AppendAlternate(string value)
		{
			int addChars = this.maxLength - this.alternate.Length;
			if (value.Length <= addChars)
			{
				this.alternate.Append(value);
			}
			else
			{
				this.alternate.Append(value.Substring(0, addChars));
			}
		}

		internal string GetPrimary()
		{
			return this.primary.ToString();
		}

		internal string GetAlternate()
		{
			return this.alternate.ToString();
		}

		internal bool IsComplete()
		{
			return this.primary.Length >= this.maxLength &&
				this.alternate.Length >= this.maxLength;
		}
	}
}