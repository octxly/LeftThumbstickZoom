using HarmonyLib;
using System;
using System.Xml.Linq;
using UnityEngine;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(ThrottleSOISwitcher))]
[HarmonyPatch("OnSetThumbstick")]
public class ThrottleSoiSwitcherPatch
{
	public static bool awaitingRelease;
	public static float activationThresh = 0.8f;
	public static float deactivationThresh = 0.3f;	

	//Patch ThrottleSOISwitcher.OnSetThumbstick with a postfix path, executes after code.
	public static void Postfix(Vector3 ts, AudioSource ___inputAudioSource, AudioClip ___switchedClip)
	{
		if (VRThrottlePatch.audioSource == null) VRThrottlePatch.audioSource = ___inputAudioSource;
		if (VRThrottlePatch.audioClip == null) VRThrottlePatch.audioClip = ___switchedClip;
		
		// //Fixes issue of thumbstick getting "stuck" down
		// if (StartPatch.thumbstickDown && (name == "EF-24G" || name == "T-55" || name == "F/A-26B" || name == "F-16" || name == "A-10D")) 
		// 	StartPatch.thumbstickDown = false;

		var vehicleMaster = FlightSceneManager.instance.playerVehicleMaster;
		
		//Exempt helicopters and EF-24G from this logic as they don't need zoom capability - helis are added separately
		if (vehicleMaster != null && vehicleMaster.playerVehicle.vehicleName != "EF-24G" && !vehicleMaster.isHelicopter)
		{
			//Check that it's the first frame above input threshhold
			if (Mathf.Abs(ts.y) > activationThresh && !awaitingRelease && !VRThrottlePatch.thumbstickDown) 
			{
				//Get objects for each mfd
				TargetingMFDPage tgp = FlightSceneManager.instance.playerVehicleMaster.comms.targetingPage;
				MFDPTacticalSituationDisplay tsd = FlightSceneManager.instance.playerVehicleMaster.comms.tsdPage;
				DashMapDisplay nav = FlightSceneManager.instance.playerVehicleMaster.comms.targetingPage?.map;

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