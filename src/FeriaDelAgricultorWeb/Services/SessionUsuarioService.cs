using FeriaDelAgricultorModels;
using System;

namespace FeriaDelAgricultorWeb.Services
{
    public class SessionUsuarioService
    {
        public Usuario? UsuarioActual { get; private set; }
        public bool EstaLogueado => UsuarioActual != null;

        // ✅ Helpers de rol
        public bool EsAdmin => UsuarioActual?.TipoUsuario == TipoUsuario.Admin;
        public bool EsCliente => UsuarioActual?.TipoUsuario == TipoUsuario.Cliente;

        // ✅ Evento para refrescar UI (NavMenu / páginas)
        public event Action? OnChange;

        public void IniciarSesion(Usuario usuario)
        {
            UsuarioActual = usuario;
            NotificarCambio();
        }

        public void CerrarSesion()
        {
            UsuarioActual = null;
            NotificarCambio();
        }

        private void NotificarCambio()
        {
            OnChange?.Invoke();
        }
    }
}
