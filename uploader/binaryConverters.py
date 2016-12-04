




def IntToBinaryArray(value,size):
    #Creates a (size) long binary-list-representation of value, MSB first

    if value<0:
        raise ValueError("only positive values can be converted")

    result=list()

    largestBit= 2**(size-1)

    currentBitToTest=largestBit
    for i in range(0,size):
        if value>=currentBitToTest:
            result.append(1)
            value-=currentBitToTest
        else:
            result.append(0)

        if currentBitToTest == 1:
            break

        currentBitToTest/=2

    return result







