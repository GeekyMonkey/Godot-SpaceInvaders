class_name TitleScene
extends Node2D


# Child Nodes
@onready var PlayButton: Button = $Environment/UI/VBoxContainer/ActionsPanel/PlayButton


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	PlayButton.grab_focus()
	PlayButton.pressed.connect(PlayButtonClicked)


## Play button was clicked
func PlayButtonClicked() -> void:
	Main.StartGame()
