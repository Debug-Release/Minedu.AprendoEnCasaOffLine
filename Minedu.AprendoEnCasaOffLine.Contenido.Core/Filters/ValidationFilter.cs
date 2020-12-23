using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Release.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Before Controller
            if (!context.ModelState.IsValid)
            {
                var errorsModel = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
                /*
                var cr = new CommandResponse();
                var d = new Dictionary<string, string>();

                foreach (var (key, value) in errorsModel)
                {
                    foreach (var subError in value)
                    {
                        if (!d.ContainsKey(key))
                        {
                            d.Add(key, subError);
                        }
                    }
                }

                cr.Success = false;
                cr.Errors = d;
                cr.Msg = "Validaciones";
                cr.StatusCode = StatusCodes.Status400BadRequest;

                context.Result = new BadRequestObjectResult(cr);
                */
                var sr = new StatusResponse
                {
                    StatusCode = HttpStatusCodes.Status400BadRequest,
                    Success = false
                };
                var lst = new List<string>();

                foreach (var (key, value) in errorsModel)
                {
                    foreach (var subError in value)
                    {
                        if (!lst.Contains(key))
                        {
                            lst.Add(subError);
                        }
                    }
                }
                sr.Messages.AddRange(lst);
                context.Result = new BadRequestObjectResult(sr);

                return;
            }

            await next();

            //After controller
        }
    }
}
