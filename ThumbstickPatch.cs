using System;
using HarmonyLib;
using UnityEngine;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(ThrottleSOISwitcher))]
[HarmonyPatch("OnSetThumbstick")]
public class ThumbstickPatch
{
	public static bool awaitingRelease;
	public static float activationThresh = 0.8f;
	public static float deactivationThresh = 0.3f;	

	//Patch ThrottleSOISwitcher.OnSetThumbstick with a postfix path, executes after code.
	public static void Postfix(Vector3 ts, AudioSource ___inputAudioSource, AudioClip ___switchedClip)
	{
		if (StartPatch.audioSource == null) StartPatch.audioSource = ___inputAudioSource;
		if (StartPatch.audioClip == null) StartPatch.audioClip = ___switchedClip;

		var name = FlightSceneManager.instance.playerVehicleMaster?.playerVehicle?.vehicleName;

		if (name != "EF-24G" && name != "AH-94")
		{
			//Check that it's the first frame above input threshhold
			if (Mathf.Abs(ts.y) > activationThresh && !awaitingRelease && !StartPatch.thumbstickDown) 
			{
				//Get objects for each mfd
				TargetingMFDPage tgp = FlightSceneManager.instance.playerVehicleMaster.comms.targetingPage;
				MFDPTacticalSituationDisplay tsd = FlightSceneManager.instance.playerVehicleMaster.comms.tsdPage;
				DashMapDisplay nav = FlightSceneManager.instance.playerVehicleMaster.comms.targetingPage?.map;
				MFDRadarUI radar = FlightSceneManager.instance.playerVehicleMaster.comms.radarPage;

				float y = ts.y * (Main.settings.invertAxis ? -1 : 1);

				//Type checking to make sure each is instantiated
				if (nav?.mfdPage?.isSOI == true || nav?.portalPage?.isSOI == true)
				{
					if (y > 0) 
					{
						nav.ZoomIn();
					}
					else if (y < 0) 
					{
						nav.ZoomOut();
					}
				}
				if (tgp?.mfdPage?.isSOI == true || tgp?.portalPage?.isSOI == true)
				{
					if (y > 0) 
					{
						tgp.ZoomIn();
					}
					else if (y < 0) 
					{
						tgp.ZoomOut();
					}
				}
				if (tsd?.isSOI == true)
				{
					if (y > 0)
					{
						tsd.PrevViewScale();
					}
					else if (y < 0)
					{
						tsd.NextViewScale();
					}
				}
				if (radar?.mfdPage?.isSOI == true || radar?.portalPage?.isSOI == true) 
				{
					if (y > 0) 
					{
						radar.RangeDown();
					}
					else if (y < 0) 
					{
						radar.RangeUp();
					}
				}

				awaitingRelease = true;
				return;
			}

			//Guard against the zoom spamming each frame
			if (Mathf.Abs(ts.y) < deactivationThresh && awaitingRelease) 
			{
				awaitingRelease = false;
			}
		}
	}
}