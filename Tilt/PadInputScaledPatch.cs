using HarmonyLib;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(TiltController))]
[HarmonyPatch("PadInputScaled")]
public class PadInputScaledPatch
{
    public static bool Prefix()
    {
        //Only execute tilt if thumbstick is down
        if (!StartPatch.thumbstickDown) return false;
        return true;
    }
}