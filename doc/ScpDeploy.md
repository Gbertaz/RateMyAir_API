# Deploy the app to your Raspberry Pi using SSH and SCP (Secure Copy)

1. Enable the SSH server on your Raspberry launching the *Configuration* from the *Preferences menu*, navigate to the *Interfaces* tab, select *Enabled* next to SSH and click OK
2. If you are using Windows install [PuTTY](https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html) to get SCP (Secure Copy). SCP command is included in Linux and Mac
3. Launch the copy:

From Windows open a Command prompt and run:

```
pscp -scp -r C:\your-publish-location* pi@192.168.1.10:/home/pi/RateMyAir/deploy/
```
From Linux and Mac run:

```
scp -r /your-publish-location/* pi@192.168.1.10:/home/pi/RateMyAir/deploy/
```

Make sure to replace *192.168.1.10* with your Raspberry IP Address.
