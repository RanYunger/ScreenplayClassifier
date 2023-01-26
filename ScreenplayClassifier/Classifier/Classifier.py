# Imports
import os, time, pandas, pickle

from pathlib import Path
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import f1_score
from sklearn.multiclass import OneVsRestClassifier
from sklearn.preprocessing import MultiLabelBinarizer

from Loader import *
from TextProcessor import *

# Globals
default_binarizer   = MultiLabelBinarizer()
default_vectorizer  = TfidfVectorizer(max_df=0.8, max_features=10000)
default_classifier  = OneVsRestClassifier(LogisticRegression())

# Methods
def load_pickle():
    pickle_file = open(Path.cwd() / "Classifier/Pickle", "rb")
    loaded_binarizer, loaded_vectorizer, loaded_classifier = pickle.load(pickle_file)
    pickle_file.close()

    binarizer = loaded_binarizer if loaded_vectorizer is not None else default_binarizer
    vectorizer = loaded_vectorizer if loaded_vectorizer is not None else default_vectorizer
    classifier = loaded_classifier if loaded_classifier is not None else default_classifier

    return [binarizer, vectorizer, classifier]

def save_pickle(binarizer, vectorizer, classifier):
    # Validation
    saved_binarizer = binarizer if binarizer is not None else default_binarizer
    saved_vectorizer = vectorizer if vectorizer is not None else default_vectorizer
    saved_classifier = classifier if classifier is not None else default_classifier

    pickle_file = open(Path.cwd() / "Classifier/Pickle", "wb")
    pickle.dump([saved_binarizer, saved_vectorizer, saved_classifier], pickle_file)
    pickle_file.close()

def probabilities_to_percentages(probabilities):
    genre_labels = json.load(open("Jsons/Genres.json"))
    probabilities_dict = dict(zip(genre_labels, probabilities))
    probabilities_dict = dict(sorted(probabilities_dict.items(), key=lambda item: item[1], reverse=True))
    sum_of_probabilities = sum(probabilities)
    percentages_dict = {}

    # Converts each genre's probability to matching percentage
    for genre, probability in probabilities_dict.items():
        percentages_dict[genre] = (probability / sum_of_probabilities) * 100

    return percentages_dict

def train():
    # Loads classifier's variables from file
    binarizer, vectorizer, classifier = load_pickle()

    # If training is required
    if (binarizer == default_binarizer) or (vectorizer == default_vectorizer) or (classifier == default_classifier):
        train_screenplays = pandas.merge(read_train_screenplays(), read_genres(), on="Title")

        binarizer.fit(train_screenplays["Actual Genres"])
        y = binarizer.transform(train_screenplays["Actual Genres"])

        x_train, x_validation, y_train, y_validation = train_test_split(train_screenplays["Text"], y, test_size=0.2,
                                                                        random_state=42)

        x_train_tfidf = vectorizer.fit_transform(x_train)
        x_validation_tfidf = vectorizer.transform(x_validation)

        classifier.fit(x_train_tfidf, y_train)

        y_probabilities = classifier.predict_proba(x_validation_tfidf)

    return [binarizer, vectorizer, classifier]

def classify(classifier_variables, test_screenplays):
    binarizer, vectorizer, classifier = classifier_variables
    classifications_dict = {}
    classifications_complete = 0

    for offset, test_screenplay in test_screenplays.iterrows():
        test_vector = vectorizer.transform([test_screenplay["Text"]])
        test_probabilities = sum(classifier.predict_proba(test_vector).tolist(), []) # Flattens the list
        test_percentages = probabilities_to_percentages(test_probabilities)

        classifications_dict[test_screenplay["Title"]] = test_percentages

        classifications_complete += 1
        print(classifications_complete)

        time.sleep(0.5) # Sleep for 0.5 seconds

    # Saves classifier's variables to file
    save_pickle(binarizer, vectorizer, classifier)

    return pandas.DataFrame({"Title": classifications_dict.keys(),
                             "GenrePercentages": classifications_dict.values()})