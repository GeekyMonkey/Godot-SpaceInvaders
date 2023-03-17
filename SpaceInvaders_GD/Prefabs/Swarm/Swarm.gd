class_name Swarm
extends Node2D

# Editor State
@export_category("Sounds")
@export var StompSounds: Array[AudioStream]

@export_category("Swarm Geometry")
@export var SpacingX: float = 16
@export var SpacingY: float = 16
@export var XMargin: float = 8

@export_category("Swarm Movement")
@export var StepX: int = 8
@export var StepY: int = 8

# Child Nodes
@onready var StompSoundPlayer: AudioStreamPlayer2D = $StompSoundPlayer

# Public State
var SwarmType: int = 0

# Private State
var DirectionX: int = 1 # 1 or -1
var IsStomping: bool = true
var ScreenSizeX: float
var StompSoundIndex: int = 0
var SwarmDead: bool = false
var SwarmExtents: Rect2
var XMin: float
var XMax: float

var SwarmPattern1: Array[String] = [
	"23332",
	" 222 ",
	" 121 ",
	"  1  "
]

var SwarmPattern2: Array[String] = [
	"33333333333",
	"22222222222",
	"22222222222",
	"11111111111",
	"11111111111"
]


# Called when the node enters the scene tree for the first time.
func _ready():
	# It's stompin' time!
	CS.connect("Stomp", Stomp)

	# When an alien dies, re-check the swarm size
	CS.connect("AlienDied", func(_alien):
		MeasureExtents()
	)

	# The player is out of lives - stop stomping
	CS.connect("LivesChanged", func(lives: int):
		if lives == 0:
			IsStomping = false
	)

	# Measure the screen
	ScreenSizeX = get_viewport_rect().size.x / 3
	XMin = ScreenSizeX / -2 + XMargin
	XMax = ScreenSizeX / 2 - XMargin

	# Create the swarm
	await get_tree().process_frame
	CreateSwarm(SwarmType)


# Timer says it's stomping time. Alert the aliens!
func StompTimer():
	if !SwarmDead:
		CS.Stomp.emit()


# Move the swarm and make the sound
func Stomp():
	if IsStomping:
		# Play a sound
		StompSoundIndex += 1
		var stompSoundCount: int = StompSounds.size()
		if stompSoundCount > 0:
			StompSoundIndex = StompSoundIndex % stompSoundCount
			StompSoundPlayer.stream = StompSounds[StompSoundIndex]
			StompSoundPlayer.play()

		# Move the swarm
		var dX: float = DirectionX * StepX
		position += Vector2(dX, 0)
		if (position.x + SwarmExtents.position.x) <= XMin || (position.x + SwarmExtents.end.x) >= XMax:
			DirectionX *= -1
			position += Vector2(-dX, StepY)


# Measure the size of the swarm so we can tell if a side is hit
func MeasureExtents():
	await get_tree().process_frame
	var children = get_children()
	var left = 999999
	var top = 999999
	var bottom = -999999
	var right = -999999
	var alienCount: int = 0
	for child in children:
		if child.is_in_group("Aliens"):
			var alien = child
			if alien.Dead:
				continue
			alienCount += 1
			var extents: Rect2 = alien.Extents
			var pos: Vector2 = alien.position
			left = min(left, pos.x + extents.position.x)
			top = min(top, pos.y + extents.position.y)
			right = max(right, pos.x + extents.end.x)
			bottom = max(bottom, pos.y + extents.end.y)

	if alienCount == 0:
		SwarmDead = true
		await get_tree().create_timer(1.0).timeout # Allow last alien death sound to finish playing
		CS.SwarmDeath.emit()
		queue_free()
	else:
		SwarmExtents = Rect2(left, top, right - left, bottom - top)
		# GD.Print("Swarm of " + alienCount + " extents: " + SwarmExtents)
		# GetNode<Sprite2D>("ExtentsMarker1").Position = SwarmExtents.Position
		# GetNode<Sprite2D>("ExtentsMarker2").Position = SwarmExtents.End


## Create a swarm of the specified type
func CreateSwarm(swarmType: int):
	await get_tree().process_frame
	print("Creating swarm type " + str(swarmType))
	ClearSwarm()

	var swarmPattern: Array[String]
	swarmType = swarmType % 2
	match (swarmType):
		0:
			swarmPattern = SwarmPattern1
		1:
			swarmPattern = SwarmPattern2

	var rows: int = swarmPattern.size()
	var columns: int = 0
	for row in swarmPattern:
		columns = max(columns, row.length())

	var left: float = (columns / 2.0) * -SpacingX
	var top: float = (rows / 2.0) * -SpacingY
	for row in rows:
		var rowChars = swarmPattern[row]
		for col in columns:
			var alienTypeIndex = rowChars[col]
			var alienType: PackedScene
			match alienTypeIndex:
				'1':
					alienType = preload("res://Prefabs/Alien/Alien_1.tscn")
				'2':
					alienType = preload("res://Prefabs/Alien/Alien_2.tscn")
				'3':
					alienType = preload("res://Prefabs/Alien/Alien_3.tscn")
				_:
					alienType = null
			if alienType != null:
				var x: float = left + col * SpacingX
				var y: float = top + row * SpacingY
				# print("Spawn alien " + str(alienTypeIndex) + " at " + str(x) + "," + str(y))
				var alien = alienType.instantiate()
				alien.position = Vector2(x,y)
				add_child.call_deferred(alien)

	await get_tree().create_timer(0.1).timeout
	await get_tree().process_frame
	MeasureExtents()


## Remove any aliens from the swarm
func ClearSwarm():
	var children = get_children(false)
	for child in children:
		if child.is_in_group("Aliens"):
			child.queue_free()
