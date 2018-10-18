p = 'D:/tmp/nrf.data'

with open(p, 'rb') as f:
    buffer = f.read()

import bitarray

ba = bitarray.bitarray()

for b in buffer:
    if b == 0:
        ba.append(False)
    elif b == 1:
        ba.append(True)
    else:
        print('Unk?')

for i in range(0, len(ba)):
    byte = ba[i:i + 8].tobytes()
    if byte != b'\xff':
        print('{:X}'.format(ord(byte)))