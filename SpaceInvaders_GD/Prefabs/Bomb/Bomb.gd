@icon("res://Prefabs/Bomb/Sprites/Bomb1B.png")
class_name Bomb
extends RigidBody2D

# Editor State
@export var Speed: float = 50


# Bomb added to scene
func _ready() -> void:
	# Start moving down
	linear_velocity = Vector2.DOWN * Speed

	# Watch for collisions
	connect("body_entered", _on_body_entered)


## Bomb collided with something
func _on_body_entered(other: Node2D) -> void:
	if other.is_in_group("Bullets"):
		print("Bomb " + name + " hit " + other.name)
		# Show an explosion where the bullet hit the bomb
		var explosion = preload("res://Prefabs/Bomb/Bomb_Explosion.tscn").instantiate()
		explosion.global_position = global_position
		get_tree().get_root().add_child(explosion)
	if other.is_in_group("Shields"):
		BombHitShieldPixel(other)
	else:
		# ToDo: bomb collide with other stuff?
		pass

	# Remove the bomb
	queue_free()


## We have struck a shield!
func BombHitShieldPixel(_shieldPixel) -> void:
	var explosion = preload("res://Prefabs/Sheild/ShieldExplosion.tscn").instantiate()
	explosion.global_position = global_position + Vector2(0,6)
	explosion.rotation_degrees = randf_range(90, 270)
	explosion.apply_scale(Vector2(1.5,1.5))
	get_tree().get_root().add_child.call_deferred(explosion)
