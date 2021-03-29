/**
 * @file PreferenceDataStructureStoreDescriptiveXMLInInternalStructureTests.cs
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
            using System.Diagnostics;
            using Preference.IntelliText.DataStructure;
            using Preference.IntelliText.DataStructure.Model;
            using Preference.IntelliText.DataStructure.DescriptiveXML;
            using System.IO;
            using System.Xml;
            using Moq;
            using Xunit;

            public class PreferenceDataStructureStoreDescriptiveXMLInInternalStructureTests : PreferenceDataStrcutureCommonPerformance
            {

                private MockRepository mockRepository;

                public PreferenceDataStructureStoreDescriptiveXMLInInternalStructureTests()
                {
                    this.mockRepository = new MockRepository(MockBehavior.Strict);
                }

                /// <summary>
                // Read the real XML Node and store the name
                // Dudas: 
                // - En el XML viene <dsc:Model prefSuiteItem="model" name="7001", que es prefSuiteItem?. 
                // R: Es la clasificación del objeto. Puede ser un model, u otra cosa. 
                /// </summary>
                [Fact]
                public void Store_Model_With_Name()
                {
                    PreferenceDataStructureEntity entity = null;

                    Gds = new PreferenceDataStructure();

                    // xml of example
                    String xml = Path.GetFullPath("./xml-descriptive/UseCase-ReadXML-Model3d.xml");

                    // class to read the descriptive xml
                    ReadDescriptiveXML descriptive = new(xml);

                    // get the information about the model
                    XmlNodeList xmlNodeList = descriptive.XmlDoc.DocumentElement.GetElementsByTagName("dsc:Model");

                    Xunit.Assert.NotNull(xmlNodeList);

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    // for each model
                    foreach (XmlNode node in xmlNodeList)
                    {
                        // create the model only with its name
                        entity = Gds.Create(node.Attributes["name"].Value, "model");
                        Assert.Equal(node.Attributes["name"].Value, Gds.GetKeyEntity());

                    }

                    stopWatch.Stop();
                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                    entity.Print();

                }


                /// <summary>
                // Read a real XML Node and store the name, and attributes
                /// </summary>
                [Fact]
                public void Store_Model_Attributes()
                {

                    Gds = new PreferenceDataStructure();

                    // xml of example
                    String xml = Path.GetFullPath("./xml-descriptive/UseCase-ReadXML-Model3d.xml");

                    // class to read the descriptive xml
                    ReadDescriptiveXML descriptive = new(xml);

                    // get the information about the model
                    XmlNodeList xmlNodeList = descriptive.XmlDoc.DocumentElement.GetElementsByTagName("dsc:Model");

                    Assert.NotNull(xmlNodeList);

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    // create the model with its attributes
                    Create_Model_And_Attributes(xmlNodeList);

                    stopWatch.Stop();

                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                    Gds.Print();

                }

                /// <summary>
                // Read a real XML Node and store the name, and attributes of the model. 
                // Store the materials
                /// </summary>
                [Fact]
                public void Store_Model_Material_Definitions()
                {
                    Gds = new PreferenceDataStructure();

                    // xml of example
                    String xml = Path.GetFullPath("./xml-descriptive/UseCase-ReadXML-Model3d.xml");

                    // class to read the descriptive xml
                    ReadDescriptiveXML descriptive = new(xml);

                    // get the information about the model
                    XmlNodeList xmlNodeList = descriptive.XmlDoc.DocumentElement.GetElementsByTagName("dsc:Model");

                    Assert.NotNull(xmlNodeList);

                    // create the model with its attributes
                    Create_Model_And_Attributes(xmlNodeList);

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    Create_Definitions(descriptive, TypeDefinitions.Materials, "dsc:Material");

                    stopWatch.Stop();

                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                    Gds.Print();

                }


                /// <summary>
                // Read a real XML Node and store the name, and attributes of the model. 
                // Store the materials and generated materials
                /// </summary>
                [Fact]
                public void Store_Model_Material_Definitions_Generated_Materials()
                {
                    Gds = new PreferenceDataStructure();

                    // xml of example
                    String xml = Path.GetFullPath("./xml-descriptive/UseCase-ReadXML-Model3d.xml");

                    // class to read the descriptive xml
                    ReadDescriptiveXML descriptive = new(xml);

                    // get the information about the model
                    XmlNodeList xmlNodeList = descriptive.XmlDoc.DocumentElement.GetElementsByTagName("dsc:Model");

                    Assert.NotNull(xmlNodeList);

                    Create_Model_And_Attributes(xmlNodeList);

                    Create_Definitions(descriptive, TypeDefinitions.Materials, "dsc:Material");

                    // time start
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    Create_Atoms(descriptive, "generatedMaterial", "dsc:GeneratedMaterial");

                    stopWatch.Stop();

                    Console.WriteLine("RunTime Query: " + stopWatch.Elapsed.TotalMilliseconds);
                    Gds.Print();

                }

                private void Create_Model_And_Attributes(XmlNodeList xmlNodeList)
                {
                    // for each model
                    foreach (XmlNode node in xmlNodeList)
                    {

                        // create the model with its name and the type of model
                        entity = Gds.Create(node.Attributes["name"].Value, node.Attributes["prefSuiteItem"].Value);

                        // for each attribute
                        foreach (XmlAttribute atr in node.Attributes)
                        {
                            // get the name of the attribute and its value
                            string key = atr.Name;
                            string value = atr.Value;

                            // if the attribute is the name and prefSuiteItem we skip them since they were previously stored
                            if (key.Equals("name") || key.Equals("prefSuiteItem")) // || value.Length == 0)
                                continue;

                            // store the attribute
                            entity.AddAttribute(key, value);
                            Assert.Equal(entity.Attributes[key], value);
                        }
                    }
                }

                private void Create_Definitions(ReadDescriptiveXML descriptive, TypeDefinitions typeDefinition, string tagName)
                {
                    XmlNodeList xmlNodeList = descriptive.XmlDoc.DocumentElement.GetElementsByTagName(tagName);

                    Assert.NotNull(xmlNodeList);

                    // for each element in xmlNodeList
                    foreach(XmlNode node in xmlNodeList) {

                        // create the atom with all its attribute
                        Atom atom = entity.AddAtomDefinition(node.Attributes["ref"].Value, typeDefinition);

                        // create the attributes of the atom
                        foreach(XmlAttribute atr in node.Attributes)
                        {
                            // get the name of the attribute and its value
                            string key = atr.Name;
                            string value = atr.Value;

                            // if the attribute is the ref we skip it since it is the name previously stored
                            if (key == "ref") // || value.Length == 0)
                                continue;

                            // store the attribute
                            atom.AddAttribute(key, value);
                            Assert.Equal(atom.Attributes[key], value);

                        }
                    }
                }


                private void Create_Atoms(ReadDescriptiveXML descriptive, string type, string tagName)
                {
                    XmlNodeList xmlNodeList = descriptive.XmlDoc.DocumentElement.GetElementsByTagName(tagName);

                    Assert.NotNull(xmlNodeList);

                    // for each element in xmlNodeList
                    foreach (XmlNode node in xmlNodeList)
                    {
                        // create the atom with all its attribute
                        Atom atom = entity.AddAtom(node.Attributes["id"].Value);
                        atom.AddAttribute("type", type);

                        // create the attributes of the atom
                        foreach (XmlAttribute atr in node.Attributes)
                        {
                            // get the name of the attribute and its value
                            string key = atr.Name;
                            string value = atr.Value;

                            // if the attribute is the ref we skip it since it is the name previously stored
                            if (key == "id") // || value.Length == 0)
                                continue;

                            // store the attribute
                            atom.AddAttribute(key, value);
                            Assert.Equal(atom.Attributes[key], value);

                        }

                        entity.AddChildren(atom);
                    }
                }
            }
        }
    }
}