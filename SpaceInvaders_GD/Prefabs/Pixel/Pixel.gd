@icon("res://Prefabs/Pixel/PixelIcon.svg")
class_name PixelPrefab
extends Node2D


# Signals
signal OnCollision(other: Node2D)

# Child Nodes
@onready var RB: RigidBody2D = $RigidBody
@onready var PixelMesh: MeshInstance2D = $RigidBody/SquareMesh


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass


## Set the pixels properties from the group
func InitPixel(group: String, color: Color, collisionLayer, collisionMask) -> void:
	# print("InitPixel " + name + " in group " + group + " color=" + str(color))
	add_to_group(group)
	RB.add_to_group(group)
	RB.collision_layer = collisionLayer
	RB.collision_mask = collisionMask
	PixelMesh.modulate = color


## Something collided with the pixel
func _on_rigid_body_body_entered(other: Node2D) -> void:
	# print("Pixel collied of unknown type: " + PixelType)
	OnCollision.emit(self, other)


## Just make it go away
func DestroySilent() -> void:
	queue_free()


## Pixel is being destroyed with an explosive force
func DestroyExplodeFrom(otherPos: Vector2, force: float):
	_DestroyExplodeFrom.call_deferred(otherPos, force)


## Pixel is being destroyed with an explosive force (private)
func _DestroyExplodeFrom(otherPos: Vector2, force: float):
	RB.freeze = false
	RB.contact_monitor = false
	RB.lock_rotation = true
	RB.collision_layer = 0
	RB.max_contacts_reported = 0
	await XDelay.NextPhysicsFrame()
	var direction: Vector2 = (global_position - otherPos).normalized()
	var randomDirection = Vector2(randf_range(-.1, .1), randf_range(-.2, .1))
	var randomForce = randf_range(0.9, 1.1)
	RB.apply_central_impulse((direction + randomDirection).normalized() * force * randomForce)
	var tween = create_tween().set_parallel().set_trans(Tween.TRANS_CUBIC)
	tween.tween_property(PixelMesh, "scale", Vector2(8,8), 0.7)
	tween.tween_property(PixelMesh, "modulate", Color(PixelMesh.modulate, 0), 0.7)
	await tween.finished
	queue_free()
