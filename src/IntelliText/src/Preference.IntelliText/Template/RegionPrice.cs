
/**
* @file RegionPrice.cs
*
* @brief This file contains the information of a region price
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Template
{
    using Preference.IntelliText.Common;
    using Preference.IntelliText.DataStructure;
    using Preference.IntelliText.DataStructure.Model;
    using System.Collections.Generic;

    /**
     * This class process the region template
     */
    class RegionPrice : Region
    {

        /**
         * Store the price text descriptions
         */

        private readonly List<IDescriptionPrice> ListPrices = new();

        public RegionPrice(ref IDictionary<string, object> attribute, IDictionary<string, object>[] products, string textGeneratorProvider) : base(ref attribute, products, textGeneratorProvider)
        {

        }

        /**
        * Get the description of the region
        * 
        * It generates the common text list and the price text lists
        * 
        * @param preference's entity that contain the model
        * @param target's audience for the description
        * 
        * @return the description (text) of the model 
        */
        public override IDescriptionModel GetDescriptions(PreferenceDataStructure model, TextSettings settings)
        {
            // filter all the general atoms we need of a given region
            FilterAtomsOfRegion(model, settings);

            // store the atributes and description model of the region and return it
            return new DescriptionModelRegionPrice(Attributes, ListCommonText, ListPrices);

        }


        /**
         * Search by the associate atoms of each group of the region
         * 
         * @param group of the region to add
         * 
         * @return set of keys of atoms
         * 
         */
        protected override string[] GetProductsByGroup(string type)
        {
            List<string> products = new();

            switch (type)
            {
                case "price-general":
                    products.Add("KPreci5");
                    products.Add("KPreci6");
                    products.Add("KPreci7");
                    products.Add("KPreci8");
                    break;

                case "price":
                    products.Add("KPreci1");
                    products.Add("KPreci2");
                    products.Add("KPreci3");
                    products.Add("KPreci4");
                    break;
            }

            return products.ToArray();
        }

        /**
        * Get the text description of the atom
        * 
        * @param list of options (text, position, etc)
        * @param atom or product
        * @param general settings
        * 
        * The function store the Description text in the list of common text and price
        */
        protected override void GetProductDescriptionText(IDictionary<string, object> product, Atom atom, TextSettings settings)
        {
            // check if the atom requires translation
            // Se sopone que algunos atoms requieren la generación de texto otros no (Sacar la información de la BD).
            string generatedText = TextGenerator.GetText(new ObjectId { Id = atom.Key, Type = (string)atom.Attributes[CommonVariables.KEY_DATASTRUCTURE_TYPE] }, settings);

            // add the description text of the model
            product[CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT] = generatedText;

            // TODO - CHECK!
            if (atom.Attributes[CommonVariables.KEY_DATASTRUCTURE_TYPE].Equals("1"))
            {
                product[CommonVariables.KEY_JSON_PRODUCT_PRICE] = atom.Attributes[CommonVariables.KEY_JSON_PRODUCT_PRICE];
                // product[CommonVariables.KEY_JSON_PRODUCT_GOALPRICE] = atom.Attributes[CommonVariables.KEY_JSON_PRODUCT_GOALPRICE];
                // product[CommonVariables.KEY_JSON_PRODUCT_LENGTH] = atom.Attributes[CommonVariables.KEY_JSON_PRODUCT_LENGTH];
                // product[CommonVariables.KEY_JSON_PRODUCT_HEIGHT] = atom.Attributes[CommonVariables.KEY_JSON_PRODUCT_HEIGHT];
                product[CommonVariables.KEY_JSON_PRODUCT_MEASURE_VALUE] = atom.Attributes[CommonVariables.KEY_JSON_PRODUCT_MEASURE_VALUE];
                product[CommonVariables.KEY_JSON_PRODUCT_MEASURE_TYPE] = atom.Attributes[CommonVariables.KEY_JSON_PRODUCT_MEASURE_TYPE];

                // to manipulate the price section
                ListPrices.Add(new DescriptionPriceImp(product));
                return;
            }

            // to manipulate the general text of the section
            ListCommonText.Add(new DescriptionTextImp(product));

        }
    }
}