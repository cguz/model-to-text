/**
 * @file ITemplateConstructor.cs
 *
 * @brief This file contains the interface ITemplateConstructor
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    /**
     * This class contains a catalog of templates. 
     */
    interface ITemplateConstructor
    {

        /**
         * Create a Template 
         * The given key template is required
         */
        ITemplate Build(string keyTemplate);

        /**
         * Retrieve the ids of the templates
         */
        string[] GetTemplatesId();

    }

}
