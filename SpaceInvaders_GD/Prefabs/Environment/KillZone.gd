extends Area2D


## Something has dared enter the dreaded kill zone
## otherObject = The thing meeting its demise
func _on_body_entered(otherObject: Node):
	print("Kill zone removed " + otherObject.name + " from group " + str(otherObject.get_groups()))
	otherObject.queue_free()
