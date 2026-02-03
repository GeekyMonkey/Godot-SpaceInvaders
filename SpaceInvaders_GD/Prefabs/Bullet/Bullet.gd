class_name Bullet
extends RigidBody2D

# Editor State
@export var Speed: float = 400


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	# Start it moving
	linear_velocity = Vector2.UP * Speed

	# Watch for collisions
	# connect("body_entered", _on_body_entered(other))


## Something collided with the bullet. The nerve!
func _on_body_entered(other: Node) -> void:
	if other.is_in_group("Aliens"):
		BulletHitAlien(other)
	elif other.is_in_group("Bombs"):
		BulletHitBomb(other)
	elif other.is_in_group("Shields"):
		BulletHitShieldPixel(other)
	elif other.is_in_group("Ufo"):
		BulletHitUfo(other)
	elif other.is_in_group("ShieldExplosions"):
		print("Bullet doesn't care about shield explosion")
	else:
		print("Bullet hit " + str(other.get_groups()))
		queue_free()


## We have struck a shield!
func BulletHitShieldPixel(_shieldPixel) -> void:
	var explosion = preload("res://Prefabs/Sheild/ShieldExplosion.tscn").instantiate()
	explosion.global_position = global_position
	get_tree().get_root().add_child.call_deferred(explosion)
	queue_free()


## We have struck an alien!
func BulletHitAlien(_alien: Alien) -> void:
	# The alien knows this is bad. Just remove the bullet
	queue_free()


## We have struck a UFO
func BulletHitUfo(_ufo):
	# The ufo knows this is bad. Just remove the bullet
	queue_free()


## The bullet hit a bomb. What luck!
func BulletHitBomb(_bomb):
	# GD.Print("Bullet hit bomb " + bomb.Name)
	get_node("/root/GameManager").ScoreAdd(3)

	# Delete the bullet from the scene
	queue_free()
