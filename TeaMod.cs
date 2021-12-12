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
using Terraria.ModLoader;

namespace TeaFramework
{
	/// <summary>
	///		Extended <see cref="Mod"/> functionality.
	/// </summary>
	public partial class TeaMod : Mod
	{
		public List<(MethodInfo, Delegate)> DelegatesToRemove = new();
		public List<Hook> HooksToRemove = new();
		
		public TeaMod()
		{
			// Manually set an instance for "internal" use.
			if (GetType() == typeof(TeaMod)) 
				TeaInstance = this;
		}

		public override void Load()
		{
			base.Load();

			if (!ExecuteInternally(() =>
			{
				int count = ModLoader.Mods.Count(x => x.GetType().IsSubclassOf(
					                                      typeof(TeaMod)) &&
				                                      x.GetType() != typeof(TeaMod)
				);
				Logger.Info($"Loaded Rejuvena's TeaFramework v{Version} by Tomat.");
				Logger.Info($"Mods found directly sub-classing {nameof(TeaMod)}: {count}");
			}))
				Logger.Info(
					$"Mod \"{Name}\" is backed by TeaFramework. Go to https://github.com/Rejuvena/TeaFramework for more information."
				);
		}

		public override void Unload()
		{
			base.Unload();
			
			foreach ((MethodInfo method, Delegate callback) in DelegatesToRemove)
				HookEndpointManager.Unmodify(method, callback);

			foreach (Hook hook in HooksToRemove.Where(hook => hook.IsApplied))
				hook.Undo();

			DelegatesToRemove.Clear();
			HooksToRemove.Clear();
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