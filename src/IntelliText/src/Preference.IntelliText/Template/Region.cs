/**
 * @file Region.cs
 *
 * @brief This file contains the Region of a Template. 
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    using Preference.IntelliText.AutomaticTextGenerator;
    using Preference.IntelliText.DataStructure;
    using Preference.IntelliText.DataStructure.Model;
    using Preference.IntelliText.Common;
    using System.Collections.Generic;
    using System;


    /**
     * The information of this class is extracted from the JSON file
     */
    class Region : ITemplate
    {

        /** 
         * Set of attributes of the region of the template
         */
        public IDictionary<string, object> Attributes { get; }

        /** 
         * Set of products that contains the region
         */
        public IDictionary<string, object>[] GroupProductsInRegion { get; }

        /**
         * Provider of text generator for the current region
         */
        protected readonly IAutomaticTextGenerator TextGenerator;

        /**
         * Store the common text descriptions
         */
        protected List<IDescriptionText> ListCommonText = new();


        /**
         * List of identifiers of the model that are needed for the region
         */
        // private List<string> Identifiers;

        /**
         * Constructor of the class
         * 
         * @param Set of attributes of the region
         */
        public Region(ref IDictionary<string, object> attribute, IDictionary<string, object>[] products, string textGeneratorProvider)
        {
            Attributes = attribute;

            // create the text generator
            object provider = textGeneratorProvider;
            if (Attributes.ContainsKey(CommonVariables.KEY_JSON_GENERAL_TEXT_GENERATOR_PROVIDER))
                Attributes.TryGetValue(CommonVariables.KEY_JSON_GENERAL_TEXT_GENERATOR_PROVIDER, out provider);

            TextGenerator = IntelliText.AutomaticTextGenerator.PreferenceAutomaticTextGeneratorFactory.GetTextGenerator((string) provider);

            GroupProductsInRegion = products;

        }
        

        /**
         * Get the description of the region
         * 
         * It generates the common text list
         * 
         * @param preference's entity that contain the model
         * @param target's audience for the description
         * 
         * @return the description (text) of the model 
         */
        public virtual IDescriptionModel GetDescriptions(PreferenceDataStructure model, TextSettings settings)
        {
            // filter all the atoms we need of a given region
            FilterAtomsOfRegion(model, settings);

            // store the atributes and description model of the region and return it
            return new DescriptionModelRegion(Attributes, ListCommonText);

        }


        /**
         * 11-03-2021:
         * 
         * Tal como está ahora esta función hace lo siguiente:
         * - por cada tipo de producto calcula en una lista sus productos (atoms) asociados (función GetProductsByType). 
         * - Seguidamente itera por cada uno de los productos y obtiene sus atoms. 
         * - Después debería consultar su traducción a la base de datos (GetDescriptionProduct)
         * 
         * Esta es una versión simplista donde la lista de productos asociados a un tipo esta puesta Ad-hoc en la clase relacionada con la region.
         * 
         * [TODO] La nueva versión que debo investigar es aquella que por medio de una consulta filtre los atoms que necesitamos.
         * A lo mejor esto implique hacerlo en otra función
         * 
         */
        protected void FilterAtomsOfRegion(PreferenceDataStructure model, TextSettings settings)
        {

            // for each group of product (or product for compatibility with the version 1)
            foreach (IDictionary<string, object> groupProduct in GroupProductsInRegion)
            {
                List<string> products = new();

                if (groupProduct.ContainsKey(CommonVariables.KEY_JSON_PRODUCTS_GROUP))
                {
                    string group = (string)groupProduct[CommonVariables.KEY_JSON_PRODUCTS_GROUP];

                    products.AddRange(GetProductsByGroup(group));

                } else {
                    if (groupProduct.ContainsKey(CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT))
                    {
                        products.Add((string)groupProduct[CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT]);
                    }
                }

                // get the atom with the product name in products.Product
                foreach (string prod in products)
                {
                    // if the atom exists
                    if (model.Entity.Elements.ContainsKey(prod))
                    {
                        // we get the atom
                        Atom atom = model.Entity.Elements[prod];

                        // assign the name of product
                        groupProduct[CommonVariables.KEY_JSON_PRODUCTS_KEY_PRODUCT] = prod;

                        // get the important information of the product for the render
                        GetProductDescriptionText(groupProduct, atom, settings);
                    }
                }
            }
        }


        /**
         * Search by the associate atoms of each group of the region
         * 
         * @param group of the region to add
         * 
         * @return set of keys of atoms
         * 
         * The function is override by RegionPrice and RegionColor
         */
        protected virtual string[] GetProductsByGroup(string group)
        {
            List<string> products = new();

            switch (group)
            {
                case "description":
                    products.Add("ProfilSystem");
                    products.Add("RahmenSystem");
                    products.Add("FlugelSystem");
                    products.Add("Entwasserung");
                    products.Add("Duebelbohrung");
                    products.Add("Abstandhalter");
                    break;
                case "dimensions":
                    products.Add("Glasmasse471");
                    products.Add("Glasmasse746");
                    products.Add("Griffhoehe1");
                    products.Add("Griffhoehe2");
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
         * The function store the Description text in the list of common text
         * 
         * The function is override by RegionPrice
         */
        protected virtual void GetProductDescriptionText(IDictionary<string, object> product, Atom atom, TextSettings settings)
        {
            // check if the atom requires translation
            // en el XML Descriptivo actual la Key "type" viene en minuscula. Debería ser "Type". Por ahora arreglado en DataProvider/PreferenceFileDataProvider.cs
            // Se supone que algunos atoms requieren la generación de texto otros no (Sacar la información de la BD).
            string generatedText = TextGenerator.GetText(new ObjectId { Id = atom.Key, Type = (string) atom.Attributes[CommonVariables.KEY_DATASTRUCTURE_TYPE] }, settings);

            product[CommonVariables.KEY_JSON_PRODUCTS_ATG_TEXT] = generatedText;

            // add the atom in the list
            ListCommonText.Add(new DescriptionTextImp(product));

        }
    }
}
