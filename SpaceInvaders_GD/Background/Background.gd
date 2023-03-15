extends Sprite2D


# Size the sprite texture
func _ready():

	var viewportSize = get_viewport_rect().size
	var textureSize = texture.get_size()
	var newScale = viewportSize.x / textureSize.x
	apply_scale(Vector2(newScale, newScale))
	print("Background scale = " + str(newScale))
