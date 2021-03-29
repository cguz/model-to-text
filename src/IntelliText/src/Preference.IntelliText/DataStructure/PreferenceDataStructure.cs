/**
 * @file PreferenceDataStructure.cs
 *
 * @brief This file contains the class PreferenceDataStructure
 *
 * @author Cesar Augusto Guzman ALvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.DataStructure
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Preference.IntelliText.DataStructure.Model;
    using System.Dynamic;

    public class PreferenceDataStructure
    {

        public PreferenceDataStructureEntity Entity { get; set; }

        /**
        * It creates the Entity
        * 
        * @param key is the key name of the entity
        * @param type of the Entity. The type is related to the field prefSuiteItem of the descriptive model
        */
        public PreferenceDataStructureEntity Create(string key, string type)
        {

            // create the model
            Entity = new PreferenceDataStructureEntity(key);
            Entity.AddAttribute("type", type);

            return Entity;

        }

        /**
        * It creates the Entity.
        * 
        * @param key is the key name of the entity
        * @param type of the Entity. The type is related to the field prefSuiteItem of the descriptive model
        * @param set of attributes
        */
        public PreferenceDataStructureEntity Create(string key, string type, Dictionary<string, object> attributes)
        {
            Create(key, type);

            Entity.AddAttribute(attributes);

            return Entity;

        }

        public string GetKeyEntity()
        {
            return Entity.Key;
        }

        public int CountDefinitions(TypeDefinitions type)
        {
            return !Entity.Definitions.ContainsKey(type)?0:Entity.Definitions[type].Count;
        }
        public void Print()
        {

            Console.WriteLine("Model created with: ");
            foreach (TypeDefinitions type in Enum.GetValues(typeof(TypeDefinitions)))
            {
                int count = CountDefinitions(type);
                if (count != 0)
                    Console.WriteLine(CountDefinitions(type) + " "+type.ToString());
            }

            Entity.Print();
        }

        static void Main(string[] args)

        {
            Atom atom; 

            PreferenceDataStructure Gds = new();

            // first we create the entity
            PreferenceDataStructureEntity entity = Gds.Create("M01", "model");

            // second we create the attributes
            entity.AddAttribute("system", "PRACTICABLE");

            // third we create the atoms
            Random rnd = new(3173);
            int total = 3;

            // create the list of materials
            for (int j = 0; j< total; ++j)
            {
                atom = entity.AddAtomDefinition("KEY_MATERIAL_" + j, TypeDefinitions.Materials);
                atom.AddAttribute("Description", "Description of the material");
                atom.AddAttribute("Order", rnd.Next(total));
            }

                


            total = 3;

            // create the list of glasses
            atom = entity.AddAtomDefinition("KEY_GLASS_0", TypeDefinitions.Glasses);
            atom.AddAttribute("Description", "Description of the profile");
            atom.AddAttribute("Order", rnd.Next(total));
            atom.AddAttribute("Material", "KEY_MATERIAL_2");
            atom.AddAttribute("Material_interior", "KEY_MATERIAL_1");
            atom.AddAttribute("Material_exterior", "KEY_MATERIAL_2");
            atom.AddAttribute("Longitud", 1000);
            atom.AddAttribute("Others1", "Any other value");
            atom.AddAttribute("Others2", "Any other value");
            atom.AddAttribute("Others3", "Any other value");
            atom.AddAttribute("Others4", "Any other value");
            atom.AddAttribute("Others5", "Any other value");
            entity.AddChildren(atom);



            total = 5;

            // create more profiles as copy of KEY_GLASS_0
            for (int j = 1; j < total; ++j)
            {
                // Create Atom as copy of
                atom = entity.CreateAtomAsCopyOf("KEY_GLASS_" + j, "KEY_GLASS_0");
                    
                atom.AddAttribute("Order", rnd.Next(total));
                atom.AddAttribute("Longitud", 200);

                entity.AddChildren(atom);

            }

            // create the children of the model. Assume two glass
            entity.AddChildren("KEY_GLASS_1", "KEY_GLASS_2");
            entity.AddChildren("KEY_GLASS_1", "KEY_GLASS_4");               
                
            entity.AddChildren("KEY_GLASS_3");
            entity.AddChildren("KEY_GLASS_1");


            // TESTS
            Console.WriteLine(Gds.Entity.Key);
            Console.WriteLine("Childs order by the field order: ");

            var result = from at in entity.Children
                            orderby at.Attributes["Order"]
                            select at;

            foreach (var item in result)
                item.Print();


            Console.WriteLine("Filter by the attribute Material: ");

            /*
            PrefGDSModelFilter filter = new PrefGDSModelFilter();
            filter.AddCriteria(new CriteriaByAttribute("Material", "KEY_MATERIAL_2"));
            Atom[] elements = filter.Filter(Gds.Entity);
            foreach (Atom e in elements)
                e.Print();
            
            */

            /** example of dynamic generation of attributes */
            // Dictionary<String, Object> Attributes = new Dictionary<String, Object>();
            // Attributes.Add("Order", 20);
            // dynamic atom1 = new Atom("Key", Attributes);
            // Console.WriteLine(atom1.Order);

            dynamic myobject = new ExpandoObject();
            IDictionary<string, object> myUnderlyingObject = myobject;
            myUnderlyingObject.Add("IsDynamic", true); // Adding dynamically named property
            Console.WriteLine(myobject.IsDynamic); // Accessing the property the usual way

        }

    }
}
