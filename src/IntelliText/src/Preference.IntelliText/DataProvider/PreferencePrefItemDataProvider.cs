using Preference.IntelliText.DataStructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preference.IntelliText.DataProviders
{
	class PreferencePrefItemDataProvider : IObjectDataProvider
	{
		public PreferenceDataStructure GetObjectData()
		{
			throw new NotImplementedException();
		}

		public PreferenceDataStructure GetObjectData(string[] identificators)
        {
            throw new NotImplementedException();
        }
    }


	class PrefItemFactory
	{
		
		//IObjectDataProvider CreateDataProvider (string ItemId)
	}
}
