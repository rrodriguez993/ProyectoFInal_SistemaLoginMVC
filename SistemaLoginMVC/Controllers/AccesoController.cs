using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SistemaLoginMVC.Models;
using System.Security.Claims;

namespace SistemaLoginMVC.Controllers
{
    public class AccesoController : Controller
    {
        private readonly string _cadenaConexion;

        // Obtiene la cadena de conexión desde appsettings.json
        public AccesoController(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("ConexionSQL");
        }

        // GET: Carga la pantalla de Login
        [HttpGet]
        public IActionResult Login()
        {
            // Si el usuario ya tiene sesión activa, lo envía directo al Dashboard
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Recibe los datos del formulario y valida en SQL Server
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            UsuarioModel usuarioEncontrado = null;

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                string query = "SELECT Id, Nombre, Correo FROM Usuarios WHERE Correo = @Correo AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conexion);

                // Parámetros SQL para bloquear ataques de Inyección SQL
                cmd.Parameters.AddWithValue("@Correo", modelo.Correo ?? "");
                cmd.Parameters.AddWithValue("@Password", modelo.Password ?? "");

                await conexion.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuarioEncontrado = new UsuarioModel
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Correo = reader.GetString(2)
                        };
                    }
                }
            }

            if (usuarioEncontrado != null)
            {
                // Se guardan los datos del usuario (Claims) dentro de la cookie de sesión
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioEncontrado.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
                    new Claim(ClaimTypes.Email, usuarioEncontrado.Correo)
                };

                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MensajeError"] = "Correo o contraseña incorrectos. Inténtalo nuevamente.";
                return View();
            }
        }

        // Cierra la sesión destruyendo la cookie y vuelve al Login
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}