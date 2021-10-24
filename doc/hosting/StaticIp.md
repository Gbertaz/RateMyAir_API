# Setup a static IP Address

Open the terminal by pressing *CTRL + ALT + T* and run the following command:

```
sudo nano /etc/dhcpcd.conf
```

enter the following lines:

```
interface <eth0>
static ip_address=<STATIC-IP-ADDRESS>/24
static routers=<ROUTER-IP-ADDRESS>
static domain_name_servers=<DNS-IP-ADDRESS>

interface <wlan0>
static ip_address=<STATIC-IP-ADDRESS>/24
static routers=<ROUTER-IP-ADDRESS>
static domain_name_servers=<DNS-IP-ADDRESS>
```

Replace **<STATIC-IP-ADDRESS>** with the static IP
Replace **<ROUTER-IP-ADDRESS>** with the Router IP
Replace **<DNS-IP-ADDRESS>** with the DNS address  

Save the file by pressing *CTRL + X* then Y and ENTER

Reboot the Raspberry by running:

```
sudo reboot
```

and after reboot run the following command to show the IP address:

```
sudo ip addr show
```