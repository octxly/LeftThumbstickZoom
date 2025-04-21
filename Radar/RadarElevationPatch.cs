using HarmonyLib;

namespace LeftThumbstickZoom.Radar;

[HarmonyPatch(typeof(AdvancedRadarController))]
[HarmonyPatch("OnElevationInput")]
public class RadarElevationPatch
{
    public static bool Prefix()
    {
        var name = FlightSceneManager.instance.playerVehicleMaster?.playerVehicle?.vehicleName;

        //Only slew radar on thumbstick down
        if (!StartPatch.thumbstickDown && name != "EF-24G" && name != "AH-94") return false;
        return true;
    }
}
