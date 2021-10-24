# Install VNC server

Install VNC server to access the Raspberry remotely from another device. This step is optional but allows you to control the Raspberry without having to plug in a monitor, keyboard and mouse.

After executing the following commands, enter a secure password when requested.

```
sudo apt-get update
sudo apt-get install tightvncserver
```

#### Run VNC server at startup

Open the terminal and run the following command:

```
cd .config
cd autostart
```

If you get the error "No such file or directory" create it:

```
mkdir autostart
cd autostart
```

Create a new file with nano by running the command:

```
nano tightvnc.desktop
```

and enter the following lines:

```
[Desktop Entry]
Type=Application
Name=TightVnc
Exec=vncserver :1
StartupNotify=false
```

Save the file by pressing *CTRL + X* then Y and ENTER

At this point it is possible to control the Raspberry from another computer or smartphone by using a VNC client.  
Configure the VNC client by entering the Raspberry static IP address that we configured in the previous step [Setup a static IP Address](doc/hosting/StaticIp.md).
It might be necessary to specify the port as well. In this case use *5901* For example:

```
192.168.1.10:5901
```
