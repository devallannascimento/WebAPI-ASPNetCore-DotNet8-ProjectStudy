﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.FIlters;

public class ApiLoggingFilter : IActionFilter
{

    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //Executa antes da Action
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("#########################################");
        _logger.LogInformation($"{DateTime.Now.ToString()}");
        _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
        _logger.LogInformation("#########################################");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //Executa depois da Action
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("#########################################");
        _logger.LogInformation($"{DateTime.Now.ToString()}");
        _logger.LogInformation($"StatusCode: {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("#########################################");

    }
}
