/**
* @file DescriptionTextImp.cs
*
* @brief This file contains the text description of a given atom or element
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
     * This class represents the text description of a given atom or element
     */
    class DescriptionTextImp : IDescriptionText
    {
        /**
         * Constructor of the description text of a product
         * 
         * @param a dictionary of the ITemplate. From here we select the text to store.
         * 
         */
        public DescriptionTextImp(IDictionary<string, object> att) 
        {
            // store the name of the product. This is the key for us
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT, att[CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT]);

            // store the generated text. This is the value.
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT, att[CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT]);

            // store the vertical order
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCT_ORDER_VERTICAL, att[CommonVariables.KEY_JSON_PRODUCT_ORDER_VERTICAL]);

            // store the horizontal order
            // Attributes.Add(CommonVariables.KEY_JSON_PRODUCT_ORDER_HORIZONTAL, att[CommonVariables.KEY_JSON_PRODUCT_ORDER_HORIZONTAL]);

        }


        /**
         * dictionary to store the text
         */
        public IDictionary<string, object> Attributes = new Dictionary<string, object>();


        public string Key { get  { return (string) Attributes[CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT];  } }
        public string Value { get { return (string)Attributes[CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT]; } }
        public virtual int VerticalOrder { get { return (int)(long)Attributes[CommonVariables.KEY_JSON_PRODUCT_ORDER_VERTICAL]; } }

    }
}