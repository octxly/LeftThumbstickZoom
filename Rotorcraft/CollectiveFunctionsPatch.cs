using System;
using HarmonyLib;
using UnityEngine;
using VTOLVR.DLC.Rotorcraft;

namespace LeftThumbstickZoom.Rotorcraft;

[HarmonyPatch(typeof(AH94CollectiveFunctions))]
[HarmonyPatch("CombatOnSetThumbstick")]
public class CollectiveFunctionsPatch
{
    private static bool awaitingRelease = false;
    
    public static bool Prefix(Vector3 axis, AH94CollectiveFunctions __instance)
    {
        //Only run on AH-94, AH-6 already has it built-in so not needed
        if (FlightSceneManager.instance.playerVehicleMaster.playerVehicle.vehicleName != "AH-94") return true;
        
        if (Math.Abs(axis.x) < 0.3 && awaitingRelease)
        {
            awaitingRelease = false;
        } 
        else if (Math.Abs(axis.x) > 0.8 && !awaitingRelease && __instance.combatCollective.IsTriggerPressed())
        {
            awaitingRelease = true;
            
            var tgp = FlightSceneManager.instance.playerVehicleMaster.comms.targetingPage;
            var nav = FlightSceneManager.instance.playerVehicleMaster.comms.targetingPage?.map;

            var x = axis.x * (Main.settings.invertAxis ? -1 : 1);

            //Type checking to make sure each is instantiated
            if (nav?.mfdPage?.isSOI == true || nav?.portalPage?.isSOI == true)
            {
                if (x > 0) 
                {
                    nav.ZoomIn();
                }
                else if (x < 0) 
                {
                    nav.ZoomOut();
                }
            }
            if (tgp?.mfdPage?.isSOI == true || tgp?.portalPage?.isSOI == true)
            {
                if (x > 0) 
                {
                    tgp.ZoomIn();
                }
                else if (x < 0) 
                {
                    tgp.ZoomOut();
                }
            }
        }

        //If awaiting release, then prevent pylon tilt code from executing
        //Also introduce a small dead zone for pylon tilt axis
        return Math.Abs(axis.y) > 0.3 && !awaitingRelease;
    }
}