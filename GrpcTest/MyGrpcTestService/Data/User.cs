namespace MyGrpcTestService.Data
{
    public interface IUser
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateBirth { get; set; }
        public float Money { get; set; }
    }
    public class User : IUser
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; } = null;
        public DateTime DateBirth { get; set; }
        public float Money { get; set; }
    }
}
