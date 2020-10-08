# MarsRoverPhotosWeb
This project is built in the following Technical enviornment: 
1. Microsoft Visual Studio Community 2019 Version 16.7.
2. .Net Core 3.1
3. Angular 8

**About:** This project is the consumer of internal web API which communicates with NASA API.
>The idea is to let this project get all the photos related data which internal Web API can brought after requesting NASA Web API. And it should be this project that not only displays those photos on the UI but it should also store those photos locally(ref. Test Excercise/Assignment)

**Remarks**:
* At of now, this project displays all the images of one hard-coded date.
* This project does not store the photos locally.
* Like it is done in **MarsRoverPhotosAPI**(server-side framework), in order to save a URL-Image locally, 'get' the memoryStreamBytes using the URL(URL/img-src of that image). Once that memoryStream is read, convert that memoryStream/blob(Binary Large Object) into a fileStream(a file on local file system)
*However, implementing this using a javascript framework became tricky.

**See the links below**:

* [Response to Preflight Request does not pass](https://stackoverflow.com/questions/35588699/response-to-preflight-request-doesnt-pass-access-control-check) - See the list Fix/Workarounds like do something in Chrome or using proxy like NginX etc.!
* [Error while Trying in Chrome](https://prnt.sc/uv7xap) - Screenshot!
* [Error while Trying in Edge](https://prnt.sc/uv7yom) - Screenshot!

**Notes**:
* The project is configured to run in "Development" Environment

**To see the work in running mode:**
| Steps To Follow |
| ------ | 
| Download the solution |
| Build the project|
| Set this project as Startup Project |
| Run the solution |
| You should see all the images captured by Mars Rover on 2017-02-27 |
| This date can be spotted/changed in ngOnInit() of photo-gallery.component.ts |