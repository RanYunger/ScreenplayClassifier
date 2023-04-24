# Imports
import sys
import classifier
import constants
import loader


# Main
if __name__ == "__main__":
    try:
        if constants.TRAIN_MODE:
            # Loads and pre-processes the train screenplays
            loader.load_train_screenplays()
        else:
            # Loads, pre-processes and classifies the test screenplays
            classifications = classifier.classify(sys.argv[1:])

            # Prints classifications to process
            print(classifications.to_json(orient="records", indent=4))

            """
            OUTPUT EXAMPLE:
            Title               |   GenrePercentages        
            "American Psycho"       {"Action": 22.43, "Adventure": 14.88 ... }
            """
    except Exception as ex:
        message = str(ex)
        if "[Errno" in message:
            message = message[message.index("]") + 2:]
        print(message)