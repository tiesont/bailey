Bailey is a set of assemblies for common Castle Windsor components.

####Bailey.Web.Mvc
Bailey.Web.Mvc wraps the cookie-cutter implementations shown in the Castle Windsor docs into a drop-in package. [View the wiki](https://github.com/tiesont/bailey/wiki/Using-Bailey.Web.Mvc) for help setting up the Windsor components.

You can import this project, or use the [Bailey.Web.Mvc NuGet packge](https://www.nuget.org/packages/Bailey.Web.Mvc/):
```
PM> Install-Package Bailey.Web.Mvc
```

####Bailey.Web.Http
Bailey.Web.Http provides Castle components for wiring up WebApi projects, using code provided by [Mark Seemann](http://blog.ploeh.dk/2012/10/03/DependencyInjectioninASP.NETWebAPIwithCastleWindsor/). [View the wiki](https://github.com/tiesont/bailey/wiki/Using-Bailey.Web.Http) for help setting up the Windsor components.

You can import this project, or use the [Bailey.Web.Http NuGet packge](https://www.nuget.org/packages/Bailey.Web.Http/):
```
PM> Install-Package Bailey.Web.Http
```

####Bailey.Web.Security
Bailey.Web.Security is simply a stand-alone assembly based on [Mauricio Scheffer's blog bost](http://bugsquash.blogspot.com/2010/11/windsor-managed-membershipproviders.html).

You can import this project, or use the [Bailey.Web.Security NuGet packge](https://www.nuget.org/packages/Bailey.Web.Security/):
```
PM> Install-Package Bailey.Web.Security
```

In short, there is nothing special or ground-breaking in these packages. They're intended to save me (and you!) some key-strokes when stubbing out DI in new projects.
