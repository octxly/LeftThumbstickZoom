using HarmonyLib;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(TiltController))]
[HarmonyPatch("PadInputScaled")]
public class PadInputScaledPatch
{
    public static bool Prefix(TiltController __instance)
    {
        //Ensure that it is actually loaded before trying to access anything
        //This class is used by AI vtol aircraft as well, so an additional check is needed to ensure only the player one is affected
        if (FlightSceneManager.instance.playerVehicleMaster == null || __instance != FlightSceneManager.instance.playerVehicleMaster.tiltController) return true;

        //Only execute tilt if thumbstick is down
        return VRThrottlePatch.thumbstickDown;
    }
}