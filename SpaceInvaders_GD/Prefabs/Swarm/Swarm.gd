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
@export var SpeedMin: float = 0.5
@export var SpeedMax: float = 6
@export var StepX: int = 8
@export var StepY: int = 12

# Child Nodes
@onready var StompSoundPlayer: AudioStreamPlayer2D = $StompSoundPlayer
@onready var StompTimer: Timer = $StompTimer
@onready var gm: GameManager = get_node("/root/GameManager")

# Public State
var SwarmType: int = 0

# Private State
var AlienCount: int
var AlienCountStart: int
var DirectionX: int = 1 # 1 or -1
var IsStomping: bool = true
var ScreenSizeX: float
var Speed: float
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
	StompTimer.timeout.connect(StompTime)

	# When an alien dies, re-check the swarm size
	CS.connect("AlienDied", func(_alien):
		MeasureExtents()
		SetSpeed()
	)

	# The player is out of lives - stop stomping
	CS.connect("LivesChanged", func(lives: int):
		if lives == 0:
			IsStomping = false
	)

	# Measure the screen
	ScreenSizeX = XViewport.Size().x
	XMin = ScreenSizeX / -2 + XMargin
	XMax = ScreenSizeX / 2 - XMargin

	# Create the swarm
	CreateSwarm.call_deferred(SwarmType)


# Timer says it's stomping time. Alert the aliens!
func StompTime():
	if !SwarmDead:
		Stomp()
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

		# Move the swarm X
		var dX: float = DirectionX * StepX
		position += Vector2(dX, 0)
		if (position.x + SwarmExtents.position.x) <= XMin || (position.x + SwarmExtents.end.x) >= XMax:
			# Move swarm down
			DirectionX *= -1
			position += Vector2(-dX, StepY)
			# Is swarm at bottom
			if (global_position.y + SwarmExtents.end.y >= gm.PlayerSpawnPoint.position.y - 8):
				SwarmDead = true
				CS.SwarmDeath.emit(0)
				await XDelay.Seconds(1)
				if (gm.PlayerLives > 0):
					queue_free()


# Measure the size of the swarm so we can tell if a side is hit
func MeasureExtents():
	await XDelay.NextFrame()
	var children = get_children()
	var left = 999999
	var top = 999999
	var bottom = -999999
	var right = -999999

	AlienCount = 0

	for child in children:
		if child.is_in_group("Aliens"):
			var alien = child
			if alien.Dead:
				continue
			AlienCount += 1
			var extents: Rect2 = alien.Extents
			var pos: Vector2 = alien.position
			left = min(left, pos.x + extents.position.x)
			top = min(top, pos.y + extents.position.y)
			right = max(right, pos.x + extents.end.x)
			bottom = max(bottom, pos.y + extents.end.y)

	if AlienCount == 0:
		SwarmDead = true
		await XDelay.Seconds(1.0) # Allow last alien death sound to finish playing
		CS.SwarmDeath.emit(500)
		queue_free()
	else:
		SwarmExtents = Rect2(left, top, right - left, bottom - top)
		# GD.Print("Swarm of " + alienCount + " extents: " + SwarmExtents)
		# GetNode<Sprite2D>("ExtentsMarker1").Position = SwarmExtents.Position
		# GetNode<Sprite2D>("ExtentsMarker2").Position = SwarmExtents.End


## Create a swarm of the specified type
func CreateSwarm(swarmType: int):
	await XDelay.NextFrame()
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
				AlienCount += 1

	await XDelay.Seconds(0.1)
	MeasureExtents()
	AlienCountStart = AlienCount
	SetSpeed()


## Remove any aliens from the swarm
func ClearSwarm():
	var children = get_children(false)
	for child in children:
		if child.is_in_group("Aliens"):
			child.queue_free()


## Set the swarm's move speed
func SetSpeed():
	Speed = SpeedMax - (SpeedMax - SpeedMin) * ((AlienCount - 1)  as float / (1.0 if AlienCountStart == 0 else AlienCountStart as float))
	StompTimer.wait_time = 1.0 / (Speed * Speed)
	print("Speed=" + str(Speed) + " Interval=" + str(StompTimer.wait_time) + " Count=" + str(AlienCount) + " Start=" + str(AlienCountStart))
