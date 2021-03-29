/**
* @file DescriptionModelRegionPrice.cs
*
* @brief This file contains the description texts, price description and the total amount of a given atom or element
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Template
{
    using System.Collections.Generic;

    /**
     * This class contains the text and price generated for an element. 
     * 
     * It intends to have basic information for the render
     */
    class DescriptionModelRegionPrice : DescriptionModelRegion, IDescriptionModelPrice
    {

        private float totalAmount;

        public DescriptionModelRegionPrice(IDictionary<string, object> attributes, List<IDescriptionText> listGeneratedText, List<IDescriptionPrice> listPrices) 
            : base(attributes, listGeneratedText)
        {
            // In the attributes we have configuration parameters of the PDG.
            // attributes

            // initialises the prices
            Prices = new List<IDescriptionPrice>();

            // store the prices
            ((List<IDescriptionPrice>) Prices).AddRange(listPrices);

            TotalAmount = -1;

        }

        public float TotalAmount
        {
            get
            {
                if (totalAmount != -1)
                    return totalAmount;
                else
                {
                    totalAmount = 0;
                    if (Prices.Count > 0) { 
                        for (int index = 0; index < Prices.Count; ++index)
                        {
                            totalAmount += (Prices[index].Price * Prices[index].MeasureValue);
                        }
                    }

                    return totalAmount;
                }
            }
            set
            {
                totalAmount = value;
            }
        }

        public virtual IList<IDescriptionPrice> Prices { get; protected set; }

    }
}