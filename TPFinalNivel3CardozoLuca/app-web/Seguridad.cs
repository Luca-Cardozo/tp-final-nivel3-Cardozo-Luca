using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace app_web
{
    public static class Seguridad
    {
        public static bool sesionActiva(object usuario)
        {
            return usuario != null;
        }

        public static Usuario usuarioActual(HttpSessionState session)
        {
            return session["usuario"] as Usuario;
        }

        public static bool esAdmin(HttpSessionState session)
        {
            Usuario usuario = usuarioActual(session);
            return usuario != null && usuario.Admin;
        }
    }
}