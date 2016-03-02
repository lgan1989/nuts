import sys
import os

files = [f for f in os.listdir('.') if os.path.isfile(f)]

template = open('template.txt', 'r').readlines()

for f in files:
    content = []
    name = f.replace('-1.png.meta', '')
    for line in template:
        content.append(line.replace('{ID}', name))
    if 'meta' in f:
        fp = open(f, 'r')
        lines = fp.readlines()
        result = lines[:4] + content
        fp = open(f , 'w')
        fp.writelines(result)
