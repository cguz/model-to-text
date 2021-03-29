/**
 * @file PreferenceAutomaticTextGeneratorFileImp.cs
 *
 * @brief This file contains a text generator from a file
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.AutomaticTextGenerator
{
    using Newtonsoft.Json;
    using Preference.IntelliText.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PreferenceAutomaticTextGeneratorFileImp : IAutomaticTextGenerator
	{
		public string GetText(ObjectId id, TextSettings settings)
		{
            // read the file json of the text generator file
            string filePath = settings.PathTextGeneratorProvider+ settings.Culture.Name+ ".json";
            string generatedText;

            if (File.Exists(filePath))
            {
                var parsedObject = JSONPref.ParseJSON(System.IO.File.ReadAllText(filePath));

                // get only the translator sections in JSON
                string translator = parsedObject[CommonVariables.KEY_JSON_TRANSLATOR_FILE_TRANSLATOR].ToString();

                // deserialize the JSON section
                var obj = JsonConvert.DeserializeObject<IDictionary<string, object>>(translator);
                generatedText = (obj.ContainsKey(id.Id))? (string)obj[id.Id]:"[WARNING] The key "+ id.Id + " does not exist in the translator file "+ settings.Culture.Name + ".json";

            }
            else
            {
                throw new FileNotFoundException("The translator file "+ filePath + " does not exist. It is required when we use the text generation from a FILE. Basically it is a translator <key, value>");
            }

            // return the text of the given id
            return generatedText;

        }

        public string GetText(ObjectId id)
        {
            throw new NotImplementedException();
        }
}
}
