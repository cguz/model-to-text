/**
 * @file PreferenceRenderFactory.cs
 *
 * @brief This file contains the factory to generate a Template
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Preference.IntelliText.Common;
    using Preference.IntelliText.Template.Serialization;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class PreferenceTemplateFactory
    {
        // Key of the template
        private readonly object KeyTemplate;

        // Automatic text generator provider
        private readonly string TextGeneratorProvider;

        // Indicate us the full path of the template
        private readonly string File;

        // ITemplate to build 
        private ITemplate Template = null;


        // Store in memory the information about the region of the template
        private IDictionary<string, object>[] listTemplateRegions;
        private List<TypeProductsInRegions> listRegionsInProducts;



        /**
         * General Factory to Construct the template
         */
        public PreferenceTemplateFactory(object keyTemplate, string file, string textGeneratorProvider)
        {
            KeyTemplate = keyTemplate;
            File = file;
            TextGeneratorProvider = textGeneratorProvider;
        }


        /**
         * Create the ITemplate just when we require it
         */
        public ITemplate Build()
        {

            // if we have not yet built the ITemplate, we create it
            if (Template == null)
                Template = CreateTemplate(KeyTemplate);

            // Otherwise, we return the ITemplate
            return Template;

        }

        /**
         * Create the ITemplate
         * 
         * @param id of the template
         */
        private ITemplate CreateTemplate(object KeyTemplate)
        {
            // read and parse the JSON file
            var parsedObject = JSONPref.ParseJSON(System.IO.File.ReadAllText(File));

            // each template has information about their regions
            // we store template's regions and region's products in memory
            StoreRegionsAndProducts((string)parsedObject[CommonVariables.KEY_JSON_TEMPLATES_REGIONS]);



            // from here we known that the JSON exists and it is well format

            // get only the template sections in JSON
            string templatesConfJson = parsedObject[CommonVariables.KEY_JSON_TEMPLATES_CONF].ToString(Formatting.None);

            // deserialize the JSON section
            var obj = JsonConvert.DeserializeObject<IDictionary<string, object>[]>(templatesConfJson);

            // retrieve the template attributes
            IDictionary<string, object> templateAttributes = obj.Where(i => i[CommonVariables.KEY_JSON_TEMPLATES_NAME].Equals(KeyTemplate)).First();

            // create the ITemplate as a Sequence 
            Sequence sequence = new(templateAttributes, TextGeneratorProvider);

            // create the list of regions
            List<ITemplate> listRegions = sequence.Regions;
            CreateListOfRegions(parsedObject, KeyTemplate, ref listRegions);

            // create the list of "TemplatesAudienceTargets"
            sequence.Audience = CreateListOfAudienceTargets(parsedObject, KeyTemplate);

            return sequence;

        }

        /**
         * Store the regions of the templates
         * 
         * @param full path of the JSON file that contains the regions
         */
        private void StoreRegionsAndProducts(string fileRegions)
        {

            string strInput = fileRegions;
            string jsonOutput;

            // if it is a path, we read it
            if (fileRegions.Contains(".json"))
                strInput = System.IO.File.ReadAllText(fileRegions);

            // parse the content of the JSON file
            var parsedRegions = JSONPref.ParseJSON(strInput);


            // get the information about the regions
            jsonOutput = parsedRegions.GetValue(CommonVariables.KEY_JSON_TEMPLATES_REGIONS).ToString(Formatting.None);
            listTemplateRegions = JsonConvert.DeserializeObject<IDictionary<string, object>[]>(jsonOutput);

            // get the information about the products inside the regions
            jsonOutput = parsedRegions.GetValue(CommonVariables.KEY_JSON_REGIONS_GROUP_PRODUCTS_IN_REGIONS).ToString(Formatting.None);
            listRegionsInProducts = JsonConvert.DeserializeObject<List<TypeProductsInRegions>>(jsonOutput);

        }


        /**
         * Create the list of regions of the template
         * 
         * @param parsed object of the JSON
         * @param id of the template
         * @param list of regions of the template
         */
        private List<ITemplate> CreateListOfRegions(JObject parsedObject, object KeyTemplate, ref List<ITemplate> listRegions)
        {
            // get the regions in JSON format
            var templatesFillRegionsJson = parsedObject[CommonVariables.KEY_JSON_TEMPLATES_FILL_REGIONS].ToString(Formatting.None);
            
            // deserialize the JSON in classes
            var obj = JsonConvert.DeserializeObject<List<TemplatesFillRegions>>(templatesFillRegionsJson);

            TemplatesFillRegions templateRegion = null;

            try
            {
                // get the regions of the current template
                templateRegion = obj.Where(i => i.TemplateName.Equals(KeyTemplate)).First();
            } catch (Exception) { }

            // if the template has regions
            if (templateRegion != null)
            {
                // for each region in the template
                foreach (int idRegion in templateRegion.Regions)
                {
                    // the id regions are the index position in the array
                    int id = idRegion - 1;
                    try
                    {
                        // if the id region is in the range of the regions
                        if (id < 0 || id >= listTemplateRegions.Length)
                            throw new IndexOutOfRangeException("Please, check the index of the region");

                        // get the region and its associated products
                        IDictionary<string, object> region = listTemplateRegions[id];

                        TypeProductsInRegions regionsInProducts = null;
                        var list = listRegionsInProducts.Where(i => i.RegionName == (int)(long)region["Id"]);
                        if (list.Any())
                        {
                            regionsInProducts = list.First();
                        }

                        // add to the list of regions
                        if (region.ContainsKey(CommonVariables.KEY_JSON_REGIONS_REGION_NAME))
                        {
                            listRegions.Add(GetRegion(((string)region[CommonVariables.KEY_JSON_REGIONS_REGION_NAME]).Trim().ToLower(), ref region, ref regionsInProducts, TextGeneratorProvider));
                        }

                    } catch (Exception) {
                        throw new Exception("Please check the definition of the region id "+ idRegion);
                    }
                }
            }

            return listRegions;

        }

        protected virtual Region GetRegion(string regionName, ref IDictionary<string, object> region, ref TypeProductsInRegions regionsInProducts, string TextGeneratorProvider)
        {
            IDictionary<string, object>[] type = null;
            if (regionsInProducts != null)
                type = regionsInProducts.Products;

            return regionName switch
            {
                "price" => new RegionPrice(ref region, type, TextGeneratorProvider),
                "color" => new RegionColor(ref region, type, TextGeneratorProvider),
                _ => new Region(ref region, type, TextGeneratorProvider),
            };
        }

        /**
         * Create the list of audience targets of the template
         * 
         * @param parsed object of the JSON
         * @param id of the template
         * @param list of audience target of the template
         */
        private IDictionary<string, object>[] CreateListOfAudienceTargets(JObject parsedObject, object keyTemplate)
        {
            // get the audience targets in JSON format
            var templatesAudienceTargetsJson = parsedObject[CommonVariables.KEY_JSON_TEMPLATES_AUDIENCE_TARGETS].ToString();
            
            // deserialize the JSON in classes
            var obj = JsonConvert.DeserializeObject<List<TemplatesAudienceTargets>>(templatesAudienceTargetsJson);

            TemplatesAudienceTargets templateAudienceTarget = null;

            try
            {
                // get the audience target of the current id template
                templateAudienceTarget = obj.Where(i => i.TemplateName.Equals(keyTemplate)).First();
            
            } catch (Exception) { }

            // if the template has audience target
            if (templateAudienceTarget != null)
            {
                return templateAudienceTarget.AudienceTargets; 
            }
            
            return null;
        }


    }
}