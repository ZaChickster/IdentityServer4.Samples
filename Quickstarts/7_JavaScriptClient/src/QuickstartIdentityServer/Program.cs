// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;

namespace QuickstartIdentityServer
{
    public class Program
    {
		public static void Main(string[] args)
	    {
		    Console.Title = "IdentityServer4";

		    BuildWebHost(args).Run();
	    }

	    public static IWebHost BuildWebHost(string[] args)
	    {
		    return WebHost.CreateDefaultBuilder(args)
			    .UseStartup<Startup>()
			    .ConfigureLogging(builder =>
			    {
				    builder.ClearProviders();
			    })
			    .Build();
	    }
	}
}