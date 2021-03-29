using Moq;
using Preference.IntelliText;
using Preference.IntelliText.AutomaticTextGenerator;
using Preference.IntelliText.DataProviders;
using Preference.IntelliText.DataStructure;
using System;
using Xunit;

namespace Preference.IntelliText.Test
{
	public class TextGeneratorTests
	{
		private MockRepository Mocks { get; } = new MockRepository(MockBehavior.Strict);

		private IAutomaticTextGenerator CreateTextGenerator()
		{
			return PreferenceAutomaticTextGeneratorFactory.GetTextGenerator("DB");
		}

		[Fact]
		public void GetText_For_Empty_Data()
		{
			// Arrange
			var generator = CreateTextGenerator();
			var id = new ObjectId { Id = "", Type = "" };
			var settings = new TextSettings() { Audience = "Consumer"};

			var provider = Mocks.Create<IObjectDataProvider>();
			provider.Setup(p => p.GetObjectData()).Returns(new PreferenceDataStructure());

			// Act
			generator.GetText(id, settings);

			// Assert
			Mocks.VerifyAll();
		}
	}
}
