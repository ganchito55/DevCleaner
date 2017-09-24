# DevCleaner

If you are a developer who works with Visual Studio, you will surely have a lot of projects in your hard drive.
However, you are also saving a lot of unnecessary files, like as packages, dlls, executables and other debug files. 
DevCleaner helps you to recover more free space, cleaning these files in your old projects.

## How to use
The usage of this software is very easy:

1. Click in Scan and select the folder where you save your visual studio projects.
2. Mark all projects that you want to clean
3. Click in clean

## What files are cleaned?
Now, all packages in the solution and all files under the bin and obj folders are deleted.

## How much space can I recover?
It depends, mainly, of the number and kind of the projects that you have, however this is what I got:

          
            +---------+---------+
            | Before  | After   |
    +-------+---------+---------+
    | Size  | 5.57GB  | 1GB     |
    +-------+---------+---------+
    | Files | 56 796  | 24 660  |
    +-------+---------+---------+
    
As you can see, I have half of the files, but the size is ~18%.

![DevCleaner](https://i.imgur.com/VBEocfS.png)
