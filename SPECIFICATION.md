# Sytem specification - Scrabble Game

## Table of contents

* 1 [Introduction](#1-introduction)
  * 1.1 [Terminology](#11-terminology)
  * 1.2 [Author](#12-author)
* 2 [Overview](#2-overview)
  * 2.1 [Usage](#21-usage)
  * 2.2 [Constraints](#22-constraints)
    * 2.2.1 [Game rules](#221-game-rules)
    * 2.2.2 [Dictionary](#222-dictionary)
  * 2.3 [External libraries](#23-external-libraries)
    * 2.4.1 [scrabble_solver](#231-scrabble_solver)
* 3 [Detailed description](#3-detailed-description)
  * 3.1 [Project structure](#31-project-structure)
    * 3.1.1 [File structure](#311-file-structure)
    * 3.1.2 [Architecture](#312-architecture)
  * 3.2 [Algorithms](#32-algorithms)
    * 3.2.1 [Player move evaluation](#321-player-move-evaluation)

---

## 1. Introduction

This Scrabble Game is a WPF-based application that allows a human player to compete against an AI. The game features interactive gameplay where players can select tiles, place them on the board, and the AI performs its move automatically.

### 1.1 Terminology

* **tile** - a single letter tile
* **blank tile** - a tile that can be used as any letter
* **word** - a sequence of tiles forming a valid word
* **move** - a word played on the game board
* **game board** - the base game board without any tiles on it
* **game board cell** - a single cell on the game board (a single square)
* **cell type** - the type of a cell on the game board (normal, double letter, triple letter, double word, triple word)
* **player's rack** - the tiles that the player currently has in their hand (not on the game board)
* **game state** - the current tiles on the game board and in player's rack

### 1.2 Author

The library was created by Jan Franěk as a semestral project for collection of C#/.NET courses at [Charles University](https://cuni.cz/UKEN-1.html) in Prague.

---

## 2. Overview

The game is written in C# on .NET 8. The entire solution consists of 4 projects: `ScrabbleCore`, `ScrabbleGame`, `ScrabbleCoreTests`, and `ScrabbleTesting`. As the name suggests, `ScrabbleCore` contains the core logic of the game, `ScrabbleGame` contains the WPF application, `ScrabbleCoreTests` contains unit tests for the core logic, and `ScrabbleTesting` contains integration tests for the game.

### 2.1 Usage

The game is not playable for anyone but me for the time being. That's because it depends on scrabble_solver C++ library, which is not included in this repository. The library is used for finding all possible moves in the game.

### 2.2 Constraints

#### 2.2.1 Game rules

The library is designed to work with [the official Scrabble rules](https://www.hasbro.com/common/instruct/Scrabble_(2003).pdf). However, there has been a slight modification to the rules to make the game easier to implement:

* Player 1 always starts the game.
* There can be a draw.
* Player can't challenge the other player's move.

#### 2.2.2 Dictionary

The dictionary of allowed words is the [CSW19 word list](https://en.wikipedia.org/wiki/SOWPODS) by deafult but can be easily changed. However, the word list file must be in the same format as the CSW19 word list.

### 2.3 External libraries

#### 2.3.1 [scrabble_solver](https://gitlab.mff.cuni.cz/teaching/nprg041/2022-23/bednarek/franekj3)

This is my C++ library that is used for finding all possible moves in the game. It is not included in this repository.

There is a known bug in the library that causes it to miss some possible moves.

---

## 3. Detailed description

### 3.1 Project structure

#### 3.1.1 File structure

The solution consists of 4 projects:

* **ScrabbleCore** - the core logic of the game
* **ScrabbleGame** - the WPF application
* **ScrabbleCoreTests** - unit tests for the core logic
* **ScrabbleTesting** - integration tests for the game

#### 3.1.2 Architecture

##### ViewModel

The viewmodel is based in the `ScrabbleGame.ViewModel` namespace. The viewmodel is responsible for handling the game logic and interfacing between the model and the UI - exposion properies for proped data binding and handling commands for player interactions.

* **MenuViewModel:** ViewModel for the main menu, handling choosing AI difficulty.
* **CellViewModel:** Represents a single cell on the game board, exposing properties for both the cell type and the tile placed on it.
* **GameViewModel:** Central management for game logic, interfacing between the model and the UI. Handles commands for player interactions.
* **GameOverViewModel:** ViewModel for the game over page, handling properties for the winner and the final game state.

##### Model

The model is based in the `ScrabbleCore.Classes`, `ScrabbleCore.Enums` and `ScrabbleCore.Structs` namespaces. Most objects implement the `INotifyPropertyChanged` interface for data binding. There are many classes, enums and structs, but the most important ones are:

* **Game:** Represents the Scrabble game logic, including board state and player management.
* **Player:** Abstract base for different types of players (Human, AI).
* **TileBoard:** Represents the Scrabble board, manages tile placement and validation.
* **TileRack:** Handles the collection of tiles that each player holds.
* **Pouch:** Represents the bag of tiles that players draw from.
* **Tile:** Represents a single tile with a type, letter and a score.

##### View

The view is based in the `ScrabbleGame.Pages` namespace. There are separate XAML files for each page of the game:

* **MenuPage:** The main menu of the game, allowing player to start a new game, choose AI difficulty, or view the help page.
* **HelpPage:** A simple page displaying the basics of the game and the rules.
* **GamePage:** The main UI component displaying the game board, player's rack, and controls for gameplay.
* **GameOverPage:** A page displayed when the game is over, showing the winner, final scores and allowing the player to return to the menu.

##### Solver

The solver is based in the `ScrabbleCode.Solver` namespace. It contains the `ScrabbleSolver` class, which is used for finding all possible moves in the game.

* **SolverInterop:** Complex class for interfacing with the C++ library. It is responsible for managing memory, converting data between C# and C++, and calling the C++ functions. It is unsafe and uses pointers.
* **Dictionary:** In this folder is located the dictionary of allowed words. The default dictionary is the CSW19 word list. It is copyrigthed so I will probably have to change it.
* **Data:** In this folder are located the data structures used by the solver. They are mostly simple structs and enums for serialization into string and deseriazation from JSON.

### 3.2 Algorithms

There aren't many complex algorithms, the complexity is mostly is in the object-oriented design of the game. The score evaluation algorithm and the algorithm for finding all possible moves are the most complex ones, but they are taken from the scrabble_solver library.

However, there is one algorithm that is worth mentioning.

#### 3.2.1 Player move evaluation

We need to determine if the tiles the player placed on the board form a valid move. This is done in the `HumanPlayer` class. The algorithm is as follows:

1. Identify the coordinates of all newly placed tiles.
2. Validate that all newly placed tiles are placed in a single row or column.
3. Validate that there are no gaps between the newly placed tiles (includind tiles already on the board).
4. Construct a word from the newly placed tiles.
    a. Include all connected tiles already on the board.
    b. In case there are blanks in the word, construct all possible words with the blanks replaced by all possible letters.
5. Get all posible plays from the solver.
6. Check if the constructed word is in the list of possible plays.
   a. If there are multiple possible plays (e.g. blanks), choose the one with the highest score.
7. If the word is valid, calculate the score and update the game state.

* Point 6a is technically against the rules since the player should declare the intended letter for the blank tile beforehand. However, this is a simplification for the sake of the game.

---

Good luck and have fun playing the game! 🎲🎉
