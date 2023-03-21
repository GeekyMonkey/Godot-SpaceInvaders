extends Node


# Scenes
@onready var TitleScene = preload("res://Scenes/TitleScene/Title.tscn")
@onready var GameScene = preload("res://Scenes/GameScene//Game.tscn")


func StartGame() -> void:
	print("Start game")
	get_tree().change_scene_to_packed(GameScene)
