using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;

namespace ShopApi.Selectors;

public class WeekdaysOnlyAttribute : ActionMethodSelectorAttribute
{
    public override bool IsValidForRequest(RouteContext context, ActionDescriptor action)
    {
        var today = DateTime.Now.DayOfWeek;
        return today != DayOfWeek.Saturday && today != DayOfWeek.Sunday;
    }
}