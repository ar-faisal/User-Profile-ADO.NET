using System.Linq;
using System.Web.Mvc;
using System.Web;
using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class AuthorizeUserAttribute : AuthorizeAttribute
{
    private string[] requiredRoleNames;

    public AuthorizeUserAttribute()
    {
        // Default constructor for cases where no specific RoleNames are required
        requiredRoleNames = new string[0];
    }

    public AuthorizeUserAttribute(params string[] roleNames)
    {
        // Constructor with specific RoleNames requirement
        requiredRoleNames = roleNames ?? new string[0];
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var userId = httpContext.Session["UserId"];
        var userRoleName = httpContext.Session["RoleName"] as string;

        // Check if the user is authenticated
        if (userId != null && !string.IsNullOrEmpty(userRoleName))
        {
            // If no specific role names are required, allow access to any action
            if (requiredRoleNames.Length == 0)
            {
                return true;
            }

            // If the user has any of the required RoleNames, allow access to the action
            if (requiredRoleNames.Any(role => string.Equals(userRoleName, role, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }

        return false; // Disallow access by default
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectResult("~/Account/Login");
    }
}
