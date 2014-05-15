using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;

namespace MvcSample.Web
{
    public class ModuleActionInvoker : FilterActionInvoker
    {
        private readonly ActionContext _actionContext;
        private readonly ModuleActionDescriptor _descriptor;
        private readonly IActionResultFactory _actionResultFactory;

        public ModuleActionInvoker(
            ActionContext actionContext,
            ModuleActionDescriptor descriptor,
            IActionResultFactory actionResultFactory,
            IControllerFactory controllerFactory,
            IActionBindingContextProvider bindingContextProvider,
            INestedProviderManager<FilterProviderContext> filterProvider)
            : base(actionContext, descriptor, actionResultFactory, controllerFactory, bindingContextProvider, filterProvider)
        {
            _actionContext = actionContext;
            _descriptor = descriptor;
            _actionResultFactory = actionResultFactory;
        }

        protected override Task<IActionResult> InvokeActionMethod(ActionExecutingContext context)
        {
            var module = (MvcModule)context.Controller;

            var action = module.Actions[_descriptor.Index];
            var result = action.Func();

            var actionResult = _actionResultFactory.CreateActionResult(
                typeof(object),
                result,
                _actionContext);
            return Task.FromResult(actionResult);
        }
    }
}