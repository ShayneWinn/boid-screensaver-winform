# boid-screensaver
 The goal of this project is to create a simple screensaver for Windows computers. 
 This is a simple <b>Microsoft Forms</b> application using <b>.NETv4.6.1</b>. 
 It uses the standard interface for Windows screensavers and should be compatible with all 
 Windows computers, though that has not been tested on my computer other than mine.

 ![boids-gif](media/EditedBoids.gif)

# Installation 
 1. Save the file. Should be in the <b>.scr</b> format.
 2. Move the file to C:/Windows or any subfolder of C:/Windows.
 3. Highlight the file and <b>right click</b>, then choose <b>Install</b>
 4. Configure, then press <b>Apply</b>

# Configuration
When configuring the screen saver there are three properties:

1. Flock: This determins how strongly the boids will prefer to move towards close neighbors.
2. Align: This determins how strongly boids will prefer to move in the same direction as close neighbors.
3. Avoid: This determins how strongly the boids will attempts to prevent colisions with close neighbors.
