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
    using System.Collections.Generic;

    /**
     * If the region in the template is a color, we associate the list of atoms
     */
    internal class RegionColor : Region
    {
        public RegionColor(ref IDictionary<string, object> attribute, IDictionary<string, object>[] products, string textGeneratorProvider) : base(ref attribute, products, textGeneratorProvider)
        {
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
                case "color":
                    products.Add("FarbeWeiss");
                    break;
            }

            return products.ToArray();
        }

    }
}