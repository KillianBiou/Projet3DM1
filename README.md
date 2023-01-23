# Projet3DM1
3D Project in Unity for an assignement in first year of MTI3D2.

# Story
In the near future, a deadly game is playing on everyone screen, The Supreme Game Show.
This show, run by a gamemaster, aims to kill the participant by the use of traps.
The poor participant, infected with a deadly virus agrees to take on the risk to get the cure for his affliction.
Wich side will win in this risky game ?

# Feature
## General
The game feature a network-based versus asymetric gameplay. On player control the gamemaster and aims to kill the player, the other tries to survive in those deadly room.

## Player
The player is a FPS Character with crouch and jump abilities. The player just have to dodge all the traps in the many room that lies ahead of him. between each room, the player have access to a shop to recover HP, getting buff and infilicting debuff to the gamemaster.

## Gamemaster
The Gamemaster is a free camera in the scene. During rest time, the gamemaster can instanciate the next room. During a room, the GM can activate the numerous traps in the room to impede the player's progression.

## Room
There is 3 template of room

- A rectangle room ![Map01](https://user-images.githubusercontent.com/62405524/213994587-88b64fc6-aecf-4b59-918c-2d42f71db4a0.png)

- A tower-like room ![Map02](https://user-images.githubusercontent.com/62405524/213994643-6682e973-2e21-4287-a48d-72183d227a92.png)

- A tunnel-like room ![Map03](https://user-images.githubusercontent.com/62405524/213994717-1dfe6e46-f978-412e-8d18-b6b5f2f88b7e.png)

Each room possess 3 level of difficulty influencing the number of traps in those rooms.

## Card
The gamemaster between each room have the choice to select the next room.
It works with a 3-card selection, where there is 3 random room (Random template, random level).
![image](https://user-images.githubusercontent.com/62405524/213995731-8bd4878a-47d8-4a55-9266-cf82079a4953.png)

## Traps 
There is a total of 7 traps with customisation parameters :

- Spike trap 

![SpikeTrap](https://user-images.githubusercontent.com/62405524/214072752-c3f41407-7c1a-46b0-be23-33bbd1ac66bd.gif)

- Arrow trap

![Arrow](https://user-images.githubusercontent.com/62405524/214073806-f7d5d257-d66b-4beb-98a3-4d9cb6d7b7a3.gif)
