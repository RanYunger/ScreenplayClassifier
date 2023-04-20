# Imports
import re
import pandas
import constants

from nltk import PorterStemmer
from nltk.corpus import stopwords
from sklearn.feature_extraction.text import TfidfVectorizer

# Globals
tf_idf_vectorizer = TfidfVectorizer(max_features=constants.FEATURES_COUNT)


# Methods
def clean_text(text):
    # Cleans the text from non-alphabetic chars
    text = re.sub(r"[^a-zA-Z]", "", text)

    # Cleans the lower-cased text from spaces and special chars
    text = " ".join(re.split(" \n\t", text.lower()))

    # Cleans the text from stopwords (e.g.: an, of, is, at, by...) and stems the remaining words
    stemmer = PorterStemmer()
    stop_words = set(stopwords.words("english"))
    text = " ".join([stemmer.stem(word) for word in text.split() if word not in stop_words])

    return text


def extract_features(screenplays):
    # Cleans all screenplays' texts
    screenplays["Text"].apply(lambda text: clean_text(str(text)))

    # Extracts features from the cleaned texts and saves them as a dataframe
    vectors = tf_idf_vectorizer.fit_transform(screenplays["Text"].values.astype("U"))
    feature_names = tf_idf_vectorizer.get_feature_names_out()
    vectors = vectors.todense().tolist()

    return pandas.DataFrame(vectors, columns=feature_names)
