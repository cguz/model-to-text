/**
 * @file IDescriptionText.cs
 *
 * @brief This file contains the text description of an element in a region
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.Template
{
    public interface IDescriptionText
    {
        string Key { get; }
        string Value { get; }
        int VerticalOrder { get; }

    }
}