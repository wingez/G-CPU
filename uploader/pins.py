from pathlib import Path
import json
import os

def loadpins():

    path = Path(__file__).resolve().parent.parent


    path = path / 'config' / 'uploadpins.json'

    with path.open() as jsondata:
        pins = json.load(jsondata)

    return pins
    








