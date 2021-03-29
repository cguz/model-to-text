using Moq;
using Preference.IntelliText;
using Preference.IntelliText.Template;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Preference.IntelliText.Test
{
	// Esta ´clase es una prueba generada mediante la herramienta 'Unit Test Boilerplate Generator'
	public class PreferenceTemplateConstructorTests
	{
		private MockRepository mockRepository;

		private ITestOutputHelper output;

		public PreferenceTemplateConstructorTests(ITestOutputHelper output)
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);
			this.output = output;
		}

		private PreferenceTemplateConstructor CreateTemplateCatalogConstructor()
		{
			// var CurrentDirectory = Directory.GetCurrentDirectory();

			TextSettings setting = new TextSettings { 
				
				PathTemplates = Path.GetFullPath("./Catalogs/Company/"),
				TextGeneratorProvider = "FILE"
			};
			return new PreferenceTemplateConstructor(setting);
		}

		[Fact]
		public void ReadTemplateFolder_Test()
		{
			// Arrange
			var provider = this.CreateTemplateCatalogConstructor();

			// Act
			string idTemplate = "WB-OneDescription-UseCase1";
			var result = provider.Build(idTemplate);

			// Assert
			Assert.NotNull(result);

			// Act
			idTemplate = "WB-SeveralDescriptions-UseCase2";
			result = provider.Build(idTemplate);

			// Assert
			Assert.NotNull(result);

			this.mockRepository.VerifyAll();
		}
	}
}
