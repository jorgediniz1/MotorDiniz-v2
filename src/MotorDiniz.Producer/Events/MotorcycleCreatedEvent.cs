namespace MotorDiniz.Producer.Events
{
    public sealed record MotorcycleCreatedEvent(
        string Identifier,
        int Year,
        string Model,
        string Plate
    )
    {
        public const string EventType = "motorcycle.created";
    }
}
