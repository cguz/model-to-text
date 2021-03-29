/**
* @file DescriptionPriceImp.cs
*
* @brief This file contains the price text description of a given atom or element
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Template
{
    using Preference.IntelliText.Common;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /**
     * This class represents the price descritpion text of a product
     */
    public class DescriptionPriceImp : IDescriptionPrice
    {
        /**
         * Constructor of the price description text of a product
         * 
         * @param a dictionary of the ITemplate. From here we select the price, quantity, and text to store
         */
        public DescriptionPriceImp(IDictionary<string, object> attribute)
        {

            // store the name of the product. This is the key for us
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT, attribute[CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT]);

            // store the generated text. This is the value.
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT, attribute[CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT]);

            // store the price.
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCT_PRICE, attribute[CommonVariables.KEY_JSON_PRODUCT_PRICE]);

            // store the measure value.
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCT_MEASURE_VALUE, attribute[CommonVariables.KEY_JSON_PRODUCT_MEASURE_VALUE]);

            // store the measure type.
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCT_MEASURE_TYPE, attribute[CommonVariables.KEY_JSON_PRODUCT_MEASURE_TYPE]);

            // store the vertical order
            Attributes.Add(CommonVariables.KEY_JSON_PRODUCT_ORDER_VERTICAL, attribute[CommonVariables.KEY_JSON_PRODUCT_ORDER_VERTICAL]);

        }

        /**
         * dictionary to store the text
         */
        public IDictionary<string, object> Attributes = new Dictionary<string, object>();

        public float MeasureValue{ get { return (float) Decimal.Parse((string) Attributes[CommonVariables.KEY_JSON_PRODUCT_MEASURE_VALUE], new CultureInfo("en-US")); } }
        public string MeasureType { get { return (string) Attributes[CommonVariables.KEY_JSON_PRODUCT_MEASURE_TYPE]; } }
        public string Text { get { return (string) Attributes[CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT]; } }
        public float Price { get { return (float) Decimal.Parse((string) Attributes[CommonVariables.KEY_JSON_PRODUCT_PRICE], new CultureInfo("en-US")); } }
        public int VerticalOrder { get { return (int) Convert.ToInt64(Attributes[CommonVariables.KEY_JSON_PRODUCT_ORDER_VERTICAL]); } }

    }
}