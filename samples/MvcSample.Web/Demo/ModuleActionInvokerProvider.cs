using System;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;

namespace MvcSample.Web
{
    /// <summary>
    /// Summary description for ModuleActionInvokerProvider
    /// </summary>
    public class ModuleActionInvokerProvider : IActionInvokerProvider
    {
        private readonly IActionResultFactory _actionResultFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IControllerFactory _controllerFactory;
        private readonly IActionBindingContextProvider _bindingProvider;
        private readonly INestedProviderManager<FilterProviderContext> _filterProvider;

        public ModuleActionInvokerProvider(
            IActionResultFactory actionResultFactory,
            IControllerFactory controllerFactory,
            IActionBindingContextProvider bindingProvider,
            INestedProviderManager<FilterProviderContext> filterProvider,
            IServiceProvider serviceProvider)
        {
            _actionResultFactory = actionResultFactory;
            _controllerFactory = controllerFactory;
            _bindingProvider = bindingProvider;
            _filterProvider = filterProvider;
            _serviceProvider = serviceProvider;
        }


        public int Order { get { return 0; } }

        public void Invoke(ActionInvokerProviderContext context, Action callNext)
        {
            var actionDescriptor = context.ActionContext.ActionDescriptor as ModuleActionDescriptor;

            if (actionDescriptor != null)
            {
                context.Result = new ModuleActionInvoker(
                    context.ActionContext,
                    actionDescriptor,
                    _actionResultFactory,
                    _controllerFactory,
                    _bindingProvider,
                    _filterProvider);
            }

            callNext();
        }
    }
}