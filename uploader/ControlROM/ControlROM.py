#from .ControlSignals import ControlSignal, ControlWire
import IO.pins
import config

pins = None

controlwires = list()
controlsignals = list()
instructions = list()


 

class ControlWire(object):

     def __init__(self, name,index,activestate):
         self.name = name
         self.index = index
         self.activestate = activestate
         
    
class ControlSignal(object):

    def __init__(self, name, index):
        self.name = name
        self.index = index
        self.controlwires = list()

class Instruction(object):
    
    def __init__(self,name,index):
        self.name = name
        self.index = index
        self.commands = list()
        
class Command(object):
    def __init__(self):
        self.signals = list()




def validatewire(wire:ControlWire):

    return True

def validatesignal(signal:ControlSignal):

    return True

def validateinstruction(instruction:Instruction):

    return True





def loadconfigs():
    pins = IO.pins.getPins('instructionrom')



def createall():
    
    controlwires = list()
    controlsignals = list()
    instructions = list()


    wiresconfig = config.loadconfig('controlwires')


    for wireinfo in wiresconfig['lines']:
        index = wireinfo['index']
        name = wireinfo['name'].lower()
        activestate = wireinfo['activestate']

        for i in controlwires:
            if i.name == name:
                raise Exception("controllines name duplicate" + name)
            if i.index == index:
                raise Exception("controllines index duplication. " + str(index))

        controlwires.append(ControlWire(name,index,activestate))




    signalconfig = config.loadconfig('controlsignals')

    signalindex = 0

    for signalinfo in signalconfig['signals']:
        
        signal = ControlSignal(signalinfo['name'].lower(),signalindex)

        for i in controlsignals:
            if signal.name == i.name:
                raise Exception("controlsignal duplicate. " + str(signal.name))


        signal.controlwires = [t for t in controlwires if t.name == signal.name]
        
        controlsignals.append(signal)

        signalindex+=1



    instructionconfig = config.loadconfig('instructions')

    for instructioninfo in instructionconfig['instructions']:

        instruction = Instruction(instructioninfo['name'].lower(),instructioninfo['index'])

        for commandinfo in instructioninfo['commands']:
            command = Command()
            for signal in commandinfo:
                for s in controlsignals:
                    if s.name == signal:
                        command.signals.append(s)
                        break
                else:
                    raise Exception('controlsignal not found. Instruction: ' + instruction.name + ' Requsted: ' + signal)
                

            instruction.commands.append(command)

        instructions.append(instruction)


    print('WIRES:')
    for i in controlwires:
        print(i.name)
        print('-' + str(i.index))
        print('-' + str(i.activestate))

    print()
    print('SIGNALS:')
    for i in controlsignals:
        print(i.name)
        print('-Wires')
        for j in i.controlwires:
            print('--' + j.name)

    print()
    print('INSTRUCTIONS:')
    for i in instructions:
        print(i.name)
        print('-' + str(i.index))
        print('-Commands')
        for j in i.commands:
            print('--Signals')
            for k in j.signals:
                print('---' + k.name)
    



        












def upload(memoryindex):
    pass








