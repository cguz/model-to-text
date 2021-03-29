/**
 * @file PrefeferenceDataStructureEntity.cs
 *
 * @brief This file contains the Data Structure of the Entity or Model
 *
 * @author Cesar Augusto Guzman ALvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.DataStructure.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    public enum TypeDefinitions
    {
        Materials = 0, Profiles = 1, Meters = 2,
        Glasses = 3, Pieces = 4, Colors = 5,
        ColorFamilies = 6, ProductionGroups = 7, CustomDimentionTypes = 8,
        Options = 9, LaborPlaces = 10, Symbols = 11,
        Volumes = 12
    }

    /** 
    * Represent the minimal component of a model.
    * We implement it as a Node. 
    * if we inherent from IEnumerable, we can use LINQ's Any.
    * Implemented as IEnumerable : https://codereview.stackexchange.com/questions/163601/iterating-an-array-of-nodes-each-of-which-can-have-children
    * 
    * V2. Dynamically add C# properties at runtime. 
    * Thus, we can have "var query = gm.Material.Imagen;" instead of 
    * "var query2 = entity.Elements[gm.Attributes["Material"]].Attributes["Color"];"
    * 
    * beware: the compiler will not be able to verify a lot of your dynamic calls and 
    * you won't get intellisense, so just keep that in mind.
    */
    public class Atom : DynamicObject, IEnumerable<Atom>
    {
        /**
        * To identify the Atom
        */
        public String Key { get; set; }

        /**
        * To indicate the parent of the Atom.
        * It can be another atom. 
        * Used to add logic relationship.
        * 
        * The Ancestors variable keeps a track of the previous Ancestors of the model
        */
        public Atom Parent { get; set; }
        public int Ancestors { get; set; }

        /**
        * Set of elements that form this Atom. 
        * logic relationship
        */
        public List<Atom> Children { get; private set; } = new List<Atom>();

        /**
        * Specific that the current Atom "copys" (inherents or extends) elements of other Atom. 
        * It could replace some properties of the extended Atom.
        * 
        * It has to be a reference to the atom. It means, a type Atom. 
        * The reason is that with the string key we do not have all the information to retrieve the attributes.
        * This is so because the elements are not in the class atom. They are in the class PrefGDSEntity that inherents from Atom.
        * Thus, any class Atom can not get the information of a copy.
        */
        public Atom CopyOf { get; private set; }

        /**
        * Set of attributes in the form <key, value>
        * 
        * attribute "order" specifies the order of the Atoms, when it is in the childs elements. 
        * In a list we can use the sort as follow:
        * 
        * childs.Sort((s1, s2) => s1.order.CompareTo(s2.order));
        * 
        * attribute "type" specifies the kind of Atom.
        * 
        * NOTE: We need to have clear what are the possible values of the key attribute.
        */
        public readonly Dictionary<String, Object> Attributes;

        public Atom(String _key = "")
        {
            Key = _key;
            Children = new List<Atom>(4);
            Attributes = new Dictionary<string, Object>();
        }

        public Atom(String _key, Dictionary<string, Object> _attributes) : this(_key)
        {
            if (_attributes == null)
                _attributes = new Dictionary<string, object>();
            Attributes = _attributes;
        }

        public Atom(string _key, Dictionary<string, Object> _attributes, Atom atom2) : this(_key, _attributes)
        {
            this.CopyOf = atom2;
        }


        /**
        * Add a new attribute
        * @param string key of the attribute
        * @param content value of the attribute
        */
        public void AddAttribute(string key, object value)
        {
            try
            {
                Attributes.Add(key, value);
            }
            catch (Exception) { }

        }

        /**
        * Add a list of attributes
        * 
        * If the attribute exists we update it
        * 
        * @param list of attributes
        */
        public void AddAttribute(Dictionary<string, object> attris)
        {
            foreach (KeyValuePair<string, object> entry in attris)
            {
                if (Attributes.ContainsKey(entry.Key))
                {
                    Attributes[entry.Key] = entry.Value;
                }
                else
                {
                    AddAttribute(entry.Key, entry.Value);
                }
            }
        }

        /**
        * Add a logic relationship between the current element and a new element
        * 
        * @param child atom
        */
        public void AddChildren(Atom atom)
        {
            // add the children if the atom is not empty
            if (atom != null)
            {
                Children.Add(atom);
                atom.Parent = this;
                atom.Ancestors = Ancestors + 1;
            }
        }

        /**
        * Create a new atom as a copy (inherent) of other atom. But we do not add it to the model
        * 
        * The general idea of the copyOf is not to repeat attributes.
        * 
        * @param key of the atom to create
        * @param atom from which the created atom inherents some information
        * @param Optionally it receives the attributes
        */
        public static Atom CreateAtom(string key, Atom atomCopyOf, Dictionary<string, object> attributes = null)
        {

            if (atomCopyOf == null)
                throw new NullReferenceException();

            // create the new Atom marking that is a "copy of" atomCopyOf
            return new Atom(key, attributes, atomCopyOf);

        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Attributes.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (Attributes.ContainsKey(binder.Name))
            {
                result = Attributes[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (Attributes.ContainsKey(binder.Name))
            {
                Attributes[binder.Name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
        * The next two methods are necessary to use LINQ
        */
        public IEnumerator<Atom> GetEnumerator() => Children.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();


        public int CountChildrenOfCopy()
        {

            if (CopyOf != null)
            {
                // Atom search = Search((om.CopyOf);
                return CopyOf.Children.Count;
            }

            return 0;

        }

        public bool IsCopyOf()
        {
            return CopyOf != null;
        }

        /**
        * Perform a basic preorder traversal to generate a list of the Atoms
        */
        public Atom[] GetList()
        {
            List<Atom> listAtoms = new();

            Stack<Atom> queue = new();
            queue.Push(this);

            while (queue.Count != 0)
            {
                Atom current = queue.Pop();
                listAtoms.Add(current);

                Atom[] childs = current.Children.ToArray();
                for (int i = 0; i < childs.Length; i++)
                {
                    queue.Push(childs[i]);
                }
            }

            return listAtoms.ToArray();
        }

        public void Print()
        {
            Console.WriteLine("\n Key: " + Key);
            Console.WriteLine(" Is Copy of other Atom: " + IsCopyOf());

            if (Parent != null)
            {
                Console.WriteLine(" Has parent: ");
                Console.WriteLine(" Total ancestors: " + Ancestors);
            }
            Console.WriteLine(" Attributes: ");

            foreach (var a in Attributes)
                Console.WriteLine($"  {a.Key}: {a.Value}");

            if (Children != null && Children.Count > 0)
            {
                Console.WriteLine(" Children (" + Children.Count + ") detected!: ");
                foreach (Atom c in Children)
                    c.Print();
                Console.WriteLine(" END.");
            }
        }
    }

    /**
    * It represents the model (entity) itself. 
    * Encapsulate the general definitions of the model. 
    */
    public class PreferenceDataStructureEntity : Atom
    {
        /**
        * Performs a match in O(1) from the type of definitions and the type of atom
        */
        private readonly string[] TypeDefinitionsToTypeAtom = new string[]{
            "Material", "Profiles", "Meters", "Glass",
            "Piece", "Color", "ColorFamily", "ProductionGroup",
            "CustomDimentionType", "Option", "LaborPlace", "Symbol",
            "Volume"
        };

        /**
            * Store all the definitions of elements.
            * It stores them by grouping
            */
        public Dictionary<TypeDefinitions, SortedList<string, Atom>> Definitions { get; set; }

        /**
        * Store all the elements with no grouping.
        * 
        * We can have two type of elements:
        *  - Elements inside the Definitions. It means they should have a TypeDefintions
        *  - Elements that are not in the definitions.
        */
        public SortedList<string, Atom> Elements { get; set; }

        /**
        * TODO
        * Enable / Disable the attribute CopyOf
        * Problem: 
        * - The received elements are not allways ordered and can have repetitions. 
        *   It requires to create the Element and searches if it exists or not. Time consumition. 
        * Solution:
        * - Elements should come with no repetition.
        * - To check whether it exists or not is only perform in the insertion with many benefits in future. 
        */
        public bool CheckForCopyOf { get; set; }


        public PreferenceDataStructureEntity(String key) : base(key)
        {
            Elements = new SortedList<string, Atom>();
            Definitions = new Dictionary<TypeDefinitions, SortedList<string, Atom>>();
            CheckForCopyOf = true;
        }


        public PreferenceDataStructureEntity(String key, Dictionary<string, Object> attributes) : base(key, attributes)
        {
        }

        /**
        * Retrieve a list of a given type of definition
        * 
        * @param type definitions
        */
        public Atom[] GetList(TypeDefinitions typeDefinition)
        {
            Definitions.TryGetValue(typeDefinition, out SortedList<string, Atom> value);
            return value.Values.ToArray();
        }

        /**
        * Add an Atom to the elements of the model
        * 
        * @param key attribute of the atom
        * @param atom to add to the model
        * 
        * NOTE: should we update the atom if it exists?
        */
        private void AddAtom(string key, Atom atom)
        {
            Elements.TryGetValue(key, out Atom newAtom);

            if (newAtom == null)
                Elements.Add(key, atom);

        }

        /**
        * Add an Atom to the elements of the model if it does not exists
        * 
        * It creates the Atom and add it only to the elements of the model. Not to the definitions
        * 
        * @param key attribute of the atom
        * 
        * NOTE: should we update key of the atom if it exists?
        */
        public Atom AddAtom(string key)
        {

            Elements.TryGetValue(key, out Atom newAtom);

            if (newAtom == null)
            {
                newAtom = new Atom(key);
                Elements.Add(key, newAtom);
            }

            return newAtom;

        }

        /**
        * Add a new Element in the definition and the model. 
        * We check if the list of elements exists, if not we create it
        * We check if the element exists, if not we create it
        */
        public Atom AddAtomDefinition(string key, TypeDefinitions type)
        {
            Definitions.TryGetValue(type, out SortedList<string, Atom> listAtom);

            // if the list of atoms is not created, we created it
            if (listAtom == null)
            {
                listAtom = new SortedList<string, Atom>();
                Definitions.Add(type, listAtom);
            }

            listAtom.TryGetValue(key, out Atom atom);

            // if the atom is not created, we created it
            if (atom == null)
            {
                atom = new Atom(key);
                atom.AddAttribute("typeDefinition", type);
                listAtom.Add(key, atom);

                // add atom in the elements
                AddAtom(key, atom);
            }

            return atom;
        }

        /**
        * Add a new Element with its attributes in the definition and the model. 
        * TODO
        * Here we use the attribute CopyOf whenever it is enable
        */
        public Atom AddAtomDefinition(string key, TypeDefinitions type, Dictionary<string, object> attributes)
        {
            // create the atom 
            Atom atomNew = AddAtomDefinition(key, type);

            if (CheckForCopyOf)
            {
                // if interserct the keys
                // bool check = keys.All(map.ContainsKey) && map.Keys.All(keys.Contains);
                // if(map.Keys.Intersect(keys).Count() == keys.Count())

                // add the attribute CopyOf

                // iterate through the keys and remove the keys with the same value

                throw new NotImplementedException();
            }

            // add the attributes
            atomNew.AddAttribute(attributes);

            return atomNew;

        }

        /**
        * Create a new atom as a copy (inherent) of other atom
        * The general idea of the copyOf is not to repeat attributes.
        */
        public dynamic CreateAtomAsCopyOf(string key, string keyCopyOf)
        {

            // search the second element
            Atom atom2 = Search(keyCopyOf);

            // create the atom as a copy of
            Atom atom = CreateAtom(key, atom2);

            // add the atom in elements
            AddAtom(key, atom);

            return atom;

        }

        /**
        * Mark a given element as copy of a second element
        * 
        * CHECK TODO
        */
        public void AddCopyOf(string key, string key2)
        {
            // search for key
            Atom atom = Search(key);

            Atom atom2 = Search(key2);

            if (atom == null)
                throw new NullReferenceException();

            // assign from which atom the created atom is a copy
            atom = new Atom(atom.Key, atom.Attributes, atom2);

        }

        /**
        * Retrieve the Element that match the given key value. 
        */
        public Atom Search(string key)
        {
            Atom atom = null;

            // since we do not known the type of the key, we can use the Elements to search it
            if (Elements.ContainsKey(key))
                atom = Elements[key];

            return atom;

        }

        /**
        * Add a logic relationship between two elements
        */
        public void AddChildren(string key1, string key2)
        {
            // search the atom 1
            Atom atom1 = Search(key1);

            // search the atom 2
            Atom atom2 = Search(key2);

            // add the children if the two atoms are not empty
            if (atom1 != null && atom2 != null)
            {
                atom1.AddChildren(atom2);
            }
        }

        /**
        * Add a logic relationship between the current element and e new element
        */
        public void AddChildren(String atom)
        {
            // search the atom 
            Atom atom1 = Search(atom);

            AddChildren(atom1);
        }

        public void ChildrenOrder(string Order)
        {
            Children.Sort((x, y) => Comparer<object>.Default.Compare(x.Attributes[Order], y.Attributes[Order]));
        }

        public Atom ChildrenSearchByAttribute(string key, object value)
        {
            int length = Children.Count;
            for (int i = 0; i < length; ++i)
            {
                Atom atom = Children[i];
                if (atom.Attributes[key].Equals(value))
                    return atom;
            }

            return null;
        }
    }
}