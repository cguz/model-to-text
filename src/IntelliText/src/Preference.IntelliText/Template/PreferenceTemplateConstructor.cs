/**
 * @file PreferenceTemplateConstructor.cs
 *
 * @brief This file contains the class PreferenceTemplateConstructor. 
 * This class build the ITemplate, which contains the information of a given template
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using Preference.IntelliText.Common;


    /**
     * Implement the class that contains a catalog of templates. 
     */
    class PreferenceTemplateConstructor : ITemplateConstructor
    {

        /**
         * Store the folder where we can find the set of templates
         */
        private string FolderTemplate;

        /**
         * Store the information of the key and the template once it is build
         */
        private IDictionary<object, PreferenceTemplateFactory> CatalogTemplate;

        /**
         * Store the initial configuration
         */
        private readonly TextSettings Settings;



        /**
         * Construct company's template catalog 
         * 
         * @param initial settings, it is required the folder of the company with the templates
         */
        public PreferenceTemplateConstructor(TextSettings settings)
        {
            Settings = settings;

            Setup(Settings.PathTemplates);
        }

        private void Setup(string path)
        {
            // instantiate the catalog of templates
            CatalogTemplate = new Dictionary<object, PreferenceTemplateFactory>();

            // full path of the templates
            FolderTemplate = path;

            // store the template ids in the variable catalogTemplate
            StoreTemplateOfCatalog(FolderTemplate);

        }

        /**
         * Iterate through the folder finding templates in JSON
         * 
         * @param full path of the directory or file
         */
        private void StoreTemplateOfCatalog(string pathDirectoryOrFile)
        {
            // if it is a directory
            if (Directory.Exists(pathDirectoryOrFile))
            {
                // read the directory
                string[] files = Directory.GetFiles(FolderTemplate);

                // for each file, call recursivelly the function
                foreach (string fileName in files)
                    StoreTemplateOfCatalog(fileName);

            } else {

                // if it is a file, we process it to get the id of the templates
                StoreIdTemplateAssociatedWithFactory(pathDirectoryOrFile);
               
            }

        }


        /**
         * With each Template (Catalog) Constructor there exists a tuple <idTemplate, TemplateFactory>
         * 
         * Where idTemplate is the template identificator
         * 
         * TemplateFactory has:
         * 
         *  - Template identificator
         *  - Path where we can find the template
         *  - And the template in its interface ITemplate (create only when it is required)
         * 
         * @param full path of the file or template
        */
        private void StoreIdTemplateAssociatedWithFactory(string pathFile)
        {
            // read the file
            string strInput = File.ReadAllText(pathFile);

            // parse the file and check whether it is a JSON or not
            var parsedObject = JSONPref.ParseJSON(strInput);

            // get the list of templates
            var listTemplates = parsedObject.GetValue(CommonVariables.KEY_JSON_TEMPLATES_CONF);

            // for each template
            foreach (JObject template in listTemplates)
            {
                // if the NameTemplate does not exist in the catalog
                string keyTemplate = (string) template.GetValue(CommonVariables.KEY_JSON_TEMPLATES_NAME);
                if (!CatalogTemplate.ContainsKey(keyTemplate))
                {
                    // we store the idTemplate and the factory
                    CatalogTemplate.Add(keyTemplate, new PreferenceTemplateFactory(keyTemplate, pathFile, Settings.TextGeneratorProvider));
                }
            }
        }

        /**
         * Create the ITemplate just when we are going to use it
         * 
         * @param id of the template to create
         */
        public ITemplate Build(string TemplateKey)
        {
            // if we have the idTemplate
            if (CatalogTemplate.ContainsKey(TemplateKey))
            {
                // we build the idTemplate with its associated factory
                return CatalogTemplate[TemplateKey].Build();
            }
            
            throw new EntryPointNotFoundException();
        }

        /**
         * Retrieve the ids of the templates
         */
        public string[] GetTemplatesId()
        {
            return (string[]) CatalogTemplate.Keys.ToArray();
        }
    }
}
