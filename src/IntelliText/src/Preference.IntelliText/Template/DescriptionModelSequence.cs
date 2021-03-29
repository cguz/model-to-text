/**
 * @file DescriptionModelSequence.cs
 *
 * @brief This file contains text description of the Sequence 
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
     * Class with the basic information for the text description of the Sequence
     */
    class DescriptionModelSequence : IDescriptionModel
    {

        /**
         * Constructor to create DescriptionModelSequence
         * 
         * @param a dictionary of the ITemplate. From here we select the text to store.
         */
        public DescriptionModelSequence(IDictionary<string, object> attributes, List<IDescriptionModel> listRegions = null)
        {

            // store the title
            if (attributes.ContainsKey(CommonVariables.KEY_JSON_GENERAL_SHOW_TITLE))
                if ((bool)attributes[CommonVariables.KEY_JSON_GENERAL_SHOW_TITLE])
                    Attributes.Add(CommonVariables.KEY_JSON_GENERAL_TITLE, attributes[CommonVariables.KEY_JSON_GENERAL_TITLE]);
                else
                    Attributes.Add(CommonVariables.KEY_JSON_GENERAL_TITLE, null);

            ListRegions = listRegions;

        }

        /**
         * dictionary to store the text
         */
        public IDictionary<string, object> Attributes = new Dictionary<string, object>();


        public virtual string Title { get { return (string) Attributes[CommonVariables.KEY_JSON_GENERAL_TITLE]; } }

        public virtual IList<IDescriptionModel> ListRegions { get; protected set; }

        
        // the following methods are not used in DescriptionSequence
        public virtual IList<IDescriptionText> Texts { get; protected set; }

        public virtual string Description { get; }

    }
}