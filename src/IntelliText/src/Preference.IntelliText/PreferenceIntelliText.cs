/**
* @file PreferenceIntelliText.cs
*
* @brief This file contains the main class of the PreferenceIntelliText
*
* @author Cesar Augusto Guzman ALvarez
* Contact: cguzman@preference.es
*
*/
namespace Preference.IntelliText
{
    using System;
    using Preference.IntelliText.DataStructure;
    using Preference.IntelliText.DataProviders;
    using Preference.IntelliText.Render;
    using Preference.IntelliText.Template;
    using System.Globalization;

    /**
    * General class that performs the printout. 
    * It loads the template, comunicates with the automatic text generator, 
    * generates the information to be used by the render, and calls the render. 
    */
    class PreferenceIntelliText
    {
        /*
         * Settings configuration (for instance the template that we want to use)
         */
        private readonly TextSettings Settings;

        /*
         * Catalog Constructor to build a given template. 
         * It reads from a path the set of templates (catalog of templates), then we can build any of them.
         */
        private readonly ITemplateConstructor TemplateConstructor;

        /**
         * Contains all the information of the model (Price Doc Group inclussive). 
         * Data received, for instance, from PrefCAD. It has the Descriptive XML.
         */
        private readonly IObjectDataProvider Data;



        /*
         * Entity that represents a preference Scene. It can be a model, window or any piece. It is our data structure
         */
        private PreferenceDataStructure Model;

        /*
         * Render to printout text to a given format
         */
        private readonly IPreferenceRender Render;

        /**
         * Template to filter the information
         */
        private ITemplate Template;



        /**
         * IntelliText constructor. It receives:
         * 
         * @param Initial setting configuration
         */
        public PreferenceIntelliText(TextSettings settings)
        {
            // we have all the information in the settings
            Settings = settings;

            // create the template catalog constructor
            TemplateConstructor = new PreferenceTemplateConstructor(Settings);

            // store the object data provider
            Data = IntelliText.DataProviders.PreferenceDataProviderFactory.getDataProvider(settings.DataProvider, settings.PathDataProvider);

            // create the render
            Render = IntelliText.Render.PreferenceRenderFactory.getRender(settings.Render);

        }

        /**
         * Build the essential elements to be used later during the execution of the printout process
         */
        public void Build()
        {

            // 1: Generate the template. 

            // With the template we know all the elements of the model
            Template = BuildTemplate();


            /*
            * 2: Solicitamos el modelo
            *    - Lo suyo es indicarle que elementos del modelo necesitamos. Esta información estaría en las Regions del Template. 
            *      Donde se especifica que tipos de elementos se necesitan del modelo. De una manera abstracta.
            *      
            *    - Por simplicidad, y hasta que Carlos lo implemente solicito todo el modelo.
            *
            * NOTA 1: 
            * Este punto 2. y el 3. se pueden pasar al punto 4. Es decir, solicitar los identificadores justo cuando se vayan a usar. 
            * Opto por enviar todo el modelo y con el template filtrar sobre el modelo. 
            * Otra opción puede ser enviar como parámetro al PreferenceIntelliTextExecutor el IObjectDataProvider
            * 
            * NOTA 2:
            * Aunque es necesario tener la DataStructure porque cualquier otro sistema puede usar esta estructura. 
            * Debemos pensar en que sea el DataProvider quien la retorne y no generarla en el modulo de IntelliText. 
            */
            // string JSON = ...;
            Model = Data.GetObjectData(); // Data.GetObjectData(Template.getIdentifiers());


            // 3. Generate the GDS structure.
            // Model = new PreferenceGDS(JSON); 

        }

        /**
         * Generate the template
         */
        private ITemplate BuildTemplate()
        {

            // check if we have the key template
            if (string.IsNullOrEmpty(Settings.Template))
                throw new NullReferenceException("The key of the template is required");

            // build the template with the template catalog constructor
            return TemplateConstructor.Build(Settings.Template);

        }

        /**
         * Execute the printout process
         */
        public void Execute()
        {

            // 4. Create the IntelliText Executor
            PreferenceIntelliTextExecutor executor = new (Model, Template, Render, Settings);

            // execute the printout process
            executor.Run();

        }

    }
}