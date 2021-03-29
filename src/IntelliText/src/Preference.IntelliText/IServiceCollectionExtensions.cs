using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preference.IntelliText
{
    ////////////////////////////////////////////////////////////////////////////////
    // IServiceCollectionExtensions
    //
    // Configuración del Contenedor DI.
    //
    // Aquí se registran los servicios del módulo IntelliText.

    public static class IServiceCollectionExtensions
    {
        
        public static IServiceCollection AddIntelliTextServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
