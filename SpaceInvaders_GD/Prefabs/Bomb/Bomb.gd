extends RigidBody2D

# Editor State
@export var Speed: float = 50


# Bomb added to scene
func _ready():
	# Start moving down
	linear_velocity = Vector2.DOWN * Speed

	# Watch for collisions
	connect("body_entered", _on_body_entered)


## Bomb collided with something
func _on_body_entered(other: Node2D):
	if other.is_in_group("Bullet"):
		print("Bomb " + name + " hit " + other.name)
		# Show an explosion where the bullet hit the bomb
		var explosion = preload("res://Prefabs/Bomb/Bomb_Explosion.tscn").instantiate()
		explosion.global_position = global_position
		get_tree().get_root().add_child(explosion)
	else:
		# ToDo: bomb collide with shield
		pass

	# Remove the bomb
	queue_free()
