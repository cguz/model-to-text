using Preference.IntelliText.DataStructure;
using Preference.IntelliText.DataStructure.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Preference.IntelliText.DataProviders
{
	///////////////////////////////////////////////////////////
	// FileDataProvider
	//
	// Implementación de la interfaz IObjectDataProvider que usa
	// el contenido de un fichero de texto.

	class PreferenceFileDataProvider : IObjectDataProvider
	{
        const string KEY_XML_MODEL = "dsc:Model";
        const string KEY_XML_MATERIAL = "dsc:Material";
        const string KEY_XML_PRICEDOCUMENTATION = "PriceDocumentation";
        const string KEY_XML_ITEM = "Item";
        const string KEY_XML_ENDID_GROUP = "EndIdGroup";
        const string KEY_XML_NAME = "name";
        const string KEY_XML_PREFSUITEITEM = "prefSuiteItem";
        const string KEY_XML_REF = "ref";
        const string KEY_XML_TYPE_ATOM = "TypeAtom";

        public string Path { get; }

        public PreferenceDataStructure Data { get; }


        public PreferenceFileDataProvider(string path)
		{
			Path = path;

            Data = ReadData();

		}

        private PreferenceDataStructure ReadData()
        {
            string xmlDescriptive = Path;

            // read the xml
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(File.ReadAllText(xmlDescriptive));

            PreferenceDataStructureEntity entity = null;

            PreferenceDataStructure Data = new PreferenceDataStructure();

            // Get the information about the model
            XmlNodeList xmlNodeList = XmlDoc.DocumentElement.GetElementsByTagName(KEY_XML_MODEL);

            // Create the model with its attributes
            Create_Model_And_Attributes(xmlNodeList, ref entity, ref Data);

            // Create the definitions
            Create_Definitions(XmlDoc, TypeDefinitions.Materials, KEY_XML_MATERIAL, ref entity);

            Create_Atoms(XmlDoc, KEY_XML_PRICEDOCUMENTATION, KEY_XML_ITEM, ref entity, KEY_XML_ENDID_GROUP);

            return Data;

        }

        private void Create_Model_And_Attributes(XmlNodeList xmlNodeList, ref PreferenceDataStructureEntity entity, ref PreferenceDataStructure Gds)
        {
            // for each model
            foreach (XmlNode node in xmlNodeList)
            {

                // create the model with its name and the type of model
                entity = Gds.Create(node.Attributes[KEY_XML_NAME].Value, node.Attributes[KEY_XML_PREFSUITEITEM].Value);

                // for each attribute
                foreach (XmlAttribute atr in node.Attributes)
                {
                    // get the name of the attribute and its value
                    string key = atr.Name;

                    // if the attribute is the name and prefSuiteItem we skip them since they were previously stored
                    if (key.Equals(KEY_XML_NAME) || key.Equals(KEY_XML_PREFSUITEITEM)) 
                        continue;

                    // store the attribute
                    entity.AddAttribute(key, atr.Value);
                }
            }
        }

        private void Create_Definitions(XmlDocument XmlDoc, TypeDefinitions typeDefinition, string tagName, ref PreferenceDataStructureEntity entity)
        {
            XmlNodeList xmlNodeList = XmlDoc.DocumentElement.GetElementsByTagName(tagName);

            // for each element in xmlNodeList
            foreach (XmlNode node in xmlNodeList)
            {

                // create the atom with all its attribute
                Atom atom = entity.AddAtomDefinition(node.Attributes[KEY_XML_REF].Value, typeDefinition);

                // create the attributes of the atom
                foreach (XmlAttribute atr in node.Attributes)
                {
                    // get the name of the attribute and its value
                    string key = atr.Name;
                    string value = atr.Value;

                    // if the attribute is the ref we skip it since it is the name previously stored
                    if (key == KEY_XML_REF) // || value.Length == 0)
                        continue;

                    if (key.Equals("type"))
                        key = "Type";

                    // store the attribute
                    atom.AddAttribute(key, value);

                }
            }
        }

        private void Create_Atoms(XmlDocument XmlDoc, string type, string tagName, ref PreferenceDataStructureEntity entity, string id = "id")
        {
            XmlNodeList xmlNodeList = XmlDoc.DocumentElement.GetElementsByTagName(tagName);

            // for each element in xmlNodeList
            foreach (XmlNode node in xmlNodeList)
            {
                // create the atom with all its attribute
                Atom atom = entity.AddAtom(node.Attributes[id].Value);
                atom.AddAttribute(KEY_XML_TYPE_ATOM, type);

                // create the attributes of the atom
                foreach (XmlAttribute atr in node.Attributes)
                {
                    // get the name of the attribute and its value
                    string key = atr.Name;
                    string value = atr.Value;

                    // if the attribute is the ref we skip it since it is the name previously stored
                    if (key == id) // || value.Length == 0)
                        continue;

                    if (key.Equals("type"))
                        key = "Type";

                    // store the attribute
                    atom.AddAttribute(key, value);

                }

                entity.AddChildren(atom);
            }
        }


        public PreferenceDataStructure GetObjectData(string[] identificators)
        {

            // falta filtrar la información acorde a los identificators
            throw new NotImplementedException();

        }

        public PreferenceDataStructure GetObjectData()
        {
            return Data;
        }
    }
}