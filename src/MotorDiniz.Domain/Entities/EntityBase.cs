namespace MotorDiniz.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }


        public void TouchUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}
