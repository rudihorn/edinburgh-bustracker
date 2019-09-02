# Bus Tracker for Edinburgh

This is a simple project I wrote for creating custom departure boards for
Lothian buses. I wanted a quick way to be able to check a custom set of services
for going known routes, since there were many relevant buses departing from
different nearby bus stops.

This project was designed with being run on a Raspberry Pi in mind. It uses .NET
Core which also runs on linux for ARM and uses the Avalonia GUI framework which
is cross platform.

## Requirements

The project requires a Raspberry Pi and uses the .NET Core SDK. It is not
necessary to install the .NET Core SDK on the Raspberry Pi, since it is possible
to publish the project with all required dependencies which can simply be run on
the target device.

## Config File

```json
{
  "destinations" : [
      {
          "name": "Dalkeith",
          "stops": [
              {
                  "name": "St Patrick Square (SE)",
                  "id": 6200208530,
                  "services": ["N3",3,7,31,300],
                  "wd": 5
              },
              {
                  "name": "Surgeons' Hall (SE)",
                  "id": 6200208460,
                  "services": ["N30",30],
                  "wd": 4
              }
          ]
      }
  ]
}
```

### Disclaimer

This project is not in any way associated with Lothian buses. It is provided as is.
