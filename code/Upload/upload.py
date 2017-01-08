
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
        if line.startswith("//") or len(line) < 2:
            continue
        
        if line.startswith("#numchips="):
            mystring = line.split("numchips=",1)[1]
            numchips = int(mystring) 
            continue

        numbers = list()
        for n in line.split("\t"):
            print(n)
            numbers.append(int(n,16))

        address = 0
        address |= (1 << numbers[1])
        address |= (numbers[0] << 6)

       

        value = numbers[2 + ((numchips - 1) - chip)]

        print(address)
        print(value)

        writerom(address,value,clockpin,datapin,latchpin,writepin)
        time.sleep(0.1)
        #print("waiting")
        #input()


    
        





def setup(pins):
    io.setmode(io.BCM)
    io.setwarnings(False)

    for i in pins:
        io.setup(i,io.OUT)
    

def writerom(address,value,clockpin,datapin,latchpin,writepin):
    setup([clockpin,datapin,latchpin,writepin])
    io.output(latchpin,False)
    io.output(writepin,True)

    offsets = [10,8,7,6,5,4,3,2,1,0]

    addresstoshift = 0
    for i in range(10):
        if address & (1 << i):
            addresstoshift|=(1 << offsets[i])




    shiftInt(clockpin,datapin,addresstoshift,16,MSBFirst)
    shiftInt(clockpin,datapin,value,8,LSBFirst)

    pulse(latchpin,True)
    pulse(writepin,False)


def uploadram(clockpin,datapin,latchpin,writepin):
    start = int(input("start: "))
    stop = int(input("stop: "))
    for addr in range(start,stop):
        value = int(input(str(addr)+": "))
        writeram(addr,value,clockpin,datapin,latchpin,writepin)

    print("done")


def writeram(address,value,clockpin,datapin,latchpin,writepin):
    setup([clockpin,datapin,latchpin,writepin])
    io.output(latchpin,False)
    io.output(writepin,False)

    shiftInt(clockpin,datapin,address,8,LSBFirst)
    shiftInt(clockpin,datapin,value,8,LSBFirst)
    pulse(latchpin,True)
    pulse(writepin,True)
    






def pulse(pin,state):
    io.output(pin,state)
    time.sleep(0.001)
    io.output(pin,not state)
    time.sleep(0.001)


def shiftInt(clockpin,datapin,data,size,mode):
    
    io.setup(clockpin,io.OUT)
    io.setup(datapin,io.OUT)

    io.output(clockpin,False)
    io.output(datapin,False)

    if mode == LSBFirst:
        for i in range(size):
            if data & (1 << i):
                io.output(datapin,True)
            else:
                io.output(datapin,False)

            io.output(clockpin,True)
            time.sleep(0.001)
            io.output(clockpin,False)
            time.sleep(0.001)
    elif mode == MSBFirst:
        for i in range(size):
            if data & (1 << ((size - 1) - i)):
                io.output(datapin,True)
            else:
                io.output(datapin,False)

            io.output(clockpin,True)
            time.sleep(0.001)
            io.output(clockpin,False)
            time.sleep(0.001)
    










