# Synopsis
This project is supposed to isolate the contracts established by the third party in charge of SimpleMaps

# History
A smell immediately smacked me in the face during a unit test setup. 

I was trying to isolate resource mapping details from bleeding into other layers. ```internal``` was an appropriate access modifier since I did not want details about the map to be accessible to any other project.

However, I was finding myself creating a SimpleMapJsonFileResource.

The UI and business doesn't need to know we are using simple maps.

Our main program setup still needs to know we are using simple maps.