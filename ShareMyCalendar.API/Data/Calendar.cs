namespace ShareMyCalendar.API.Data
{
    public record Calendar(string Id, string UserId, string Name, List<Appointment> Appointments);
}