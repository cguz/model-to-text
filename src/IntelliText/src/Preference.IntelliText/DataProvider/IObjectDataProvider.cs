using Preference.IntelliText.DataStructure;

namespace Preference.IntelliText.DataProviders
{
	///////////////////////////////////////////////////////////////////////
	// IObjectDataProvider
	//
	// Esta interfaz proporciona acceso a los datos que necesitamos de
	// los objetos a describir.
	//
	// Por ahora la dejo muy sencilla y mas adelante podemos añadir:
	//
	// - Filtrar mediante una lista de identificadores los datos requeridos
	// - Usar un timestamp para evitar obtener datos que tengamos en cache

	public interface IObjectDataProvider
	{
		// Devuelve los datos necesarios para describir un objeto en formato JSON.
		PreferenceDataStructure GetObjectData();

		// retrieve the necessary data to describe an entity in JSON format
		PreferenceDataStructure GetObjectData(string[] identifiers);
	}
}