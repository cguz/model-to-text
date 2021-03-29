/**
 * @file ITemplate.cs
 *
 * @brief This file contains the interface of the Template
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    using Preference.IntelliText.DataStructure;

    interface ITemplate
    {

        /**
         * 
         * 4.1. Consultando a la base de datos en caso de que requieran descripción automática
         */
        IDescriptionModel GetDescriptions(PreferenceDataStructure model, TextSettings settings);
    }
}
