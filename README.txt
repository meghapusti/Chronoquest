================================================================================
                    CHRONOQUEST: A JOURNEY THROUGH TIME
                  Final Project - 50.052 Extended Reality
                                Team 02
================================================================================

TABLE OF CONTENTS
-----------------
1. Project Overview
2. System Requirements
3. Installation Instructions
4. How to Start the Application
5. Controls and Interactions
6. Gameplay Instructions
7. Troubleshooting
8. Team Members
9. Additional Notes

================================================================================
1. PROJECT OVERVIEW
================================================================================

Chronoquest is an immersive Virtual Reality educational experience that takes
users on a journey through three distinct historical eras: Prehistoric (Caveman),
Medieval, and Futuristic periods. Using gamified learning mechanics, players
explore each era, interact with the environment, and discover historical facts
through an engaging target-hitting and artifact collection system.

Core Features:
- Three fully explorable historical environments
- Interactive ball-throwing mechanics with target-based gameplay
- Educational "fun facts" delivered through successful interactions
- Artifact collection and cross-era progression system
- Scoring system (90 points total)
- Immersive spatial audio and era-appropriate visuals

================================================================================
2. SYSTEM REQUIREMENTS
================================================================================

Hardware:
- Meta Quest 3 VR Headset
- Meta Touch Plus Controllers
- Sufficient play space (2m x 2m recommended for safe movement)

Software:
- Unity 6000.2.7f2 LTS or compatible version
- XR Interaction Toolkit package
- Visual Studio 2022 (for code modifications)

Performance:
- The application is optimized to run at 72fps on Meta Quest 3
- Standalone VR build (no PC connection required)

================================================================================
3. INSTALLATION INSTRUCTIONS
================================================================================

For Unity Developers:
---------------------
1. Clone or download the project repository from GitHub
2. Open Unity Hub
3. Click "Add" and navigate to the project folder
4. Select Unity version 6000.2.7f2 (or allow Unity to upgrade if prompted)
5. Wait for Unity to import all assets and packages
6. Ensure XR Interaction Toolkit is properly installed via Package Manager

For End Users (Meta Quest 3):
-----------------------------
1. Enable Developer Mode on your Meta Quest 3:
   - Install the Meta Quest Developer Hub on your PC
   - Connect your Quest 3 via USB-C cable
   - Enable Developer Mode in the headset settings

2. Install the APK:
   - Use SideQuest or Meta Quest Developer Hub
   - Navigate to the build folder and select the Chronoquest APK
   - Install to your headset

3. Launch from "Unknown Sources" in your Quest 3 library

================================================================================
4. HOW TO START THE APPLICATION
================================================================================

IMPORTANT: STARTING THE GAME
-----------------------------
** YOU MUST START FROM THE CAVEMAN ERA SCENE **

In Unity Editor:
1. Open the Project window
2. Navigate to: Assets > Scenes > CavemanEra
3. Double-click "Caveman Era.unity" to load the scene
4. Press the Play button in Unity Editor OR
5. Build and Run to deploy directly to Meta Quest 3

Starting Sequence:
- The game MUST begin in the Caveman Era (Prehistoric scene)
- DO NOT start from Medieval or Futuristic scenes directly
- Starting from the correct scene ensures proper initialization of:
  * Score Manager
  * Inventory System
  * Game State
  * Player Spawn Position

================================================================================
5. CONTROLS AND INTERACTIONS
================================================================================

Meta Quest 3 Controller Mapping:
---------------------------------

LEFT CONTROLLER:
- Thumbstick: Move forward/backward/left/right (smooth locomotion)
- Grip Button: Grab objects (artifacts, ball)

RIGHT CONTROLLER:
- Thumbstick: Rotate camera view left/right
- Grip Button: Grab objects (artifacts, ball)
- Trigger Button: Activate teleportation buttons / Spawn ball

BOTH CONTROLLERS:
- Grip Button: Hold to grab and pick up artifacts or balls
- Release Grip: Drop or throw objects

Physical Interactions:
----------------------
- THROWING: Grab the ball, swing your controller naturally, and release
  the grip button to throw
- GRABBING ARTIFACTS: Point at artifact, squeeze grip button to pick up
- PLACING ARTIFACTS: Move artifact to designated location and release grip
- TELEPORTATION: Point at the push button and press trigger to change eras

================================================================================
6. GAMEPLAY INSTRUCTIONS
================================================================================

Objective:
----------
Explore three historical eras, hit all targets to learn fun facts, collect
artifacts, and progress through time while earning points.

Gameplay Loop (Per Era):
-------------------------

1. EXPLORE THE ENVIRONMENT
   - Look around and familiarize yourself with the historical setting
   - Use smooth locomotion (left thumbstick) to move freely
   - Locate the three targets scattered throughout the scene

2. HIT THE TARGETS
   - Grab a ball using the grip button
   - Aim at a target by looking at it
   - Throw the ball with a natural throwing motion
   - Release the grip button during the throw

3. LEARN FUN FACTS
   - When a target is destroyed, a fun fact panel appears
   - Read the historical information about that era
   - Gain 10 points per target hit

4. COLLECT THE ARTIFACT
   - After hitting the third target, a mystery artifact appears
   - This artifact belongs to the NEXT era
   - Grab the artifact using the grip button
   - Store it in your backpack (automatic when grabbed)

5. TELEPORT TO NEXT ERA
   - Locate the push button in the current scene
   - Point at it with your controller
   - Press the trigger button to activate
   - You will be transported to the next era

6. RETURN THE ARTIFACT
   - In the new era, find where the artifact belongs
   - Place the artifact in its correct location
   - This unlocks the targets in the new era

7. REPEAT THE PROCESS
   - Continue the cycle through all three eras
   - Complete all targets to maximize your score

Era Progression:
----------------
Caveman Era → Medieval Era → Futuristic Era

Each era contains:
- 3 targets to hit (10 points each = 30 points per era)
- 3 fun facts to discover
- 1 artifact to collect and return
- 1 teleportation button to the next era

Final Completion:
-----------------
- After completing all targets in the Futuristic Era
- A trophy will appear as your completion reward
- Total possible score: 90 points

Tips for Success:
-----------------
- Take your time exploring each environment
- Targets may be placed at different heights and distances
- Practice your throwing technique for better accuracy
- Listen to the spatial audio cues for immersion
- Read all fun facts to maximize learning outcomes

================================================================================
7. TROUBLESHOOTING
================================================================================

Common Issues and Solutions:
----------------------------

Issue: Game doesn't start properly
Solution: Ensure you started from the Caveman Era scene, not Medieval or
          Futuristic scenes

Issue: Score not updating
Solution: Make sure you started from Caveman Era. Score Manager initializes
          at game start

Issue: Can't find the teleportation button
Solution: Look for a push button object in the environment. It's usually
          clearly visible and integrated into the scene

Issue: Targets aren't being destroyed
Solution: Check that the ball actually makes contact with the target

Issue: Fun fact panel not appearing
Solution: Wait a moment after hitting the target. The panel should appear
          automatically in front of your view

Issue: Performance issues / low frame rate
Solution: Restart the application. The game is optimized for 72fps on Quest 3

Issue: Controller tracking loss
Solution: Ensure adequate lighting in your play space and that cameras on
          the headset are clean

================================================================================
8. TEAM MEMBERS
================================================================================

Team 02:
- Megha Pusti (1007128)
- Hetavi Shah (1007034)
- Priyanshi Saraogi (1007144)
- Julia Ruiz Fernandez (1010888)

Course: 50.052 Extended Reality
Institution: Singapore University of Technology and Design (SUTD)

================================================================================
9. ADDITIONAL NOTES
================================================================================

Development Information:
------------------------
- Engine: Unity 6000.2.7f2
- Framework: XR Interaction Toolkit
- Platform: Meta Quest 3 (Standalone)
- Programming Language: C#
- Version Control: GitHub

Known Limitations:
------------------
- Experience is limited to three historical eras
- Single-player only (no multiplayer support)
- Content is in English only
- Requires standing or room-scale VR setup

Educational Value:
------------------
- 9 total historical fun facts across three eras
- Experiential learning through active participation
- Spatial memory reinforcement through artifact mechanics
- Gamified engagement to maintain interest

Future Enhancements:
--------------------
- Additional historical eras
- Multiplayer cooperative mode
- More varied gameplay mechanics
- Assessment and knowledge testing features
- Multi-language support

Play Time:
----------
Average completion time: 5-10 minutes (depending on exploration style)

Safety Reminders:
-----------------
- Clear your play space of obstacles before starting
- Take breaks if you experience any discomfort
- Be aware of your surroundings during gameplay
- Stop immediately if you feel motion sickness

================================================================================

For questions, feedback, or technical support, please contact Team 02 through
the course instructors or via the project GitHub repository.

Thank you for experiencing Chronoquest: A Journey Through Time!

"People will forget what they read, but they'll remember what they experienced."

================================================================================
                              END OF README
================================================================================
