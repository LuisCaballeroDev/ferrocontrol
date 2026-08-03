using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities
{
    public sealed class Category
    {
        private const int MaximumNameLength = 100;
        private const int MaximumDescriptionLength = 500;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAtUtc { get; private set; } 
        public DateTime? UpdatedAtUtc { get; private set; }

        private Category() {
            Name = string.Empty;
        }

        public Category(string name, string? description)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetDescription(description);
            IsActive = true;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Update(string name, string? description)
        {
            SetName(name);
            SetDescription(description);
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if(!IsActive)
            {
                return;
            }

            IsActive = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        private void SetName(string name) {
            if (string.IsNullOrEmpty(name))
            {
                throw new DomainException(
                    "El nombre de la categoría es obligatorio.");
            }

            var normalizedName = name.Trim();
            if(normalizedName.Length > MaximumNameLength)
            {
                throw new DomainException(
                    $"El nombre de la categoría no puede exceder {MaximumNameLength} caracteres.");
            }

            Name = normalizedName;

        }

        private void SetDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Description = null;
                return;
            }

            var normalizedDescription = description.Trim();
            if(normalizedDescription.Length > MaximumDescriptionLength)
            {
                throw new DomainException(
                    $"La descripción de la categoría no puede exceder {MaximumDescriptionLength} caracteres.");
            }
            Description = normalizedDescription;
        }
    }
}