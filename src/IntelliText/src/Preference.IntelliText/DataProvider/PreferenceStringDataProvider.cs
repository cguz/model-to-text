using Preference.IntelliText.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preference.IntelliText.DataProviders
{
	///////////////////////////////////////////////////////////
	// PreferenceStringDataProvider
	//
	// Implementación de la interfaz IObjectDataProvider a partir
	// de una cadena de texto.

	public class PreferenceStringDataProvider : IObjectDataProvider
	{
		public PreferenceStringDataProvider(string data)
		{
			Data = data;
		}

		private string Data { get; }

		public PreferenceDataStructure GetObjectData()
		{
			throw new NotImplementedException();
		}

		public PreferenceDataStructure GetObjectData(string[] identifiers)
		{
			throw new NotImplementedException();
		}
	}
}
