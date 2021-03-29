/**
* @file IDescriptionModel.cs
*
* @brief This file contains the interface of the Description
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Template
{
    using System.Collections.Generic;

    /**
     * This class contains the text generated for a model. 
     * 
     * It intends to have basic information for the render
     */
    public interface IDescriptionModel
    {        
        public string Title { get;  }
        
        public string Description { get;  }

        public IList<IDescriptionModel> ListRegions { get; }

        public IList<IDescriptionText> Texts { get; }

    }
}