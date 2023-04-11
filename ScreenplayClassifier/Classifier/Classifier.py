# Imports
import time
import numpy
import pandas
import pickle

from sklearn.ensemble import BaggingClassifier

import Constants
from Loader import load_test_screenplays
from ScreenplayProcessor import protagonist_roles_dict, time_periods, times_of_day
from sklearn.metrics import accuracy_score
from sklearn.multioutput import MultiOutputClassifier
from sklearn.svm import SVC
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.preprocessing import MultiLabelBinarizer

# Methods
def save_model(model):
    # Writes model variables to pickle file
    pickle_file = open(Constants.model_pickle_path, "wb")
    pickle.dump(model, pickle_file)
    pickle_file.close()

def encode(screenplays):
    # Encodes the textual values in the screenplays dataframe
    protagonist_roles = list(protagonist_roles_dict.values())

    screenplays.replace({"Protagonist Roles": {role: protagonist_roles.index(role) for role in protagonist_roles},
                         "Time Period": {period: time_periods.index(period) for period in time_periods},
                         "Dominant Time of Day": {time: times_of_day.index(time) for time in times_of_day}},
                        inplace=True)

    return screenplays

def get_optimal_amount_of_estimators(x, t, base_estimator):
    hyper_parameters = {"n_estimators": list(range(10, 21)), "bootstrap": [True, False]}
    gridSearch = GridSearchCV(BaggingClassifier(base_estimator=base_estimator, random_state=42), hyper_parameters,
                              scoring="neg_log_loss").fit(x, t)

    return gridSearch.best_params_["n_estimators"], gridSearch.best_params_["bootstrap"]

def create_model():
    # Creates a classification model
    train_screenplays = encode(pandas.read_csv(Constants.train_csv_path))
    train_screenplays["Genres"] = [eval(genres) for genres in train_screenplays["Genres"]]

    t = MultiLabelBinarizer().fit_transform(train_screenplays["Genres"])
    x = train_screenplays.drop(["Title", "Genres"], axis=1)
    x_train, x_validation, t_train, t_validation = train_test_split(x, t, test_size=0.2, random_state=42)

    # Builds classifier and predicts its accuracy score (current best: SVC - 0.1441)
    base_estimator = MultiOutputClassifier(SVC(probability=True)).fit(x_train, t_train)
    # TODO: IMPROVE BASE ESTIMATOR USING HYPER_PARAMETERS & ENSEMBLES
    # best_amount_of_estimators, using_bootstrap = get_optimal_amount_of_estimators(x_train, t_train, base_estimator)
    # ensembles_classifier = BaggingClassifier(base_estimator=base_estimator, n_estimators=best_amount_of_estimators,
    #                                          bootstrap=using_bootstrap, random_state=1).fit(x_train, t_train)
    classifier = base_estimator

    t_predictions = classifier.predict(x_validation)
    score = accuracy_score(t_validation, t_predictions)
    print("Accuracy: {:.4f}".format(score))

    # Saves model variables to file
    save_model(classifier)

    return classifier

def load_model():
    # Validates existence of pickle file
    if not Constants.model_pickle_path.exists():
        return create_model()

    # Reads model variables from pickle file
    pickle_file = open(Constants.model_pickle_path, "rb")
    model = pickle.load(pickle_file)
    pickle_file.close()

    return model

def probabilities_to_percentages(probabilities):
    probabilities_dict = dict(zip(Constants.genre_labels, probabilities))
    probabilities_dict = dict(sorted(probabilities_dict.items(), key=lambda item: item[1], reverse=True))
    sum_of_probabilities = sum(probabilities)
    percentages_dict = {}

    # Converts each genre's probability to matching percentage
    for genre, probability in probabilities_dict.items():
        percentages_dict[genre] = (probability / sum_of_probabilities) * 100

    return percentages_dict

def classify(file_paths):
    # Loads test screenplays and classification model
    test_screenplays = encode(load_test_screenplays(file_paths))
    classifier = load_model()
    classifications_dict = {}
    classifications_complete = 0

    # Classifies the test screenplays
    for offset, screenplay in test_screenplays.iterrows():
        features_vector = [feature[0] for feature in numpy.array(screenplay[1:]).reshape(-1, 1)]
        genre_probabilities = [probability.tolist()[0] for probability in classifier.predict_proba([features_vector])]
        genre_probabilities = [probability[0] for probability in genre_probabilities]
        classifications_dict[file_paths[offset]] = probabilities_to_percentages(genre_probabilities)

        # Prints progress (for GUI to update progress)
        classifications_complete += 1
        print(classifications_complete)

        time.sleep(0.5)  # seconds

    return pandas.DataFrame({"FilePath": classifications_dict.keys(),
                             "GenrePercentages": classifications_dict.values()})