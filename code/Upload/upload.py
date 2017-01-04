
import RPi.GPIO as io
import time

LSBFirst = 0
MSBFirst = 1


def writeromfromfile(file,clockpin,datapin,latchpin,writepin):
    
    f = open(file)

    print("chip #?")
    chip = int(input())


    numchips = 0

    for line in f:
        line.strip()
        if line.startswith("//" or not line):
            continue
        
        if line.startswith("#numchips="):
            mystring = line.split("numchips",1)[1]
            numchips = int(mystring) 
            continue

        numbers = list()
        for n in line.split("\t"):
            numers.append(int(n))

        address = 0
        address |= (1 << numbers[1])
        address |= (numbers[0] << 6)

        value = numbers[2 + chip]


        print(address)
        print(value)

    
        






def setup(pins):
    io.setmode(io.BCM)
    io.setwarnings(False)

    for i in pins:
        io.setup(i,io.Out)
    

def writerom(address,value,clockpin,datapin,latchpin,writepin):
    setup([clockpin,datapin,latchpin,writepin])
    io.output(latchpin,False)
    io.output(writepin,True)

    shiftInt(clockpin,datapin,address,16,LSBFirst)
    shiftInt(clockpin,datapin,data,8,LSBFirst)

    pulse(latchpin,True)
    pulse(writepin,False)



def writeram(address,value,clockpin,datapin,latchpin,writepin):
    setup([clockpin,datapin,latchpin,writepin])
    io.output(latchpin,False)
    io.output(writepin,True)

    shiftInt(clockpin,datapin,address,8,LSBFirst)
    shiftInt(clockpin,datapin,value,8,LSBFirst)
    pulse(latchpin,True)
    pulse(writepin,False)
    






def pulse(pin,state):
    io.output(latchpin,state)
    time.sleep(0.001)
    io.output(latchpin,not state)
    time.sleep(0.001)


def shiftInt(clockpin,datapin,data,size,mode):
    
    io.setup(clockpin,io.Out)
    io.setup(datapin,io.Out)

    io.output(clockpin,False)
    io.output(datapin,False)

    if mode is LSBFirst:
        for i in range(size):
            if data & (1 << i):
                io.output(datapin,True)
            else:
                io.output(datapin,False)

            io.output(clockpin,True)
            time.sleep(0.001)
            io.output(clockpin,False)
            time.sleep(0.001)
    elif mode is MSBFirst:
        for i in range(size):
            if data & (1 << ((size - 1) - i)):
                io.output(datapin,True)
            else:
                io.output(datapin,False)

            io.output(clockpin,True)
            time.sleep(0.001)
            io.output(clockpin,False)
            time.sleep(0.001)
    










