/**
 * @file Sequence.cs
 *
 * @brief This file contains a Sequence of Region. In a sense, a Template is a Sequence of Region or only one Region
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    using Preference.IntelliText.AutomaticTextGenerator;
    using Preference.IntelliText.DataStructure;
    using Preference.IntelliText.Common;
    using System.Collections.Generic;

    /**
     * The information of this class is extracted from the JSON file
     */
    class Sequence : ITemplate
    {


        /** 
         * Set of attributes of the template
         */
        private readonly IDictionary<string, object> Attributes;

        /**
         * Set of regions of the template
         */
        public List<ITemplate> Regions { get; private set; }

        /**
         * The target audience of the template
         */
        public IDictionary<string, object>[] Audience { get; set; }

        /**
         * Provider of text generator for the current template
         */
        private readonly IAutomaticTextGenerator TextGenerator;



        /**
         * Constructor of the class
         * 
         * @param Set of attributes of the template
         */
        public Sequence(IDictionary<string, object> attribute, string automaticTextGenerator)
        {
            Attributes = attribute;
            object textGeneratorProvider;

            // create the text generator
            if (!Attributes.TryGetValue(CommonVariables.KEY_JSON_GENERAL_TEXT_GENERATOR_PROVIDER, out _))
                textGeneratorProvider = CommonVariables.KEY_DEFAULT_DATABASE_PROVIDER;
            else
                textGeneratorProvider = automaticTextGenerator;

            TextGenerator = IntelliText.AutomaticTextGenerator.PreferenceAutomaticTextGeneratorFactory.GetTextGenerator((string)textGeneratorProvider);

            Regions = new List<ITemplate>();

        }

        /**
         * Get the description of the template
         * 
         * @param preference's entity that contain the model
         * @param target's audience for the description
         * 
         * @return the description (text) of the model 
         */
        public IDescriptionModel GetDescriptions(PreferenceDataStructure model, TextSettings settings)
        {

            // We need to keep the attributes OrderVertical and OrderHorizontal in the interface IDescription. To sort the elements

            // if there is not language at the beginning
            if (settings.Culture == null)
            {
                // default language
                string language = CommonVariables.KEY_DEFAULT_LANGUAGE;

                // identify the language with the target audience, if there is one. 
                IDictionary<string, object> audience = FindTargetAudience(settings.Audience);
                if (audience != null && audience.ContainsKey(CommonVariables.KEY_JSON_TEMPLATES_LANGUAGE_TEXT))
                    language = (string)audience[CommonVariables.KEY_JSON_TEMPLATES_LANGUAGE_TEXT];

                settings.Culture = new System.Globalization.CultureInfo(language, false);
            }

            List<IDescriptionModel> ListDescriptionsByRegion = new();

            // for each region 
            foreach (ITemplate region in Regions)
            {
                // add the list description of the region
                ListDescriptionsByRegion.Add(region.GetDescriptions(model, settings));

            }

            // store the atributes and the list of text generated of the sequence
            IDescriptionModel descriptions = new DescriptionModelSequence(Attributes, ListDescriptionsByRegion);

            // return description model of the sequence
            return descriptions;

        }

        private IDictionary<string, object> FindTargetAudience(string targetAudience)
        {

            foreach (IDictionary<string, object> l in Audience)
            {
                if (l.ContainsKey(CommonVariables.KEY_JSON_TEMPLATES_TARGET)){
                    if (!targetAudience.Equals(l[CommonVariables.KEY_JSON_TEMPLATES_TARGET]))
                        continue;

                    return l;
                }
            }

            return null;
            
        }
    }
}
