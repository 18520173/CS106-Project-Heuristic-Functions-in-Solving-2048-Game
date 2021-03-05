# CS106 Project: Heuristic Functions in Solving 2048 Game

**Credit:** for Dev Tutorials for the UI and the basic functions of the game. We only implement the AI part onto their game, we did not really build the whole game (but we did make some modification).\

For the AI implementation, go to Assets/Scripts/. The Priority-based AI is in the file 'PriorityBased.cs', and the Best-first one is in 'BestFirstAlgorithm.cs' file.

**How to run the AI in Unity**

1. Download all files in the folders (excluded 'Application' folder).
2. Build a project which contains the folder with all files.
3. After done building and importing files, there will be a scene in the center of the screen, which is the game. Click on the (pretty) small play button above the scene to run the game.
4. There are indeed four modes in the game:
* Player can play on their own using four keys down, up, left, right to move the tiles.
* Random Mode: check the toggle named Random. This will randomly move the tiles in every direction.
* Best-First Mode: check the toggle named Best-first Algorithm. This will choose the best move according to our heuristic functions.
* Priority-Based: check the toggle named Priority-Based. This will choose the move which has highest priority.

If you desire to run the application (without actually build it in Unity), download the folder named ‘Application’ with a sub-folder contains game data and an .exe file in it. Run the .exe file to play the game. Have fun!

For an insight of the implementation, please read 'Report'.
