using Microsoft.AspNetCore.Mvc.Filters;

public class CheckSessionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var cookie = context.HttpContext.Request.Cookies["jwt"];
        if (cookie != null)
        {
            context.HttpContext.Session.SetString("IsLoggedIn", "true");
        }

        // var isLoggedIn = context.HttpContext.Session.GetString("IsLoggedIn");


        // if (string.IsNullOrEmpty(isLoggedIn) || isLoggedIn != "true")
        // {
        //     context.Result = new RedirectToActionResult("Login", "Account", null);
        // }
    }
}