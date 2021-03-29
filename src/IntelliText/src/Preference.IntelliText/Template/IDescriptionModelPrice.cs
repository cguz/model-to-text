/**
* @file IDescriptionModelPrice.cs
*
* @brief This file contains the interface of the Price Description
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Template
{

    using System.Collections.Generic;


    /**
     * This class contains the text price. 
     * 
     * It intends to have basic information for the render
     */
    public interface IDescriptionModelPrice : IDescriptionModel
    {

        public float TotalAmount { get; set; }

        public IList<IDescriptionPrice> Prices { get; }

    }
}