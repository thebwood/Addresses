namespace Addresses.Domain.Models
{
    public class EntityModel
    {
        public EntityModel()
        {
        }
        public EntityModel(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }

    }
}
