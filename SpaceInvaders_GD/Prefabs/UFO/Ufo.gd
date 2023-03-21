@icon("res://Prefabs/Alien/Sprites/Alien1A.png")
class_name Ufo
extends Node2D


# Child Nodes
@onready var UfoPixels: PixelGroup = $UfoPixels
@onready var UfoThreatSound: AudioStreamPlayer2D = $UfoThreatSound
@onready var UfoDeathSound: AudioStreamPlayer2D = $UfoDeathSound
@onready var gm: GameManager = get_node("/root/GameManager")

# Editor State
@export var Speed: float = 200

# Private State
var Dead: bool = false
var XMax: float
var XPadding: float = 20


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	XMax = XViewport.Size().x / 2 + XPadding
	UfoPixels.PixelGroupCollision.connect(UfoPixelsCollision)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if Dead == false:
		position += Vector2(Speed * delta, 0)
		if (position.x > XMax):
			queue_free()


## Ufo pixels hit by something
func UfoPixelsCollision(_pixel: PixelPrefab, other: Node2D) -> void:
	if other.is_in_group("Bullets"):
		UfoDeath()


## Ufo is dead
func UfoDeath() -> void:
	if Dead == false:
		Dead = true
		gm.ScoreAdd(randi_range(2, 5) * 100)
		UfoThreatSound.stop()
		UfoDeathSound.play()
		await UfoPixels.ExplodeFrom(global_position, 400)
		await UfoDeathSound.finished
		queue_free()

