/**
 * @file PreferenceRenderFactory.cs
 *
 * @brief This file contains a factory to create the renders
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Render
{
    class PreferenceRenderFactory
    {

        public static IPreferenceRender getRender(string keyRender = "")
        {
            return keyRender switch
            {
                "TXT" => new PreferenceRenderTXTImp(),
                "JSON" => new PreferenceRenderJSONImp(),
                "RTF" => new PreferenceRenderRTFImp(),
                "HTML" => new PreferenceRenderHTMLImp(),
                _ => new PreferenceRenderDefaultImp(),
            };
        }

    }
}
