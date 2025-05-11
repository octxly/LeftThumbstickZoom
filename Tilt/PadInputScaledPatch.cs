using HarmonyLib;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(TiltController))]
[HarmonyPatch("PadInputScaled")]
public class PadInputScaledPatch
{
    public static bool Prefix(TiltController __instance)
    {
        //Only execute tilt if thumbstick is down
        //Also checks whether the instance of TiltController is for the player or AI vehicles
        if (!StartPatch.thumbstickDown && __instance == FlightSceneManager.instance.playerVehicleMaster.tiltController) return false;
        return true;
    }
}