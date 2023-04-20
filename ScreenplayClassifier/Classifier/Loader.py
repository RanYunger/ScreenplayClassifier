# Imports
import math
import time
import pandas
import constants
import screenplay_processor
import multiprocessing

from pathlib import Path
from datetime import datetime
from script_info import ScriptInfo
from concurrent.futures import ThreadPoolExecutor


# Methods
def load_screenplay(file_path):
    # Loads and processes a screenplay by its file path
    screenplay_filename = Path(file_path).stem
    screenplay_text = open(file_path, "r", encoding="utf8").read()

    time.sleep(0.01)

    return {"Filename": screenplay_filename, "Text": screenplay_text}


def load_train_screenplays():
    # Validates existence of required directories
    constants.CLASSIFIER_PATH.mkdir(parents=True, exist_ok=True)
    if not Path.exists(constants.TRAIN_SCREENPLAYS_PATH):
        raise FileNotFoundError("TrainScreenplays directory not found.")

    # Retrieves the paths of the train screenplays left to load
    train_screenplays_paths = constants.TRAIN_SCREENPLAYS_PATHS
    if Path.exists(constants.TRAIN_CSV_PATH):
        trained_screenplays_filenames = pandas.read_csv(constants.TRAIN_CSV_PATH,
                                                        usecols=["Filename"],
                                                        dtype={"Filename": str}).Filename
        trained_screenplays_paths = map(lambda filename: constants.TRAIN_SCREENPLAYS_PATH / f"{filename}.txt",
                                        trained_screenplays_filenames)
        train_screenplays_paths = list(filter(lambda path: path not in trained_screenplays_paths,
                                              train_screenplays_paths))

    # Calculates amount of batches required
    batch_size = multiprocessing.cpu_count()
    batch_count = math.ceil(len(train_screenplays_paths) / batch_size)
    print(f"{datetime.now()}: Processing begun.")

    # Loads the train screenplays
    with ThreadPoolExecutor(batch_size) as executor:
        for i in range(batch_count):
            # Takes a batch of train screenplays file paths
            limit = min((i + 1) * batch_size, len(train_screenplays_paths))
            file_paths_batch = train_screenplays_paths[i * batch_size:limit]

            # Loads the screenplays batch using thread for each file path
            screenplay_threads = [executor.submit(load_screenplay, file_path) for file_path in file_paths_batch]
            screenplays_batch = [thread.result() for thread in screenplay_threads]

            # Appends the loaded batch to csv file
            screenplays_batch = pandas.DataFrame(screenplays_batch)
            screenplays_batch.to_csv(constants.TRAIN_CSV_PATH,
                                     mode="a",
                                     index=False,
                                     header=not constants.TRAIN_CSV_PATH.exists())

            print(f"{datetime.now()}: screenplay records were written to csv file.")

    # Merges the loaded train screenplays with their respective features and labels
    screenplays = pandas.read_csv(constants.TRAIN_CSV_PATH)
    features = screenplay_processor.extract_features(screenplays)
    genres = load_genres()

    screenplays = screenplays.drop(columns=["Text"], axis=1)
    screenplays = screenplays.join(features).merge(genres, on="Filename")
    screenplays.to_csv(constants.TRAIN_CSV_PATH, index=False)

    print(f"{datetime.now()}: Processing ended.")


def load_test_screenplays(file_paths):
    # Loads the test screenplays using thread for each screenplay
    batch_size = len(file_paths)

    with ThreadPoolExecutor(batch_size) as executor:
        screenplay_threads = [executor.submit(load_screenplay, file_path) for file_path in file_paths]
        screenplay_records = [thread.result() for thread in screenplay_threads]

    # Merges the loaded test screenplays with their respective features
    screenplays = pandas.DataFrame(screenplay_records)
    features = screenplay_processor.extract_features(screenplays)

    screenplays = screenplays.drop(columns=["Text"], axis=1)

    return screenplays.join(features)


def load_genres():
    # Loads the genres labels for the train screenplays
    movie_info = ScriptInfo.schema().loads(constants.MOVIE_INFO_PATH.read_text(), many=True)

    screenplays = [[screenplay_info.title,
                    screenplay_info.filename,
                    tuple(screenplay_info.genres)] for screenplay_info in movie_info]

    return pandas.DataFrame.from_records(screenplays, columns=["Title", "Filename", "Genres"])
