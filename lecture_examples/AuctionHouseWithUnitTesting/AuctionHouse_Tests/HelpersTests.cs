namespace AuctionHouse_Tests;

using AuctionHouse.Models;

public class HelpersTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ExtractFirstName_WithValidFullName_ReturnsFirstName()
    {
        // Arrange
        string fullName = "Johanna Klein";
        string expected = "Johanna";

        // Act
        string found = Helpers.ExtractFirstName(fullName);

        // Assert
        Assert.That(found, Is.EqualTo(expected));
    }

    [Test]
    public void ExtractFirstName_WithOnlyFirstName_ReturnsFirstName()
    {
        // Arrange
        string fullName = "Johanna";
        string expected = "Johanna";

        // Act
        string found = Helpers.ExtractFirstName(fullName);

        // Assert
        Assert.That(found, Is.EqualTo(expected));
    }

    [Test]
    public void ExtractFirstName_WithThreeNames_ReturnsFirstName()
    {
        // Arrange
        string fullName = "Johanna Klein Schwartz";
        string expected = "Johanna";

        // Act
        string found = Helpers.ExtractFirstName(fullName);

        // Assert
        Assert.That(found, Is.EqualTo(expected));
    }

    [Test]
    public void ExtractFirstName_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        string fullName = "";
        string expected = "";

        // Act
        string found = Helpers.ExtractFirstName(fullName);

        // Assert
        Assert.That(found, Is.EqualTo(expected));
    }

    [Test]
    public void ExtractFirstName_WithValidNameAndLeadingWhitespace_ReturnsFirstName()
    {
        // Arrange
        string fullName = "  Johanna Klein";
        string expected = "Johanna";

        // Act
        string found = Helpers.ExtractFirstName(fullName);

        // Assert
        Assert.That(found, Is.EqualTo(expected));
    }
}