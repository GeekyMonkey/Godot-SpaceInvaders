@icon("res://Prefabs/Alien/Sprites/AlienDeath.png")
class_name BombExplosion
extends Node2D

# Editor State
@export var FadeSeconds = 1.1

# Child Nodes
@onready var ExplosionSound: AudioStreamPlayer2D = $ExplosionSound
@onready var ExplosionLight: PointLight2D = $ExplosionLight
@onready var Sprite: Sprite2D = $BombExplosionSprite

# Private state
var Age: float = 0
var LightEnergy: float


# Called when the node enters the scene tree for the first time.
func _ready():
	LightEnergy = ExplosionLight.energy


# Fade out the explosion
func _process(delta):
	Age += delta

	# Fade the sprite
	var opacity = clamp(1.0 - (Age / FadeSeconds), 0.0, 1.0)
	Sprite.modulate = Color(Sprite.modulate, opacity)

	# Fade the light
	ExplosionLight.energy = LightEnergy * opacity

	# Done fading - remove the explosion
	if opacity <= 0 && !ExplosionSound.playing:
		queue_free()
