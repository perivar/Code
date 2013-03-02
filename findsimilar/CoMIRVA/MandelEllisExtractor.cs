﻿using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

using Comirva.Audio;
using Comirva.Audio.Util.Maths;
using Comirva.Audio.Feature;

namespace Comirva.Audio.Extraction
{
	/// <summary>
	/// CoMIRVA: Collection of Music Information Retrieval and Visualization Applications
	/// Ported from Java to C# by perivar@nerseth.com
	/// </summary>
	public class MandelEllisExtractor
	{
		public float sampleRate = 11025.0f;
		public int windowSize = 512;
		public int numberCoefficients = 20;
		public int numberFilters = 40;

		protected internal MFCC mfcc;

		public MandelEllisExtractor(float sampleRate, int windowSize, int numberCoefficients, int numberFilters)
		{
			this.sampleRate = sampleRate;
			this.windowSize = windowSize;
			this.numberCoefficients = numberCoefficients;
			this.numberFilters = numberFilters;
			
			this.mfcc = new MFCC(sampleRate, windowSize, numberCoefficients, true, 20.0, sampleRate/2, numberFilters);
		}

		public AudioFeature Calculate(double[] input)
		{
			//pack the mfccs into a pointlist
			double[][] mfccCoefficients = mfcc.Process(input);

			try {
				//check if element 0 exists
				if(mfccCoefficients.Length == 0)
					throw new ArgumentException("The input stream is to short to process;");

				//create mfcc matrix
				Matrix mfccs = new Matrix(mfccCoefficients);
				#if DEBUG
				mfccs.WriteText("mfccdata-mandelellis.txt");
				#endif

				// compute mean
				Matrix mean = mfccs.Mean(1).Transpose();
				#if DEBUG
				mean.WriteText("mean-mandelellis.txt");
				#endif
				
				// create covariance matrix
				Matrix covarMatrix = mfccs.Cov();
				#if DEBUG
				covarMatrix.WriteText("covariance-mandelellis.txt");
				#endif
				
				// compute inverse covariance
				Matrix covarMatrixInv = covarMatrix.Inverse();
				#if DEBUG
				covarMatrixInv.WriteText("inverse_covariance-mandelellis.txt");
				#endif

				MandelEllis.GmmMe gmmMe = new MandelEllis.GmmMe(mean, covarMatrix, covarMatrixInv);
				MandelEllis mandelEllis = new MandelEllis(gmmMe);
				return mandelEllis;
			} catch (Exception) {
				Console.Error.WriteLine("Mandel Ellis Extraction Failed!");
				return null;
			}
		}

		public virtual int GetAttributeType()
		{
			return typeof(MandelEllis).Name.GetHashCode();
		}

		public override string ToString()
		{
			return "Mandel Ellis";
		}
	}
}