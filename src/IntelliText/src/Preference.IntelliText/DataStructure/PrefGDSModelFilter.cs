/**
 * @file PrefGDSModelFilter.cs
 *
 * @brief This file filters elements from the GDS Model. Filter Design Pattern
 * 
 * TODO - Require some improve
 *
 * @author Cesar Augusto Guzman ALvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.GDS.Model
{
    using System;
    using System.Collections.Generic;

    /**
        * 
        * Filters the information inside a model
        * 
        * Employs the Criteria or Filter Design Pattern 
        * 
        * */
    public class PrefGDSModelFilter
    {
        /**
            * List of criteria to apply
            */
        public List<ICriteria> Criterias { get; set; }

        public PrefGDSModelFilter()
        {
            Criterias = new List<ICriteria>();
        }

        /**
            * Add a new criteria
            */
        public void AddCriteria(ICriteria criteria)
        {
            Criterias.Add(criteria);
        }

        /**
            * Apply the filter to one element and all its logic relationships
            */
        public Atom[] Filter(Atom model)
        {
            if (Criterias.Count == 0)
                throw new OperationCanceledException("The criteria can not be empty");

            List<Atom> nodes = new List<Atom>();

            Stack<Atom> queue = new Stack<Atom>();
            queue.Push(model);

            while (queue.Count != 0)
            {
                Atom current = queue.Pop();

                // we check the criterias
                if(MatchCriteria(current) == true)
                    nodes.Add(current);

                // for each logic succesor
                Atom[] childs = current.Children.ToArray();
                for (int i = 0; i < childs.Length; ++i)
                {
                    if (childs[i] == null)
                        throw new OperationCanceledException("Please check your model, there is a child that is empty!. The parent of the child is: "+ current.Key);

                    // we add the child to the queue to be evaluated
                    queue.Push(childs[i]);
                }
            }                    

            if (nodes != null)
                return nodes.ToArray();
            return null;

        }

        /**
            * Check if the Element matchs the criterias
            */
        private bool MatchCriteria(Atom current)
        {

            Atom[] evaluate = { current };

            foreach (ICriteria c in Criterias)
            {
                evaluate = c.Filter(evaluate);

                if (evaluate.Length == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /**
            * Apply the filter to a list of elements
            */
        public Atom[] Filter(Atom[] model)
        {

            if (Criterias.Count == 0)
                throw new OperationCanceledException("The criteria can not be empty");

            List<Atom> nodes = new List<Atom>(); 

            foreach (Atom element in model)
                if (MatchCriteria(element))
                    nodes.Add(element);
                    
            return nodes.ToArray();

        }
    }
}
