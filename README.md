# DARF

**Project Description**

A set of tools and libraries which help software developers create fully distributed and component-based applications.

Here's the whole story in a picture. This image shows a sample scenario of DARF application:

![overview] (http://i57.tinypic.com/sq1teo.png "What can DARF do")

The source code contains below projects:

| Project Name	|Description |
| ------------- |----------- |
|DCRF	| Fundamental project of the system which contains base definitions and interfaces|
|BlockBroker | Implementation of some block brokers (Classes which provide and load blocks)|
|BlockApp | Underlying of DARF which contains grammar to explain scripts of .xda files|
|DARF.IDE | A windows application which helps block authors, write xda scripts to create, compose and execute their blocks|
|AdminConsole | A part of DCRF which helps a system adminitrator inspect a blockWeb and its elements|
|BlockWebHost | A windows application which when executed, hosts a set of blocks which will be accessible for other blocks in a distributed system|
|NetSockets | Provides support for distribution functionality for blocks|
|GeneralBlocks | A sample project which contains some basic blocks. You can use these blocks in your xda script|
|WinBlocks | A sample project which contains some basic blocks for windows UI elements e.g. Button |


You can browse/download source code of the project in 'Source Code' section. In download section, a working demo of the project is provided for download which can get you started with the concepts of DARF.

In case of any question, feedback, issue, bug, ... just contact me at mahdix at sign gmail dot com! Thanks.


Note: For any commercial usage of this library in your product, please contact library author.
