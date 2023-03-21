@icon("res://Prefabs/Alien/Sprites/AlienDeath.png")
class_name ShieldExplosion
extends Node2D

# Editor State
@export var FadeSeconds = 1.0

# Child Nodes
@onready var ExplosionLight: PointLight2D = $ExplosionLight
@onready var ExplosionSound: AudioStreamPlayer2D = $ExplosionSound
@onready var ExplosionPixels: PixelGroup = $ExplosionPixels

# Private State
var Age:= 0.0
var LightEnergy: float


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	LightEnergy = ExplosionLight.energy
	ExplosionPixels.PixelGroupCollision.connect(OnExplostionPixelCollision)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	Age += delta
	var opacity = clamp(1.0 - (Age / FadeSeconds), 0.0, 1.0)
	ExplosionLight.energy = LightEnergy * opacity

	if opacity <= 0 && !ExplosionSound.playing:
		queue_free()


## Explostion pixel collided with something
func OnExplostionPixelCollision(pixel: PixelPrefab, _other: Node2D) -> void:
	pixel.DestroySilent()
