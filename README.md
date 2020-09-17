# Connect-Four
[Connect Four](https://en.wikipedia.org/wiki/Connect_Four) is a classic two-player game where the objective for a player is to be the first one to form four of their game chips in either a horizontal, vertical, or diagonal line. There are three outcomes that can occur in this game, the person playing red wins, the person playing yellow wins, or the game ends in a tie.

This program is an exact copy of the classic game. It was written in C# and all graphics were made with GDI+. This program requires .NET Framework 4.7.2 in order to function and run.

## Current Features
* Ability to play against a computer player
* Ability to play against a human player (must share the same computer and pointing device)
* Ability to mute sound effects
* Ability to start a new game if one is already ongoing

## Screenshots
![New game starts](https://raw.githubusercontent.com/BeardedFish/Connect-Four/master/Screenshots/screenshot1.png)
![Red player wins](https://raw.githubusercontent.com/BeardedFish/Connect-Four/master/Screenshots/screenshot2.png)
![Game ends in a tie](https://raw.githubusercontent.com/BeardedFish/Connect-Four/master/Screenshots/screenshot3.png)

## Credits
* **Programming & Graphics**
  * Darian Benam
* **Sound Effects**
  * Pop Sound Effect - Mark DiAngelo (from https://www.soundbible.com)
  * Game Over Sound Effect - Mike Koenig (from https://www.soundbible.com)

## Future Plans
- [ ] Add in difficulty levels for computer player (ex: Easy, Medium, Hard)
- [ ] Replace current computer opponent algorithm with Minimax algorithm (https://en.wikipedia.org/wiki/Minimax)
- [ ] Make game playable over a network
