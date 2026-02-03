class_name CustomSignals
extends Node


## An alien has perished
signal AlienDied(alien)

## It's stompin' time
signal Stomp()

## The score has changed
signal ScoreChanged(newScore: int)

## Number of lives has changed
signal LivesChanged(newLives: int)

## The swarm has been vanquished!
signal SwarmDeath(points: int)
