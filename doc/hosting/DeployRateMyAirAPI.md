# Deploy and run RateMyAir_API

**Important**: make sure to create the folders with the exact following names (case included) and path because the *RateMyAir_API* project and all the configurations provided in this guide expect to find the application, database and logs folder in these exact location.
 
Create a folder named *RateMyAir* in the following path: */home/pi/*  
Then create 3 folders named *app*, *database* and *logs* in it. You should end up with a structure like the following:

* /home/pi/RateMyAir/
* /home/pi/RateMyAir/app/
* /home/pi/RateMyAir/database/
* /home/pi/RateMyAir/logs/

## Install dependencies

Even if the app is self-contained, it is necessary to install some packages with the following command:

```
sudo apt-get update
sudo apt-get install curl libunwind8 gettext apt-transport-https
```

## Compile and Deploy the .NET Core 5 API Project

Open *RateMyAir_API* project in Visual Studio and compile it by running the following command in package manager console:

```
dotnet publish -c Release -r linux-arm
```

Copy all files from *..\RateMyAir_API\RateMyAir\RateMyAir.API\bin\Release\net5.0\linux-arm\publish* to the Raspberry in the *app* directory created previously.
Also make sure to copy the *airquality.sqlite* database from *..\RateMyAir_API\Database* to the *database* folder.

Now make the *RateMyAir.API* binary executable: open terminal and run:

```
cd /home/pi/RateMyAir/app
chmod 755 ./RateMyAir.API
```

Run the app like this:

```
./RateMyAir.API
```


## Run the app as a service

Running the app as a service ensures that it gets automatically started at startup or restarted in case of a crash. Open the terminal and run:

```
cd /lib/systemd/system/
sudo nano RateMyAir.API.service
```

insert the following lines:

```
[Unit]
Description=RateMyAir .NET Core 5 API
After=nginx.service

[Service]
Type=simple
User=pi
WorkingDirectory=/home/pi/RateMyAir/app
ExecStart=/home/pi/RateMyAir/app/RateMyAir.API
Restart=always

[Install]
WantedBy=multi-user.target
```

Save the file by pressing *CTRL + X* then Y and ENTER

Enable the service:

```
sudo systemctl enable RateMyAir.API
```

and start it (only the first time):

```
sudo systemctl start RateMyAir.API
```

At this point the application should automatically start at every reboot of the Raspberry. In case it doesn't, open the log file in the terminal and check for errors:

```
sudo nano /var/log/syslog
```
