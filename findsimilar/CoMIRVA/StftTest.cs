/*
 * Mirage - High Performance Music Similarity and Automatic Playlist Generator
 * http://hop.at/mirage
 * 
 * Copyright (C) 2007 Dominik Schnitzer <dominik@schnitzer.at>
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA  02110-1301, USA.
 */

using System;
using System.Diagnostics;

using Comirva.Audio.Util.Maths;

namespace Comirva.Audio.Extraction {

	/// <summary>
	/// Class to perform a Short Time Fourier Transformation
	/// </summary>
	public class StftTest
	{
		int winsize;
		int hopsize;
		Mirage.Fft fft;
		
		/// <summary>
		/// Instantiate a new Stft Class
		/// </summary>
		/// <param name="winsize">FFT window size</param>
		/// <param name="hopsize">Value to hop on to the next window</param>
		/// <param name="window">Window function to apply to every window
		///     processed</param>
		public StftTest(int winsize, int hopsize, Mirage.IWindowFunction window)
		{
			this.winsize = winsize;
			this.hopsize = hopsize;
			fft = new Mirage.Fft(winsize, window);
		}
		
		/// <summary>
		/// Apply the STFT on the audiodata
		/// </summary>
		/// <param name="audiodata">Audiodata to apply the STFT on</param>
		/// <returns>A matrix with the result of the STFT</returns>
		public Matrix Apply(float[] audiodata)
		{
			Mirage.DbgTimer t = new Mirage.DbgTimer();
			t.Start();
			
			int hops = (audiodata.Length - winsize)/hopsize;
			
			// Create a Matrix with "winsize" Rows and "hops" Columns
			// Matrix[Row, Column]
			Matrix stft = new Matrix(winsize/2 +1, hops);
			
			for (int i = 0; i < hops; i++) {
				fft.ComputeComirvaMatrix(ref stft, i, audiodata, i*hopsize);
			}
			
			Mirage.Dbg.WriteLine("Stft Execution Time: " + t.Stop() + "ms");
			
			return stft;
		}
	}
}
