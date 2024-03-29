@icon("res://Scenes/TitleScene/Ufo.png")
class_name UfoSpawner
extends Node2D

# Editor State
@export var MinSeconds: float = 10
@export var MaxSeconds: float = 30

# Prefabs & References
@onready var UfoPrefab = preload("res://Prefabs/UFO/Ufo.tscn")
@onready var gm: GameManager = get_node("/root/GameManager")

# Private State
var UfoTimer: Timer


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	UfoTimer = Timer.new()
	add_child(UfoTimer)
	UfoTimer.timeout.connect(LaunchTime)
	var u = UfoPrefab
	StartTimer()


## Start/restart the timer
func StartTimer() -> void:
	var seconds = randf_range(MinSeconds, MaxSeconds)
	print("Launching UFO in " + str(seconds) + " seconds")
	UfoTimer.wait_time = seconds
	UfoTimer.start()


## Time to launch the UFO
func LaunchTime() -> void:
	print("Launch Time")
	# Only if the player is still alive
	if (gm.PlayerLives > 0):
		LaunchUfo()
		# Get ready for the next one
		StartTimer()


## Launch now
func LaunchUfo() -> void:
	print("Launch UFO")
	var ufo: Ufo = UfoPrefab.instantiate()
	ufo.global_position = global_position
	gm.add_child(ufo)
