#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using TeaFramework.Common.Utilities.Extensions;
using TeaFramework.Core.Compatibility.Calls;
using TeaFramework.Core.Compatibility.Calls.Implementation;
using TeaFramework.Core.Localization;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TeaFramework
{
	/// <summary>
	///		Extended <see cref="Mod"/> functionality.
	/// </summary>
	public partial class TeaMod : Mod
	{
		public readonly List<(MethodInfo, Delegate)> EditsToRemove = new();
		public readonly List<Hook> DetoursToRemove = new();

		/// <summary>
		///		Handles localization loading for your mod.
		/// </summary>
		public virtual ILocalizationLoader LocalizationLoader { get; } =
			new Core.Localization.Implementation.LocalizationLoader();

		/// <summary>
		///		The <see cref="IModCaller"/> instance of your mod that handles <see cref="Mod.Call"/>s.
		/// </summary>
		public virtual IModCaller CallHandler { get; protected set; } = new ModCaller();

		public TeaMod()
		{
			// Manually set an instance for "internal" use.
			if (GetType() == typeof(TeaMod)) 
				TeaInstance = this;
		}

		public override void Load()
		{
			base.Load();

			MonoModHooks.RequestNativeAccess();
			
			if (!ExecuteInternally(() =>
			{
				int count = ModLoader.Mods.Count(x => x.GetType().IsSubclassOf(
					                                      typeof(TeaMod)) &&
				                                      x.GetType() != typeof(TeaMod)
				);
				Logger.Info($"Loaded TeaFramework v{Version} by Tomat.");
				Logger.Info($"Mods found directly sub-classing {nameof(TeaMod)}: {count}");
			}))
				Logger.Info(
					$"Mod \"{Name}\" is backed by TeaFramework. Go to https://github.com/Rejuvena/TeaFramework for more information."
				);

			ExecuteInternally(() =>
			{
				CreateDetour(typeof(LocalizationLoader).GetCachedMethod("Autoload"),
					GetType().GetCachedMethod(nameof(AutoloadLocalization)));
				
				ApplyEdits();
			});
		}

		public override void Unload()
		{
			base.Unload();
			
			foreach ((MethodInfo method, Delegate callback) in EditsToRemove)
				HookEndpointManager.Unmodify(method, callback);

			foreach (Hook hook in DetoursToRemove.Where(hook => hook.IsApplied))
				hook.Undo();

			EditsToRemove.Clear();
			DetoursToRemove.Clear();
		}

		public override object Call(params object[] args) => CallHandler.HandleCall(this, args);

		/// <summary>
		///		Returns a localized string.
		/// </summary>
		/// <param name="key">The key, without Mods.YourModName</param>
		/// <param name="args">any passable arguments.</param>
		/// <returns>A localized string.</returns>
		public virtual string GetText(string key, params object[] args) =>
			Language.GetTextValue($"Mods.{Name}.{key}", args);
		
		private static void AutoloadLocalization(Action<Mod> orig, Mod mod)
		{
			orig(mod);
			
			if (mod is TeaMod teaMod)
				teaMod.LocalizationLoader.Load(mod);
		}

		/// <summary>
		///		Executes an operation only if this is the core TeaFramework assembly.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <returns><see langword="true"/> if the <paramref name="action"/> was executed, otherwise <see langword="false"/>.</returns>
		/// <remarks>
		///		This is <see langword="private"/> as it should never be used outside of <see cref="TeaMod"/>.
		/// </remarks>
		private bool ExecuteInternally(Action? action)
		{
			if (!(GetType().Assembly.FullName?.StartsWith("TeaFramework, ") ?? false))
				return false;
			
			action?.Invoke();
			return true;
		}
	}
}