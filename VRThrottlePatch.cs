using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;

namespace LeftThumbstickZoom;

//This file controls the switching behaviour of the thumbstick click

[HarmonyPatch(typeof(VRThrottle))]
[HarmonyPatch("Start")]
public class VRThrottlePatch
{
    public static bool thumbstickDown;
    private static bool canClickAgain;

    public static AudioClip audioClip;
    public static AudioSource audioSource;

    public static void Postfix(UnityEvent ___OnStickPressUp, UnityEvent ___OnStickPressDown)
    {
        //Otherwise another non-vtol aircraft could be loaded and have the thumbstick stuck down
        thumbstickDown = false;
        canClickAgain = true;
        
        // Helicopters, non-VTOL jets, and the EF-24 do not need the click, so exempt them
        if (FlightSceneManager.instance.playerVehicleMaster.isHelicopter ||
            !FlightSceneManager.instance.playerVehicleMaster.isVTOLCapable ||
            FlightSceneManager.instance.playerVehicleMaster.playerVehicle.vehicleName == "EF-24G") return;
        
        //Adds listeners to thottle thumbstick to listen for the click
        ___OnStickPressDown.AddListener(() =>
        {
            if (!canClickAgain) return;

            canClickAgain = false;
            thumbstickDown = !thumbstickDown;
            audioSource?.PlayOneShot(audioClip);
        });
        ___OnStickPressUp.AddListener(() => canClickAgain = true);
    }
}