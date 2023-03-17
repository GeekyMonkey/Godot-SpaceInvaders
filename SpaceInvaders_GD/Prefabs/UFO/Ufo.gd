class_name Ufo
extends Node2D

# Editor State
@export var Speed: float = 200

# Private State
var XMax: float
var XPadding: float = 20


# Called when the node enters the scene tree for the first time.
func _ready():
	XMax = XViewport.Size().x / 2 + XPadding


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	position += Vector2(Speed * delta, 0)
	if (position.x > XMax):
		queue_free()
