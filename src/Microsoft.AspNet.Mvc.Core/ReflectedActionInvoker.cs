// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc
{
    public class ReflectedActionInvoker : FilterActionInvoker
    {
        private readonly ActionContext _actionContext;
        private readonly ReflectedActionDescriptor _descriptor;
        private readonly IActionResultFactory _actionResultFactory;

        public ReflectedActionInvoker([NotNull] ActionContext actionContext,
                                      [NotNull] ReflectedActionDescriptor descriptor,
                                      [NotNull] IActionResultFactory actionResultFactory,
                                      [NotNull] IControllerFactory controllerFactory,
                                      [NotNull] IActionBindingContextProvider bindingContextProvider,
                                      [NotNull] INestedProviderManager<FilterProviderContext> filterProvider)
            : base(actionContext, descriptor, actionResultFactory, controllerFactory, bindingContextProvider, filterProvider)
        {
            _actionContext = actionContext;
            _descriptor = descriptor;
            _actionResultFactory = actionResultFactory;

            if (descriptor.MethodInfo == null)
            {
                throw new ArgumentException(
                    Resources.FormatPropertyOfTypeCannotBeNull(typeof(ReflectedActionDescriptor),
                                                               "MethodInfo"),
                    "descriptor");
            }
        }

        protected override async Task<IActionResult> InvokeActionMethod(ActionExecutingContext context)
        {
            var actionMethodInfo = _descriptor.MethodInfo;
            var actionReturnValue = await ReflectedActionExecutor.ExecuteAsync(
                actionMethodInfo,
                _actionContext.Controller,
                context.ActionArguments);

            var underlyingReturnType = TypeHelper.GetTaskInnerTypeOrNull(actionMethodInfo.ReturnType) ?? actionMethodInfo.ReturnType;
            var actionResult = _actionResultFactory.CreateActionResult(
                underlyingReturnType,
                actionReturnValue,
                _actionContext);
            return actionResult;
        }
    }
}
