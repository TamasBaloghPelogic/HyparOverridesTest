using Elements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace OverrideTraining
{
	/// <summary>
	/// Override metadata for ReprColorGroupOverride
	/// </summary>
	public partial class ReprColorGroupOverride : IOverride
	{
        public static string Name = "ReprColorGroup";
        public static string Dependency = null;
        public static string Context = "[*discriminator=Elements.ServiceCore]";
		public static string Paradigm = "Group";

        /// <summary>
        /// Get the override name for this override.
        /// </summary>
        public string GetName() {
			return Name;
		}

		public object GetIdentity() {
			return Identities;
		}

	}


}