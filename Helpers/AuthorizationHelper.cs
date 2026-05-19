using Microsoft.AspNetCore.Http;

namespace VehiclePartsManagementSystem.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool IsAdmin(HttpContext context)
        {
            return context.Session.GetString("UserRole")
                == "Admin";
        }

        public static bool IsStaff(HttpContext context)
        {
            var role =
                context.Session.GetString("UserRole");

            return role == "Staff" ||
                   role == "Admin";
        }

        public static bool IsLoggedIn(HttpContext context)
        {
            return !string.IsNullOrEmpty(
                context.Session.GetString("UserName"));
        }
    }
}