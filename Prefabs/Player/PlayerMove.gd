extends Node2D

@export var MOVE_SPEED = 500
						
@onready var _screen_size_x = get_viewport_rect().size.x

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var input = 0
	if (Input.is_action_pressed("ui_right")):
		input = 1
	if (Input.is_action_pressed("ui_left")):
		input = -1
	var newX = position.x + input * MOVE_SPEED * delta
	newX = clamp(newX, _screen_size_x / -2 + 16, _screen_size_x / 2 - 16)
	position.x = newX
