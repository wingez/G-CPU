import config


def getPins(name):

    jsondata = config.loadconfig('uploadpins')
    jsondata = jsondata[name]
    result = dict()

    for id, data in jsondata.items():
        print(id)
        print(data)
        result[id] = Pin(data['pin'],data['activestate'])

    return result



class Pin(object):
    def __init__(self,pin,activestate):
        self.pinnumber = pin
        self.activestate = activestate



#def loadpins():

#    path = Path(__file__).resolve().parent.parent


#    path = path / 'config' / 'uploadpins.json'

#    with path.open() as jsondata:
#        pins = json.load(jsondata)

#    return pins
    








 