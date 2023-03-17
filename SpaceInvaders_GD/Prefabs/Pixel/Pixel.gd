class_name PixelPrefab
extends Node2D

# Child Nodes
@onready var RB: RigidBody2D = $RigidBody
@onready var PixelMesh: MeshInstance2D = $RigidBody/SquareMesh

# Private State
var PixelType = ""


# Called when the node enters the scene tree for the first time.
func _ready():
	pass


## Set the pixels properties from the group
func InitPixel(group: String, color: Color, collisionLayer, collisionMask):
	# print("InitPixel " + name + " in group " + group + " color=" + str(color))
	PixelType = group
	add_to_group(group)
	RB.add_to_group(group)
	RB.collision_layer = collisionLayer
	RB.collision_mask = collisionMask
	PixelMesh.modulate = color


## Something collided with the pixel
func _on_rigid_body_body_entered(other: Node2D):
	match PixelType:
		"Shields":
			if (other.is_in_group("Bullets") || other.is_in_group("Bombs") || other.is_in_group("Aliens") || other.is_in_group("ShieldExplosions")):
				var otherPos: Vector2 = other.global_position
				BeginPixelDestroy.call_deferred(otherPos)
			else:
				# print("Pixel " + name + " collided with " + other.name)
				pass
		"ShieldExplosions":
			queue_free()
		_:
			print("Pixel collied of unknown type: " + PixelType)


## Pixel is being destroyed
func BeginPixelDestroy(otherPos: Vector2):
	RB.freeze = false
	RB.contact_monitor = false
	RB.lock_rotation = true
	RB.collision_layer = 0
	RB.max_contacts_reported = 0
	await XDelay.NextPhysicsFrame()
	#RB.apply_central_impulse(Vector2(otherPos.x - global_position.x + randf_range(-0.5, 0.1), -2) * 400)
	RB.apply_central_impulse(Vector2(otherPos.x - global_position.x + randf_range(-0.5, 0.1), global_position.y - otherPos.y ).normalized() * 400)
	var tween = create_tween().set_parallel().set_trans(Tween.TRANS_CUBIC)
	tween.tween_property(PixelMesh, "scale", Vector2(8,8), 0.7)
	tween.tween_property(PixelMesh, "modulate", Color(PixelMesh.modulate, 0), 0.7)
	await tween.finished
	queue_free()
