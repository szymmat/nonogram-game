# nonogram-game
Windows Forms app for solving nonogram puzzles (see https://en.wikipedia.org/wiki/Nonogram for details).
IMPORTANT: There may be more than one solution to the puzzle, but only one solution is recognized as valid by application.
Task description:

Main window and controls:
        Main window has a size of 1000x800 and starts centered on the screen
        Main window is non-resizable and has maximalize button disabled
        On the top of the window there is a menu bar with 2 options: 'New Game' and 'Create...'
        Under 'New Game' there are three options: 'Random', 'Choose puzzle...' and 'Load puzzle'
        Under 'Create...' there is one option: 'Create puzzle'
        The main window area is composed from:
            board 10x10 (each tile has size of 30x30)
            labels beside each row and above each column
Board:
        Start board size : 10x10 tiles
        Each tile color: white when empty, black when filled
        On the start of the application, there is a random puzzle generated and the board is already painted accordingly to the generated board values
Labels:
        Number in row indicate how many unbroken painted lines there are in this row and how long they are
        Analogously, column text indicate how many unbroken painted lines there are in this column and how long they ar
   
Board additional features:

    Supports crossing out fields. Crossing out fields makes easier to solve the puzzle (it marks the fields that certainly can't be painted from logical assumption)
    Left-click: When the field was unpainted, it becomes filled with black color
    Left-click: When the field was filled, it becomes empty (white color)
    Left-click: When the field was crossed, it becomes filled with black color
    Right-click: When the field was unpainted, it becomes crossed
    Right-click: When the field was filled, it becomes crossed
    Right-click: When the field was crossed, it becomes empty (white color)
    Two modes:
        Play Mode
            All fields are initially empty
            When all rows/columns clues are fulfilled the game is finished - appropriate message is shown and clicking on fields is disabled
            Board should be centered in the window
        Create Mode
            All fields are initially empty
            Numbers in rows/columns are updated when field is painted
            Numbers in rows/columns are not changed when field is crossed out

Create puzzle:

    Clicking on 'Create puzzle' menu item shows window asking to input Width and Height of new puzzle
    Width/Height window:
        Textbox values should be in range 2-15
        Validates the numbers entered in textboxes
        Red cross is shown beside textbox when value is out of range or is invalid (e.g. is literal, not numerical)
        When mouse is over the red cross icon, the tooltip is shown with valid value range
        Textboxes have initial values when the window is shown
    Generates clear board of given size and sets row/column labels '0'
    The puzzle is moved to the right side of the window
    On the left side of the window is panel with Puzzle's information: Title and Difficulty (each of them has textbox to input data)
    Below the puzzle information there is a button to save puzzle
    Puzzle is in Create Mode
    Saving puzzle
        Writes to file puzzle's Title, Width, Height, Difficulty and row/column clues
        Save file dialog should force default file extension (chosen appropriately)

Random game:

    Opens window with Width/Height textboxes - the requirements for this window are the same as Width/Height Window in 'Create puzzle' section
    Generates random board and sets row/column labels to correct values
    Game should be solvable (that is, rows/columns clues shouldn't contradict each other)
    Puzzle is in Game Mode

Choose puzzle:

    Window area is composed of ListView with listing of all available puzzles
    There is one texbox (disabled for input) and button to choose folder with saved puzzles
    When folder is chosen, folder name is loaded into textbox and all puzzles are loaded into ListView
    Only onle item can be chosen in ListView at the time
    Items in ListView are composed of the puzzle's: Name, Width, Height and Difficulty
    Puzzle list can be refreshed by clicking 'Refresh' button
    When one item is chosen, clicking on 'Load puzzle' button loads the puzzle
    Puzzle is in Game Mode

Load puzzle:

    Opens file dialog to load puzzle file
    Should force user to load files with correct extension
    Puzzle is in Game Mode

