/**
 * @file IPreferenceRender.cs
 *
 * @brief This file contains the interface to be used for rendering the text
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Render
{
    using Preference.IntelliText.Template;

    interface IPreferenceRender
    {
        public string Run(IDescriptionModel descriptionByRegions);
    }
}
