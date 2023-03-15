extends RigidBody2D

# Editor State
@export var BombTypes: int = 2
@export var Points: int = 10
@export var ReloadSecMin: float = 1
@export var ReloadSecMax: float = 15

#Child nodes
@onready var AnimatedSprite: AnimatedSprite2D = $AnimatedSprite
@onready var CollisionShape: CollisionShape2D = $CollisionShape2D
@onready var GunPosition: Marker2D = $GunPosition
@onready var GunSprite: PointLight2D = $GunSprite

# Public State
var Dead: bool = false
var Extents: Rect2

# Private State
var Width: float = 0
var Height: float = 0
var PlayerIsDead: bool = false
var ReloadTime: float = 1
var SpriteScale: float = 1
var ViewIsClear: bool = false


## Alien added to the scene
func _ready():
	print ("Alien start")

	# Watch for swarm stomp events
	CS.connect("Stomp", Stomp)

	# Watch for alien died event
	CS.connect("AlienDied", OnAlienDied)

	# Watch for player lives changed to stop stomping when he's dead
	CS.connect("LivesChanged", OnLivesChanged)

	connect("body_entered", _on_body_entered)

	# Measure the size of the alien so we can get the size of the swarm
	SpriteScale = CollisionShape.scale.x
	var collisionRect = CollisionShape.shape.get_rect()
	Extents = Rect2(collisionRect.position.x * SpriteScale, collisionRect.position.y * SpriteScale, collisionRect.size.x * SpriteScale, collisionRect.size.y * SpriteScale);
	print ("Alien extents " + str(Extents));

	await get_tree().create_timer(0.5).timeout
	CheckView()


## Has the player used all lives
func OnLivesChanged(lives: int):
	if lives == 0:
		PlayerIsDead = true
		CheckView()


## Begin reloading
func Reload():
	ReloadTime = randf_range(ReloadSecMin, ReloadSecMax)


## An alien has perished. Perhaps now we have a clear shot at the player.
func OnAlienDied(alien):
	if alien != self:
		await get_tree().create_timer(0.5).timeout
		CheckView()


## Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if ViewIsClear == true && ReloadTime > 0:
		ReloadTime -= delta
		if ReloadTime < 0 && !PlayerIsDead:
			Shoot()
			Reload()


## Drop a bomb
func Shoot():
	var bombTypeIndex = randi_range(1, BombTypes)
	print("Alien shoot " + str(bombTypeIndex))
	var bomb: Node
	if bombTypeIndex == 1:
		bomb = preload("res://Prefabs/Bomb/Bomb_1.tscn").instantiate()
	else:
		bomb = preload("res://Prefabs/Bomb/Bomb_2.tscn").instantiate()
	bomb.global_position = GunPosition.global_position
	get_tree().get_root().add_child.call_deferred(bomb)


## Alien has collided with something
func _on_body_entered(otherObject: Node):
	if otherObject.is_in_group("Bullet"):
		HitByBullet(otherObject)


## A bullet has hit this alien
func HitByBullet(_bullet):
	if !Dead:
		Dead = true;

		# Spread the word
		CS.AlienDied.emit(self)

		# Show an explosion
		var explosion = preload("res://Prefabs/Alien/Alien_Explosion.tscn").instantiate()
		add_sibling(explosion)
		explosion.position = position

		# Remove alien from the scene
		queue_free()


## We're all stompin'. Do a little dance.
func Stomp():
	AnimatedSprite.frame = 1 if AnimatedSprite.frame == 0 else 0


## Make sure there are no aliens under this alien's gun
func CheckView():
	var spaceState = get_world_2d().direct_space_state
	var query = PhysicsRayQueryParameters2D.create(global_position, global_position + Vector2(0, 1000), 2, [self])
	var result = spaceState.intersect_ray(query)

	var newViewIsClear: bool = true if result.size() == 0 else false

	# The view has just become clear - Begin reloading
	if newViewIsClear == true && ViewIsClear == false:
		Reload()

	ViewIsClear = newViewIsClear
	GunSprite.texture_scale = 1 if ViewIsClear && !PlayerIsDead else 0
