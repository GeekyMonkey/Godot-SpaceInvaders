@icon("res://Prefabs/Player/Sprites/Player.png")
class_name Player
extends Node2D

# Editor State
@export var MoveSpeed: float = 500
@export var ReloadSec: float = 0.25

# Child Nodes
@onready var GunPosition: Marker2D = $GunPosition
@onready var gm: GameManager = get_node("/root/GameManager")
@onready var ExplosionPrefab = preload("res://Prefabs/Player/Player_Explosion.tscn")

# Private State
var ScreenSizeX: float
var XMin: float
var XMax: float
var XMargin: float = 16


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	ScreenSizeX = XViewport.Size().x
	XMin = ScreenSizeX / -2 + XMargin
	XMax = ScreenSizeX / 2 - XMargin
	CS.SwarmDeath.connect(SwarmDied)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	ProcessMoveInput(delta)
	ProcessShootInput(delta)


## Left/right movement
func ProcessMoveInput(delta: float) -> void:
	var input: float = 0
	if Input.is_action_pressed("ui_right"):
		input = 1.0
	elif Input.is_action_pressed("ui_left"):
		input = -1.0

	var newX: float = position.x + input * MoveSpeed * delta
	newX = clamp(newX, XMin, XMax)
	newX = round(newX)
	position = Vector2(newX, position.y)


## Shoot Input
func ProcessShootInput(_delta: float) -> void:
	var input: float = 0
	if (Input.is_action_just_pressed("Shoot")):
		input = 1

	if input == 1:
		var bullet = preload("res://Prefabs/Bullet/Bullet.tscn").instantiate()
		bullet.global_position = GunPosition.global_position
		get_tree().get_root().add_child(bullet)


## Something collided with the player
func _on_area_2d_body_entered(other: Node2D) -> void:
	print("Player hit by " + other.name)
	if other.is_in_group("Bombs"):
		await ExplodePlayer()
	elif other.is_in_group("Aliens"):
		await ExplodePlayer()


# Explode the player
func ExplodePlayer() -> void:
	var explosion = ExplosionPrefab.instantiate()
	explosion.global_position = global_position
	get_tree().get_root().add_child(explosion)
	await XDelay.Seconds(0.1)
	queue_free()
	gm.PlayerDied()


# The Swarm died
func SwarmDied(points: int) -> void:
	if (points == 0):
		# Swarm reached the bottom
		ExplodePlayer()
