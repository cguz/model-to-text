using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preference.IntelliText
{
	////////////////////////////////////////////////////////////
	// Module
	//
	// Acceso a los servicios generales (Cross Cutting Concerns)
	//
	// No tengo claro que se vaya a usar esta clase para acceder a
	// los servicios generales, pero por ahora quizás sea util
	// para diagnosticar el funcionamiento de la inyección de dependencias
	// con las librerías de Microsoft.Extensions.DependencyInjection.

	class Module
	{
		Module(IConfiguration configuration, ILogger logger)
		{
			Configuration = configuration;
			Logger = logger;
		}

		public IConfiguration Configuration { get; private set; }
		public ILogger Logger { get; private set; }
	}
}
