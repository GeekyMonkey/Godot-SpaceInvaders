extends Node2D


## Get the visible playfield size
func Size() -> Vector2:
	var viewportSize = get_viewport_rect().size
	var canvasScale = get_canvas_transform().get_scale()
	return Vector2(viewportSize.x / canvasScale.x, viewportSize.y / canvasScale.y)
