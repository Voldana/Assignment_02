# Assignment_02
This is the second assignment for COMP_376 class. It's a clone of candy crush.

- The project uses Zenject plugin, a dependancy injection framework for Unity. you can find the zenject documents here: https://github.com/modesttree/Zenject?tab=readme-ov-file
   - Based on the dependancy injection principle, almost all required dependancies by the files are injected in the using the [inject] keyword. The injection is done via the sceneinstaller present at each scene.
   - Zenject provides more than just a dependancy injection framework, it offers templates for using several design patterns such as factory pattern (Used in almost every case an instantiation was required) and Observer pattern (Signals in zenject).
   - Almost each namespace has a unique Installer.cs script inside which is responsible for binding dependencies in that certain namespace, this way no dependencies are binded out of scope to other sripts which do not use them and encapsulate the injections.

#Scripts explanation:
- UI Namespace:
  - Menus.cs: WinMenu.cs, LoseMenu.cs and other scripts in this namespace are all panels that created using the Factory pattern, they contain simple buttons for navigation.
  - HUD Namespace:
    -  HUD.cs : This scripts handles the main HUD of the game controlling many aspects such as the timer, the objectives (How many of what color is left), the number of moves and etc. The HUD scripts communicates with the main manager system using signals such as OnObjectiveComplete, AddToScore and others that are subscribed in the script. it is also responsible for creation of lose, win and pause panels.
    - Objective.cs: Each objective is representing a goal that must be completed it takes a certain GemType to distinguish itself from other objectives.
    - Timer.cs: This is a simple timer class that works using Demigiant's DoTween package. It has functionalities such as calling custom functions each second to mimic an actual timer.
    - PauseButton.cs: This script simply creates the pause menu.
- Game Namespace:
  - LevelSetting.cs: Is simply a ScriptableObject that holds the data for each level of the game.
  - InputReader.cs: Gandles user inputs using InputAction and the event system.
  - Board Namespace: Contains most of the scripts for the game logic.
    - Gem.cs: This script is attached to each gem on the board containing data such as it's position and its GemType and its sprite.
    - Grid.cs: This script represents the game's board, it validates the positions that are given to it by the input system and returns the corresponding gem residing in that spot.
    - GridObject.cs: This script represents each cell of the Grid which holds a Gem.
    - Manager.cs: This script is the main manager for the game, it handles almost every logical operation in the game such as finding matches, initializing the board, cascading gems and exploding them and also does the scoring.
    - Spawner.cs: The manager class uses this script to determine the type of the gem that it should spawn in a certain position it uses normal disturbution and uniform disturbution to determine the gem type in a certain position in the board.
