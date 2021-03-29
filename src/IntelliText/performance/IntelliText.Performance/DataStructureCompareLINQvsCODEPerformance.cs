/**
 * @file PreferenceDataStructureCompareLINQvsCODEPerformance.cs
 *
 * @brief This file compares some operations between LINQ and CODE
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

            public class PreferenceDataStructureCompareLINQvsCODEPerformance : PreferenceDataStrcutureCommonPerformance
            {

                private readonly MockRepository mockRepository;

                public PreferenceDataStructureCompareLINQvsCODEPerformance()
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

                    atom = entity.AddAtomDefinition("KEY_PROFILE_0", TypeDefinitions.Profiles);
                    atom.AddAttribute("Description", "Description of the profile");
                    atom.AddAttribute("Order", rnd.Next(total));
                    atom.AddAttribute("Material", "KEY_MATERIAL_0");
                    atom.AddAttribute("Material_interior", "KEY_MATERIAL_1");
                    atom.AddAttribute("Material_exterior", "KEY_MATERIAL_2");
                    atom.AddAttribute("Longitud", 1000);
                    atom.AddAttribute("Others1", "Any other value");
                    atom.AddAttribute("Others2", "Any other value");
                    atom.AddAttribute("Others3", "Any other value");
                    atom.AddAttribute("Others4", "Any other value");
                    atom.AddAttribute("Others5", "Any other value");
                    entity.AddChildren(atom);


                    total = 10000;

                    // create the two more profiles
                    for (int i = 1; i < total; ++i)
                    {
                        atom = entity.CreateAtomAsCopyOf("KEY_PROFILE_" + i, "KEY_PROFILE_0");

                        atom.AddAttribute("Order", 51); //  rnd.Next(total));
                        atom.AddAttribute("Longitud", 200);

                        entity.AddChildren(atom);

                    }

                    return entity;
                }



                /// <summary>
                // Three Materials, 10000 profiles. One profile with associated materials.
                // Logic relationship as follows:" 
                // Model has 10000 children, all the profiles
                // Copy of:
                // 9999 profiles are copy of the first profile
                /// </summary>
                [Fact]
                public PreferenceDataStructureEntity ACreate_Exahustive_ModelGDS()
                {
                    return Create_Exahustive_ModelGDS();
                }

                [Fact]
                public void Model_Order_Children_By_Attribute_Order_LINQ()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(entity.Key);
                    Console.WriteLine("Childs order by the field order: ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        var result = from atom in entity.Children
                                     orderby atom.Attributes["Order"]
                                     select atom;
                        _ = result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Print(result);
                }

                [Fact]
                public void Model_Order_Children_By_Attribute_Order_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(entity.Key);
                    Console.WriteLine("Childs order by the field order: ");

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
                public void Model_Count_Children_With_Attribute_Order_Equal_To_X_LINQ()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(entity.Key);
                    Console.WriteLine("Count children with attribute order equals to 51 (LINQ): ");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        var result = from atom in entity.Children
                                     where ((int)atom.Attributes["Order"]) == 51
                                     select atom;
                        _ = result.ToList();
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Console.WriteLine(result.Count());
                }

                [Fact]
                public void Model_Count_Children_With_Attribute_Order_Equal_To_X_CODE()
                {
                    PreferenceDataStructureEntity entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(entity.Key);
                    Console.WriteLine("Count children with attribute order equals to 51 (CODE): ");

                    // time start
                    Stopwatch stopWatch = new ();
                    stopWatch.Start();

                    for (int i = 0; i < 1000; i++)
                    {
                        int count = 0;
                        int length = entity.Children.Count;
                        for (int j = 0; j < length; ++j)
                        {
                            if ((int) entity.Children[j].Attributes["Order"] == 51)
                                count++;
                        }
                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                    // Console.WriteLine(count);
                }

                /*
                public void Model_Count_Children_With_Attribute_Order_Equal_To_X_CRITERIA_PATTERN()
                {
                    PrefGDSEntity entity = Create_Exahustive_ModelGDS();

                    Console.WriteLine(entity.Key);
                    Console.WriteLine("Count children with attribute order equals to 51 (CODE): ");

                    // time start
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    PrefGDSModelFilter filter = new PrefGDSModelFilter();
                    filter.AddCriteria(new CriteriaByAttribute("Order", 51));

                    for (int i = 0; i < 1000; i++)
                    {
                        filter.Filter(entity.Children.ToArray());

                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);

                }*/


                // In normal list: 
                // to search for the first element, it is slower. 
                // to count elements it is similar
                // to order and shows elements it seems faster.

            }
        }
    }
}
