# Rhythm Game

First release for the rhythm game that I've begun to work on.

Developed in Unity 2D, currently features 5 songs that I made. Image assets are placeholders for now. Main keys to press from left to right are D, F, K, and L. Future versions will allow you to change what buttons you can press.

### Where to Download ###

___

You can find the download links for both Windows and Mac over [here].

### Steps to Launch ### 

___

**Windows**
1. Unzip "Alpha.1.0.Windows.zip"
2. Open the application labeled "RhythmGame" with the custom icon

**Mac** 
1. Unzip "Alpha.1.0.zip"
2. Run the following two commands on Terminal. Make sure to change directory (cd) into the newly unzipped folder "Alpha 1.0" (You can do this by opening Terminal and typing in `cd` and dragging the "Alpha 1.0" folder into the terminal to automatically retrieve the path)
`chmod 700 unlockperms.command`
`sed -i -e 's/\r$//' unlockperms.command`
3. Double click on the unlockperms.command
4. Open the application labelled "Alpha 1.0" with the custom icon

**Note:** Your antivirus will likely stop you from opening the application due to untrusted author. You can allow the application to run regardless no problem.

### How to Play ###

___

Upon loading in, use the mouse to hit the 'Click to Play' text on the screen.

You will then be met with a Song Select screen with 5 songs. You can navigate these songs with both mouse and left/right arrow keys, with the purple outlined crystal being the currently selected one.

The number below the image is your highest number of notes hit for that song.

Pressing Enter or clicking on the currently selected crystal will move you to the song.

In the song, you will see four 'buttons' at the bottom left of the screen. These buttons are controlled by 'D', 'F', 'K', and 'L'. Pressing the appropriate key shows an indicator on the button being pressed.

Your main goal is to hit the notes flying down the screen on time with the notes overlapping with their respective button. There will be a counter close to the middle which shows your highest combo (number of notes hit without missing).

After completing the song, the game will tell you how many notes were hit, as well as the maximum number of notes in the song. 

You can choose to replay the song or return to the song select screen.

Pressing 'Escape' on the Song Select screen will open a prompt to quit the game. Pressing 'Escape' during a song will pause the song and allow you to resume, replay, or return to menu.

**Song Difficulties**

___

From a scale of 1-10, with 1 being "Never touched a rhythm game in my life" to 10 being "I have over 100 hours in Osu, Guitar Hero, DDR, etc"

Song 1: Easy (3)  
Song 2: Very Easy (2)  
Song 3: Medium (6)  
Song 4: Hard (8)  
Song 5: Medium (4)  

This line is also a separate paragraph, but...
This line is only separated by a single newline, so it's a separate line in the *same paragraph*.

___

There's still quite a lot of things I want to do for this, but for now here's the initial version to show the concept. 

[here]: https://github.com/ArvindBhogal/rhythmgame/releases/tag/v1.0-alpha
