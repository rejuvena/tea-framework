#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Collections.Generic;
using TeaFramework.API.Patching;
using Terraria.ModLoader;

namespace TeaFramework
{
	/// <summary>
	///		The <see cref="Mod"/> instance used by tModLoader for Tea Framework. Your mods will typically inherit this.
	/// </summary>
	/// <remarks>
	///		If you do not want to inherit this class, you can implement functionality directly with <see cref="ITeaMod"/>.
	/// </remarks>
	public class TeaMod : Mod, ITeaMod, IPatchRepository
	{
		Mod ITeaMod.ModInstance => this;

		public List<IMonoModPatch> Patches { get; } = new();

		public override void Load()
		{
			base.Load();
			
			MonoModHooks.RequestNativeAccess();
		}

		public override void Unload()
		{
			base.Unload();
			
			foreach (IMonoModPatch patch in Patches)
				patch.Unapply();
		}

		/// <summary>
		///		Executes a tasks only intended to be done by Tea Framework. Used as a workaround for a tModLoader issue (TML-2332).
		/// </summary>
		private bool ExecutePrivately(Action task)
		{
			if (!GetType().Assembly.FullName?.StartsWith("TeaFramework, ") ?? false)
				return false;

			task();
			return true;
		}
	}
}