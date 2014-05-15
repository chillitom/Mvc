using System;

namespace MvcSample.Web
{
    public class DemoModule : MvcModule
    {
	    public DemoModule()
	    {
            Get["/foo"] = () => "Hello, world!";
	    }
    }
}