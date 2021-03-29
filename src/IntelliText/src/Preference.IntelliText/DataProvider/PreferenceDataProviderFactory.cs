/**
 * @file PreferenceDataProviderFactory.cs
 *
 * @brief This file contains a factory to create the data provider
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.DataProviders
{
    class PreferenceDataProviderFactory
    {

        public static IObjectDataProvider getDataProvider(string keyDataProvider = "", string path = "")
        {
            return keyDataProvider switch
            {
                "FILE" => new PreferenceFileDataProvider(path),
                "STRING" => new PreferenceStringDataProvider(path),
                "PREFITEM" => new PreferencePrefItemDataProvider(),
                _ => new PreferencePrefItemDataProvider()
            };
        }

    }
}
