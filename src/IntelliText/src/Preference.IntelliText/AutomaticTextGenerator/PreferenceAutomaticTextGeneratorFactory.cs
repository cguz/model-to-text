/**
 * @file PreferenceTextGeneratorFactory.cs
 *
 * @brief This file contains a factory to create the text generators
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.AutomaticTextGenerator
{
    class PreferenceAutomaticTextGeneratorFactory
    {

        public static IAutomaticTextGenerator GetTextGenerator(string keyGenerator)
        {
            return keyGenerator switch
            {
                "DB" => new PreferenceAutomaticTextGeneratorDBImp(),
                "FILE" => new PreferenceAutomaticTextGeneratorFileImp(),
                _ => new PreferenceAutomaticTextGeneratorOtherImp(),
            };
        }

    }
}
