@icon("res://Prefabs/Sheild/Sprites/Shield.png")
class_name Shield
extends Node2D

# Child Nodes
@onready var ShieldPixels: PixelGroup = $ShieldPixels


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	ShieldPixels.PixelGroupCollision.connect(OnShieldPixelCollision)


## Shield pixel collided with something
func OnShieldPixelCollision(pixel: PixelPrefab, other: Node2D) -> void:
	if (other.is_in_group("Bullets") || other.is_in_group("Bombs") || other.is_in_group("Aliens") || other.is_in_group("ShieldExplosions")):
		pixel.DestroyExplodeFrom(other.global_position, 300)
		# print("Shield exploding from " + other.name)
	else:
		pass # print("Shield collided with " + other.name)
