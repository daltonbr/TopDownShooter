# TopDownShooter

![Unity](https://img.shields.io/badge/Unity-6000.3.24f1-black?logo=unity)

## A **Utility AI** Bot in Unity3D

Inside this Top Down Shooter prototype, I implemented a Bot using the concept of *Utility AI*.

> Originally built in Unity 2017 during my graduation; updated to Unity 6 about 9 years later so it still compiles and runs.

## 2026 Modernization
Brought the project from Unity 2017 to 6000.3, including a move from the Built-in Render Pipeline to URP: installed packages, converted materials, and fixed the camera stack.

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
A Debug Tool helps tune the Scanner (the grid of spheres): color-graded spheres show the score of every position the Scanner evaluates when picking where to move. The grid size and sampling density can be tweaked, but incrementally, since a bigger grid costs more to compute.

## Tweaks and maintainability
Considerations, such as distance to the nearest Pickup, Enemy, ammo, or life, each carry a weight, so behaviors can be added, removed, or tuned quickly, for example [in this script](https://github.com/daltonbr/TopDownShooter/blob/master/Assets/Scripts/AIManager/MoveToBestPosition.cs). Adding a new consideration, like avoiding bombs, is just a new weighted distance check. This approach is easier to maintain and tweak than the more established FSMs (Finite State Machines) or Behaviour Trees.