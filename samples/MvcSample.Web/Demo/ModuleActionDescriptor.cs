using System;
using Microsoft.AspNet.Mvc;

namespace MvcSample.Web
{
    public class ModuleActionDescriptor : ActionDescriptor
    {
        public int Index { get; set; }
    }
}