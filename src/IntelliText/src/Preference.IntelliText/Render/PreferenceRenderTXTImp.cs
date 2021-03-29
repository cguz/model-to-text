/**
* @file PreferenceRenderTXTImp.cs
*
* @brief This file contains the TXT implementation of the render. 
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Render
{
    using Preference.IntelliText.Template;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class PreferenceRenderTXTImp : IPreferenceRender
    {

        private readonly string Separation = "\n";
        private readonly string Tabulation = "\t";

        private string GetOutPutRender()
        {
            return "";
        }


        /**
         * Execute the render
         * 
         * @param description of the model. It contains only the important elements.
         */
        public string Run(IDescriptionModel modelDescription)
        {

            // head : description and shortdescription and full description

            string output = GetOutPutRender();


            /** General Title **/

            // check if the element has title, and stores it
            if (!string.IsNullOrEmpty(modelDescription.Title))
                output += modelDescription.Title + "\n";


            // check if the element (sequence) has other elements
            IList<IDescriptionModel> list = modelDescription.ListRegions;

            // if there are more elements
            if (list?.Any() ?? false)
            {
                int count = list.Count;
                IDescriptionModel current;

                // for each element, we process the region.
                for (int index = 0; index < count-1; index++)
                {
                    current = list[index];

                    output += RunRegion(current) + Separation;
                }

                // we process the last one apart to avoid the last separation and the conditions inside the for
                current = list[count-1];
                output += RunRegion(current);

                return output;
                
            }

            return output;

        }

        private string RunRegion(IDescriptionModel regionDescription)
        {

            string output = "";

            /** General Title and Description**/

            // check if the element has title, and stores it
            if (!string.IsNullOrEmpty(regionDescription.Title))
                output += regionDescription.Title + "\n";

            // check if the element has description, and stores it
            if (!string.IsNullOrEmpty(regionDescription.Description))
                output += regionDescription.Description + "\n";

            // if the region is price
            if (regionDescription is IDescriptionModelPrice price)
            {
                output += "Total Amount: " + price.TotalAmount + Separation;

                // NOTE. For now, we do not consider more elements (regions inside a region)

                // we print the text descritpions of a region price DescriptionModelRegionPrice
                if (price.Texts?.Any() ?? false)
                {
                    output += RunTexts(price.Texts) + Separation;
                }

                output += PriceElements(price.Prices);

            }
            else
            {
                // we print the text descritpions of a normal DescriptionModelRegion 
                if (regionDescription.Texts?.Any() ?? false)
                {
                    output += RunTexts(regionDescription.Texts);
                }
            }

            return output;

        }

        private string PriceElements(IList<IDescriptionPrice> prices)
        {
            string output = "";

            int count = prices.Count;
            float total;

            // we sort the elements
            IEnumerable<IDescriptionPrice> listT = prices.OrderBy(x => x.VerticalOrder);
            IDescriptionPrice[] listOrdered = listT.ToArray();

            IDescriptionPrice current, next;
            if (count > 0) { 
                for (int index = 0; count > 1 && index < count - 1; index++)
                {
                    current = listOrdered[index];

                    next = listOrdered[index + 1];

                    total = (current.MeasureValue * current.Price);
                    output += current.MeasureValue + " " + current.MeasureType + Tabulation + current.Text + Tabulation + current.Price + Tabulation + total;

                    // if the current and next have the same vertical order, we print them in the same line
                    string sep = Separation;
                    if (current.VerticalOrder == next.VerticalOrder)
                        sep = ", ";

                    output += sep;

                }

                current = listOrdered[count - 1];

                total = (current.MeasureValue * current.Price);
                output += current.MeasureValue + " " + current.MeasureType + Tabulation + current.Text + Tabulation + current.Price + Tabulation + total;

            }

            return output;
        }


        private string RunTexts(IList<IDescriptionText> texts)
        {
            string output = "";

            int count = texts.Count;

            // we sort the elements
            IEnumerable<IDescriptionText> listT = texts.OrderBy(x => x.VerticalOrder);
            IDescriptionText[] listOrdered = listT.ToArray();
            
            IDescriptionText current, next;
            for (int index = 0; index < count-1; index++)
            {
                current = listOrdered[index];

                next = listOrdered[index+1];

                if (current.Key.Equals(current.Value))
                    output += current.Key;
                else
                    output += current.Value;

                // if the current and next have the same vertical order, we print them in the same line
                string sep = Separation;
                if (current.VerticalOrder == next.VerticalOrder)
                    sep = ", ";

                output += sep;

            }

            current = listOrdered[count-1];

            if (current.Key.Equals(current.Value))
                output += current.Key;
            else
                output += current.Value;

            return output;

        }

        /**
        * Execute the render
        * 
        * @param description of the model. It contains only the important elements.
        public string Run(IDescriptionModel modelDescription)
        {

            // head : description and shortdescription and full description


            string output = "";
            string textOut;

            // check if the element has title, and stores it
            textOut = modelDescription.Title;
            if (!string.IsNullOrEmpty(textOut))
                output += textOut + "\n";

            // check if the element has description, and stores it
            textOut = modelDescription.Description;
            if (!string.IsNullOrEmpty(textOut))
                output += textOut + "\n";


            // check if the element (sequence) has other elements
            IList<IDescriptionModel> list = null;
            list = modelDescription.ListRegions;

            // if there are more elements
            if (list?.Any() ?? false)
            {
                string separation;
                int count = list.Count;
                IDescriptionModel current, next;

                // we sort the elements
                IEnumerable<IDescriptionModel> listT = list.OrderBy(x => x.VerticalOrder).ThenBy(x => x.HorizontalOrder);
                IDescriptionModel[] listOrdered = listT.ToArray();

                // for each element, we get its text
                for (int index = 0; index < count - 1; index++)
                {
                    separation = " ";

                    current = listOrdered[index];

                    next = listOrdered[index + 1];

                    if (modelDescription.GetType() == typeof(DescriptionModelSequence))
                    {
                        separation = "\n";
                    }
                    else
                    {
                        if (current.VerticalOrder != next.VerticalOrder)
                            separation = "\n";
                        else
                            separation = ", ";
                    }

                    output += Run(current) + separation;
                }

                current = listOrdered[count - 1];
                output += Run(current) + " ";

                return output;

            }

            // when the element is a product, we store its text
            if (modelDescription.GetType() == typeof(DescriptionModelRegionPrice))
            {
                IDescriptionModelPrice price = (IDescriptionModelPrice)modelDescription;
                // output += price.Quantity + "\t" + price.Text + "\t" + price.Price + "\t" + price.GoalPrice;
                output += price.Texts + "\t" + price.Price + "\t" + price.GoalPrice;
            }
            else
            {
                output += modelDescription.Texts;
            }

            return output;

        }
        */
    }
}