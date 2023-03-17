@icon("res://Prefabs/Pixel/PixelGroupIcon.svg")
class_name PixelGroup
extends Node2D


## Editor State
@export var PixelColor: Color
@export var CollisionLayer: int = 0
@export var CollisionMask: int = 0

# Signals
signal PixelGroupCollision(pixel: PixelPrefab, object: Node2D)


# Called when the node enters the scene tree for the first time.
func _ready():
	var pixels = find_children("Pixel*", "", true) as Array[PixelPrefab]
	print("Pixel group " + name + " has " + str(pixels.size()) + " pixels")
	var group = GetGroupFromParent()
	for pixel in pixels:
		pixel.InitPixel(group, PixelColor, CollisionLayer, CollisionMask)
		pixel.OnCollision.connect(OnPixelCollision)


## Get the group from the pixel group settings, or its parent
func GetGroupFromParent():
	var group = ""
	if get_groups().size() > 0:
		group = get_groups()[0]
	else:
		if get_parent().get_groups().size() > 0:
			group = get_parent().get_groups()[0]
	if group == "":
		group = get_parent().name
	return group


## A pixel got a collision - broadcast signal to parent
func OnPixelCollision(pixel: PixelPrefab, other: Node2D):
	PixelGroupCollision.emit(pixel, other)


## Explode all pixels in the group from the group origin
func ExplodeFrom(center: Vector2, force: float):
	var pixels = find_children("Pixel*", "", true) as Array[PixelPrefab]
	print("Exploding pixel group " + name + " has " + str(pixels.size()) + " pixels")
	for pixel in pixels:
		pixel.DestroyExplodeFrom(center, force)
	await XDelay.Seconds(2)


## Explode all pixels in the group from the group origin
func ExplodeFromGroupOrigin(force: float):
	ExplodeFrom(global_position, force)
