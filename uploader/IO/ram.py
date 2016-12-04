
from .protocols import setuppins, shiftoutbyte, LSBfirst, pulse
import pins


rampins = pins.loadpins()['ram']
setuppins(rampins)


def write(address,value):
    shiftoutbyte(address,LSBfirst,rampins)
    shiftoutbyte(value,LSBfirst,rampins)
    pulse('latch',rampins)
    pulse('write',rampins)




