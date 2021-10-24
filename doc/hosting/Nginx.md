# Install nginx web server

Nginx is the web server that we are going to use to handle the incoming HTTP requests to our APIs. It is also going to act as a reverse proxy to protect the application.
To install nginx run the following command in the terminal:

```
sudo apt-get install nginx
```

and start it with:

```
sudo service nginx start
```

Now we need to configure the web server to forward the incoming web requests on port 80 (or port 443 in case you install an SSL certificate) of the Rapsberry to the .NET Core app on port 5000 by editing the nginx config file. Open the nginx configuration file:

```
sudo nano /etc/nginx/sites-available/default
```

look for the section starting with *location /* and modify it to look like the following:

```
location /RateMyAir/ {
    proxy_pass http://localhost:5000/;
    proxy_http_version 1.1;
    proxy_set_header Connection keep-alive;
    proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    proxy_set_header X-Forwarded-Host $http_host;
    proxy_set_header X-Forwarded-Proto http;
    proxy_set_header X-Forwarded-Path /RateMyAir;
}
```
Save the file by pressing *CTRL + X* then Y and ENTER. Then reload nginx with the following:

```
sudo nginx -s reload
```
