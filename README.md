# UnityUtilities
A collection of commonly used Unity/game concepts and functions I'm developing as I figure out what the heck I'm even supposed to be doing in the other Unity projects I'm working on. Basically everything is a work-in-progress.

## Namespace Table of Contents

### 1. `Basic`
- `Events`: Handles subscribing and broadcasting of standard C# events.
- `Physics`: Contains some utilities regarding physics.
- `StateManager`: A state machine/state manager.
- `Utils`: Contains miscellaneous utilitiy functions.

### 2. `Collections`
- `Deck`: A representation of a deck of cards.
- `ObjectGrid`: Manages GameObjects organized in a 2D grid.
- `ObjectPool`: Instantiate a number of GameObjects at one time to recycle later.

### 3. `Collections/Generic`
- `List2D`: A 2D representation of a List.
- `List3D`: A 3D representation of a List.
- `MulitLinkedList`: Linked list where nodes can have multiple links.
- `SquareGrid`: Represents a 2D of items along the XY axes. Handles positioning items and size of cells of the grid.
- `SquareGridXZ`: A subclass of `SquareGrid` that reorients position functions along XZ axes.

### 4. `FileSystem`
- `FileHelper`: Basic functions for handling local files.
- `Json`: Serialize and deserialize data.
- `SaveFile`: Create a local save file using your own data models.

### 5. `Random`
- `Dice`: Rolls a number of dice with a number of faces.
- `Shuffle`: Randomizes the order of items in an array.

### 6. `Systems`
- `KeyDoor`: Open locked doors by collecting keys.
- `InteractWithWorldSpaceUI`: Toggles world space UI to indicate player can "interact" with certain objects.