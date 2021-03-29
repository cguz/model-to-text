/**
 * @file PreferenceDataStructureComplexModelAndQueryPerformance.cs
 *
 * @brief This file compare performance of some complex model and queries
 *
 * @author Cesar Augusto Guzman ALvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference
{
    namespace DataStructure
    {
        namespace Tests
        {
            using System;
            using System.Linq;
            using System.Diagnostics;
            using Preference.IntelliText.DataStructure;
            using Preference.IntelliText.DataStructure.Model;
            using Xunit;
            using Moq;

            public class PreferenceDataStructureComplexModelAndQueryPerformance : PreferenceDataStrcutureCommonPerformance
            {

                private MockRepository mockRepository;

                public PreferenceDataStructureComplexModelAndQueryPerformance()
                {
                    this.mockRepository = new MockRepository(MockBehavior.Strict);
                }


                private PreferenceDataStructureEntity Create_Exahustive_ModelGDS()
                {
                    Atom atom;

                    Gds = new PreferenceDataStructure();
                    PreferenceDataStructureEntity entity = Gds.Create("M01", "model");

                    Random rnd = new(3173);
                    int total = 3;

                    // create the list of materials
                    for (int i = 0; i < total; ++i)
                    {
                        atom = entity.AddAtomDefinition("KEY_MATERIAL_" + i, TypeDefinitions.Materials);
                        atom.AddAttribute("Description", "Description of the material");
                        atom.AddAttribute("Order", rnd.Next(total));
                    }


                    total = 2000;

                    // create the list of glasses
                    for (int i = 0; i < total; ++i)
                    {
                        atom = entity.AddAtomDefinition("KEY_GLASS_" + i, TypeDefinitions.Glasses);
                        atom.AddAttribute("Description", "Description of the glass");
                        atom.AddAttribute("Order", rnd.Next(total));

                        bool assignMaterial = (rnd.Next(0, 2) == 1);

                        if (assignMaterial)
                        {
                            atom.AddAttribute("Material", "KEY_MATERIAL_" + rnd.Next(entity.Definitions[TypeDefinitions.Materials].Count));
                        }

                        entity.AddChildren(atom);

                    }

                    // logic relationships
                    entity.AddChildren("KEY_GLASS_0", "KEY_GLASS_5");
                    entity.AddChildren("KEY_GLASS_0", "KEY_GLASS_10");
                    entity.AddChildren("KEY_GLASS_0", "KEY_GLASS_12");

                    entity.AddChildren("KEY_GLASS_5", "KEY_GLASS_16");
                    entity.AddChildren("KEY_GLASS_16", "KEY_GLASS_17");
                    entity.AddChildren("KEY_GLASS_17", "KEY_GLASS_19");

                    entity.AddCopyOf("KEY_GLASS_2", "KEY_GLASS_0");

                    return entity;

                }

                /// <summary>
                // Three Materials, 2000 glasses some of them with an associated material.
                // Logic relationship as follows:" 
                // Model has 2000 children, all the glasses
                // KEY_GLASS_0 has three children, KEY_GLASS_5, KEY_GLASS_10, and KEY_GLASS_12
                // KEY_GLASS_5 has one child, KEY_GLASS_16
                // KEY_GLASS_16 has one child, KEY_GLASS_17
                // KEY_GLASS_17 has one child, KEY_GLASS_19
                // Copy of:
                // KEY_GLASS_2 is copy of KEY_GLASS_0
                /// </summary>
                [Fact]
                public PreferenceDataStructureEntity ACreate_Exahustive_ModelGDS()
                {
                    return Create_Exahustive_ModelGDS();
                }

                /// <summary>
                // 1. Cuenta el numero de vidrios que tienen la misma copiaOf que 
                // el vidrio con número de orden X
                /// </summary>
                [Fact]
                public void Count_Children_Of_CopyOF_Glass_With_Order_X_LINQ()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    // search a glass that is copy of another glass
                    Atom search = entity.Search("KEY_GLASS_2");

                    // with its order number
                    int order = (int)search.Attributes["Order"];

                    for (int i = 0; i < 1000; i++)
                    {

                        // perform the search
                        var result = from atom in entity
                                     where (int)atom.Attributes["Order"] == order
                                     join atomOrder in entity on atom.CopyOf equals atomOrder.CopyOf
                                     join f in entity on atomOrder.CopyOf.Key equals f.Key
                                     select f.Children.Count;
                        result.ToList();

                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                }

                /// <summary>
                // 1. Cuenta el numero de vidrios que tienen la misma copiaOf que 
                // el vidrio con número de orden X
                /// </summary>
                [Fact]
                public void Count_Children_Of_CopyOF_Glass_With_Order_X_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    // search a glass that is copy of another glass
                    Atom search = entity.Search("KEY_GLASS_2");

                    // with its order number
                    int order = (int)search.Attributes["Order"];

                    for (int i = 0; i < 1000; i++)
                    {
                        // perform the search
                        Atom atom = entity.ChildrenSearchByAttribute("Order", order);

                        atom.CountChildrenOfCopy();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                }

                /// <summary>
                // 2. Cuenta el numero de ancestros para el vidrio con orden X
                /// </summary>
                [Fact]
                public void Count_Ancestors_Glass_With_Order_X_LINQ()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();


                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    // search a glass with many Ancestors
                    Atom search = entity.Search("KEY_GLASS_19");

                    for (int i = 0; i < 1000; i++)
                    {

                        // search a glass with many Ancestors
                        var result = from atom in entity
                                     where (int)atom.Attributes["Order"] == (int)search.Attributes["Order"]
                                     select atom.Ancestors;
                        result.ToList();
                    }

                    // Print(result);

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                }

                /// <summary>
                // 2. Cuenta el numero de ancestros para el vidrio con orden X
                /// </summary>
                [Fact]
                public void Count_Ancestors_Glass_With_Order_X_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    // search a glass with many Ancestors
                    Atom search = entity.Search("KEY_GLASS_19");

                    for (int i = 0; i < 1000; i++)
                    {

                        // we use the order of the glass to perform the query
                        Atom atom = entity.ChildrenSearchByAttribute("Order", search.Attributes["Order"]);

                        /*if (atom != null)
                            Console.WriteLine(atom.Ancestors);*/
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                }
            }
        }
    }
}
