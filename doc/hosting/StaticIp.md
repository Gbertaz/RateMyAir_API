# Setup a static IP Address

In order to edit your Raspberry's network configuration, open the terminal by pressing *CTRL + ALT + T* and run the following command:

```
sudo nano /etc/dhcpcd.conf
```

enter the following lines:

```
interface <eth0>
static ip_address=STATIC-IP-ADDRESS/24
static routers=ROUTER-IP-ADDRESS
static domain_name_servers=DNS-IP-ADDRESS

interface <wlan0>
static ip_address=STATIC-IP-ADDRESS/24
static routers=ROUTER-IP-ADDRESS
static domain_name_servers=DNS-IP-ADDRESS
```

Replace STATIC-IP-ADDRESS with the static IP you want to assign to your Raspberry Pi.  
Replace ROUTER-IP-ADDRESS with the Router IP.  
Replace DNS-IP-ADDRESS with the DNS address. I am using the Google DNS 8.8.8.8

Save the file by pressing *CTRL + X* then Y and ENTER

Reboot the Raspberry by running:

```
sudo reboot
```

and after reboot run the following command to show the IP address:

```
sudo ip addr show
```
