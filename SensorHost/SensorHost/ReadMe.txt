Install Instructions:

Copy the application to the server you want to monitor
Copy and sensors you want to monitor into the \Sensors folder
Modify the file SensorHost.Exe.Config 
- Change the BaseUrl to have the hosts fqdn in it. 
- Add in any configuration entries that your sensors required.
Ensure the host's firewall has an exception for the port you have configured the server to listen on.

Run the application in a command window to test it works.

If everything looks good ctrl+c the application, and run it again but with the argument Install. 
You will then see "PRTG Sensor Host" added to the list of services. You will need to start it for the first time.

You can see all the configuration options for the service here: http://docs.topshelf-project.com/en/latest/overview/commandline.html

