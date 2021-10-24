# Install Letsencrypt SSL certificate

Secure our API with an SSL certificate. I am using Let's encrypt because it is free.

Before installing the certificate, make sure that port TCP/80 and port TCP/443 are correctly forwarded to point to your Raspberry static IP address configured in this step [Setup a static IP Address](StaticIp.md). I am not gonna dig into configuring the port forwarding because every router is different, there are a lot of tutorials on how to do that.

### Installing snap

```
$ sudo apt update
$ sudo apt install snapd
```

Now reboot your Raspberry in order to properly update the system.
After reboot install install the core snap in order to get the latest snapd:

```
sudo snap install core
```

### Install certbot

```
sudo snap install --classic certbot
sudo ln -s /snap/bin/certbot /usr/bin/certbot
```

This command will install the certificate and automatically edit the nginx configuration turning on HTTPS access. Provide the requested information when prompted during the installation process.

```
sudo certbot --nginx
```

When prompted to enter the domain name, use the domain created in Duckdns [Install a dynamic DNS](DynamicDns.md). For example:

```
<YOUR DOMAIN NAME>.duckdns.org
mydomain.duckdns.org
```

The certbot packages will automatically renew the certificate before expiration. In order to test automatic renewal run this command:

```
sudo certbot renew --dry-run
```

Finally you can check if the nginx configuration has been correctly edited by opening the config file:

```
sudo nano /etc/nginx/sites-available/default
```
