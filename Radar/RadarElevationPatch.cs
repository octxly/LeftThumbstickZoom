using HarmonyLib;

namespace LeftThumbstickZoom.Radar;

[HarmonyPatch(typeof(AdvancedRadarController))]
[HarmonyPatch("OnElevationInput")]
public class RadarElevationPatch
{
    public static bool Prefix()
    {
        var name = FlightSceneManager.instance.playerVehicleMaster?.playerVehicle?.vehicleName;

        var radar = FlightSceneManager.instance.playerVehicleMaster.comms.radarPage;

        //Only slew radar on thumbstick down
        if (name == "EF-24G" || name == "AH-94") return false;

        //Only slew in SOI
        return radar?.mfdPage?.isSOI == true || radar?.portalPage?.isSOI == true;
    }
}
