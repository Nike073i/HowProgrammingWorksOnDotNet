using Unit = System.ValueTuple;

namespace HowProgrammingWorksOnDotNet.WebApi.V.Domain
{
    public record CarCreatedEvent(
        Guid CarId,
        string Model,
        string Manufacturer,
        string LicensePlate,
        bool IsAvailable
    );

    public record CreateCarPayload(string Model, string Manufacturer, string LicensePlate);

    public record CarUnavailableEvent(Guid CarId);

    public record MarkAsUnavailableState(Guid CarId);

    public static class CarDecider
    {
        public static CarCreatedEvent Create(Unit state, CreateCarPayload payload)
        {
            var id = Guid.NewGuid();
            return new(id, payload.Model, payload.Manufacturer, payload.LicensePlate, false);
        }

        public static CarUnavailableEvent MarkAsUnavailable(
            MarkAsUnavailableState state,
            Unit payload
        ) => new(state.CarId);
    }

    public enum RentalStatus
    {
        Created,
        Paid,
        Confirmed,
        Cancelled,
    }

    public record RentalCreatedEvent(
        Guid RentalId,
        Guid CarId,
        DateTime StartDate,
        DateTime EndDate,
        RentalStatus Status
    );

    public record CreateRentalPayload(DateTime Start, int Days);

    public record CreateRentalState(Guid CarId, bool IsCarAvailable, bool IsAvailableForPeriod);

    public record RentalCancelledEvent(Guid RentalId);

    public record CancelRentalState(Guid RentalId, RentalStatus Status);

    public static class RentalDecider
    {
        public static RentalCreatedEvent Create(
            CreateRentalState state,
            CreateRentalPayload payload
        )
        {
            if (!state.IsCarAvailable || !state.IsAvailableForPeriod)
                throw new Exception();
            var id = Guid.NewGuid();
            var endDate = payload.Start.AddDays(payload.Days);
            return new RentalCreatedEvent(
                id,
                state.CarId,
                payload.Start,
                endDate,
                RentalStatus.Created
            );
        }

        public static RentalCancelledEvent Cancel(CancelRentalState state, Unit payload)
        {
            if (state.Status == RentalStatus.Cancelled || state.Status == RentalStatus.Confirmed)
                throw new Exception();
            return new RentalCancelledEvent(state.RentalId);
        }
    }

    // TODO: пупупу... Событийная проблема. Car -> Rental, Rental -> Car... 
    // TODO: Да разделение на State/Payload не совсем адекватное
    // public static class CarRentalProjection
    // {
    //     public static void OnCarUnavailable(CarUnavailableEvent @event, )
    // }
}
