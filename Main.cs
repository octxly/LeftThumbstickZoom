global using static LeftThumbstickZoom.Logger;
using System.IO;
using System.Reflection;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using HarmonyLib;
using UnityEngine;
using System.Configuration;

namespace LeftThumbstickZoom;

[ItemId("octxly.leftthumbstickzoom")] // Harmony ID for your mod, make sure this is unique
public class Main : VtolMod
{
	public string ModFolder;

	public static Settings settings;

	private void Awake()
	{
		ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		Log($"Awake at {ModFolder}");

		settings = JsonUtility.FromJson<Settings>(File.ReadAllText($"{ModFolder}\\options.json"));
	}

	private void Start()
	{
		var instance = new Harmony("octxly.leftthumbstickzoom");
		instance.PatchAll(Assembly.GetExecutingAssembly());
	}

	public override void UnLoad()
	{
		// Destroy any objects
	}

	public class Settings(bool invertAxis)
	{
		public bool invertAxis = invertAxis;
	}
}