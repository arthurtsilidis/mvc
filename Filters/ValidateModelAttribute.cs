using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace ClientsExercise.Filters
{
    [ProducesResponseType(422)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Dictionary<string, string>();
                foreach (var key in context.ModelState.Keys)
                {
                    result.Add(key, String.Join(", ", context.ModelState[key].Errors.Select(p => p.ErrorMessage)));
                }

                context.Result = new ObjectResult(result) { StatusCode = 422 };
            }
        }
    }
}
