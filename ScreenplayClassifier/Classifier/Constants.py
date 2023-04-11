import json
from pathlib import Path

movie_info_path = Path("Jsons/Movie Script Info.json")

genres_path = Path("Jsons/Genres.json")
genre_labels = json.loads(genres_path.read_text())

classifier_path = Path("Classifier")
model_pickle_path = classifier_path / "Model.pkl"
train_csv_path = classifier_path / "Train.csv"
test_csv_path = classifier_path / "Test.csv"

train_screenplays_directory = Path("TrainScreenplays")
test_screenplays_directory = Path("TestScreenplays")
train_screenplays_paths = list(train_screenplays_directory.glob("*.txt"))
test_screenplays_paths = list(test_screenplays_directory.glob("*.txt"))