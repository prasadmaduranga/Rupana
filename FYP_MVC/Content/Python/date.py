import sys
from datetime import*
path=[]
path = sys.argv[1].split(',')
from dateutil import parser
dateString =""
result = [None] * len(path)
for i in range(len(path)):
    try:
        dateString = parser.parse(path[i])
        result[i] = dateString 
    except:
        result[i] = "error"

for res in result:
    print(res)
    

