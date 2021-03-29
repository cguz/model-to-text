/**
 * @file CriteriaByAttribute.cs
 *
 * @brief This file contains the interface ICriteria. Filter Design Pattern
 * 
 * TODO - Require some improve.
 *
 * @author Cesar Augusto Guzman ALvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.GDS.Model
{
    using Preference.IntelliText.GDS.Model;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ICriteria
    {
        public Atom[] Filter(Atom[] nodes);
    }
}
