# TopDownShooter

## A **Utility AI** Bot in Unity3D

Inside this Top Down Shooter prototype, I implemented a Bot using the concept of *Utility AI*

## Youtube Teaser

This project in action
<a href="http://www.youtube.com/watch?feature=player_embedded&v=vDKTZSRe_5A
" target="_blank"><img src="http://img.youtube.com/vi/vDKTZSRe_5A/hqdefault.jpg" 
width="240" height="180" border="10" /></a>

## Screenshot

![Utility AI Bot](http://imgur.com/8Z4DCQm.png "Debug Enabled")

## Disclaimer
This is a project based on [another tutorial from Sebastian Lague](https://www.youtube.com/watch?v=SviIeTt2_Lc&list=PLFt_AvWsXl0ctd4dgE1F8g3uec4zKNRV0) (an awesome developer and tutor, follow him on Youtube). In this Top Down Shooter, I develop the UI, the Pickup, and the Reload System.

## Objectives
My goal in this project was to develop a Utility AI Bot to play this game.
Utility AI uses a reasoning system, that is essentially a scored-base algorithm that calculates the best course of action every refresh cycle.
In this implementation, we calculate the best position that the bot should move (considering some strategic goals) inside the scanned positions (represented by the little spheres).

## Debug Tool
I've also made a Debug Tool that helps the Game Designer tune the Scanner (the grid of spheres). This tool shows color-graded spheres with a score, that represent every position used by the Scanner to evaluate the best position to move.
The designer can tweak several key aspects of the Scanner, but it must be done with caution, in incremental steps, to avoid overwhelming the computational power. We can, for example, expand the grid size (or the sampling density) while to compensate, we diminish the refresh interval.

## Tweaks and maintainability
Another key aspect of this Utility AI approach is the ability (trough scripts and methods - [e.g. this specific script](https://github.com/daltonbr/TopDownShooter/blob/master/Assets/Scripts/AIManager/MoveToBestPosition.cs) ) to add, remove or tweak several  "considerations" or "reasonings", such as distance from the player to the nearest Pickup, nearest Enemy, the amount of ammo, amount of life. All these things have their respective "weight" and by tweaking, adding or removing them, we can achieve quickly several different behaviors.
If for example, we want to make the bot avoid bombs, we add a simple method that considers the distance between the player and the nearest bomb, and we give it a weight to that consideration, some sort of precedence order.

## Conclusion
This approach is much more easy to maintain and tweak when compared to the well-established, but harder to tweak, FSM (Finite State Machines) or Behaviour-Trees.

If you want to talk more about this and others AI approaches, just give me a shout!