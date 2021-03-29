/**
 * @file DescriptionModelRegion.cs
 *
 * @brief This file contains text description of a region in the Template. 
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    using Preference.IntelliText.Common;
    using System.Collections.Generic;

    /**
     * The class contains the text descriptions of a region
     * We inherent the same functionality of DescriptionSequence
     * We manage this class as a IDescriptionModel
     */
    class DescriptionModelRegion : DescriptionModelSequence
    {

        // regions can have a set of description texts, which are generated from Atoms. 
        // we handle them with the variable Texts 
        // It inherents from DescriptionModelSequence to reuse the title, and all the get methods implementations. 
        // The list of regions will be null in this class

        /**
         * Constructor of DescriptionRegion
         * 
         * @param a dictionary of the ITemplate. From here we select the text to store.
         */
        public DescriptionModelRegion(IDictionary<string, object> attributes, List<IDescriptionText> listGeneratedText) : base(attributes)
        {
            // store the name of the region
            if (attributes.ContainsKey(CommonVariables.KEY_JSON_REGIONS_REGION_NAME))
                Attributes.Add(CommonVariables.KEY_JSON_REGIONS_REGION_NAME, attributes[CommonVariables.KEY_JSON_REGIONS_REGION_NAME]);

            // store the description
            if (attributes.ContainsKey(CommonVariables.KEY_JSON_GENERAL_SHOW_DESCRIPTION))
                if ((bool)attributes[CommonVariables.KEY_JSON_GENERAL_SHOW_DESCRIPTION])
                    Attributes.Add(CommonVariables.KEY_JSON_GENERAL_DESCRIPTION, attributes[CommonVariables.KEY_JSON_GENERAL_DESCRIPTION]);
                else
                    Attributes.Add(CommonVariables.KEY_JSON_GENERAL_DESCRIPTION, null);

            // initialize the variable
            Texts = new List<IDescriptionText>();

            // store the generated text
            ((List<IDescriptionText>) Texts).AddRange(listGeneratedText);

        }

        public override string Description { get { return (string)Attributes[CommonVariables.KEY_JSON_GENERAL_DESCRIPTION]; } }

    }
}