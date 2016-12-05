 
from IO.protocols import setuppins, shiftoutbyte, LSBfirst, pulse
import IO.pins


rampins = IO.pins.getPins('ram')
setuppins(rampins)


def uploadbyte(address,value):
    shiftoutbyte(address,LSBfirst,rampins)
    shiftoutbyte(value,LSBfirst,rampins)
    pulse('latch',rampins)
    pulse('write',rampins)




