# inventory for CitizenFX

This inventory project has been automatically generated by the CitizenFX resource template.

To edit it, open `inventory.sln` in Visual Studio.

To build it, run `build.cmd`. To run it, run the following commands to make a symbolic link in your server data directory:

```dos
cd /d [PATH TO THIS RESOURCE]
mklink /d X:\cfx-server-data\resources\[local]\inventory dist
```

Afterwards, you can use `ensure inventory` in your server.cfg or server console to start the resource.