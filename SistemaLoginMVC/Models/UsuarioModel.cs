namespace SistemaLoginMVC.Models
{
    // Representa el registro completo que viene de SQL Server
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    // Representa únicamente los datos que el usuario escribe en el formulario de Login
    public class LoginViewModel
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}