# Imports
import json

import pandas, os, pathlib, sys

from Classifier import *
from TextProcessor import *

# Methods
def read_train_screenplays():
    screenplays_directory = f"./TrainScreenplays/"
    file_paths = os.listdir(screenplays_directory)
    screenplays_dict = {}

    # Builds a dictionary of screenplay text by its title
    for file_path in file_paths:
        screenplay_title = pathlib.Path(file_path).stem
        screenplay_text = open(f"{screenplays_directory}{file_path}", "r", encoding="utf8").read()
        screenplays_dict[screenplay_title] = process_text(screenplay_text)

    return pandas.DataFrame({"Title": screenplays_dict.keys(), "Text": screenplays_dict.values()})

def read_test_screenplays(file_paths):
    screenplays_dict = {}

    # Builds a dictionary of screenplay text by its title
    for file_path in file_paths:
        screenplay_title = pathlib.Path(file_path).stem
        screenplay_text = open(file_path, "r", encoding="utf8").read()
        screenplays_dict[screenplay_title] = screenplay_text #process_text(screenplay_text)

    return pandas.DataFrame({"Title": screenplays_dict.keys(), "Text": screenplays_dict.values()})

def read_genres():
    genre_labels = json.load(open("Jsons/Genres.json"))
    info_ds = pandas.read_json("Movie Script Info.json")
    genres_dict = {}

    # Builds a dictionary of screenplay genres by its title
    for offset, info in info_ds.iterrows():
        genres_dict[info["title"]] = info["genres"]

    return pandas.DataFrame({"Title": genres_dict.keys(), "Actual Genres": genres_dict.values()})

# Main
if __name__ == "__main__":
    # Loads test screenplays (script's arguments)
    test_screenplays = read_test_screenplays(sys.argv[1:])

    # Loads model variables from pickle and trains them (if necessary)
    model_variables = train()

    # Classifies test screenplays
    classifications = classify(model_variables, test_screenplays)

    """
    OUTPUT EXAMPLE:
    Title               |   GenrePercentages        
    "american psycho"       {"Action"   : 23.67, "Adventure": 12.92 ... }
    """

    # Prints classifications to process
    print(classifications.to_json(orient="records", indent=4))