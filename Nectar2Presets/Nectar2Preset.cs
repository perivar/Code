using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using CommonUtils;

namespace Nectar2Preset
{
	/// <summary>
	/// Description of Nectar2Preset.
	/// </summary>
	public class Nectar2Preset
	{
		public enum DynamicsMode
		{
			Digital = 0,
			Vintage = 1,
			Optical = 2,
			SolidState = 3
		};

		public enum PitchRange
		{
			Low = 0,
			Medium = 1,
			High = 2
		};

		public enum HarmonyVoiceInterval
		{
			Voices_2 = 0,
			Voices_3 = 1,
			Voices_4 = 2,
			Voices_5 = 3,
			Voices_6 = 4,
			Voices_7 = 5,
			Voices_Octave = 6
		};

		public enum HarmonyVoiceIntervalDirection
		{
			Down = 0,
			Unison = 1,
			Up = 2
		};

		public enum SaturationMode
		{
			Analog = 0,
			Retro = 1,
			Tape = 2,
			Tube = 3,
			Warm = 4
		};

		public enum EqualizerBandShape
		{
			Bell_Bell = 5,
			Bell_VintageBell = 6,
			LowShelf_Analog = 4,
			LowShelf_Vintage = 3,
			LowShelf_Resonant = 2,
			HighShelf_Analog = 7,
			HighShelf_Vintage = 8,
			HighShelf_Resonant = 9,
			Lowpass_Flat = 11,
			Lowpass_Resonant = 10,
			Highpass_Flat = 0,
			Highpass_Resonant = 1,
			Baxandall_Bass = 12,
			Baxandall_Treble = 13
		};
		
		public enum HostTempoSyncRatio
		{
			Sync_8_dot = 29,
			Sync_8 = 28,
			Sync_8_triple = 27,
			Sync_4_dot = 26,
			Sync_4 = 25,
			Sync_4_triple = 24,
			Sync_2_dot = 23,
			Sync_2 = 22,
			Sync_2_triple = 21,
			Sync_1_dot = 20,
			Sync_1 = 19,
			Sync_1_triple = 18,
			Sync_1_2_dot = 17,
			Sync_1_2 = 16,
			Sync_1_2_triple = 15,
			Sync_1_4_dot = 14,
			Sync_1_4 = 13, // Is not used
			Sync_1_4_triple = 12,
			Sync_1_8_dot = 11,
			Sync_1_8 = 10,
			Sync_1_8_triple = 9,
			Sync_1_16_dot = 8,
			Sync_1_16 = 7,
			Sync_1_16_triple = 6,
			Sync_1_32_dot = 5,
			Sync_1_32 = 4,
			Sync_1_32_triple = 3,
			Sync_1_64_dot = 2,
			Sync_1_64 = 1,
			Sync_1_64_triple = 0,
		}
		
		public enum FXModulationModes {
			Phase,
			Flange,
			Chorus
		}
		
		public enum FXTimeModes
		{
			Echo,
			Shred
		}
		
		#region Define the attributes
		public decimal Dynamics1Attack { get; set; }
		public decimal Dynamics1Gain { get; set; }
		public DynamicsMode Dynamics1Mode { get; set; }
		public bool Dynamics1RMSDetection { get; set; }
		public decimal Dynamics1Ratio { get; set; }
		public decimal Dynamics1Release { get; set; }
		public decimal Dynamics1Threshold { get; set; }
		public decimal Dynamics2Attack { get; set; }
		public bool Dynamics2Bypass { get; set; }
		public decimal Dynamics2Gain { get; set; }
		public DynamicsMode Dynamics2Mode { get; set; }
		public bool Dynamics2RMSDetection { get; set; }
		public decimal Dynamics2Ratio { get; set; }
		public decimal Dynamics2Release { get; set; }
		public decimal Dynamics2Threshold { get; set; }
		
		public bool DeEsserBypass { get; set; }
		public decimal DeEsserEssReduction { get; set; }
		public decimal DeEsserFrequency { get; set; }
		public bool DeEsserOutputEssOnly { get; set; }
		
		public bool DelayBypass { get; set; }
		public bool DelayHostTempoSync { get; set; }
		public HostTempoSyncRatio DelayHostTempoSyncRatio { get; set; }
		public decimal DelayDelay { get; set; }
		public bool DelayDistortionMode { get; set; }
		public decimal DelayDryMixPercent { get; set; }
		public bool DelayEnableModulation { get; set; }
		public decimal DelayFeedbackPercent { get; set; }
		public decimal DelayHighCutoff { get; set; }
		public decimal DelayLowCutoff { get; set; }
		public decimal DelayModulationDepth { get; set; }
		public decimal DelayModulationRate { get; set; }
		public decimal DelaySpread { get; set; }
		public decimal DelayTrash { get; set; }
		public decimal DelayWetMixPercent { get; set; }
		
		public decimal Dynamics2EqualizerBand0Frequency { get; set; }
		public decimal Dynamics2EqualizerBand0Gain { get; set; }
		public decimal Dynamics2EqualizerBand1Frequency { get; set; }
		public decimal Dynamics2EqualizerBand1Gain { get; set; }
		public bool Dynamics2EqualizerBypass { get; set; }
		
		public bool EqualizerBand0Enable { get; set; }
		public decimal EqualizerBand0Frequency { get; set; }
		public decimal EqualizerBand0Gain { get; set; }
		public decimal EqualizerBand0Q { get; set; }
		public EqualizerBandShape EqualizerBand0Shape { get; set; }
		
		public bool EqualizerBand1Enable { get; set; }
		public decimal EqualizerBand1Frequency { get; set; }
		public decimal EqualizerBand1Gain { get; set; }
		public decimal EqualizerBand1Q { get; set; }
		public EqualizerBandShape EqualizerBand1Shape { get; set; }
		
		public bool EqualizerBand2Enable { get; set; }
		public decimal EqualizerBand2Frequency { get; set; }
		public decimal EqualizerBand2Gain { get; set; }
		public decimal EqualizerBand2Q { get; set; }
		public EqualizerBandShape EqualizerBand2Shape { get; set; }
		
		public bool EqualizerBand3Enable { get; set; }
		public decimal EqualizerBand3Frequency { get; set; }
		public decimal EqualizerBand3Gain { get; set; }
		public decimal EqualizerBand3Q { get; set; }
		public EqualizerBandShape EqualizerBand3Shape { get; set; }
		
		public bool EqualizerBand4Enable { get; set; }
		public decimal EqualizerBand4Frequency { get; set; }
		public decimal EqualizerBand4Gain { get; set; }
		public decimal EqualizerBand4Q { get; set; }
		public EqualizerBandShape EqualizerBand4Shape { get; set; }
		
		public bool EqualizerBand5Enable { get; set; }
		public decimal EqualizerBand5Frequency { get; set; }
		public decimal EqualizerBand5Gain { get; set; }
		public decimal EqualizerBand5Q { get; set; }
		public EqualizerBandShape EqualizerBand5Shape { get; set; }
		
		public bool EqualizerBand6Enable { get; set; }
		public decimal EqualizerBand6Frequency { get; set; }
		public decimal EqualizerBand6Gain { get; set; }
		public decimal EqualizerBand6Q { get; set; }
		public EqualizerBandShape EqualizerBand6Shape { get; set; }
		
		public bool EqualizerBand7Enable { get; set; }
		public decimal EqualizerBand7Frequency { get; set; }
		public decimal EqualizerBand7Gain { get; set; }
		public decimal EqualizerBand7Q { get; set; }
		public EqualizerBandShape EqualizerBand7Shape { get; set; }
		
		public bool FXBypass { get; set; }
		public decimal FXDistortionOverdrive { get; set; }
		public decimal FXDistortionSend { get; set; }
		public decimal FXDownsamplingAmount { get; set; }
		public bool FXDownsamplingEnable { get; set; }
		public decimal FXDryMix { get; set; }
		public decimal FXManualTempo { get; set; }
		public decimal FXModulationDepth { get; set; }
		public decimal FXModulationFeedback { get; set; }
		public FXModulationModes FXModulationMode { get; set; }
		public decimal FXModulationSend { get; set; }
		public bool FXParallelProcessing { get; set; }
		public decimal FXTimeDepth { get; set; }
		public decimal FXTimeFeedback { get; set; }
		public FXTimeModes FXTimeMode { get; set; }
		public decimal FXTimeSend { get; set; }
		public decimal FXWetMix { get; set; }

		public decimal GateExpanderAttack { get; set; }
		public decimal GateExpanderBandGain { get; set; }
		public bool GateExpanderBypass { get; set; }
		public decimal GateExpanderFloorOutputThreshold { get; set; }
		public bool GateExpanderRMSDetection { get; set; }
		public decimal GateExpanderRatio { get; set; }
		public decimal GateExpanderRelease { get; set; }
		public decimal GateExpanderThreshold { get; set; }

		public decimal GlobalGlobalCompressionMix { get; set; }
		public bool GlobalGlobalParallelCompression { get; set; }

		public bool HarmonyBypass { get; set; }
		public decimal HarmonyGainSpread { get; set; }
		public decimal HarmonyHighShelfFreq { get; set; }
		public decimal HarmonyHighShelfGain { get; set; }
		public decimal HarmonyLowShelfFreq { get; set; }
		public decimal HarmonyLowShelfGain { get; set; }
		public decimal HarmonyPanSpread { get; set; }
		public decimal HarmonyPitchCorrection { get; set; }
		public decimal HarmonyPitchVariation { get; set; }
		public bool HarmonySoloHarmony { get; set; }
		public decimal HarmonyTimeVariation { get; set; }
		public bool HarmonyVoiceEnable3 { get; set; }
		public bool HarmonyVoiceEnable4 { get; set; }
		public decimal HarmonyVoiceGain1 { get; set; }
		public decimal HarmonyVoiceGain2 { get; set; }
		public decimal HarmonyVoiceGain3 { get; set; }
		public decimal HarmonyVoiceGain4 { get; set; }
		public HarmonyVoiceInterval HarmonyVoiceInterval1 { get; set; }
		public HarmonyVoiceInterval HarmonyVoiceInterval2 { get; set; }
		public HarmonyVoiceInterval HarmonyVoiceInterval3 { get; set; }
		public HarmonyVoiceInterval HarmonyVoiceInterval4 { get; set; }
		public HarmonyVoiceIntervalDirection HarmonyVoiceIntervalDirection1 { get; set; }
		public HarmonyVoiceIntervalDirection HarmonyVoiceIntervalDirection2 { get; set; }
		public HarmonyVoiceIntervalDirection HarmonyVoiceIntervalDirection3 { get; set; }
		public HarmonyVoiceIntervalDirection HarmonyVoiceIntervalDirection4 { get; set; }
		public decimal HarmonyVoicePan1 { get; set; }
		public decimal HarmonyVoicePan2 { get; set; }
		public decimal HarmonyVoicePan3 { get; set; }
		public decimal HarmonyVoicePan4 { get; set; }

		public bool MaximizerBypass { get; set; }
		public decimal MaximizerMargin { get; set; }
		public decimal MaximizerThreshold { get; set; }

		public bool PitchBypass { get; set; }
		public decimal PitchFormantScaling { get; set; }
		public decimal PitchFormantShift { get; set; }
		public decimal PitchSmoothing { get; set; }
		public PitchRange PitchVocalRange { get; set; }
		public decimal PitchVoice1Transposition { get; set; }

		// Reverb (EMT 140 plate reverb)
		public bool ReverbBypass { get; set; }
		public decimal ReverbDamping { get; set; }
		public decimal ReverbDryMix { get; set; }
		public decimal ReverbHighCutoff { get; set; }
		public decimal ReverbLowCutoff { get; set; }
		public decimal ReverbPreDelay { get; set; }
		public decimal ReverbSaturation { get; set; }
		public bool ReverbSaturationEnable { get; set; }
		public decimal ReverbWetMix { get; set; }
		public decimal ReverbWidth { get; set; }
		
		// Saturation
		public decimal ExciterAmountPercent { get; set; }
		public bool ExciterBypass { get; set; }
		public decimal ExciterMixPercent { get; set; }
		public SaturationMode ExciterMode { get; set; }
		public decimal ExciterPostfilterFreq { get; set; }
		public decimal ExciterPostfilterGain { get; set; }
		#endregion

		public string PresetName { get; set; }
		
		public Nectar2Preset(string presetName)
		{
			PresetName = presetName;
			
			// add this to the constructor
			#region Initialize the attributes
			GlobalGlobalCompressionMix = 100m;
			GlobalGlobalParallelCompression = false;
			
			// Compressors
			Dynamics1Attack = 5m;
			Dynamics1Gain = 0m;
			Dynamics1Mode = DynamicsMode.Digital;
			Dynamics1RMSDetection = false;
			Dynamics1Ratio = 2.5m;
			Dynamics1Release = 50m;
			Dynamics1Threshold = 0m;
			
			Dynamics2Bypass = true;
			Dynamics2Attack = 5m;
			Dynamics2Gain = 0m;
			Dynamics2Mode = DynamicsMode.Digital;
			Dynamics2RMSDetection = false;
			Dynamics2Ratio = 2.5m;
			Dynamics2Release = 50m;
			Dynamics2Threshold = 0m;
			
			Dynamics2EqualizerBand0Frequency = 250m;
			Dynamics2EqualizerBand0Gain = 0m;
			Dynamics2EqualizerBand1Frequency = 4000m;
			Dynamics2EqualizerBand1Gain = 0m;
			Dynamics2EqualizerBypass = false;
			
			// De-Esser
			DeEsserBypass = true;
			DeEsserEssReduction = -4.0m;
			DeEsserFrequency = 2500m;
			DeEsserOutputEssOnly = false;
			
			// Delay
			DelayBypass = true;
			DelayHostTempoSync = false;
			DelayHostTempoSyncRatio = HostTempoSyncRatio.Sync_1_4;
			DelayDelay = 200m;
			DelayDistortionMode = false;
			DelayDryMixPercent = 80m;
			DelayEnableModulation = false;
			DelayFeedbackPercent = 20m;
			DelayHighCutoff = 20000m;
			DelayLowCutoff = 20m;
			DelayModulationDepth = 50m;
			DelayModulationRate = 2m;
			DelaySpread = 100m;
			DelayTrash = 25m;
			DelayWetMixPercent = 20m;

			// EQ
			EqualizerBand0Enable = true;
			EqualizerBand0Frequency = 30m;
			EqualizerBand0Gain = 0m;
			EqualizerBand0Q = 1.00m;
			EqualizerBand0Shape = EqualizerBandShape.LowShelf_Analog;
			
			EqualizerBand1Enable = true;
			EqualizerBand1Frequency = 100m;
			EqualizerBand1Gain = 0m;
			EqualizerBand1Q = 0.20m;
			EqualizerBand1Shape = EqualizerBandShape.Bell_Bell;
			
			EqualizerBand2Enable = false;
			EqualizerBand2Frequency = 300m;
			EqualizerBand2Gain = 0m;
			EqualizerBand2Q = 0.30m;
			EqualizerBand2Shape = EqualizerBandShape.Bell_Bell;
			
			EqualizerBand3Enable = true;
			EqualizerBand3Frequency = 700m;
			EqualizerBand3Gain = 0m;
			EqualizerBand3Q = 0.30m;
			EqualizerBand3Shape = EqualizerBandShape.Bell_Bell;
			
			EqualizerBand4Enable = false;
			EqualizerBand4Frequency = 1800m;
			EqualizerBand4Gain = 0m;
			EqualizerBand4Q = 0.30m;
			EqualizerBand4Shape = EqualizerBandShape.Bell_Bell;
			
			EqualizerBand5Enable = true;
			EqualizerBand5Frequency = 4000m;
			EqualizerBand5Gain = 0m;
			EqualizerBand5Q = 0.30m;
			EqualizerBand5Shape = EqualizerBandShape.Bell_Bell;
			
			EqualizerBand6Enable = false;
			EqualizerBand6Frequency = 8000m;
			EqualizerBand6Gain = 0m;
			EqualizerBand6Q = 0.30m;
			EqualizerBand6Shape = EqualizerBandShape.Bell_Bell;
			
			EqualizerBand7Enable = true;
			EqualizerBand7Frequency = 16000m;
			EqualizerBand7Gain = 0m;
			EqualizerBand7Q = 1.00m;
			EqualizerBand7Shape = EqualizerBandShape.HighShelf_Analog;
			
			// FX
			FXBypass = true;
			FXDownsamplingEnable = false; 	// Decimate
			FXDistortionOverdrive = 7m;
			FXDistortionSend = 0m;
			FXDownsamplingAmount = 50m; 	// Decimate

			FXModulationMode = FXModulationModes.Chorus;
			FXModulationFeedback = 50m;
			FXModulationDepth = 50m;
			FXModulationSend = 0m;

			// Repeat
			FXTimeMode = FXTimeModes.Echo;
			FXTimeFeedback = 20m;
			FXTimeDepth = 20m;
			FXTimeSend = 0m;
			
			FXParallelProcessing = false;
			FXManualTempo = 120m;
			FXDryMix = 100m;
			FXWetMix = 100m;
			
			// Gate
			GateExpanderBypass = true;
			GateExpanderBandGain = 0m;
			GateExpanderRMSDetection = false;
			GateExpanderRatio = 15.0m;
			GateExpanderAttack = 10m;
			GateExpanderRelease = 100m;
			GateExpanderThreshold = -96.0m;
			GateExpanderFloorOutputThreshold = -96.0m;
			
			// Harmony
			HarmonyBypass = true;
			HarmonySoloHarmony = false;
			HarmonyHighShelfFreq = 4000m;
			HarmonyHighShelfGain = 0m;
			HarmonyLowShelfFreq = 200m;
			HarmonyLowShelfGain = 0m;
			HarmonyGainSpread = 100.0m;
			HarmonyPanSpread = 100.0m;
			HarmonyPitchCorrection = 0m;
			HarmonyPitchVariation = 0m;
			HarmonyTimeVariation = 0m;
			HarmonyVoiceEnable3 = false;
			HarmonyVoiceEnable4 = false;
			HarmonyVoiceInterval1 = HarmonyVoiceInterval.Voices_4;
			HarmonyVoiceInterval2 = HarmonyVoiceInterval.Voices_3;
			HarmonyVoiceInterval3 = HarmonyVoiceInterval.Voices_Octave;
			HarmonyVoiceInterval4 = HarmonyVoiceInterval.Voices_Octave;
			HarmonyVoiceIntervalDirection1 = HarmonyVoiceIntervalDirection.Down;
			HarmonyVoiceIntervalDirection2 = HarmonyVoiceIntervalDirection.Up;
			HarmonyVoiceIntervalDirection3 = HarmonyVoiceIntervalDirection.Down;
			HarmonyVoiceIntervalDirection4 = HarmonyVoiceIntervalDirection.Up;
			HarmonyVoiceGain1 = -12.0m;
			HarmonyVoiceGain2 = -12.0m;
			HarmonyVoiceGain3 = -12.0m;
			HarmonyVoiceGain4 = -12.0m;
			HarmonyVoicePan1 = -66.0m;
			HarmonyVoicePan2 = 66.0m;
			HarmonyVoicePan3 = -33.0m;
			HarmonyVoicePan4 = 33.0m;
			
			//Limiter
			MaximizerBypass = true;
			MaximizerMargin = 0m;
			MaximizerThreshold = 0m;
			
			// Pitch
			PitchBypass = true;
			PitchVocalRange = PitchRange.Medium;
			PitchSmoothing = 20m;	// Speed
			PitchVoice1Transposition = 0m;
			PitchFormantShift = 0m;
			PitchFormantScaling = 10m;
			
			// EMT 140 plate reverb
			ReverbBypass = true;
			ReverbPreDelay = 0m;
			ReverbDamping = 1.8m;	// Decay
			ReverbWidth = 100m;
			ReverbHighCutoff = 20000m;
			ReverbLowCutoff = 20m;
			ReverbSaturationEnable = false;
			ReverbSaturation = 3m;
			ReverbDryMix = 100m;
			ReverbWetMix = 20m;
			
			// Saturation
			ExciterBypass = true;
			ExciterMode = SaturationMode.Tape;
			ExciterAmountPercent = 0m;
			ExciterMixPercent = 100m;
			ExciterPostfilterFreq = 7000m;
			ExciterPostfilterGain = 0m;
			#endregion
		}
		
		#region Read attributes from Xml
		public void ReadPreset(XmlDocument xmldoc) {
			XmlNode dynamics1AttackNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Attack']");
			if (dynamics1AttackNode != null) {
				// read the dynamics1Attack attribute value
				Dynamics1Attack = NumberUtils.DecimalTryParseOrZero(dynamics1AttackNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics1GainNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Gain']");
			if (dynamics1GainNode != null) {
				// read the dynamics1Gain attribute value
				Dynamics1Gain = NumberUtils.DecimalTryParseOrZero(dynamics1GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics1ModeNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Mode']");
			if (dynamics1ModeNode != null) {
				// read the dynamics1Mode attribute value
				Dynamics1Mode = StringUtils.StringToEnum<DynamicsMode>(dynamics1ModeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics1RMSDetectionNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='RMS Detection']");
			if (dynamics1RMSDetectionNode != null) {
				// read the dynamics1RMSDetection attribute value
				Dynamics1RMSDetection = NumberUtils.BooleanTryParseOrZero(dynamics1RMSDetectionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics1RatioNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Ratio']");
			if (dynamics1RatioNode != null) {
				// read the dynamics1Ratio attribute value
				Dynamics1Ratio = NumberUtils.DecimalTryParseOrZero(dynamics1RatioNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics1ReleaseNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Release']");
			if (dynamics1ReleaseNode != null) {
				// read the dynamics1Release attribute value
				Dynamics1Release = NumberUtils.DecimalTryParseOrZero(dynamics1ReleaseNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics1ThresholdNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 1' and @ParamID='Threshold']");
			if (dynamics1ThresholdNode != null) {
				// read the dynamics1Threshold attribute value
				Dynamics1Threshold = NumberUtils.DecimalTryParseOrZero(dynamics1ThresholdNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2AttackNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Attack']");
			if (dynamics2AttackNode != null) {
				// read the dynamics2Attack attribute value
				Dynamics2Attack = NumberUtils.DecimalTryParseOrZero(dynamics2AttackNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2BypassNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Bypass']");
			if (dynamics2BypassNode != null) {
				// read the dynamics2Bypass attribute value
				Dynamics2Bypass = NumberUtils.BooleanTryParseOrZero(dynamics2BypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2GainNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Gain']");
			if (dynamics2GainNode != null) {
				// read the dynamics2Gain attribute value
				Dynamics2Gain = NumberUtils.DecimalTryParseOrZero(dynamics2GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2ModeNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Mode']");
			if (dynamics2ModeNode != null) {
				// read the dynamics2Mode attribute value
				Dynamics2Mode = StringUtils.StringToEnum<DynamicsMode>(dynamics2ModeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2RMSDetectionNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='RMS Detection']");
			if (dynamics2RMSDetectionNode != null) {
				// read the dynamics2RMSDetection attribute value
				Dynamics2RMSDetection = NumberUtils.BooleanTryParseOrZero(dynamics2RMSDetectionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2RatioNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Ratio']");
			if (dynamics2RatioNode != null) {
				// read the dynamics2Ratio attribute value
				Dynamics2Ratio = NumberUtils.DecimalTryParseOrZero(dynamics2RatioNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2ReleaseNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Release']");
			if (dynamics2ReleaseNode != null) {
				// read the dynamics2Release attribute value
				Dynamics2Release = NumberUtils.DecimalTryParseOrZero(dynamics2ReleaseNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2ThresholdNode = xmldoc.SelectSingleNode("/Nectar/Compressors/Param[@ElementID='Dynamics 2' and @ParamID='Threshold']");
			if (dynamics2ThresholdNode != null) {
				// read the dynamics2Threshold attribute value
				Dynamics2Threshold = NumberUtils.DecimalTryParseOrZero(dynamics2ThresholdNode.SelectSingleNode("@Value").Value);
			}

			XmlNode deesserBypassNode = xmldoc.SelectSingleNode("/Nectar/DeEsser/Param[@ElementID='DeEsser' and @ParamID='Bypass']");
			if (deesserBypassNode != null) {
				// read the deesserBypass attribute value
				DeEsserBypass = NumberUtils.BooleanTryParseOrZero(deesserBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode deesserEssReductionNode = xmldoc.SelectSingleNode("/Nectar/DeEsser/Param[@ElementID='DeEsser' and @ParamID='Ess Reduction']");
			if (deesserEssReductionNode != null) {
				// read the deesserEssReduction attribute value
				DeEsserEssReduction = NumberUtils.DecimalTryParseOrZero(deesserEssReductionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode deesserFrequencyNode = xmldoc.SelectSingleNode("/Nectar/DeEsser/Param[@ElementID='DeEsser' and @ParamID='Frequency']");
			if (deesserFrequencyNode != null) {
				// read the deesserFrequency attribute value
				DeEsserFrequency = NumberUtils.DecimalTryParseOrZero(deesserFrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode deesserOutputEssOnlyNode = xmldoc.SelectSingleNode("/Nectar/DeEsser/Param[@ElementID='DeEsser' and @ParamID='Output Ess Only']");
			if (deesserOutputEssOnlyNode != null) {
				// read the deesserOutputEssOnly attribute value
				DeEsserOutputEssOnly = NumberUtils.BooleanTryParseOrZero(deesserOutputEssOnlyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayBypassNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Bypass']");
			if (delayBypassNode != null) {
				// read the delayBypass attribute value
				DelayBypass = NumberUtils.BooleanTryParseOrZero(delayBypassNode.SelectSingleNode("@Value").Value);
			}
			
			XmlNode delayHostTempoSyncNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Host Tempo Sync']");
			if (delayHostTempoSyncNode != null) {
				// read the delayHostTempoSync attribute value
				DelayHostTempoSync = NumberUtils.BooleanTryParseOrZero(delayHostTempoSyncNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayHostTempoSyncRatioNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Host Tempo Sync Ratio']");
			if (delayHostTempoSyncRatioNode != null) {
				// read the delayHostTempoSyncRatioNode attribute value
				DelayHostTempoSyncRatio = StringUtils.StringToEnum<HostTempoSyncRatio>(delayHostTempoSyncRatioNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayDelayNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Delay']");
			if (delayDelayNode != null) {
				// read the delayDelay attribute value
				DelayDelay = NumberUtils.DecimalTryParseOrZero(delayDelayNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayDistortionModeNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Distortion Mode']");
			if (delayDistortionModeNode != null) {
				// read the delayDistortionMode attribute value
				DelayDistortionMode = NumberUtils.BooleanTryParseOrZero(delayDistortionModeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayDryMixPercentNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Dry Mix Percent']");
			if (delayDryMixPercentNode != null) {
				// read the delayDryMixPercent attribute value
				DelayDryMixPercent = NumberUtils.DecimalTryParseOrZero(delayDryMixPercentNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayEnableModulationNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Enable Modulation']");
			if (delayEnableModulationNode != null) {
				// read the delayEnableModulation attribute value
				DelayEnableModulation = NumberUtils.BooleanTryParseOrZero(delayEnableModulationNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayFeedbackPercentNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Feedback Percent']");
			if (delayFeedbackPercentNode != null) {
				// read the delayFeedbackPercent attribute value
				DelayFeedbackPercent = NumberUtils.DecimalTryParseOrZero(delayFeedbackPercentNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayHighCutoffNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='High Cutoff']");
			if (delayHighCutoffNode != null) {
				// read the delayHighCutoff attribute value
				DelayHighCutoff = NumberUtils.DecimalTryParseOrZero(delayHighCutoffNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayLowCutoffNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Low Cutoff']");
			if (delayLowCutoffNode != null) {
				// read the delayLowCutoff attribute value
				DelayLowCutoff = NumberUtils.DecimalTryParseOrZero(delayLowCutoffNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayModulationDepthNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Modulation Depth']");
			if (delayModulationDepthNode != null) {
				// read the delayModulationDepth attribute value
				DelayModulationDepth = NumberUtils.DecimalTryParseOrZero(delayModulationDepthNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayModulationRateNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Modulation Rate']");
			if (delayModulationRateNode != null) {
				// read the delayModulationRate attribute value
				DelayModulationRate = NumberUtils.DecimalTryParseOrZero(delayModulationRateNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delaySpreadNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Spread']");
			if (delaySpreadNode != null) {
				// read the delaySpread attribute value
				DelaySpread = NumberUtils.DecimalTryParseOrZero(delaySpreadNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayTrashNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Trash']");
			if (delayTrashNode != null) {
				// read the delayTrash attribute value
				DelayTrash = NumberUtils.DecimalTryParseOrZero(delayTrashNode.SelectSingleNode("@Value").Value);
			}

			XmlNode delayWetMixPercentNode = xmldoc.SelectSingleNode("/Nectar/Delay/Param[@ElementID='Delay' and @ParamID='Wet Mix Percent']");
			if (delayWetMixPercentNode != null) {
				// read the delayWetMixPercent attribute value
				DelayWetMixPercent = NumberUtils.DecimalTryParseOrZero(delayWetMixPercentNode.SelectSingleNode("@Value").Value);
			}


			// Dynamics2Equalizer
			XmlNode dynamics2EqualizerBand0FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Dynamics 2 Equalizer' and @ParamID='Band 0 Frequency']");
			if (dynamics2EqualizerBand0FrequencyNode != null) {
				// read the dynamics2EqualizerBand0FrequencyNode attribute value
				Dynamics2EqualizerBand0Frequency = NumberUtils.DecimalTryParseOrZero(dynamics2EqualizerBand0FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2EqualizerBand0GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Dynamics 2 Equalizer' and @ParamID='Band 0 Gain']");
			if (dynamics2EqualizerBand0GainNode != null) {
				// read the dynamics2EqualizerBand0GainNode attribute value
				Dynamics2EqualizerBand0Gain = NumberUtils.DecimalTryParseOrZero(dynamics2EqualizerBand0GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2EqualizerBand1FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Dynamics 2 Equalizer' and @ParamID='Band 1 Frequency']");
			if (dynamics2EqualizerBand1FrequencyNode != null) {
				// read the dynamics2EqualizerBand1FrequencyNode attribute value
				Dynamics2EqualizerBand1Frequency = NumberUtils.DecimalTryParseOrZero(dynamics2EqualizerBand1FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode dynamics2EqualizerBand1GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Dynamics 2 Equalizer' and @ParamID='Band 1 Gain']");
			if (dynamics2EqualizerBand1GainNode != null) {
				// read the dynamics2EqualizerBand1GainNode attribute value
				Dynamics2EqualizerBand1Gain = NumberUtils.DecimalTryParseOrZero(dynamics2EqualizerBand1GainNode.SelectSingleNode("@Value").Value);
			}
			
			XmlNode dynamics2EqualizerBypassNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Dynamics 2 Equalizer' and @ParamID='Bypass']");
			if (dynamics2EqualizerBypassNode != null) {
				// read the dynamics2EqualizerBypass attribute value
				Dynamics2EqualizerBypass = NumberUtils.BooleanTryParseOrZero(dynamics2EqualizerBypassNode.SelectSingleNode("@Value").Value);
			}

			#region Equalizer
			XmlNode equalizerBand0EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 0 Enable']");
			if (equalizerBand0EnableNode != null) {
				// read the equalizerBand0Enable attribute value
				EqualizerBand0Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand0EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand0FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 0 Frequency']");
			if (equalizerBand0FrequencyNode != null) {
				// read the equalizerBand0Frequency attribute value
				EqualizerBand0Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand0FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand0GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 0 Gain']");
			if (equalizerBand0GainNode != null) {
				// read the equalizerBand0Gain attribute value
				EqualizerBand0Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand0GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand0QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 0 Q']");
			if (equalizerBand0QNode != null) {
				// read the equalizerBand0Q attribute value
				EqualizerBand0Q = NumberUtils.DecimalTryParseOrZero(equalizerBand0QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand0ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 0 Shape']");
			if (equalizerBand0ShapeNode != null) {
				// read the equalizerBand0Shape attribute value
				EqualizerBand0Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand0ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand1EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 1 Enable']");
			if (equalizerBand1EnableNode != null) {
				// read the equalizerBand1Enable attribute value
				EqualizerBand1Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand1EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand1FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 1 Frequency']");
			if (equalizerBand1FrequencyNode != null) {
				// read the equalizerBand1Frequency attribute value
				EqualizerBand1Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand1FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand1GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 1 Gain']");
			if (equalizerBand1GainNode != null) {
				// read the equalizerBand1Gain attribute value
				EqualizerBand1Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand1GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand1QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 1 Q']");
			if (equalizerBand1QNode != null) {
				// read the equalizerBand1Q attribute value
				EqualizerBand1Q = NumberUtils.DecimalTryParseOrZero(equalizerBand1QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand1ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 1 Shape']");
			if (equalizerBand1ShapeNode != null) {
				// read the equalizerBand1Shape attribute value
				EqualizerBand1Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand1ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand2EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 2 Enable']");
			if (equalizerBand2EnableNode != null) {
				// read the equalizerBand2Enable attribute value
				EqualizerBand2Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand2EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand2FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 2 Frequency']");
			if (equalizerBand2FrequencyNode != null) {
				// read the equalizerBand2Frequency attribute value
				EqualizerBand2Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand2FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand2GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 2 Gain']");
			if (equalizerBand2GainNode != null) {
				// read the equalizerBand2Gain attribute value
				EqualizerBand2Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand2GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand2QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 2 Q']");
			if (equalizerBand2QNode != null) {
				// read the equalizerBand2Q attribute value
				EqualizerBand2Q = NumberUtils.DecimalTryParseOrZero(equalizerBand2QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand2ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 2 Shape']");
			if (equalizerBand2ShapeNode != null) {
				// read the equalizerBand2Shape attribute value
				EqualizerBand2Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand2ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand3EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 3 Enable']");
			if (equalizerBand3EnableNode != null) {
				// read the equalizerBand3Enable attribute value
				EqualizerBand3Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand3EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand3FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 3 Frequency']");
			if (equalizerBand3FrequencyNode != null) {
				// read the equalizerBand3Frequency attribute value
				EqualizerBand3Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand3FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand3GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 3 Gain']");
			if (equalizerBand3GainNode != null) {
				// read the equalizerBand3Gain attribute value
				EqualizerBand3Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand3GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand3QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 3 Q']");
			if (equalizerBand3QNode != null) {
				// read the equalizerBand3Q attribute value
				EqualizerBand3Q = NumberUtils.DecimalTryParseOrZero(equalizerBand3QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand3ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 3 Shape']");
			if (equalizerBand3ShapeNode != null) {
				// read the equalizerBand3Shape attribute value
				EqualizerBand3Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand3ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand4EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 4 Enable']");
			if (equalizerBand4EnableNode != null) {
				// read the equalizerBand4Enable attribute value
				EqualizerBand4Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand4EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand4FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 4 Frequency']");
			if (equalizerBand4FrequencyNode != null) {
				// read the equalizerBand4Frequency attribute value
				EqualizerBand4Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand4FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand4GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 4 Gain']");
			if (equalizerBand4GainNode != null) {
				// read the equalizerBand4Gain attribute value
				EqualizerBand4Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand4GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand4QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 4 Q']");
			if (equalizerBand4QNode != null) {
				// read the equalizerBand4Q attribute value
				EqualizerBand4Q = NumberUtils.DecimalTryParseOrZero(equalizerBand4QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand4ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 4 Shape']");
			if (equalizerBand4ShapeNode != null) {
				// read the equalizerBand4Shape attribute value
				EqualizerBand4Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand4ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand5EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 5 Enable']");
			if (equalizerBand5EnableNode != null) {
				// read the equalizerBand5Enable attribute value
				EqualizerBand5Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand5EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand5FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 5 Frequency']");
			if (equalizerBand5FrequencyNode != null) {
				// read the equalizerBand5Frequency attribute value
				EqualizerBand5Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand5FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand5GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 5 Gain']");
			if (equalizerBand5GainNode != null) {
				// read the equalizerBand5Gain attribute value
				EqualizerBand5Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand5GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand5QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 5 Q']");
			if (equalizerBand5QNode != null) {
				// read the equalizerBand5Q attribute value
				EqualizerBand5Q = NumberUtils.DecimalTryParseOrZero(equalizerBand5QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand5ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 5 Shape']");
			if (equalizerBand5ShapeNode != null) {
				// read the equalizerBand5Shape attribute value
				EqualizerBand5Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand5ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand6EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 6 Enable']");
			if (equalizerBand6EnableNode != null) {
				// read the equalizerBand6Enable attribute value
				EqualizerBand6Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand6EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand6FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 6 Frequency']");
			if (equalizerBand6FrequencyNode != null) {
				// read the equalizerBand6Frequency attribute value
				EqualizerBand6Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand6FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand6GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 6 Gain']");
			if (equalizerBand6GainNode != null) {
				// read the equalizerBand6Gain attribute value
				EqualizerBand6Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand6GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand6QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 6 Q']");
			if (equalizerBand6QNode != null) {
				// read the equalizerBand6Q attribute value
				EqualizerBand6Q = NumberUtils.DecimalTryParseOrZero(equalizerBand6QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand6ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 6 Shape']");
			if (equalizerBand6ShapeNode != null) {
				// read the equalizerBand6Shape attribute value
				EqualizerBand6Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand6ShapeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand7EnableNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 7 Enable']");
			if (equalizerBand7EnableNode != null) {
				// read the equalizerBand7Enable attribute value
				EqualizerBand7Enable = NumberUtils.BooleanTryParseOrZero(equalizerBand7EnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand7FrequencyNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 7 Frequency']");
			if (equalizerBand7FrequencyNode != null) {
				// read the equalizerBand7Frequency attribute value
				EqualizerBand7Frequency = NumberUtils.DecimalTryParseOrZero(equalizerBand7FrequencyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand7GainNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 7 Gain']");
			if (equalizerBand7GainNode != null) {
				// read the equalizerBand7Gain attribute value
				EqualizerBand7Gain = NumberUtils.DecimalTryParseOrZero(equalizerBand7GainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand7QNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 7 Q']");
			if (equalizerBand7QNode != null) {
				// read the equalizerBand7Q attribute value
				EqualizerBand7Q = NumberUtils.DecimalTryParseOrZero(equalizerBand7QNode.SelectSingleNode("@Value").Value);
			}

			XmlNode equalizerBand7ShapeNode = xmldoc.SelectSingleNode("/Nectar/EQ/Param[@ElementID='Equalizer' and @ParamID='Band 7 Shape']");
			if (equalizerBand7ShapeNode != null) {
				// read the equalizerBand7Shape attribute value
				EqualizerBand7Shape = StringUtils.StringToEnum<EqualizerBandShape>(equalizerBand7ShapeNode.SelectSingleNode("@Value").Value);
			}
			#endregion

			XmlNode fxBypassNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Bypass']");
			if (fxBypassNode != null) {
				// read the fxBypass attribute value
				FXBypass = NumberUtils.BooleanTryParseOrZero(fxBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxDistortionOverdriveNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Distortion Overdrive']");
			if (fxDistortionOverdriveNode != null) {
				// read the fxDistortionOverdrive attribute value
				FXDistortionOverdrive = NumberUtils.DecimalTryParseOrZero(fxDistortionOverdriveNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxDistortionSendNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Distortion Send']");
			if (fxDistortionSendNode != null) {
				// read the fxDistortionSend attribute value
				FXDistortionSend = NumberUtils.DecimalTryParseOrZero(fxDistortionSendNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxDownsamplingAmountNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Downsampling Amount']");
			if (fxDownsamplingAmountNode != null) {
				// read the fxDownsamplingAmount attribute value
				FXDownsamplingAmount = NumberUtils.DecimalTryParseOrZero(fxDownsamplingAmountNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxDownsamplingEnableNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Downsampling Enable']");
			if (fxDownsamplingEnableNode != null) {
				// read the fxDownsamplingEnable attribute value
				FXDownsamplingEnable = NumberUtils.BooleanTryParseOrZero(fxDownsamplingEnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxDryMixNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Dry Mix']");
			if (fxDryMixNode != null) {
				// read the fxDryMix attribute value
				FXDryMix = NumberUtils.DecimalTryParseOrZero(fxDryMixNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxManualTempoNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Manual Tempo']");
			if (fxManualTempoNode != null) {
				// read the fxManualTempo attribute value
				FXManualTempo = NumberUtils.DecimalTryParseOrZero(fxManualTempoNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxModulationDepthNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Modulation Depth']");
			if (fxModulationDepthNode != null) {
				// read the fxModulationDepth attribute value
				FXModulationDepth = NumberUtils.DecimalTryParseOrZero(fxModulationDepthNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxModulationFeedbackNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Modulation Feedback']");
			if (fxModulationFeedbackNode != null) {
				// read the fxModulationFeedback attribute value
				FXModulationFeedback = NumberUtils.DecimalTryParseOrZero(fxModulationFeedbackNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxModulationModeNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Modulation Mode']");
			if (fxModulationModeNode != null) {
				// read the fxModulationMode attribute value
				FXModulationMode = StringUtils.StringToEnum<FXModulationModes>(fxModulationModeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxModulationSendNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Modulation Send']");
			if (fxModulationSendNode != null) {
				// read the fxModulationSend attribute value
				FXModulationSend = NumberUtils.DecimalTryParseOrZero(fxModulationSendNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxParallelProcessingNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Parallel Processing']");
			if (fxParallelProcessingNode != null) {
				// read the fxParallelProcessing attribute value
				FXParallelProcessing = NumberUtils.BooleanTryParseOrZero(fxParallelProcessingNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxTimeDepthNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Time Depth']");
			if (fxTimeDepthNode != null) {
				// read the fxTimeDepth attribute value
				FXTimeDepth = NumberUtils.DecimalTryParseOrZero(fxTimeDepthNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxTimeFeedbackNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Time Feedback']");
			if (fxTimeFeedbackNode != null) {
				// read the fxTimeFeedback attribute value
				FXTimeFeedback = NumberUtils.DecimalTryParseOrZero(fxTimeFeedbackNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxTimeModeNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Time Mode']");
			if (fxTimeModeNode != null) {
				// read the fxTimeMode attribute value
				FXTimeMode = StringUtils.StringToEnum<FXTimeModes>(fxTimeModeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxTimeSendNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Time Send']");
			if (fxTimeSendNode != null) {
				// read the fxTimeSend attribute value
				FXTimeSend = NumberUtils.DecimalTryParseOrZero(fxTimeSendNode.SelectSingleNode("@Value").Value);
			}

			XmlNode fxWetMixNode = xmldoc.SelectSingleNode("/Nectar/FX/Param[@ElementID='FX' and @ParamID='Wet Mix']");
			if (fxWetMixNode != null) {
				// read the fxWetMix attribute value
				FXWetMix = NumberUtils.DecimalTryParseOrZero(fxWetMixNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderAttackNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Attack']");
			if (gateExpanderAttackNode != null) {
				// read the gateExpanderAttack attribute value
				GateExpanderAttack = NumberUtils.DecimalTryParseOrZero(gateExpanderAttackNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderBandGainNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Band Gain']");
			if (gateExpanderBandGainNode != null) {
				// read the gateExpanderBandGain attribute value
				GateExpanderBandGain = NumberUtils.DecimalTryParseOrZero(gateExpanderBandGainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderBypassNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Bypass']");
			if (gateExpanderBypassNode != null) {
				// read the gateExpanderBypass attribute value
				GateExpanderBypass = NumberUtils.BooleanTryParseOrZero(gateExpanderBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderFloorOutputThresholdNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Floor Output Threshold']");
			if (gateExpanderFloorOutputThresholdNode != null) {
				// read the gateExpanderFloorOutputThreshold attribute value
				GateExpanderFloorOutputThreshold = NumberUtils.DecimalTryParseOrZero(gateExpanderFloorOutputThresholdNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderRMSDetectionNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='RMS Detection']");
			if (gateExpanderRMSDetectionNode != null) {
				// read the gateExpanderRMSDetection attribute value
				GateExpanderRMSDetection = NumberUtils.BooleanTryParseOrZero(gateExpanderRMSDetectionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderRatioNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Ratio']");
			if (gateExpanderRatioNode != null) {
				// read the gateExpanderRatio attribute value
				GateExpanderRatio = NumberUtils.DecimalTryParseOrZero(gateExpanderRatioNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderReleaseNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Release']");
			if (gateExpanderReleaseNode != null) {
				// read the gateExpanderRelease attribute value
				GateExpanderRelease = NumberUtils.DecimalTryParseOrZero(gateExpanderReleaseNode.SelectSingleNode("@Value").Value);
			}

			XmlNode gateExpanderThresholdNode = xmldoc.SelectSingleNode("/Nectar/Gate/Param[@ElementID='Gate Expander' and @ParamID='Threshold']");
			if (gateExpanderThresholdNode != null) {
				// read the gateExpanderThreshold attribute value
				GateExpanderThreshold = NumberUtils.DecimalTryParseOrZero(gateExpanderThresholdNode.SelectSingleNode("@Value").Value);
			}

			XmlNode globalGlobalCompressionMixNode = xmldoc.SelectSingleNode("/Nectar/Global/Param[@ElementID='Global' and @ParamID='Global Compression Mix']");
			if (globalGlobalCompressionMixNode != null) {
				// read the globalGlobalCompressionMix attribute value
				GlobalGlobalCompressionMix = NumberUtils.DecimalTryParseOrZero(globalGlobalCompressionMixNode.SelectSingleNode("@Value").Value);
			}

			XmlNode globalGlobalParallelCompressionNode = xmldoc.SelectSingleNode("/Nectar/Global/Param[@ElementID='Global' and @ParamID='Global Parallel Compression']");
			if (globalGlobalParallelCompressionNode != null) {
				// read the globalGlobalParallelCompression attribute value
				GlobalGlobalParallelCompression = NumberUtils.BooleanTryParseOrZero(globalGlobalParallelCompressionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyBypassNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Bypass']");
			if (harmonyBypassNode != null) {
				// read the harmonyBypass attribute value
				HarmonyBypass = NumberUtils.BooleanTryParseOrZero(harmonyBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyGainSpreadNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Gain Spread']");
			if (harmonyGainSpreadNode != null) {
				// read the harmonyGainSpread attribute value
				HarmonyGainSpread = NumberUtils.DecimalTryParseOrZero(harmonyGainSpreadNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyHighShelfFreqNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='High Shelf Freq']");
			if (harmonyHighShelfFreqNode != null) {
				// read the harmonyHighShelfFreq attribute value
				HarmonyHighShelfFreq = NumberUtils.DecimalTryParseOrZero(harmonyHighShelfFreqNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyHighShelfGainNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='High Shelf Gain']");
			if (harmonyHighShelfGainNode != null) {
				// read the harmonyHighShelfGain attribute value
				HarmonyHighShelfGain = NumberUtils.DecimalTryParseOrZero(harmonyHighShelfGainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyLowShelfFreqNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Low Shelf Freq']");
			if (harmonyLowShelfFreqNode != null) {
				// read the harmonyLowShelfFreq attribute value
				HarmonyLowShelfFreq = NumberUtils.DecimalTryParseOrZero(harmonyLowShelfFreqNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyLowShelfGainNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Low Shelf Gain']");
			if (harmonyLowShelfGainNode != null) {
				// read the harmonyLowShelfGain attribute value
				HarmonyLowShelfGain = NumberUtils.DecimalTryParseOrZero(harmonyLowShelfGainNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyPanSpreadNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Pan Spread']");
			if (harmonyPanSpreadNode != null) {
				// read the harmonyPanSpread attribute value
				HarmonyPanSpread = NumberUtils.DecimalTryParseOrZero(harmonyPanSpreadNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyPitchCorrectionNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Pitch Correction']");
			if (harmonyPitchCorrectionNode != null) {
				// read the harmonyPitchCorrection attribute value
				HarmonyPitchCorrection = NumberUtils.DecimalTryParseOrZero(harmonyPitchCorrectionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyPitchVariationNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Pitch Variation']");
			if (harmonyPitchVariationNode != null) {
				// read the harmonyPitchVariation attribute value
				HarmonyPitchVariation = NumberUtils.DecimalTryParseOrZero(harmonyPitchVariationNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonySoloHarmonyNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Solo Harmony']");
			if (harmonySoloHarmonyNode != null) {
				// read the harmonySoloHarmony attribute value
				HarmonySoloHarmony = NumberUtils.BooleanTryParseOrZero(harmonySoloHarmonyNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyTimeVariationNode = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Time Variation']");
			if (harmonyTimeVariationNode != null) {
				// read the harmonyTimeVariation attribute value
				HarmonyTimeVariation = NumberUtils.DecimalTryParseOrZero(harmonyTimeVariationNode.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceEnable3Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Enable 3']");
			if (harmonyVoiceEnable3Node != null) {
				// read the harmonyVoiceEnable3 attribute value
				HarmonyVoiceEnable3 = NumberUtils.BooleanTryParseOrZero(harmonyVoiceEnable3Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceEnable4Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Enable 4']");
			if (harmonyVoiceEnable4Node != null) {
				// read the harmonyVoiceEnable4 attribute value
				HarmonyVoiceEnable4 = NumberUtils.BooleanTryParseOrZero(harmonyVoiceEnable4Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceGain1Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Gain 1']");
			if (harmonyVoiceGain1Node != null) {
				// read the harmonyVoiceGain1 attribute value
				HarmonyVoiceGain1 = NumberUtils.DecimalTryParseOrZero(harmonyVoiceGain1Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceGain2Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Gain 2']");
			if (harmonyVoiceGain2Node != null) {
				// read the harmonyVoiceGain2 attribute value
				HarmonyVoiceGain2 = NumberUtils.DecimalTryParseOrZero(harmonyVoiceGain2Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceGain3Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Gain 3']");
			if (harmonyVoiceGain3Node != null) {
				// read the harmonyVoiceGain3 attribute value
				HarmonyVoiceGain3 = NumberUtils.DecimalTryParseOrZero(harmonyVoiceGain3Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceGain4Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Gain 4']");
			if (harmonyVoiceGain4Node != null) {
				// read the harmonyVoiceGain4 attribute value
				HarmonyVoiceGain4 = NumberUtils.DecimalTryParseOrZero(harmonyVoiceGain4Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceInterval1Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval 1']");
			if (harmonyVoiceInterval1Node != null) {
				// read the harmonyVoiceInterval1 attribute value
				HarmonyVoiceInterval1 = StringUtils.StringToEnum<HarmonyVoiceInterval>(harmonyVoiceInterval1Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceInterval2Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval 2']");
			if (harmonyVoiceInterval2Node != null) {
				// read the harmonyVoiceInterval2 attribute value
				HarmonyVoiceInterval2 = StringUtils.StringToEnum<HarmonyVoiceInterval>(harmonyVoiceInterval2Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceInterval3Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval 3']");
			if (harmonyVoiceInterval3Node != null) {
				// read the harmonyVoiceInterval3 attribute value
				HarmonyVoiceInterval3 = StringUtils.StringToEnum<HarmonyVoiceInterval>(harmonyVoiceInterval3Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceInterval4Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval 4']");
			if (harmonyVoiceInterval4Node != null) {
				// read the harmonyVoiceInterval4 attribute value
				HarmonyVoiceInterval4 = StringUtils.StringToEnum<HarmonyVoiceInterval>(harmonyVoiceInterval4Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceIntervalDirection1Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval Direction 1']");
			if (harmonyVoiceIntervalDirection1Node != null) {
				// read the harmonyVoiceIntervalDirection1 attribute value
				HarmonyVoiceIntervalDirection1 = StringUtils.StringToEnum<HarmonyVoiceIntervalDirection>(harmonyVoiceIntervalDirection1Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceIntervalDirection2Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval Direction 2']");
			if (harmonyVoiceIntervalDirection2Node != null) {
				// read the harmonyVoiceIntervalDirection2 attribute value
				HarmonyVoiceIntervalDirection2 = StringUtils.StringToEnum<HarmonyVoiceIntervalDirection>(harmonyVoiceIntervalDirection2Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceIntervalDirection3Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval Direction 3']");
			if (harmonyVoiceIntervalDirection3Node != null) {
				// read the harmonyVoiceIntervalDirection3 attribute value
				HarmonyVoiceIntervalDirection3 = StringUtils.StringToEnum<HarmonyVoiceIntervalDirection>(harmonyVoiceIntervalDirection3Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoiceIntervalDirection4Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Interval Direction 4']");
			if (harmonyVoiceIntervalDirection4Node != null) {
				// read the harmonyVoiceIntervalDirection4 attribute value
				HarmonyVoiceIntervalDirection4 = StringUtils.StringToEnum<HarmonyVoiceIntervalDirection>(harmonyVoiceIntervalDirection4Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoicePan1Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Pan 1']");
			if (harmonyVoicePan1Node != null) {
				// read the harmonyVoicePan1 attribute value
				HarmonyVoicePan1 = NumberUtils.DecimalTryParseOrZero(harmonyVoicePan1Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoicePan2Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Pan 2']");
			if (harmonyVoicePan2Node != null) {
				// read the harmonyVoicePan2 attribute value
				HarmonyVoicePan2 = NumberUtils.DecimalTryParseOrZero(harmonyVoicePan2Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoicePan3Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Pan 3']");
			if (harmonyVoicePan3Node != null) {
				// read the harmonyVoicePan3 attribute value
				HarmonyVoicePan3 = NumberUtils.DecimalTryParseOrZero(harmonyVoicePan3Node.SelectSingleNode("@Value").Value);
			}

			XmlNode harmonyVoicePan4Node = xmldoc.SelectSingleNode("/Nectar/Harmony/Param[@ElementID='Harmony' and @ParamID='Voice Pan 4']");
			if (harmonyVoicePan4Node != null) {
				// read the harmonyVoicePan4 attribute value
				HarmonyVoicePan4 = NumberUtils.DecimalTryParseOrZero(harmonyVoicePan4Node.SelectSingleNode("@Value").Value);
			}

			XmlNode maximizerBypassNode = xmldoc.SelectSingleNode("/Nectar/Limiter/Param[@ElementID='Maximizer' and @ParamID='Bypass']");
			if (maximizerBypassNode != null) {
				// read the maximizerBypass attribute value
				MaximizerBypass = NumberUtils.BooleanTryParseOrZero(maximizerBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode maximizerMarginNode = xmldoc.SelectSingleNode("/Nectar/Limiter/Param[@ElementID='Maximizer' and @ParamID='Margin']");
			if (maximizerMarginNode != null) {
				// read the maximizerMargin attribute value
				MaximizerMargin = NumberUtils.DecimalTryParseOrZero(maximizerMarginNode.SelectSingleNode("@Value").Value);
			}

			XmlNode maximizerThresholdNode = xmldoc.SelectSingleNode("/Nectar/Limiter/Param[@ElementID='Maximizer' and @ParamID='Threshold']");
			if (maximizerThresholdNode != null) {
				// read the maximizerThreshold attribute value
				MaximizerThreshold = NumberUtils.DecimalTryParseOrZero(maximizerThresholdNode.SelectSingleNode("@Value").Value);
			}

			XmlNode pitchBypassNode = xmldoc.SelectSingleNode("/Nectar/Pitch/Param[@ElementID='Pitch' and @ParamID='Bypass']");
			if (pitchBypassNode != null) {
				// read the pitchBypass attribute value
				PitchBypass = NumberUtils.BooleanTryParseOrZero(pitchBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode pitchFormantScalingNode = xmldoc.SelectSingleNode("/Nectar/Pitch/Param[@ElementID='Pitch' and @ParamID='Formant Scaling']");
			if (pitchFormantScalingNode != null) {
				// read the pitchFormantScaling attribute value
				PitchFormantScaling = NumberUtils.DecimalTryParseOrZero(pitchFormantScalingNode.SelectSingleNode("@Value").Value);
			}

			XmlNode pitchFormantShiftNode = xmldoc.SelectSingleNode("/Nectar/Pitch/Param[@ElementID='Pitch' and @ParamID='Formant Shift']");
			if (pitchFormantShiftNode != null) {
				// read the pitchFormantShift attribute value
				PitchFormantShift = NumberUtils.DecimalTryParseOrZero(pitchFormantShiftNode.SelectSingleNode("@Value").Value);
			}

			XmlNode pitchSmoothingNode = xmldoc.SelectSingleNode("/Nectar/Pitch/Param[@ElementID='Pitch' and @ParamID='Smoothing']");
			if (pitchSmoothingNode != null) {
				// read the pitchSmoothing attribute value
				PitchSmoothing = NumberUtils.DecimalTryParseOrZero(pitchSmoothingNode.SelectSingleNode("@Value").Value);
			}

			XmlNode pitchVocalRangeNode = xmldoc.SelectSingleNode("/Nectar/Pitch/Param[@ElementID='Pitch' and @ParamID='Vocal Range']");
			if (pitchVocalRangeNode != null) {
				// read the pitchVocalRange attribute value
				PitchVocalRange = StringUtils.StringToEnum<PitchRange>(pitchVocalRangeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode pitchVoice1TranspositionNode = xmldoc.SelectSingleNode("/Nectar/Pitch/Param[@ElementID='Pitch' and @ParamID='Voice 1 Transposition']");
			if (pitchVoice1TranspositionNode != null) {
				// read the pitchVoice1Transposition attribute value
				PitchVoice1Transposition = NumberUtils.DecimalTryParseOrZero(pitchVoice1TranspositionNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbBypassNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Bypass']");
			if (reverbBypassNode != null) {
				// read the reverbBypass attribute value
				ReverbBypass = NumberUtils.BooleanTryParseOrZero(reverbBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbDampingNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Damping']");
			if (reverbDampingNode != null) {
				// read the reverbDamping attribute value
				ReverbDamping = NumberUtils.DecimalTryParseOrZero(reverbDampingNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbDryMixNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Dry Mix']");
			if (reverbDryMixNode != null) {
				// read the reverbDryMix attribute value
				ReverbDryMix = NumberUtils.DecimalTryParseOrZero(reverbDryMixNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbHighCutoffNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='High Cutoff']");
			if (reverbHighCutoffNode != null) {
				// read the reverbHighCutoff attribute value
				ReverbHighCutoff = NumberUtils.DecimalTryParseOrZero(reverbHighCutoffNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbLowCutoffNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Low Cutoff']");
			if (reverbLowCutoffNode != null) {
				// read the reverbLowCutoff attribute value
				ReverbLowCutoff = NumberUtils.DecimalTryParseOrZero(reverbLowCutoffNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbPreDelayNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='PreDelay']");
			if (reverbPreDelayNode != null) {
				// read the reverbPreDelay attribute value
				ReverbPreDelay = NumberUtils.DecimalTryParseOrZero(reverbPreDelayNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbSaturationNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Saturation']");
			if (reverbSaturationNode != null) {
				// read the reverbSaturation attribute value
				ReverbSaturation = NumberUtils.DecimalTryParseOrZero(reverbSaturationNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbSaturationEnableNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Saturation Enable']");
			if (reverbSaturationEnableNode != null) {
				// read the reverbSaturationEnable attribute value
				ReverbSaturationEnable = NumberUtils.BooleanTryParseOrZero(reverbSaturationEnableNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbWetMixNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Wet Mix']");
			if (reverbWetMixNode != null) {
				// read the reverbWetMix attribute value
				ReverbWetMix = NumberUtils.DecimalTryParseOrZero(reverbWetMixNode.SelectSingleNode("@Value").Value);
			}

			XmlNode reverbWidthNode = xmldoc.SelectSingleNode("/Nectar/Reverb/Param[@ElementID='Reverb' and @ParamID='Width']");
			if (reverbWidthNode != null) {
				// read the reverbWidth attribute value
				ReverbWidth = NumberUtils.DecimalTryParseOrZero(reverbWidthNode.SelectSingleNode("@Value").Value);
			}

			XmlNode exciterAmountPercentNode = xmldoc.SelectSingleNode("/Nectar/Saturation/Param[@ElementID='Exciter' and @ParamID='Amount Percent']");
			if (exciterAmountPercentNode != null) {
				// read the exciterAmountPercent attribute value
				ExciterAmountPercent = NumberUtils.DecimalTryParseOrZero(exciterAmountPercentNode.SelectSingleNode("@Value").Value);
			}

			XmlNode exciterBypassNode = xmldoc.SelectSingleNode("/Nectar/Saturation/Param[@ElementID='Exciter' and @ParamID='Bypass']");
			if (exciterBypassNode != null) {
				// read the exciterBypass attribute value
				ExciterBypass = NumberUtils.BooleanTryParseOrZero(exciterBypassNode.SelectSingleNode("@Value").Value);
			}

			XmlNode exciterMixPercentNode = xmldoc.SelectSingleNode("/Nectar/Saturation/Param[@ElementID='Exciter' and @ParamID='Mix Percent']");
			if (exciterMixPercentNode != null) {
				// read the exciterMixPercent attribute value
				ExciterMixPercent = NumberUtils.DecimalTryParseOrZero(exciterMixPercentNode.SelectSingleNode("@Value").Value);
			}

			XmlNode exciterModeNode = xmldoc.SelectSingleNode("/Nectar/Saturation/Param[@ElementID='Exciter' and @ParamID='Mode']");
			if (exciterModeNode != null) {
				// read the exciterMode attribute value
				ExciterMode = StringUtils.StringToEnum<SaturationMode>(exciterModeNode.SelectSingleNode("@Value").Value);
			}

			XmlNode exciterPostfilterFreqNode = xmldoc.SelectSingleNode("/Nectar/Saturation/Param[@ElementID='Exciter' and @ParamID='Postfilter Freq']");
			if (exciterPostfilterFreqNode != null) {
				// read the exciterPostfilterFreq attribute value
				ExciterPostfilterFreq = NumberUtils.DecimalTryParseOrZero(exciterPostfilterFreqNode.SelectSingleNode("@Value").Value);
			}

			XmlNode exciterPostfilterGainNode = xmldoc.SelectSingleNode("/Nectar/Saturation/Param[@ElementID='Exciter' and @ParamID='Postfilter Gain']");
			if (exciterPostfilterGainNode != null) {
				// read the exciterPostfilterGain attribute value
				ExciterPostfilterGain = NumberUtils.DecimalTryParseOrZero(exciterPostfilterGainNode.SelectSingleNode("@Value").Value);
			}

		}
		#endregion

		#region toString Method
		public override string ToString() {
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("Preset Name: {0}\n", PresetName);
			sb.AppendLine();
			
			sb.Append("Global Global Compression Mix:".PadRight(40)).AppendFormat("{0:0.##} %\n", GlobalGlobalCompressionMix);
			sb.Append("Global Global Parallel Compression:".PadRight(40)).AppendFormat("{0}\n", GlobalGlobalParallelCompression);
			sb.AppendLine();
			
			sb.Append("Dynamics 1 Threshold:".PadRight(40)).AppendFormat("{0:0.##} dB\n", Dynamics1Threshold);
			sb.Append("Dynamics 1 Mode:".PadRight(40)).AppendFormat("{0}\n", Dynamics1Mode);
			sb.Append("Dynamics 1 Ratio:".PadRight(40)).AppendFormat("{0:0.##} : 1\n", Dynamics1Ratio);
			sb.Append("Dynamics 1 Attack:".PadRight(40)).AppendFormat("{0:0.##} ms\n", Dynamics1Attack);
			sb.Append("Dynamics 1 Release:".PadRight(40)).AppendFormat("{0:0.##} ms\n", Dynamics1Release);
			sb.Append("Dynamics 1 RMS Detection:".PadRight(40)).AppendFormat("{0}\n", Dynamics1RMSDetection);
			sb.Append("Dynamics 1 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", Dynamics1Gain);
			sb.Append("Dynamics 2 Bypass:".PadRight(40)).AppendFormat("{0}\n", Dynamics2Bypass);
			sb.Append("Dynamics 2 Threshold:".PadRight(40)).AppendFormat("{0:0.##} dB\n", Dynamics2Threshold);
			sb.Append("Dynamics 2 Mode:".PadRight(40)).AppendFormat("{0}\n", Dynamics2Mode);
			sb.Append("Dynamics 2 Ratio:".PadRight(40)).AppendFormat("{0:0.##} : 1\n", Dynamics2Ratio);
			sb.Append("Dynamics 2 Attack:".PadRight(40)).AppendFormat("{0:0.##} ms\n", Dynamics2Attack);
			sb.Append("Dynamics 2 Release:".PadRight(40)).AppendFormat("{0:0.##} ms\n", Dynamics2Release);
			sb.Append("Dynamics 2 RMS Detection:".PadRight(40)).AppendFormat("{0}\n", Dynamics2RMSDetection);
			sb.Append("Dynamics 2 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", Dynamics2Gain);
			
			sb.Append("Dynamics 2 Equalizer Bypass:".PadRight(40)).AppendFormat("{0}\n", Dynamics2EqualizerBypass);
			sb.Append("Dynamics 2 Equalizer Band 0 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", Dynamics2EqualizerBand0Frequency);
			sb.Append("Dynamics 2 Equalizer Band 0 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", Dynamics2EqualizerBand0Gain);
			sb.Append("Dynamics 2 Equalizer Band 1 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", Dynamics2EqualizerBand1Frequency);
			sb.Append("Dynamics 2 Equalizer Band 1 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", Dynamics2EqualizerBand1Gain);
			sb.AppendLine();
			
			sb.Append("DeEsser Bypass:".PadRight(40)).AppendFormat("{0}\n", DeEsserBypass);
			sb.Append("DeEsser Ess Reduction:".PadRight(40)).AppendFormat("{0:0.##} dB\n", DeEsserEssReduction);
			sb.Append("DeEsser Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", DeEsserFrequency);
			sb.Append("DeEsser Output Ess Only:".PadRight(40)).AppendFormat("{0}\n", DeEsserOutputEssOnly);
			sb.AppendLine();
			
			sb.Append("Delay Bypass:".PadRight(40)).AppendFormat("{0}\n", DelayBypass);
			sb.Append("Delay Host Tempo Sync:".PadRight(40)).AppendFormat("{0}\n", DelayHostTempoSync);
			sb.Append("Delay Host Tempo Sync Ratio:".PadRight(40)).AppendFormat("{0}\n", DelayHostTempoSyncRatio);
			sb.Append("Delay Delay (Not Host Tempo Sync):".PadRight(40)).AppendFormat("{0:0.##} ms\n", DelayDelay);
			sb.Append("Delay Feedback Percent:".PadRight(40)).AppendFormat("{0:0.##} %\n", DelayFeedbackPercent);
			sb.Append("Delay Low Cutoff:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", DelayLowCutoff);
			sb.Append("Delay High Cutoff:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", DelayHighCutoff);
			sb.Append("Delay Enable Modulation:".PadRight(40)).AppendFormat("{0}\n", DelayEnableModulation);
			sb.Append("Delay Modulation Depth:".PadRight(40)).AppendFormat("{0:0.##}\n", DelayModulationDepth);
			sb.Append("Delay Modulation Rate:".PadRight(40)).AppendFormat("{0:0.##}\n", DelayModulationRate);
			sb.Append("Delay Distortion Mode:".PadRight(40)).AppendFormat("{0:0.##}\n", DelayDistortionMode);
			sb.Append("Delay Trash:".PadRight(40)).AppendFormat("{0:0.##} %\n", DelayTrash);
			sb.Append("Delay Spread (Width):".PadRight(40)).AppendFormat("{0:0.##} %\n", DelaySpread);
			sb.Append("Delay Wet Mix Percent:".PadRight(40)).AppendFormat("{0:0.##} %\n", DelayWetMixPercent);
			sb.Append("Delay Dry Mix Percent:".PadRight(40)).AppendFormat("{0:0.##} %\n", DelayDryMixPercent);
			sb.AppendLine();
			
			sb.Append("Equalizer Band 0 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand0Enable);
			sb.Append("Equalizer Band 0 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand0Shape);
			sb.Append("Equalizer Band 0 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand0Frequency);
			sb.Append("Equalizer Band 0 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand0Gain);
			sb.Append("Equalizer Band 0 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand0Q);
			sb.Append("Equalizer Band 1 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand1Enable);
			sb.Append("Equalizer Band 1 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand1Shape);
			sb.Append("Equalizer Band 1 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand1Frequency);
			sb.Append("Equalizer Band 1 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand1Gain);
			sb.Append("Equalizer Band 1 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand1Q);
			sb.Append("Equalizer Band 2 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand2Enable);
			sb.Append("Equalizer Band 2 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand2Shape);
			sb.Append("Equalizer Band 2 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand2Frequency);
			sb.Append("Equalizer Band 2 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand2Gain);
			sb.Append("Equalizer Band 2 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand2Q);
			sb.Append("Equalizer Band 3 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand3Enable);
			sb.Append("Equalizer Band 3 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand3Shape);
			sb.Append("Equalizer Band 3 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand3Frequency);
			sb.Append("Equalizer Band 3 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand3Gain);
			sb.Append("Equalizer Band 3 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand3Q);
			sb.Append("Equalizer Band 4 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand4Enable);
			sb.Append("Equalizer Band 4 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand4Shape);
			sb.Append("Equalizer Band 4 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand4Frequency);
			sb.Append("Equalizer Band 4 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand4Gain);
			sb.Append("Equalizer Band 4 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand4Q);
			sb.Append("Equalizer Band 5 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand5Enable);
			sb.Append("Equalizer Band 5 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand5Shape);
			sb.Append("Equalizer Band 5 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand5Frequency);
			sb.Append("Equalizer Band 5 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand5Gain);
			sb.Append("Equalizer Band 5 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand5Q);
			sb.Append("Equalizer Band 6 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand6Enable);
			sb.Append("Equalizer Band 6 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand6Shape);
			sb.Append("Equalizer Band 6 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand6Frequency);
			sb.Append("Equalizer Band 6 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand6Gain);
			sb.Append("Equalizer Band 6 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand6Q);
			sb.Append("Equalizer Band 7 Enable:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand7Enable);
			sb.Append("Equalizer Band 7 Shape:".PadRight(40)).AppendFormat("{0}\n", EqualizerBand7Shape);
			sb.Append("Equalizer Band 7 Frequency:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", EqualizerBand7Frequency);
			sb.Append("Equalizer Band 7 Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", EqualizerBand7Gain);
			sb.Append("Equalizer Band 7 Q:".PadRight(40)).AppendFormat("{0:0.##} Q\n", EqualizerBand7Q);
			sb.AppendLine();
			
			sb.Append("FX Bypass:".PadRight(40)).AppendFormat("{0}\n", FXBypass);
			sb.Append("FX Distortion Send:".PadRight(40)).AppendFormat("{0:0.##} %\n", FXDistortionSend);
			sb.Append("FX Downsampling Enable:".PadRight(40)).AppendFormat("{0}\n", FXDownsamplingEnable);
			sb.Append("FX Downsampling Amount (Decimate):".PadRight(40)).AppendFormat("{0:0.##}\n", FXDownsamplingAmount);
			sb.Append("FX Distortion Overdrive:".PadRight(40)).AppendFormat("{0:0.##}\n", FXDistortionOverdrive);
			sb.Append("FX Modulation Send:".PadRight(40)).AppendFormat("{0:0.##} %\n", FXModulationSend);
			sb.Append("FX Modulation Mode:".PadRight(40)).AppendFormat("{0}\n", FXModulationMode);
			sb.Append("FX Modulation Feedback:".PadRight(40)).AppendFormat("{0:0.##}\n", FXModulationFeedback);
			sb.Append("FX Modulation Depth:".PadRight(40)).AppendFormat("{0:0.##}\n", FXModulationDepth);
			sb.Append("FX Time Send:".PadRight(40)).AppendFormat("{0:0.##} %\n", FXTimeSend);
			sb.Append("FX Time Mode:".PadRight(40)).AppendFormat("{0}\n", FXTimeMode);
			sb.Append("FX Time Feedback:".PadRight(40)).AppendFormat("{0:0.##}\n", FXTimeFeedback);
			sb.Append("FX Time Depth:".PadRight(40)).AppendFormat("{0:0.##}\n", FXTimeDepth);
			sb.Append("FX Parallel Processing:".PadRight(40)).AppendFormat("{0}\n", FXParallelProcessing);
			sb.Append("FX Manual Tempo:".PadRight(40)).AppendFormat("{0:0.##}\n", FXManualTempo);
			sb.Append("FX Dry Mix:".PadRight(40)).AppendFormat("{0:0.##} %\n", FXDryMix);
			sb.Append("FX Wet Mix:".PadRight(40)).AppendFormat("{0:0.##} %\n", FXWetMix);
			sb.AppendLine();
			
			sb.Append("Gate Expander Bypass:".PadRight(40)).AppendFormat("{0}\n", GateExpanderBypass);
			sb.Append("Gate Expander Threshold:".PadRight(40)).AppendFormat("{0:0.##} dB\n", GateExpanderThreshold);
			sb.Append("Gate Expander Floor Output Threshold:".PadRight(40)).AppendFormat("{0:0.##} dB\n", GateExpanderFloorOutputThreshold);
			sb.Append("Gate Expander Ratio:".PadRight(40)).AppendFormat("{0:0.##} : 1\n", GateExpanderRatio);
			sb.Append("Gate Expander Attack:".PadRight(40)).AppendFormat("{0:0.##} ms\n", GateExpanderAttack);
			sb.Append("Gate Expander Release:".PadRight(40)).AppendFormat("{0:0.##} ms\n", GateExpanderRelease);
			sb.Append("Gate Expander RMS Detection:".PadRight(40)).AppendFormat("{0}\n", GateExpanderRMSDetection);
			sb.Append("Gate Expander Band Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", GateExpanderBandGain);
			sb.AppendLine();
			
			sb.Append("Harmony Bypass:".PadRight(40)).AppendFormat("{0}\n", HarmonyBypass);
			sb.Append("Harmony Voice Enable 3:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceEnable3);
			sb.Append("Harmony Voice Enable 4:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceEnable4);
			sb.Append("Harmony Voice Interval 1:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceInterval1);
			sb.Append("Harmony Voice Interval 2:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceInterval2);
			sb.Append("Harmony Voice Interval 3:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceInterval3);
			sb.Append("Harmony Voice Interval 4:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceInterval4);
			sb.Append("Harmony Voice Interval Direction 1:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceIntervalDirection1);
			sb.Append("Harmony Voice Interval Direction 2:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceIntervalDirection2);
			sb.Append("Harmony Voice Interval Direction 3:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceIntervalDirection3);
			sb.Append("Harmony Voice Interval Direction 4:".PadRight(40)).AppendFormat("{0}\n", HarmonyVoiceIntervalDirection4);
			sb.Append("Harmony Solo Harmony:".PadRight(40)).AppendFormat("{0}\n", HarmonySoloHarmony);
			sb.Append("Harmony Pitch Variation:".PadRight(40)).AppendFormat("{0:0.##} %\n", HarmonyPitchVariation);
			sb.Append("Harmony Time Variation:".PadRight(40)).AppendFormat("{0:0.##} %\n", HarmonyTimeVariation);
			sb.Append("Harmony Pitch Correction:".PadRight(40)).AppendFormat("{0:0.##}\n", HarmonyPitchCorrection);
			sb.Append("Harmony Gain Spread:".PadRight(40)).AppendFormat("{0:0.##}\n", HarmonyGainSpread);
			sb.Append("Harmony Pan Spread:".PadRight(40)).AppendFormat("{0:0.##}\n", HarmonyPanSpread);
			sb.Append("Harmony Voice Pan 1:".PadRight(40)).AppendFormat("{0:0.##} %\n", HarmonyVoicePan1);
			sb.Append("Harmony Voice Pan 2:".PadRight(40)).AppendFormat("{0:0.##} %\n", HarmonyVoicePan2);
			sb.Append("Harmony Voice Pan 3:".PadRight(40)).AppendFormat("{0:0.##} %\n", HarmonyVoicePan3);
			sb.Append("Harmony Voice Pan 4:".PadRight(40)).AppendFormat("{0:0.##} %\n", HarmonyVoicePan4);
			sb.Append("Harmony Voice Gain 1:".PadRight(40)).AppendFormat("{0:0.##} dB\n", HarmonyVoiceGain1);
			sb.Append("Harmony Voice Gain 2:".PadRight(40)).AppendFormat("{0:0.##} dB\n", HarmonyVoiceGain2);
			sb.Append("Harmony Voice Gain 3:".PadRight(40)).AppendFormat("{0:0.##} dB\n", HarmonyVoiceGain3);
			sb.Append("Harmony Voice Gain 4:".PadRight(40)).AppendFormat("{0:0.##} dB\n", HarmonyVoiceGain4);
			sb.Append("Harmony Low Shelf Freq:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", HarmonyLowShelfFreq);
			sb.Append("Harmony Low Shelf Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", HarmonyLowShelfGain);
			sb.Append("Harmony High Shelf Freq:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", HarmonyHighShelfFreq);
			sb.Append("Harmony High Shelf Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", HarmonyHighShelfGain);
			sb.AppendLine();
			
			sb.Append("Maximizer Bypass:".PadRight(40)).AppendFormat("{0}\n", MaximizerBypass);
			sb.Append("Maximizer Margin:".PadRight(40)).AppendFormat("{0:0.##} dB\n", MaximizerMargin);
			sb.Append("Maximizer Threshold:".PadRight(40)).AppendFormat("{0:0.##} dB\n", MaximizerThreshold);
			sb.AppendLine();
			
			sb.Append("Pitch Bypass:".PadRight(40)).AppendFormat("{0}\n", PitchBypass);
			sb.Append("Pitch Vocal Range:".PadRight(40)).AppendFormat("{0}\n", PitchVocalRange);
			sb.Append("Pitch Smoothing (Speed):".PadRight(40)).AppendFormat("{0:0.##} ms\n", PitchSmoothing);
			sb.Append("Pitch Voice 1 Transposition:".PadRight(40)).AppendFormat("{0:0.##} s.t.\n", PitchVoice1Transposition);
			sb.Append("Pitch Formant Shift:".PadRight(40)).AppendFormat("{0:0.##} s.t.\n", PitchFormantShift);
			sb.Append("Pitch Formant Scaling:".PadRight(40)).AppendFormat("{0:0.##} %\n", PitchFormantScaling);
			sb.AppendLine();
			
			sb.Append("Reverb Bypass:".PadRight(40)).AppendFormat("{0}\n", ReverbBypass);
			sb.Append("Reverb Pre-Delay:".PadRight(40)).AppendFormat("{0:0.##} ms\n", ReverbPreDelay);
			sb.Append("Reverb Damping (Decay):".PadRight(40)).AppendFormat("{0:0.##} s\n", ReverbDamping);
			sb.Append("Reverb Width:".PadRight(40)).AppendFormat("{0:0.##} %\n", ReverbWidth);
			sb.Append("Reverb Saturation Enable:".PadRight(40)).AppendFormat("{0}\n", ReverbSaturationEnable);
			sb.Append("Reverb Saturation:".PadRight(40)).AppendFormat("{0:0.##}\n", ReverbSaturation);
			sb.Append("Reverb Low Cutoff:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", ReverbLowCutoff);
			sb.Append("Reverb High Cutoff:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", ReverbHighCutoff);
			sb.Append("Reverb Wet Mix:".PadRight(40)).AppendFormat("{0:0.##} %\n", ReverbWetMix);
			sb.Append("Reverb Dry Mix:".PadRight(40)).AppendFormat("{0:0.##} %\n", ReverbDryMix);
			sb.AppendLine();
			
			sb.Append("Saturation Bypass:".PadRight(40)).AppendFormat("{0}\n", ExciterBypass);
			sb.Append("Saturation Mode:".PadRight(40)).AppendFormat("{0}\n", ExciterMode);
			sb.Append("Saturation Amount Percent:".PadRight(40)).AppendFormat("{0:0.##} %\n", ExciterAmountPercent);
			sb.Append("Saturation Mix Percent:".PadRight(40)).AppendFormat("{0:0.##} %\n", ExciterMixPercent);
			sb.Append("Saturation Postfilter Freq:".PadRight(40)).AppendFormat("{0:0.##} Hz\n", ExciterPostfilterFreq);
			sb.Append("Saturation Postfilter Gain:".PadRight(40)).AppendFormat("{0:0.##} dB\n", ExciterPostfilterGain);
			return sb.ToString();
		}
		#endregion
	}
}
