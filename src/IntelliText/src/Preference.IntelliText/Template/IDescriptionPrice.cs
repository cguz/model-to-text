/**
* @file IDescriptionPrice.cs
*
* @brief This file contains the interface of a Price
*
* @author Cesar Augusto Guzman Alvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText.Template
{
    public interface IDescriptionPrice
    {
        float MeasureValue { get; }
        string MeasureType { get; }
        string Text { get; }
        float Price { get; }
        int VerticalOrder { get; }
    }
}