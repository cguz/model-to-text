/**
 * @file PreferenceDataStructureSimpleModelAndQueryPerformance.cs
 *
 * @brief This file compare performance of a simple model and queries
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
            using System.Collections.Generic;
            using Preference.IntelliText.DataStructure;
            using Preference.IntelliText.DataStructure.Model;
            using Xunit;
            using Moq;

            /**
             * @file PreferenceDataStrcutureCommonPerformance.cs
             *
             * @brief This file contains common information for the classes used to tests the performance
             *
             * @author Cesar Augusto Guzman ALvarez
             * Contact: cguzman@preference.es
             *
             */
            public class PreferenceDataStrcutureCommonPerformance
            {
                public PreferenceDataStructure Gds { get; set; }

                public PreferenceDataStructureEntity entity;

                protected void Print(List<Atom> results)
                {
                    foreach (var item in results)
                    {
                        item.Print();
                    }

                }

                protected void Print(IEnumerable<Atom> results)
                {
                    foreach (var item in results)
                    {
                        item.Print();
                    }
                }
            }


            public class PreferenceDataStructureSimpleModelAndQueryPerformance : PreferenceDataStrcutureCommonPerformance
            {

                private MockRepository mockRepository;

                public PreferenceDataStructureSimpleModelAndQueryPerformance()
                {
                    this.mockRepository = new MockRepository(MockBehavior.Strict);
                }


                private PreferenceDataStructureEntity Create_Simple_ModelGDS()
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



                    total = 4;

                    // create the list of glasses
                    for (int i = 0; i < total; ++i)
                    {
                        atom = entity.AddAtomDefinition("KEY_GLASS_" + i, TypeDefinitions.Glasses);
                        atom.AddAttribute("Description", "Description of the glass");
                        atom.AddAttribute("Order", rnd.Next(total));

                        bool assignMaterial = (rnd.Next(0, 2) == 1);

                        if (assignMaterial)
                            atom.AddAttribute("Material", "KEY_MATERIAL_" + rnd.Next(entity.Definitions[TypeDefinitions.Materials].Count));

                    }

                    // create the children of the model. Assume two glass
                    entity.AddChildren("KEY_GLASS_0", "KEY_GLASS_1");
                    entity.AddChildren("KEY_GLASS_0", "KEY_GLASS_3");

                    entity.AddChildren("KEY_GLASS_2");
                    entity.AddChildren("KEY_GLASS_0");

                    return entity;
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


                    total = 1000;

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

                        entity.Children.Add(atom);

                    }

                    return entity;
                }



                /// <summary>
                // Three Materials, four glasses some of them with an associated material.
                // Logic relationship as follows:" 
                // Model has two children KEY_GLASS_0 and KEY_GLASS_2." 
                // KEY_GLASS_0 has two children KEY_GLASS_1 and KEY_GLASS_3"
                /// </summary>
                [Fact]
                public PreferenceDataStructureEntity ACreate_Simple_ModelGDS()
                {
                    return Create_Simple_ModelGDS();
                }

                /// <summary>
                // Three Materials, 1000 glasses some of them with an associated material.
                // Logic relationship as follows:" 
                // Model has 1000 children, all the glasses
                /// </summary>
                [Fact]
                public PreferenceDataStructureEntity ACreate_Exahustive_ModelGDS()
                {
                    return Create_Exahustive_ModelGDS();
                }

                [Fact]
                public void Model_Order_Children_By_Attribute_Order_LINQ()
                {
                    entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(Gds.GetKeyEntity());
                    Console.WriteLine("Childs order by the attribute order - LINQ: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {

                        var result = from atom in entity.Children
                                     orderby atom.Attributes["Order"]
                                     select atom;
                        result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Print(result);

                }

                [Fact]
                public void Model_Order_Children_By_Attribute_Order_CODE()
                {
                    entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(entity.Key);
                    Console.WriteLine("Childs order by the attribute order - CODE: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        entity.ChildrenOrder("Order");
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Print(Gds.Model.Children);
                }

                [Fact]
                public void Model_Search_Children_With_A_Given_Attribute_Material_LINQ()
                {
                    entity = Create_Simple_ModelGDS();

                    // Sabemos que se puede consultar también por Object
                    Console.WriteLine("\nConsultar sobre un hijo que tiene un material especifico (String): ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    IEnumerable<Atom> result = null;
                    for (int i = 0; i < 1000; i++)
                    {
                        result = from atom in entity.Children
                                     where atom.Attributes.ContainsKey("Material") && "KEY_MATERIAL_1".Equals((string)atom.Attributes["Material"])
                                     select atom;
                        result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Print(result);

                    // Gds.Print();
                }

                [Fact]
                public void Model_Search_Children_That_Exists_LINQ()
                {
                    entity = Create_Simple_ModelGDS();

                    Console.WriteLine("\nConsultar sobre un hijo especifico que existe: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    IEnumerable<Atom> result = null;
                    for (int i = 0; i < 1000; i++)
                    {
                        result = from atom in entity
                                     where atom.Key == "KEY_GLASS_3"
                                     select atom;
                        result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    Print(result);

                }

                [Fact]
                public void Model_Search_Children_That_Exists_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Simple_ModelGDS();

                    Console.WriteLine("\nConsultar sobre un hijo especifico que existe: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        entity.Search("KEY_GLASS_3");
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Print(result);

                }

                [Fact]
                public void Model_Search_Children_That_No_Exists_LINQ()
                {
                    entity = Create_Simple_ModelGDS();


                    Console.WriteLine("\nConsultar sobre un hijo que no existe: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        var result = from atom in entity
                                     where atom.Key == "KEY_GLASS_2"
                                     select atom;
                        result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Assert.IsNotNull(result);

                }

                [Fact]
                public void Model_Search_Children_That_No_Exists_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Simple_ModelGDS();

                    Console.WriteLine("\nConsultar sobre un hijo que no existe: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        var result = entity.Search("KEY_GLASS_2");
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query:" + stopWatch.Elapsed.TotalMilliseconds);

                    // Assert.IsNotNull(result);

                }

                [Fact]
                public void Model_Query_In_Dictionary_Definitions_Retrieve_All_Materials_LINQ()
                {
                    PreferenceDataStructureEntity entity = Create_Simple_ModelGDS();

                    Console.WriteLine("\nQuerys in the first dictionary: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        var result = from atom in entity.Definitions
                                     where atom.Key == TypeDefinitions.Materials
                                     select atom;
                        result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    /*foreach (var c in result)
                    {
                        foreach (var item in c.Value)
                        {
                            Print(item.Value);
                        }
                    }*/
                }

                [Fact]
                public void Model_Query_In_Dictionary_Definitions_Retrieve_All_Materials_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Simple_ModelGDS();

                    Console.WriteLine("\nQuerys in the first dictionary: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        SortedList<string, Atom> result = entity.Definitions[TypeDefinitions.Materials];
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    /*foreach (var c in result)
                    {
                        Print(c.Value);
                    }*/
                }
            }
        }
    }
}
