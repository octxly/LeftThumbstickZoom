// using HarmonyLib;
//
// namespace LeftThumbstickZoom.Radar;
//
// [HarmonyPatch(typeof(AdvancedRadarController))]
// [HarmonyPatch("OnElevationInput")]
// public class RadarElevationPatch
// {
//     public static bool Prefix()
//     {
//         var name = FlightSceneManager.instance.playerVehicleMaster?.playerVehicle?.vehicleName;
//
//         var radar = FlightSceneManager.instance.playerVehicleMaster.comms.radarPage;
//
//         //Exceptions - exclude from logic
//         if (name == "EF-24G" || name == "AH-94" || name == "AH-6") return true;
//
//         //Only slew in SOI
//         return radar?.mfdPage?.isSOI == true || radar?.portalPage?.isSOI == true;
//     }
// }

//I don't think this patch is relevant, since this mod doesn't touch the radar anymore