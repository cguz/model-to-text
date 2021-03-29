/**
 * @file CriteriaByAttribute.cs
 *
 * @brief This file contains the class CriteriaByAttribute. Filter Design Pattern
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


    public class CriteriaByAttribute : ICriteria
    {
        public string Key { get; }

        public object Value { get; }

        public CriteriaByAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public Atom[] Filter(Atom[] nodes)
        {
            List<Atom> newNodes = new List<Atom>();

            for (int i = 0; i < nodes.Length; i++)
            {
                Atom atoms = nodes[i];

                if (atoms.Attributes.ContainsKey(Key) && atoms.Attributes[Key].Equals(Value))
                {
                    newNodes.Add(atoms);
                }
            }

            return newNodes.ToArray();

        }
    }
}
