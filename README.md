# Description :
This is a project where I play around with websockets and crawling.
It is an ASP.NET Core app ( 3 tier architecture ), where user can measure website perfomance :
   1. User enters a URI;
   2. WebSocket connection starts;
   3. Server starts crawling website ( Breadth-first approach ): parses HTML pages for links of the same domain and collectis results, such as : download speed, page size;
   4. Server informs client about the process via websocket connection;
   5. After parsing all available links of the same domain ( or reaching some limit ), entry is saved into the database;
   

_Not every site folows [sitemap convention](https://en.wikipedia.org/wiki/Sitemaps) and holds /sitemap.xml in the root directory, so I have chosen manual parsing via AngleSharp package_
