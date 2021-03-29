/**
 * @file Common.cs
 *
 * @brief This file contains the common variables and methods to be used
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Common
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    class CommonVariables
    {
        // GENERAL = It can be in a Template or a region

        public const string KEY_JSON_GENERAL_TITLE = "Title";
        public const string KEY_JSON_GENERAL_SHOW_TITLE = "ShowTitle";
        public const string KEY_JSON_GENERAL_DESCRIPTION = "Description";
        public const string KEY_JSON_GENERAL_SHOW_DESCRIPTION = "ShowDescription";

        public const string KEY_JSON_GENERAL_TEXT_GENERATOR_PROVIDER = "TextGeneratorProvider";


        public const string KEY_JSON_TEMPLATES_NAME = "TemplateName";

        public const string KEY_JSON_TEMPLATES_CONF = "TemplatesConf";
        public const string KEY_JSON_TEMPLATES_FILL_REGIONS = "TemplatesFillRegions";
        public const string KEY_JSON_TEMPLATES_REGIONS = "TemplateRegions";
        public const string KEY_JSON_TEMPLATES_AUDIENCE_TARGETS = "TemplatesAudienceTargets";
        public const string KEY_JSON_TEMPLATES_LANGUAGE_TEXT = "LanguageText";
        public const string KEY_JSON_TEMPLATES_TARGET = "Target";

        public const string KEY_JSON_REGIONS_REGION_NAME = "RegionName";
        public const string KEY_JSON_REGIONS_GROUP_PRODUCTS_IN_REGIONS = "GroupProductsInRegions";

        public const string KEY_JSON_PRODUCTS_ATG_TEXT = "ATGText";
        public const string KEY_JSON_PRODUCTS_GROUP = "Group"; // if this exists, we do not use the next one, Product. It means that if we decide to keep Type we can remove Product
        public const string KEY_JSON_PRODUCTS_KEY_PRODUCT = "Product";
        public const string KEY_JSON_PRODUCT_ORDER_VERTICAL = "OrderVertical";
        public const string KEY_JSON_PRODUCT_ORDER_HORIZONTAL = "OrderHorizontal";

        public const string KEY_JSON_PRODUCT_PRICE = "Price";
        public const string KEY_JSON_PRODUCT_MEASURE_VALUE = "Quantity";
        public const string KEY_JSON_PRODUCT_MEASURE_TYPE = "MeasureType";
        public const string KEY_JSON_PRODUCT_GOALPRICE = "GoalPrice";
        public const string KEY_JSON_PRODUCT_HEIGHT = "Height";
        public const string KEY_JSON_PRODUCT_LENGTH = "Length";

        public const string KEY_DATASTRUCTURE_TEXT = "Text";
        public const string KEY_DATASTRUCTURE_TYPE = "Type";
        public const string KEY_DATASTRUCTURE_DESCRIPTION = "description";


        public const string KEY_DEFAULT_DATABASE_PROVIDER = "DB";
        public const string KEY_DEFAULT_LANGUAGE = "en-EN";
        public const string KEY_LANGUAGE_ES = "es-ES";
        public const string KEY_LANGUAGE_EN = "en-EN";
        public const string KEY_LANGUAGE_DE = "de-DE";

        public const string KEY_JSON_TRANSLATOR_FILE_TRANSLATOR = "Translator";
    }



    class JSONPref
    {
        public static JObject ParseJSON(string strInput)
        {
            if (!string.IsNullOrWhiteSpace(strInput))
            {
                strInput = strInput.Trim();

                if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                    (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
                {
                    try
                    {
                        return JObject.Parse(strInput);
                    }
                    catch (JsonReaderException jex)
                    {
                        //Exception in parsing json
                        Console.WriteLine(jex.Message);
                    }
                    catch (Exception ex)
                    {
                        // some other exception
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            return null;
        }
    }

    public static class CommonMethod
    {
        public static void AddRange<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }

    }

}
