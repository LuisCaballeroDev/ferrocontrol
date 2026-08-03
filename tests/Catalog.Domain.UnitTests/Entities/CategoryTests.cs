using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.UnitTests.Entities
{
    public sealed class CategoryTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateActiveCategory()
        {
            // Arrange
            const string name = "Herramientas eléctricas";
            const string description = "Taladros, sierras y pulidoras";

            // Act
            var category = new Category(name, description);

            // Assert
            Assert.NotEqual(Guid.Empty, category.Id);
            Assert.Equal(name, category.Name);
            Assert.Equal(description, category.Description);
            Assert.True(category.IsActive); 
            Assert.NotEqual(default, category.CreatedAtUtc);
            Assert.Null(category.UpdatedAtUtc);
        }
        
        [Fact]
        public void Constructor_WithSpaces_ShouldTrimValues()
        {
            // Act
            var category = new Category(
                "  Herramientas eléctricas  ",
                "  Taladros y sierras  ");

            // Assert
            Assert.Equal("Herramientas eléctricas", category.Name);
            Assert.Equal("Taladros y sierras", category.Description);
        }


        [Fact]
        public void Constructor_WithEmptyDescription_ShouldSetDescriptionToNull()
        {
            // Act
            var category = new Category("Pintura", "  ");

            // Assert
            Assert.Null(category.Description);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowDomainException()
        {
            // Act
            var action = () => new Category(
                string.Empty, 
                "Descripción");

            // Assert
            var exception = Assert.Throws<DomainException>(action);
            Assert.Equal(
                "El nombre de la categoría es obligatorio.",
                exception.Message
             );
        }

        [Fact]
        public void Constructor_WithNameLongerThanMaximum_ShouldThrowDomainException()
        {
            // Arrange
            var name = new string('A', 101);

            // Act
            var action = () => new Category(name, null);
            
            // Assert
            var exception = Assert.Throws<DomainException>(action);
            Assert.Equal(
                "El nombre de la categoría no puede exceder 100 caracteres.",
                exception.Message
             );
        }

        [Fact]
        public void Constructor_WithDescriptionLongerThanMaximum_ShouldThrowDomainException()
        {
            // Arrange
            var description = new string('A', 501);

            // Act
            var action = () => new Category(
                "Plomería",
                description);

            // Assert
            var exception = Assert.Throws<DomainException>(action);

            Assert.Equal(
                "La descripción de la categoría no puede exceder 500 caracteres.",
                exception.Message);
        }


        [Fact]
        public void Update_WithValidData_ShouldUpdateCategory()
        {
            // Arrange
            var category = new Category(
                "Herramientas",
                "Descripción inicial");

            // Act
            category.Update(
                "Herramientas manuales",
                "Martillos, pinzas y llaves");

            // Assert
            Assert.Equal("Herramientas manuales", category.Name);
            Assert.Equal(
                "Martillos, pinzas y llaves",
                category.Description);

            Assert.NotNull(category.UpdatedAtUtc);
        }

        [Fact]
        public void Deactivate_WhenCategoryIsActive_ShouldDeactivateCategory()
        {
            // Arrange
            var category = new Category(
                "Material eléctrico",
                null);

            // Act
            category.Deactivate();

            // Assert
            Assert.False(category.IsActive);
            Assert.NotNull(category.UpdatedAtUtc);
        }

        [Fact]
        public void Activate_WhenCategoryIsInactive_ShouldActivateCategory()
        {
            // Arrange
            var category = new Category(
                "Tornillería",
                null);

            category.Deactivate();

            // Act
            category.Activate();

            // Assert
            Assert.True(category.IsActive);
            Assert.NotNull(category.UpdatedAtUtc);
        }

        [Fact]
        public void Deactivate_WhenCategoryIsAlreadyInactive_ShouldNotChangeUpdateDate()
        {
            // Arrange
            var category = new Category(
                "Pintura",
                null);

            category.Deactivate();

            var firstUpdateDate = category.UpdatedAtUtc;

            // Act
            category.Deactivate();

            // Assert
            Assert.False(category.IsActive);
            Assert.Equal(firstUpdateDate, category.UpdatedAtUtc);
        }

        [Fact]
        public void Activate_WhenCategoryIsAlreadyActive_ShouldNotSetUpdateDate()
        {
            // Arrange
            var category = new Category(
                "Plomería",
                null);

            // Act
            category.Activate();

            // Assert
            Assert.True(category.IsActive);
            Assert.Null(category.UpdatedAtUtc);
        }

    }
}
