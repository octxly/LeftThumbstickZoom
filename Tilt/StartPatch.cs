using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;

namespace LeftThumbstickZoom;

[HarmonyPatch(typeof(VRThrottle))]
[HarmonyPatch("Start")]
public class StartPatch
{
    public static bool thumbstickDown = false;
    private static bool canClickAgain = true;

    public static AudioClip audioClip;
    public static AudioSource audioSource;

    public static void Postfix(UnityEvent ___OnStickPressUp, UnityEvent ___OnStickPressDown)
    {
        //Adds listeners to thottle to allow me to access the thumbstick state
        ___OnStickPressDown.AddListener(() => 
        {
            var name = FlightSceneManager.instance.playerVehicleMaster?.playerVehicle?.vehicleName;

            //Exemptions for the aircraft where a thumbstick click isnt necessary, they don't need this
            if (!canClickAgain || name == "T-55" || name == "F/A-26B" || name == "F-16" || name == "A-10D") return;
            canClickAgain = false;
            thumbstickDown = !thumbstickDown;
            audioSource?.PlayOneShot(audioClip);
        });
        ___OnStickPressUp.AddListener(() => canClickAgain = true);
    }
}