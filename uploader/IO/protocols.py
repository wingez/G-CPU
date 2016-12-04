import RPi.GPIO as io
import time

from binaryConverters import IntToBinaryArray

MSBfirst = 0
LSBfirst = 1









def setpin(pin,state):
    io.output(pin['pin'],not (bool(state) ^ pin['activestate']))
    
def setuppins(pins):
    io.setmode(io.BCM)
    io.setwarnings(False)
    
    for pin in pins.values():
        io.setup(pin['pin'],io.OUT)
        setpin(pin,0)



def shiftoutint(data,size,mode,pins):
    if size < 1:
        raise ValueError('size must be greater than 0')

    binary = IntToBinaryArray(data,size)
    shiftoutlist(binary,mode,pins)

def shiftoutbyte(data,mode,pins):
    shiftoutint(data,8,mode,pins)
     

def shiftoutlist(values,mode,pins):

    bits = list()
    if mode == MSBfirst:
        bits = values
    elif mode == LSBfirst:
        bits = reversed(values)

    clock = pins['clock']
    data = pins['data']

    io.output(clock['pin'], not clock['activestate'])
    
    for bit in bits:
        setpin(data,bit)
        time.sleep(0.001)
        setpin(clock,1)
        time.sleep(0.001)
        setpin(clock,0)
        time.sleep(0.001)

def latch(pins):
    latchpin = pins['latch']
    setpin(latchpin,1)
    time.sleep(0.001)
    setpin(latchpin,0)
    time.sleep(0.001)

def pulse(pin,pins):
    pulsepin = pins[pin]
    setpin(pulsepin,1)
    time.sleep(0.001)
    setpin(pulsepin,0)
    time.sleep(0.001)
