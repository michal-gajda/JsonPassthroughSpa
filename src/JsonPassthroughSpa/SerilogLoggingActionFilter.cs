namespace JsonPassthroughSpa
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Serilog;

    public sealed class SerilogLoggingActionFilter : IActionFilter
    {
        private readonly IDiagnosticContext diagnosticContext;
        public SerilogLoggingActionFilter(IDiagnosticContext diagnosticContext)
        {
            this.diagnosticContext = diagnosticContext
                ?? throw new ArgumentNullException(nameof(diagnosticContext));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Contract.Assert(context != null);
            diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
            diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
            diagnosticContext.Set("ValidationState", context.ModelState.IsValid);
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
