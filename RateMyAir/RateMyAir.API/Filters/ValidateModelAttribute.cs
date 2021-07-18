using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RateMyAir.Entities.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RateMyAir.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.ErrorCount > 0)
            {
                List<Object> list = new List<Object>();

                var modelType = context.ActionDescriptor.Parameters
                    .FirstOrDefault(p => p.BindingInfo.BindingSource.Id.Equals("Body", StringComparison.InvariantCultureIgnoreCase))?.ParameterType; //Get model type

                foreach (var e in context.ModelState)
                {
                    PropertyInfo property = null;
                    if(modelType != null) property = modelType.GetProperties().FirstOrDefault(p => p.Name.Equals(e.Key, StringComparison.InvariantCultureIgnoreCase));

                    String propertyName = property != null ? property.Name : e.Key;
                    String displayName = propertyName;
                    if (property != null)
                    {
                        var displayNameAttributeValue = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault()?.DisplayName;
                        displayName = displayNameAttributeValue ?? displayName;
                    }

                    list.Add(new
                    {
                        Property = propertyName,
                        Display = displayName,
                        Errors = e.Value.Errors.Select(r => r.ErrorMessage).ToArray()
                    });
                }

                Response<string> apiResponse = new Response<string>
                {
                    Success = false,
                    Message = "One or more validation errors occurred.",
                    Errors = list
                };

                context.Result = new BadRequestObjectResult(apiResponse);
            }

            base.OnActionExecuting(context);
        }

    }
}
