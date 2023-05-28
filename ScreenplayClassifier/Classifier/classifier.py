# Imports
import time
from pathlib import Path

import numpy
import pandas
import pickle
import constants

from sklearn.multiclass import OneVsRestClassifier
from sklearn.tree import DecisionTreeClassifier
from loader import load_test_screenplays
from sklearn.preprocessing import MultiLabelBinarizer
from sklearn.model_selection import train_test_split


# Methods
def load_model():
    # Validates existence of pickle file
    if not constants.MODEL_PICKLE_PATH.exists():
        return create_model()

    # Reads model variables from pickle file
    pickle_file = open(constants.MODEL_PICKLE_PATH, "rb")
    model = pickle.load(pickle_file)
    pickle_file.close()

    return model


def save_model(model):
    # Writes model variables to pickle file
    pickle_file = open(constants.MODEL_PICKLE_PATH, "wb")
    pickle.dump(model, pickle_file)
    pickle_file.close()


def create_model():
    # Converts the genres of each screenplay into a list
    train_screenplays = pandas.read_csv(constants.TRAIN_CSV_PATH, dtype={"Filename": str, "Title": str})
    train_screenplays["Genres"] = [eval(genres) for genres in train_screenplays["Genres"]]

    # Splits the data into labels (t) and features (x)
    t = MultiLabelBinarizer().fit_transform(train_screenplays["Genres"])
    x = train_screenplays.drop(["Title", "Filename", "Genres"], axis=1)

    # Creates a classification model and prints its accuracy score
    classifier = OneVsRestClassifier(DecisionTreeClassifier(max_depth=constants.DECISION_TREE_DEPTH))

    if constants.DATA_SPLIT:
        x_train, x_validation, t_train, t_validation = train_test_split(x, t, test_size=0.2, random_state=1)

        classifier.fit(x_train, t_train)
        score = classifier.score(x_validation, t_validation)
    else:
        classifier.fit(x, t)
        score = classifier.score(x, t)

    print("Accuracy: {:.4f}".format(score))

    # Saves the model to file
    save_model(classifier)

    return classifier


def probabilities_to_percentages(probabilities):
    # Creates a sorted probabilities dictionary
    probabilities_dict = dict(zip(constants.GENRE_LABELS, probabilities))
    probabilities_dict = dict(sorted(probabilities_dict.items(), key=lambda item: item[1], reverse=True))
    sum_of_probabilities = sum(probabilities)
    percentages_dict = {}

    # Converts each genre's probability to matching percentage
    for genre, probability in probabilities_dict.items():
        percentages_dict[genre] = (probability / sum_of_probabilities) * 100

    return percentages_dict


def classify(file_paths):
    # Loads test screenplays and classification model
    test_screenplays = load_test_screenplays(file_paths)
    model = load_model()
    classifications = []
    classifications_complete = 0

    # Classifies the test screenplays
    for offset, screenplay in test_screenplays.iterrows():
        features_vector = [feature[0] for feature in numpy.array(screenplay[1:]).reshape(-1, 1)]
        genre_probabilities = model.predict_proba([features_vector])[0]
        classifications.append({
            "Title": Path(file_paths[offset]).stem,
            "FilePath": file_paths[offset],
            "GenrePercentages": probabilities_to_percentages(genre_probabilities)
        })

        # Prints progress (for GUI to update progress)
        classifications_complete += 1
        print(classifications_complete)

        time.sleep(0.5)  # seconds

    return pandas.DataFrame(classifications)
