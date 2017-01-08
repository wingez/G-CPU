from pathlib import Path
import json
import os



def loadconfig(name):
    thisPath = Path(__file__)
    basefolder = thisPath.resolve().parent.parent
    configfolder = basefolder / 'config'
    
    path = configfolder / (name + '.json')
    
    if not path.exists():
        raise ValueError('File not found.  ' + str(path))
    
    with path.open() as jsonfile:
         result = json.load(jsonfile)

    return result









