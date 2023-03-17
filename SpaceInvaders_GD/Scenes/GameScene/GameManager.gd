class_name GameManager
extends Node2D

# Public State
var Level = 1
var Score = 0
var PlayerLives = 3


# Called when the node enters the scene tree for the first time.
func _ready():
	# Listen for alien deaths
	CS.connect("AlienDied", OnAlienDied)

	# Listen for swarm deaths
	CS.connect("SwarmDeath", OnSwarmDeath)


func OnSwarmDeath():
	# Add a bunch of points
	await XDelay.Seconds(0.5)
	ScoreAdd(201)
	await XDelay.Seconds(2.5)

	# Level up
	SpawnNewSwarm()


# An alien is pining for the fjords
func OnAlienDied(alien):
	var points: int = alien.Points
	ScoreAdd(points)


# Add some points
func ScoreAdd(points: int):
	Score += points

	# Alert the text box
	CS.ScoreChanged.emit(Score);


# Spawn a new swarm
func SpawnNewSwarm():
	Level += 1
	var swarm = preload("res://Prefabs/Swarm/Swarm.tscn").instantiate()
	swarm.SwarmType = Level - 1
	get_tree().get_root().add_child(swarm)


# The player has lost a life
func PlayerDied():
	PlayerLives -= 1
	CS.LivesChanged.emit(PlayerLives)

	if PlayerLives > 0:
		var t = get_tree().create_timer(2.0)
		await t.timeout
		SpawnPlayer()


# Spawn a player
func SpawnPlayer():
	var spawnMarker: Marker2D = get_node("PlayerSpawnPoint")
	var player = preload("res://Prefabs/Player/Player.tscn").instantiate()
	player.global_position = spawnMarker.global_position
	get_tree().get_root().add_child(player)
