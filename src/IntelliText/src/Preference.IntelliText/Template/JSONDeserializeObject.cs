/**
 * @file JSONDeserializeObject
 * .cs
 *
 * @brief This file contains the objects to deserialize a JSON with information about the template
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template.Serialization
{
    using System.Collections.Generic;

    // Templates.json

    // change TemplatesConf class to IDictionary<string, object> attributes

    class TemplatesFillRegions
    {
        public string TemplateName;
        public List<int> Regions;
    }

    class TemplatesAudienceTargets
    {
        public string TemplateName;
        public IDictionary<string, object>[] AudienceTargets; 
    }


    // GTA_Regions.json

    // change TemplateRegions class to IDictionary<string, object> attributes

    class TypeProductsInRegions
    {
        public int RegionName;
        public IDictionary<string, object>[] Products;
    }

    // change Products class to IDictionary<string, object> attributes

}
