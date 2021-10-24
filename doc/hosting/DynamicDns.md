# Install a dynamic DNS

A dynamic DNS makes it possible to access the APIs from anywhere outside your LAN by periodically associating your public network IP to a domain name.
I am using Duckdns which is free and works great!

Create a free account [Duckdns](https://www.duckdns.org/) and create a domain name for example: *ratemyair*.

Now create a folder named *duckdns* in the following path of your Raspberry */home/pi/*.

Open the terminal and run:

```
cd /home/pi/duckdns/
sudo nano duck.sh
```

enter the following line:

```
echo url="https://www.duckdns.org/update?domains=<YOUR DOMAIN NAME>&token=<YOUR TOKEN>&ip=" | curl -k -o /home/pi/duckdns/duck.log -K -
```

Make sure to replace **<YOUR DOMAIN NAME>** with the domain name you created and **<YOUR TOKEN>** with the token you can find in the Duckdns dashboard.

Save the file by pressing *CTRL + X* then Y and ENTER

Now make the duck.sh file executable:

```
sudo chmod 700 duck.sh
```

Next we are going to create a cron job to run the script every 5 minutes which is responsible to let the DNS know your public IP address in case it changed.

In the terminal run:

```
crontab -e
```

and enter the following line at the bottom:

```
*/5 * * * * /home/pi/duckdns/duck.sh >/dev/null 2>&1
```

Save the file by pressing *CTRL + X* then Y and ENTER

Test the script. The following command should generate a log file named *duck.log* in */home/pi/duckdns/*. 

```
sudo ./duck.sh
```

If the log doesn't contain **OK** check if your Token and Domain are correct in the */home/pi/duckdns/duck.sh* script.

Finally start the cron job:

```
sudo service cron start
```

Now you should be able to make a request even outside your LAN. You can test it by requesting the last AirQuality record in the database using Postman. If the database is empty you should at least get a HTTP 200 OK response code to confirm that the application and the DNS are working correctly. The request url is:

```
http://<YOUR DOMAIN NAME>.duckdns.org/RateMyAir/api/airquality/last
```

**Do not forget that the URL is case sensitive**