namespace Catalog.Domain.Entities
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreateAtUtc { get; private set; } 
        public DateTime? UpdateAtUtc { get; private set; }

        private Category() {
            Name = string.Empty;
        }

        public Category(string name, string? description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = true;
            CreateAtUtc = DateTime.UtcNow;
        }

    }
}
