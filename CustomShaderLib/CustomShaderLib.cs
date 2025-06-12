using System;
using System.Threading.Tasks;
using SkyFrost.Base;
using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;

namespace ExampleMod;
//More info on creating mods can be found https://github.com/resonite-modding-group/ResoniteModLoader/wiki/Creating-Mods
public class ExampleMod : ResoniteMod {
	internal const string VERSION_CONSTANT = "1.0.0"; //Changing the version here updates it in all locations needed
	public override string Name => "CustomShaderLib";
	public override string Author => "__Choco__";
	public override string Version => VERSION_CONSTANT;
	public override string Link => "https://github.com/AwesomeTornado/Resonite-CustomShaderLib";

	public override void OnEngineInit() {
		Harmony harmony = new Harmony("com.__Choco__.CustomShaderLib");
		harmony.PatchAll();
	}

	[HarmonyPatch(typeof(MaterialProvider), "EnsureSharedShader")]
	class AnyShaderAnywherePatch {
		public static bool Prefix(AssetRef<Shader> assetRef, Uri url, MaterialProvider __instance, ref IAssetProvider<Shader> __result) {
			if (assetRef.Target == null)
				assetRef.Target = (IAssetProvider<Shader>)AccessTools.Method(typeof(MaterialProvider), "GetSharedShader").Invoke(__instance, new object[] { url });
			__result = assetRef.Target;
			return false;
		}
	}

	[HarmonyPatch(typeof(AssetInterface), "IsValidShader")]
	class AllShadersValidPatch {
		public static bool Prefix(ref Task<bool> __result) {
			__result = Task<bool>.Run(() => {
				return true;
			});
			return false;
		}
	}
}
