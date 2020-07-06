# Helios 1.4 Source Code Repository

The BlueFinBima repo "Helios" has been renamed to [Helios 1.4](https://github.com/BlueFinBima/Helios14) because this chapter will shortly come to an end.  Helios 1.4 is no longer accepting new issues or pull requests.
## Fear Not!
[Helios 1.6](https://github.com/HeliosVirtualCockpit/Helios) is the next step in the journey, and a lot of work has taken place under the covers, principally by @derammo and his team of testers, which will provide a foundation for future improvements to the virtual cockpit environment.
The last release of [the BlueFinBima fork of Helios 1.4](https://github.com/BlueFinBima/Helios14/releases/tag/1.4.2020.0530) can still be downloaded, however the future is 
very definitely Helios 1.6 whose releases will appear [here](https://github.com/HeliosVirtualCockpit/Helios/releases).

The [Helios 1.4 Wiki](https://github.com/BlueFinBima/Helios14/wiki) remains available, however the only wiki to be maintained is at [Helios Virtual Cockpit](https://github.com/HeliosVirtualCockpit/Helios/wiki)


## [Project Wiki](https://github.com/BlueFinBima/Helios14/wiki)

This is the source code repository for the Helios Virtual Cockpit System for DCS World.  This repository is for core and module developers use.  If you are looking for end user downloads and documentation please visit http://www.gadrocsworkshop.com.

The latest binaries from this fork of Helios are available at https://www.digitalcombatsimulator.com/en/files/3302014/

Helios is composed to two programs, the first **Profile Editor** allows the creation of virtual cockpits.  Visual components in the profile can then be bound to an interface which allows communication between DCS and the virtual cockpit.  Input bindings take DCS TRIGGERS and links them to ACTIONS on the visual components.  Output bindings take TRIGGERs in the virtual cockpit, and links them to ACTIONs on the interface into DCS.  The interface that gets bound to the visual components is usually aircraft specific.

The Helios **Control Center** program executes the profile that was created by the **Profile Editor** and displays the virtual cockpit, usually on a touch screen, for the virtual pilot to interact with.

When the aircraft specific interface is deployed from the **Profile Editor**, it creates an **export.lua** script file in the %userprofile%\saved games\DCS\Scripts\ directory, and this is the code that is invoked by DCS at aircraft start-up, and communicates with the **Control Center** over a UDP link.

## [Change History](https://github.com/BlueFinBima/Helios14/wiki/Change-Log)

## YouTube Resources

* Autobinding and Hornet IFEI & UFC demonstration [![Autobinding and Hornet IFEI & UFC demonstration](http://img.youtube.com/vi/lEhG-TtENWc/0.jpg)](http://www.youtube.com/watch?v=lEhG-TtENWc)
* AV-8B Interface - A video of the new AV-8B interface in action [![AV-8B Interface demonstration](http://img.youtube.com/vi/4kQG0dJMROg/0.jpg)](http://www.youtube.com/watch?v=4kQG0dJMROg)
