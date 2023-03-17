class_name GunSprite
extends PointLight2D

# Editor State
@export var Speed: float = 10

# Private state
var Age: float = 0
var Energy: float

# Called when the node enters the scene tree for the first time.
func _ready():
	Energy = 5 # self.energy # Get the designed energy as the max


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	Age += delta
	# energy = max(1.0,(sin(Age * Speed) / 2.0 + 0.5) * Energy)
	enabled = (sin(Age * Speed)) > 0
