namespace BilleteraApp.Dtos
{
    public class RegisterDto
    {

        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

    }
}
