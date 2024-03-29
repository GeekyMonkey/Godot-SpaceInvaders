class_name GameManager
extends Node2D


# Child Nodes
@onready var PlayerSpawnPoint: Marker2D = $PlayerSpawnPoint
@onready var UI: CanvasLayer = get_node("./Environment/UI")

# Public State
var Level = 1
var Score = 0
var PlayerLives = 3


# Called when the node enters the scene tree for the first time.
func _ready() -> void:

	# Listen for alien deaths
	CS.connect("AlienDied", OnAlienDied)

	# Listen for swarm deaths
	CS.connect("SwarmDeath", OnSwarmDeath)


## Game Inputs - Pause
func _input(event: InputEvent) -> void:
	if event.is_action_pressed("Pause"):
		get_tree().paused = !get_tree().paused


## Swarm Died
func OnSwarmDeath(points: int) -> void:
	# Add a bunch of points
	await XDelay.Seconds(0.5)
	ScoreAdd(points)
	await XDelay.Seconds(2.5)

	# Level up
	if (PlayerLives > 0):
		SpawnNewSwarm()


# An alien is pining for the fjords
func OnAlienDied(alien) -> void:
	var points: int = alien.Points
	ScoreAdd(points)


# Add some points
func ScoreAdd(points: int) -> void:
	Score += points

	# Alert the text box
	CS.ScoreChanged.emit(Score)


# Spawn a new swarm
func SpawnNewSwarm() -> void:
	Level += 1
	var swarm = preload("res://Prefabs/Swarm/Swarm.tscn").instantiate()
	swarm.SwarmType = Level - 1
	get_tree().get_root().add_child(swarm)


# The player has lost a life
func PlayerDied() -> void:
	PlayerLives -= 1
	CS.LivesChanged.emit(PlayerLives)

	if PlayerLives > 0:
		var t = get_tree().create_timer(2.0)
		await t.timeout
		SpawnPlayer()
	else:
		var gameOverPanel = preload("res://Background/GameOverPanel.tscn").instantiate()
		UI.add_child(gameOverPanel)


# Spawn a player
func SpawnPlayer() -> void:
	var player = preload("res://Prefabs/Player/Player.tscn").instantiate()
	player.global_position = PlayerSpawnPoint.global_position
	get_tree().get_root().add_child(player)
