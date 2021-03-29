/**
 * @file PreferenceIntelliTextExecutor.cs
 *
 * @brief This file contains the class PreferenceIntelliTextExecutor
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText
{
    using Preference.IntelliText.DataStructure;
    using Preference.IntelliText.Render;
    using Preference.IntelliText.Template;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    class PreferenceIntelliTextExecutor
    {
        /*
         * Entity that represents a preference Scene. It can be a model, window or any piece. It is our data structure
         */
        private readonly PreferenceDataStructure Model;

        /*
         * Render to printout text to a given format
         */
        private readonly IPreferenceRender Render;

        /**
         * Template to filter the information
         */
        private readonly ITemplate Template;

        /**
         * Configuration of audience or language
         */
        private readonly TextSettings Settings;



        /**
         * Constructor of the class
         * 
         * @param model  Entity that represents a preference Scene. It can be a model, window or any piece. It is our data structure
         * @param template Template to filter the information
         * @param render printout text to a given format
         */
        public PreferenceIntelliTextExecutor(PreferenceDataStructure model, ITemplate template, IPreferenceRender render, TextSettings settings)
        {
            Model = model;
            Template = template;
            Render = render;
            Settings = settings;
        }

        public void Run()
        {

            // Algoritmo:

            // 4: Generamos de forma automática los textos de cada uno 
            // de los elementos de las estructuras que están especificadas en el Template.

            // El componente Automatic Text Generator, creo que tiene más sentido que esté incluido en el Template. 
            // De esta manera durante la creación se le puede indicar si se desea traducir una región especifica con un Automatic Text Generator X o Y.


            // 5. Los procesamos en una forma valida para el render. Teniendo en cuenta el orden para ser mostrados.
            IDescriptionModel modelText = Template.GetDescriptions(Model, Settings);


            // 6. Enviamos este último procesamiento al render. 
            // JSON - 1 y 2 - TEXTO.
            StoreFile(Render.Run(modelText));

        }

        /**
         * Store the rendered text to a file. Just used for testing purpouse, so we can execute different Theory of a same test.
         */
        private void StoreFile(string textFromRender)
        {
            int NumberOfRetries = 3;
            int DelayOnRetry = 1000;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {

                    // This text is added only once to the file.
                    // if (!File.Exists(Settings.SaveTo))
                    // {
                    // Create a file to write to.
                    File.WriteAllText(Settings.SaveTo, textFromRender, Encoding.Default);
                    // }
                    // else
                    // {
                    // File.AppendAllText(Settings.SaveTo, "\n" + textFromRender, Encoding.UTF8);
                    // }

                    break; // When done we can break loop
                }
                catch (IOException) when (i <= NumberOfRetries)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    Thread.Sleep(DelayOnRetry);
                }
            }
        }
    }
}
