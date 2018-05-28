using System.Collections.Generic;
using System.Security.Claims;
using Mapa.Data;

namespace Mapa.Extensions
{
    public static class ObjectListExtensions
    {
		public static string GetAt(this IList<object> row, int index)
        {
            if (row == null)
            {
                throw new System.ArgumentNullException(nameof(row));
            }

            if (index < row.Count)
                return (string)row[index];
            return "";
        }
    }
    public static class IdentityExtensions
    {
        // public static string GetUserId(this ClaimsPrincipal principal)
        // {
        // 	if (principal == null)
        // 		throw new ArgumentNullException(nameof(principal));

        // 	return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // }

        public static bool IsAdmin(this ClaimsPrincipal principal) => principal.IsInRole(ApplicationRoles.Administrator);

        // public static int GetOrganizationId(this ClaimsPrincipal principal)
        // {
        // 	var claim = principal.FindFirst("OrganizationId");
        // 	return (claim != null) ? Convert.ToInt32(claim.Value) : 0;
        // }

        // public static string GetOrganizationName(this ClaimsPrincipal principal)
        // {
        // 	var claim = principal.FindFirst("OrganizationName");
        // 	return (claim != null) ? claim.Value : string.Empty;
        // }

        // public static string GetRoleName(this ClaimsPrincipal principal)
        // {
        // 	if (principal.IsAdmin()) return ApplicationRoles.Gestor;
        // 	if (principal.IsClient()) return ApplicationRoles.Gerente;
        // 	return ApplicationRoles.Analista;
        // }

    }

}