# MarsRoverPhotosAPI
This Project is built in the following Technical enviornment: 
1. Microsoft Visual Studio Community 2019 Version 16.7
2. .Net Core 3.1

**About:** This project is the consumer of external web API i.e. Mars Rover Photos API

This is a Web API which exposes the following three Web Methods:
1. GetPhotosByEarthDateAsync
2. GetPhotosAsync
3. GetAsync

### GetPhotosByEarthDateAsync
*This method fetches all the photos related data from the external API for a given date
*This method returns the fetched data in the form of JSON
*Relative API Path/End Point: /api/marsroverphotos/photos/{date} where date is string

### GetPhotosAsync
*Like, GetPhotosByEarthDateAsync, this method also fetches all the photos related data from the external API for a given date
*This method stores the fetched photos locally
**Relative API Path/End Point**: /api/marsroverphotos/v1/photos/{date} where date is string

### GetAsync
*This method fetches all the photos related data from the external API for a fix list of the dates that it reads one-by-one from "dates.txt" file
*This method stores the fetched photos locally
**Relative API Path/End Point**: /api/marsroverphotos/v1/photos where date is string

*Like, GetPhotosByEarthDateAsync, this method also fetches all the photos related data from the external API for a given date
*This method stores the fetched photos locally
**Relative API Path/End Point**: /api/marsroverphotos/v1/photos/{date} where date is string

**Remarks**:
a. The project is based upon "Development" Enviornment
b. Methods which store the photos locally go and fetch for the photos each time the request is made.
c. GetAsync()
>Proper handling of HTTPResponse Status Code is needed 
>Does not check for duplicate values in "dates.txt"

**To see the work in running mode:**
| Steps To Follow |
| ------ | 
| Download the solution |
| Build the project|
| Set this project as Startup Project |
| Run the solution |
| You should launch the default browser with URL ending with '/api/marsroverphotos' |
| Follow above mentioned **Relative API Path/End Point** of actions/web methods |
| To modify the list of the dates that **GetAsync()** works on, please go to wwwroot folder to find **dates.txt** file |

