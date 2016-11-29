import RPi.GPIO as IO
import time

latchpin = 16
clockpin = 20
datapin = 21
writepin = 26


IO.setmode(IO.BCM)
IO.setwarnings(False)

IO.setup(latchpin,IO.OUT)
IO.setup(clockpin,IO.OUT)
IO.setup(datapin,IO.OUT)
IO.setup(writepin,IO.OUT)


IO.output(latchpin,0)
IO.output(clockpin,0)
IO.output(datapin,0)
IO.output(writepin,0)

def shiftout(data):
    if(data > 255):
        print("number to large")
        return

    IO.output(clockpin,0)
    IO.output(datapin,0)

    bits = list()
    startbit = 128
    for i in range(0,8):
        if data >= startbit:
            bits.append(1)
            data-=startbit
        else:
            bits.append(0)
        startbit/=2
    
    print(bits)
    
    for i in reversed(bits):
        IO.output(datapin,i)
        time.sleep(0.01)
        IO.output(clockpin,1)
        time.sleep(0.01)
        IO.output(clockpin,0)
        time.sleep(0.01)

def latch():
    IO.output(latchpin,1)
    time.sleep(0.01)
    IO.output(latchpin,0)
    time.sleep(0.01)

def pulseWritePin():
    IO.output(writepin,1)
    time.sleep(0.01)
    IO.output(writepin,0)
    time.sleep(0.01)


def write(address,value):
    shiftout(address)
    shiftout(value)
    latch()
    pulseWritePin()


while True:
    
    try:
        address=int(input('address: '))
        value=int(input('value: '),2)
    except :
        print('error')
        text=input()
        if text=='exit':
            break

    write(address,value)
    print('byte written')













