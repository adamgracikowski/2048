# 2048

## Table of Contents:

- [What is the 2048 game?](#what-is-the-2048-game?)
- [Gameplay](#gameplay)
- [Strategy](#strategy)
- [About the implementation](#about-the-implementation)
- [Author](#author)
- [Contributing](#contributing)
- [Show your support](#show-your-support)

## [What is the 2048 game?](https://en.wikipedia.org/wiki/2048_(video_game))

2048 is a single-player sliding tile puzzle game. The objective of the game is to slide numbered tiles on a grid to combine them to create a tile with the number 2048.

The 2048 game has been described by The Wall Street Journal as *"almost like Candy Crush for math geeks"*, and Business Insider called it *"Threes on steroids"*.

The game was originally written by by Italian web developer [Gabriele Cirulli](https://github.com/gabrielecirulli).

## Gameplay:

2048 is played on a plain 4×4 grid, with numbered tiles that slide when a player moves them using the four arrow keys. The game begins with two tiles already in the grid, having a value of either 2 or 4, and another such tile appears in a random empty space after each turn. Tiles slide as far as possible in the chosen direction until they are stopped by either another tile or the edge of the grid. If two tiles of the same number collide while moving, they will merge into a tile with the total value of the two tiles that collided. The resulting tile cannot merge with another tile again in the same move.

If a move causes three consecutive tiles of the same value to slide together, only the two tiles farthest along the direction of motion will combine. If all four spaces in a row or column are filled with tiles of the same value, a move parallel to that row/column will combine the first two and last two. A scoreboard on the upper-right keeps track of the user's score. The user's score starts at zero, and is increased whenever two tiles combine, by the value of the new tile.

The game is won when a tile with a value of 2048 appears on the board. When the player has no legal moves (there are no empty spaces and no adjacent tiles with the same value), the game ends.

## Strategy:

Strategies in 2048 include keeping the largest tiles in a specific corner and to keep that tile in that corner and to fill the specified row with the largest numbers.

## About the implementation:

The clone of 2048 game was written in **C#** using GUI framework **WPF** (Windows Presentation Foundation developed by Microsoft) and **XAML**.

The project was structured in such a way so as to separate the core (`Tile` and `Board` classes) from graphical user interface, so it is easy to extract the core and build a then build a new GUI around it.

## Author:

My GitHub: [@adamgracikowski](https://github.com/adamgracikowski)

## Contributing:

All contributions, issues, and feature requests are welcome! 🤝

## Show your support:

Give a ⭐️ if you like this project and its documentation!
