using InventoryControl.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class NotificationFilter : IActionFilter
{
    private readonly INotifierService _notifierService;

    public NotificationFilter(INotifierService notifierService)
    {
        _notifierService = notifierService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Não faz nada antes da execução da ação
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

        if (_notifierService.HasNotifications())
        {
            var notifications = _notifierService.Notifications().Select(n => new { n.Key, n.Message }).ToList();

            context.Result = new BadRequestObjectResult(new { Errors = notifications });
        }
    }
}
