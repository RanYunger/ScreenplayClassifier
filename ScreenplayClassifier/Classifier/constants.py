# Imports
import json

from pathlib import Path


# Folders
TRAIN_SCREENPLAYS_PATH = Path("TrainScreenplays")

CLASSIFIER_PATH = Path("Classifier")


# Files
MOVIE_INFO_PATH = Path("Jsons/Movie Script Info.json")
GENRES_PATH = Path("Jsons/Genres.json")

TRAIN_SCREENPLAYS_PATHS = list(TRAIN_SCREENPLAYS_PATH.glob("*.txt"))

MODEL_PICKLE_PATH = CLASSIFIER_PATH / "Model.pkl"
TRAIN_CSV_PATH = CLASSIFIER_PATH / "Train.csv"


# Variables
GENRE_LABELS = json.loads(GENRES_PATH.read_text())

TRAIN_MODE = False
DATA_SPLIT = False
FEATURES_COUNT = 5000
DECISION_TREE_DEPTH = 7
