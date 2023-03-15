extends Node2D

# Editor State
@export var FadeSeconds := 1.1

# Child Nodes
@onready var Sprite: Sprite2D = $AlienDeathSprite
@onready var ExplosionSound: AudioStreamPlayer2D = $ExplosionSound
@onready var ExplosionLight: PointLight2D = $ExplosionLight

# Private State
var Age := 0.0
var LightEnergy: float


# Called when the node enters the scene tree for the first time.
func _ready():
	LightEnergy = ExplosionLight.energy


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	Age += delta
	var opacity = clamp(1-(Age/FadeSeconds), 0, 1)
	Sprite.modulate = Color(Sprite.modulate, opacity)
	ExplosionLight.energy = LightEnergy * opacity
	if opacity <= 0 && !ExplosionSound.playing:
		queue_free()
