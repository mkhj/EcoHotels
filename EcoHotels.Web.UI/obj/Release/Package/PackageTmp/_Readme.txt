http://aboutcode.net/2011/03/21/caching-versioned-static-files-with-asp-net-mvc-and-iis7.html


	TODOs:

	Create a static web site, so JS, CSS an so and can be servered from another domain
		- Ensure that cookies are not getting send for this domain

	Build server with pre-compiled views



	Notes on Setting up the development enviroment
====================================================================

Requirements:

	- Visual Studio 2010 SP1
	- Asp.NET MVC 3
	- IIS Expres 8 ???

Setup IIS

	Add site to the IIS

		- dev.iloveecohotels.com
		- static.dev.iloveecohotels.com

	Change .NET Framework on the App Pool to 4.0

	Change Identity to LocalSystem for the App Pool (database related)

	Add the following lines to your host file C:\Windows\System32\drivers\etc

		127.0.0.1	dev.iloveecohotels.com
		127.0.0.1	static.dev.iloveecohotels.com

	Check that the Startup Url for the UI Projekt is set correctly. It Should say "Use Custom Web Server" and point to http://dev.iloveecohotels.com/



	Notes on Deployment
====================================================================

Automating Deployment with Microsoft Web Deploy
http://weblogs.asp.net/scottgu/archive/2010/09/13/automating-deployment-with-microsoft-web-deploy.aspx

Web.config transformation between envoriments
http://blogs.msdn.com/b/webdev/archive/2009/05/04/web-deployment-web-config-transformation.aspx


Elmah

System.Data.SQLite.dll is a mixed assembly, i.e. it contains both managed code and native code. 
Therefore a particular System.Data.SQLite.dll is either x86 or x64, but never both.


TODO:
	- Setup Pre-compiling


	Notes on Caching (with nHibernate)
====================================================================

http://weblogs.asp.net/scottgu/archive/2009/09/15/auto-start-asp-net-applications-vs-2010-and-net-4-0-series.aspx

http://www.danielansari.com/wordpress/2010/02/nhibernate-caching-solution-for-web-farms/


FYI:
http://stackoverflow.com/questions/8761249/how-do-i-make-nhibernate-cache-fetched-child-collections



	Notes on Performance Counters
====================================================================

http://miniprofiler.com/

http://haacked.com/archive/2008/07/02/httpmodule-for-timing-requests.aspx

http://stackoverflow.com/questions/2688234/count-number-of-queries-executed-by-nhibernate-in-a-unit-test
https://access.redhat.com/knowledge/docs/en-US/JBoss_Operations_Network/3.1/html/Dev_Complete_Resource_Reference/Hibernate-Hibernate_Statistics.html





	Notes on Elmah MVC
====================================================================

http://www.beletsky.net/search/label/ELMAH.MVC

http://code.google.com/p/elmah/wiki/MVC



	Notes on WebFarming
====================================================================

Introducing the Microsoft Web Farm Framework
http://weblogs.asp.net/scottgu/archive/2010/09/08/introducing-the-microsoft-web-farm-framework.aspx



http://www.hanselman.com/blog/LoadBalancingAndASPNET.aspx

AntiforgeryToken
MachineKey should be the same for every application in the farm

Request context
Any request that hits a web farm gets served by an available IIS server. Context gets created there and the whole request gets served by the same server. So context shouldn't be a problem. A request is a stateless execution pipeline so it doesn't need to share data with other servers in any way shape or form. It will be served from the beginning to the end by the same machine.
User information is read from a cookie and processed by the server that serves the request. It depends then if you cache complete user object somewhere.

Session
If you use TempData dictionary you should be aware that it's stored inside Session dictionary. In a server farm that means you should use other means that InProc sessions, because they're not shared between IIS servers across the farm. You should configure other session managers that either use a DB or others (State server etc.).

Cache
When it comes to cache it's a different story. To make it as efficient as possible cache should as well be served. By default it's not. But looking at cache it barely means that when there's no cache it should be read and stored in cache. So if a particular server farm server doesn't have some cache object it would create it. In time all of them would cache some shared publicly used data.
Or... You could use libraries like memcached (as you mentioned it) and take advantage of shared cache. There are several examples on the net how to use it. This is just one.

But these solutions all bring additional overhead of several things (like network and third process processing and data fetching etc.) if nothing else. So default cache is the fastest and if you explicitly need shared cache then decide for one. Don't share cache unless really necessary.




	Notes on Architecture
====================================================================

http://ofps.oreilly.com/titles/9781449320317/_web_application_architecture.html