


def confirm(prompt,retries=4, complaint="Yes or No please"):
    while True:
        ok = input()
        if ok in ('y','yes','ok'):
            return True
        if ok in ('n','no'):
            return False

        retries-=1
        if retries<0:
            raise IOError('uncooperative user')
        print(complaint)



