# Custom Name List

It allows defining a custom name list, which will be used exclusively for naming your beavers.

Place a text file named `names.txt` in the same directory as `Timberborn.exe`.
`names.txt` should contain one line per name that you want to see in your game.

Names are picked from the list at random. Once all names have been used up, the list gets shuffled and names will be reused.

If the mod is active but can't find `names.txt` in the correct place, it will print an error message to the log and fall back to standard in-game names. 


## Notes

This mod was originally developed by [thundersen](https://github.com/thundersen/timberborn_customnamelist).

This version is an update that simplifies the custom name implementation to use the system provided by the Timberborn devs, and as such, the name lists will persist across reloads within a save. 
This means if you have 20 custom names, with 5 names from that list having already been used, and you restart Timberborn, you are now guaranteed to not get those 5 used names until the other 15 are given out.
It also supports "Hot Reloading" the names list. Once the list of names is used up, it will recheck the names.txt file and load the newest list for immediate use without restarting Timberborn.


# Change Log

- 0.2.0
  - Added persistent name-list state. Names will not get jumbled on reloading of game.
  - Added Hot-Reload of names.txt file. Once name-list is used up, pull the newest list of names from the file.

- 0.1.2
  - Fix: Adapt mod for experimental release from 2021/11/24.
    (The game changed the location and name of the naming service that the mod needs to modify.)

- 0.1.1
  - Minimal version based on names.txt