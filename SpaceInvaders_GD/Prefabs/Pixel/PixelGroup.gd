@icon("res://Prefabs/Pixel/PixelGroupIcon.svg")
class_name PixelGroup
extends Node2D


## Editor State
@export var PixelColor: Color
@export var CollisionLayer: int = 0
@export var CollisionMask: int = 0


# Called when the node enters the scene tree for the first time.
func _ready():
	var pixels = find_children("Pixel*", "", true) as Array[PixelPrefab]
	print("Pixel group " + name + " has " + str(pixels.size()) + " pixels")
	var group = "";
	if get_groups().size() > 0:
		group = get_groups()[0]
	for pixel in pixels:
		pixel.InitPixel(group, PixelColor, CollisionLayer, CollisionMask)
