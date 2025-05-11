using HarmonyLib;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(TiltController))]
[HarmonyPatch("PadInput")]
public class PadInputPatch
{
    public static bool Prefix()
    {
        //I THINK THIS CAN BE DEPRECATED, NOT NECESSARY

        //Only execute tilt if thumbstick is down
        //if (StartPatch.thumbstickDown) return false;
        return true;
    }
}